using NUnit.Framework;
using RethinkDb.Spec;
using System;
using RethinkDb.DatumConverters;

namespace RethinkDb.Test.DatumConverters
{
    [TestFixture]
    public class BoolDatumConverterTests
    {
        [Test]
        public void ConvertDatum_ReturnsValue()
        {
            var value = PrimitiveDatumConverterFactory.Instance.Get<bool>().ConvertDatum(true.ToDatum());
            Assert.That(value, Is.True);
            value = PrimitiveDatumConverterFactory.Instance.Get<bool>().ConvertDatum(false.ToDatum());
            Assert.That(value, Is.False);
        }

        [Test]
        public void ConvertDatum_NullReturnsException()
        {
            Assert.That((TestDelegate)(() => {
                PrimitiveDatumConverterFactory.Instance.Get<bool>().ConvertDatum(new Datum { type = Datum.DatumType.R_NULL });
            }), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void ConvertDatum_UnsupportedTypeReturnsException()
        {
            Assert.That((TestDelegate)(() => {
                PrimitiveDatumConverterFactory.Instance.Get<bool>().ConvertDatum(new Datum { type = Datum.DatumType.R_NUM });
            }), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void ConvertObject()
        {
            var obj = PrimitiveDatumConverterFactory.Instance.Get<bool>().ConvertObject(true);
            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.type, Is.EqualTo(Datum.DatumType.R_BOOL));
            Assert.That(obj.r_bool, Is.EqualTo(true));
        }

        [Test]
        public void ConvertObject_Null_ThrowsException()
        {
            Assert.That((TestDelegate)(() => {
                PrimitiveDatumConverterFactory.Instance.Get<bool>().ConvertObject(null);
            }), Throws.TypeOf<NotSupportedException>());
        }
    }
}
