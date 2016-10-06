using System;
using NUnit.Framework;
using RethinkDb.DatumConverters;

namespace RethinkDb.Test.DatumConverters
{
    [TestFixture]
    public class FloatDatumConverterTests
    {
        [Test]
        public void ConvertDatum_ValueTooLargeToRepresentAsLongProperly_ThrowException()
        {
            Assert.That((TestDelegate)(() => {
                PrimitiveDatumConverterFactory.Instance.Get<float>().ConvertDatum(new RethinkDb.Spec.Datum(){type = RethinkDb.Spec.Datum.DatumType.R_NUM, r_num = 1.0 + float.MaxValue});
            }), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void ConvertDatum_ValueTooSmallToRepresentAsLongProperly_ThrowException()
        {
            Assert.That((TestDelegate)(() => {
                PrimitiveDatumConverterFactory.Instance.Get<float>().ConvertDatum(new RethinkDb.Spec.Datum(){type = RethinkDb.Spec.Datum.DatumType.R_NUM, r_num = float.MinValue - 1.0});
            }), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void ConvertDatum_ValueWithinRange_ReturnsValue()
        {
            const float expectedValue = 3000;
            var value = PrimitiveDatumConverterFactory.Instance.Get<float>().ConvertDatum(new RethinkDb.Spec.Datum(){type = RethinkDb.Spec.Datum.DatumType.R_NUM, r_num = expectedValue});

            Assert.AreEqual(expectedValue, value, "should be equal");
        }
    }
}

