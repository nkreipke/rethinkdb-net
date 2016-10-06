using System;
using NUnit.Framework;
using RethinkDb.DatumConverters;

namespace RethinkDb.Test.DatumConverters
{
    [TestFixture]
    public class GuidDatumConverterTests
    {
        [Test]
        public void ConvertDatum_ValidDatum_ReturnsGuid()
        {
            var guid = Guid.NewGuid();

            var result = GuidDatumConverterFactory.Instance.Get<Guid>().ConvertDatum(new RethinkDb.Spec.Datum(){type = RethinkDb.Spec.Datum.DatumType.R_STR, r_str = guid.ToString()});

            Assert.AreEqual(guid.ToString(), result.ToString(), "should match");
        }

        [Test]
        public void ConvertDatum_InvalidGuid_ThrowsException()
        {
            var guidString = "NonsenseStringThatDoesNotSerializeToGuid";

            Assert.That((TestDelegate)(() => {
                GuidDatumConverterFactory.Instance.Get<Guid>().ConvertDatum(new RethinkDb.Spec.Datum() {type = RethinkDb.Spec.Datum.DatumType.R_STR, r_str = guidString});
            }), Throws.TypeOf<Exception>());
        }

        [Test]
        public void ConvertObject_ValidGuid_ReturnsDatum()
        {
            var guid = Guid.NewGuid();

            var result = GuidDatumConverterFactory.Instance.Get<Guid>().ConvertObject(guid);

            Assert.AreEqual(guid.ToString(), result.r_str, "should match");
        }
    }
}

