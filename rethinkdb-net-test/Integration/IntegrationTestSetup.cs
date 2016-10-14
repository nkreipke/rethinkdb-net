﻿using System;
using System.Linq;
using NUnit.Framework;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using RethinkDb.Configuration;
using System.Net.Sockets;
using System.Net;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace RethinkDb.Test.Integration
{
    [SetUpFixture]
    public class IntegrationTestSetup
    {
        protected Process rethinkProcess;

        [OneTimeSetUp]
        public void Setup()
        {
            StartRethinkDb();
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            if (rethinkProcess != null)
                rethinkProcess.Kill();
        }

        private string GetRethinkPath()
        {
            var path = Environment.GetEnvironmentVariable("PATH");
            var locations = path.Split(Path.PathSeparator).ToList();

            // Something is modifying PATH when running via xamarin studio, making it exclude several things
            // including /usr/local/bin. EnvironmentVariableTarget.User returns null.
            // So this uglyness has to stay for now?
            locations.Add("/usr/local/bin");

            foreach (var guess in locations.Select(l => Path.Combine(l, "rethinkdb")))
                if (File.Exists(guess))
                    return guess;

            return null;
        }

        private IPEndPoint GetRethinkEndpoint()
        {
            var configuration = new RethinkDbConfiguration();

            new ConfigurationBuilder().AddJsonFile("rethinkdb.json").Build().Bind(configuration);
            Validator.ValidateObject(configuration, new ValidationContext(configuration));

            var endpoint = configuration.Clusters.Single(c => c.Name == "testCluster").EndPoints.Single();
            return new IPEndPoint(IPAddress.Parse(endpoint.Address), endpoint.Port);
        }

        private bool IsEndpointAvailable(IPEndPoint endpoint)
        {
            using (var c = new TcpClient())
            {
                try
                {
                    c.ConnectAsync(endpoint.Address, endpoint.Port).Wait();
                } catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        private void StartRethinkDb()
        {
            var rethinkPath = GetRethinkPath();
            // if we can't find rethink, assume the user wants to use a remote instance
            if (rethinkPath == null)
                return;

            var rethinkEndpoint = GetRethinkEndpoint();
            // don't try to start if there is already something on the configured endpoint (it might even be rethinkdb!)
            if (rethinkEndpoint == null || IsEndpointAvailable(rethinkEndpoint))
                return;

            var dbPath = Path.Combine(Path.GetTempPath(), "rethink-integration-test");

            if (Directory.Exists(dbPath))
                Directory.Delete(dbPath, true);

            var processInfo = new ProcessStartInfo()
            {
                FileName = GetRethinkPath(),
                Arguments = string.Format("-d {0} --cluster-port 55557 --driver-port {1} --no-http-admin", dbPath, rethinkEndpoint.Port),
                UseShellExecute = false
            };
            rethinkProcess = Process.Start(processInfo);

            // wait for it to start up, but not forever
            int waited = 0;
            while (!IsEndpointAvailable(rethinkEndpoint) && waited < 60)
            {
                Thread.Sleep(250);
                waited++;
            }

            if (waited >= 60)
                throw new Exception("Could not start rethinkdb.");
        }
    }
}

