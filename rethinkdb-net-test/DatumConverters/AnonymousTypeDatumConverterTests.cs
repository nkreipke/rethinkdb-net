using NUnit.Framework;
using RethinkDb.Spec;
using System;
using RethinkDb;
using NSubstitute;
using System.Collections.Generic;
using System.Runtime.Serialization;
using RethinkDb.DatumConverters;

namespace RethinkDb.Test.DatumConverters
{
    [TestFixture]
    public class AnonymousTypeDatumConverterTests
    {
        private IDatumConverterFactory datumConverterFactory;

        [OneTimeSetUp]
        public void TestFixtureSetUp()
        {
            datumConverterFactory = Substitute.For<IDatumConverterFactory>();

            string[] strs = { "one", "two", "three" };

            var stringDatumConverter = Substitute.For<IDatumConverter>();
            foreach (var i in strs)
            {
                var dtm = i.ToDatum();
                stringDatumConverter
                    .ConvertObject(i)
                        .Returns(dtm);
                stringDatumConverter
                    .ConvertDatum(Arg.Is<Datum>(d => d.type == dtm.type && d.r_str == dtm.r_str))
                        .Returns(i);
            }

            IDatumConverter value;
            datumConverterFactory
                .TryGet(typeof(string), datumConverterFactory, out value)
                    .Returns(args =>
                    {
                        args[2] = stringDatumConverter;
                        return true;
                    });
        }

        [Test]
        public void ConvertDatum_ReturnsValue()
        {
            var anon = new {First = "one", Second = "two"};
            var value = AnonymousTypeDatumConverterFactory.Instance.Get(((object)anon).GetType(), datumConverterFactory)
                .ConvertDatum(new Dictionary<string, string> { {"First", "one"}, {"Second", "two"} }.ToDatum());

            Assert.That(value, Is.EqualTo(anon));
        }

        [Test]
        public void ConvertDatum_ThrowsExceptionForMissingProperty()
        {
            var anon = new {First = "one"};

            Assert.That((TestDelegate)(() => {
                AnonymousTypeDatumConverterFactory.Instance.Get(((object)anon).GetType(), datumConverterFactory)
                                .ConvertDatum(new Dictionary<string, string> { {"First", "one"}, {"Second", "two"} }.ToDatum());
            }), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ConvertDatum_NullReturnsNull()
        {
            var anon = new {First = "one"};
            var value = AnonymousTypeDatumConverterFactory.Instance.Get(((object)anon).GetType(), datumConverterFactory)
                .ConvertDatum(new Datum { type = Datum.DatumType.R_NULL });

            Assert.That(value, Is.Null);
        }

        [Test]
        public void ConvertDatum_UnsupportedTypeThrowsException()
        {
            var anon = new {First = "one"};

            Assert.That((TestDelegate)(() => {
                AnonymousTypeDatumConverterFactory.Instance.Get(((object)anon).GetType(), datumConverterFactory)
                    .ConvertDatum(new Datum { type = Datum.DatumType.R_NUM });
            }), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void ConvertDatum_ThrowsExceptionForNonAnonymousType()
        {
            Assert.That((TestDelegate)(() => {
                AnonymousTypeDatumConverterFactory.Instance.Get<TestObject>(datumConverterFactory)
                    .ConvertDatum(new Dictionary<string, string> { {"First", "one"}, {"Second", "two"} }.ToDatum());
            }), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void ConvertObject_Null()
        {
            var anon = new {First = "one", Second = "two"};
            var obj = AnonymousTypeDatumConverterFactory.Instance.Get(((object)anon).GetType(), datumConverterFactory)
                .ConvertObject(null);
            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.type, Is.EqualTo(Datum.DatumType.R_NULL));
        }

        [Test]
        public void ConvertObject()
        {
            var anon = new {First = "one", Second = "two"};
            var obj = AnonymousTypeDatumConverterFactory.Instance.Get(((object)anon).GetType(), datumConverterFactory)
                .ConvertObject(anon);
            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.type, Is.EqualTo(Datum.DatumType.R_OBJECT));
            Assert.That(obj.r_object.Count, Is.EqualTo(2));
            Assert.That(obj.r_object[0].key, Is.EqualTo("First"));
            Assert.That(obj.r_object[0].val.r_str, Is.EqualTo("one"));
            Assert.That(obj.r_object[1].key, Is.EqualTo("Second"));
            Assert.That(obj.r_object[1].val.r_str, Is.EqualTo("two"));
        }

    }

    [DataContract]
    public class TestObject
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "children")]
        public TestObject[] Children;

        [DataMember(Name = "childrenList")]
        public List<TestObject> ChildrenList;

        [DataMember(Name = "childrenListInterface")]
        public IList<TestObject> ChildrenIList;

        [DataMember(Name = "number")]
        public double SomeNumber;

        [DataMember(Name = "tags")]
        public string[] Tags;

        [DataMember(Name = "guid")]
        public Guid Guid;

        [DataMember(Name = "data")]
        public byte[] Data;

        public override bool Equals(object obj)
        {
            var objTo = obj as TestObject;
            if (objTo != null)
                return Id != null && objTo.Id != null && String.Equals(Id, objTo.Id);
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            if (Id != null)
                return Id.GetHashCode();
            else
                return base.GetHashCode();
        }
    }
}
