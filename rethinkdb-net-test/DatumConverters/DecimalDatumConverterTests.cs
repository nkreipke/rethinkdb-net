using System;
using NUnit.Framework;
using RethinkDb.DatumConverters;

namespace RethinkDb.Test.DatumConverters
{
    [TestFixture]
    public class DecimalDatumConverterTests
    {
        [Test]
        public void ConvertDatum_ValueTooLargeToRepresentAsLongProperly_ThrowException()
        {
            Assert.That((TestDelegate)(() => {
                PrimitiveDatumConverterFactory.Instance.Get<decimal>().ConvertDatum(new RethinkDb.Spec.Datum(){type = RethinkDb.Spec.Datum.DatumType.R_NUM, r_num = 1.0 + (double)decimal.MaxValue});
            }), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void ConvertDatum_ValueTooSmallToRepresentAsLongProperly_ThrowException()
        {
            Assert.That((TestDelegate)(() => {
                PrimitiveDatumConverterFactory.Instance.Get<decimal>().ConvertDatum(new RethinkDb.Spec.Datum(){type = RethinkDb.Spec.Datum.DatumType.R_NUM, r_num = (double)decimal.MinValue - 1.0});
            }), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void ConvertDatum_ValueWithinRange_ReturnsValue()
        {
            const decimal expectedValue = 3000;
            var value = PrimitiveDatumConverterFactory.Instance.Get<decimal>().ConvertDatum(new RethinkDb.Spec.Datum(){type = RethinkDb.Spec.Datum.DatumType.R_NUM, r_num = (double)expectedValue});

            Assert.AreEqual(expectedValue, value, "should be equal");
        }
    }
}

