using System;
using NUnit.Framework;
using RethinkDb.DatumConverters;

namespace RethinkDb.Test.DatumConverters
{
    [TestFixture]
    public class LongDatumConverterTests
    {
        [Test]
        public void ConvertDatum_ValueTooLargeToRepresentAsLongProperly_ThrowException()
        {
            Assert.That((TestDelegate)(() => {
                PrimitiveDatumConverterFactory.Instance.Get<long>().ConvertDatum(new RethinkDb.Spec.Datum(){type = RethinkDb.Spec.Datum.DatumType.R_NUM, r_num = 10000.0 + long.MaxValue});
            }), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void ConvertDatum_ValueTooSmallToRepresentAsLongProperly_ThrowException()
        {
            Assert.That((TestDelegate)(() => {
                PrimitiveDatumConverterFactory.Instance.Get<long>().ConvertDatum(new RethinkDb.Spec.Datum(){type = RethinkDb.Spec.Datum.DatumType.R_NUM, r_num = long.MinValue - 10000.0});
            }), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void ConvertDatum_ValueIsFraction_ThrowsException()
        {
            Assert.That((TestDelegate)(() => {
                PrimitiveDatumConverterFactory.Instance.Get<long>().ConvertDatum(new RethinkDb.Spec.Datum(){type = RethinkDb.Spec.Datum.DatumType.R_NUM, r_num = 0.25});
            }), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void ConvertDatum_ValueWithinRange_ReturnsValue()
        {
            const long expectedValue = 3000;
            var value = PrimitiveDatumConverterFactory.Instance.Get<long>().ConvertDatum(new RethinkDb.Spec.Datum(){type = RethinkDb.Spec.Datum.DatumType.R_NUM, r_num = expectedValue});

            Assert.AreEqual(expectedValue, value, "should be equal");
        }
    }
}