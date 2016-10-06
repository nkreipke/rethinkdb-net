using System;
using NUnit.Framework;
using RethinkDb.DatumConverters;

namespace RethinkDb.Test.DatumConverters
{
    [TestFixture]
    public class UnsignedShortDatumConverterTests
    {
        [Test]
        public void ConvertDatum_ValueTooLarge_ThrowException()
        {
            Assert.That((TestDelegate)(() => {
                PrimitiveDatumConverterFactory.Instance.Get<ushort>().ConvertDatum(new RethinkDb.Spec.Datum(){type = RethinkDb.Spec.Datum.DatumType.R_NUM, r_num = 1.0 + ushort.MaxValue});
            }), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void ConvertDatum_ValueTooSmall_ThrowException()
        {
            Assert.That((TestDelegate)(() => {
                PrimitiveDatumConverterFactory.Instance.Get<ushort>().ConvertDatum(new RethinkDb.Spec.Datum(){type = RethinkDb.Spec.Datum.DatumType.R_NUM, r_num = ushort.MinValue - 1.0});
            }), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void ConvertDatum_ValueIsFraction_ThrowsException()
        {
            Assert.That((TestDelegate)(() => {
                PrimitiveDatumConverterFactory.Instance.Get<ushort>().ConvertDatum(new RethinkDb.Spec.Datum(){type = RethinkDb.Spec.Datum.DatumType.R_NUM, r_num = 0.25});
            }), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void ConvertDatum_ValueWithinRange_ReturnsValue()
        {
            const ushort expectedValue = 3000;
            var value = PrimitiveDatumConverterFactory.Instance.Get<ushort>().ConvertDatum(new RethinkDb.Spec.Datum(){type = RethinkDb.Spec.Datum.DatumType.R_NUM, r_num = expectedValue});

            Assert.AreEqual(expectedValue, value, "should be equal");
        }
    }
}

