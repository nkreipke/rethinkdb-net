﻿using System;
using System.Linq;
using System.Runtime.Serialization;
using RethinkDb;
using RethinkDb.Configuration;
using RethinkDb.ConnectionFactories;
using Microsoft.Extensions.Configuration;

[DataContract]
public class Person
{
    public static IDatabaseQuery Db = Query.Db("test");
    public static ITableQuery<Person> Table = Db.Table<Person>("people");

    [DataMember(Name = "id", EmitDefaultValue = false)]
    public Guid Id;

    [DataMember]
    public string Name;
}

public static class MainClass
{
    public static void Main(string[] args)
    {
        var config = new ConfigurationBuilder().AddJsonFile("rethinkdb.json").Build();
        var factory = new ConnectionFactoryBuilder().FromConfiguration(config).Build("example");

        using (var conn = factory.Get())
        {
            // Create DB if needed
            if (!conn.Run(Query.DbList()).Contains("test"))
                conn.Run(Query.DbCreate("test"));

            // Create table if needed
            if (!conn.Run(Person.Db.TableList()).Contains("people"))
                conn.Run(Person.Db.TableCreate("people"));

            // Read all the contents of the table
            foreach (var person in conn.Run(Person.Table))
                Console.WriteLine("Id: {0}, Name: {1}", person.Id, person.Name);

            // Insert a new record
            conn.Run(Person.Table.Insert(new Person() { Name = "Jack Black" }));
        }
    }
}