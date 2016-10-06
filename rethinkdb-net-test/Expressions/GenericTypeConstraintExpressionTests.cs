﻿using System;
using System.Runtime.Serialization;
using NUnit.Framework;
using FluentAssertions;
using RethinkDb.DatumConverters;
using RethinkDb.QueryTerm;
using RethinkDb.Spec;

namespace RethinkDb.Test.Expressions
{
    // The purpose of these unit tests is to handle reported issue #169, wherein a level of indirection in the types
    // being used in expression trees in the .NET CLR can sometimes cause useless Convert nodes to be added to the
    // expression tree.  rethinkdb-net needs to be able to handle these convert nodes.
    //
    // .NET CLR will insert Convert nodes into the expression tree in the IndirectCastByTypeConstraint... and
    // DirectCastWithTypeConstraint... unit tests, whereas mono does not.  The DirectIntentionalCastToExactSameType...
    // unit test under mono replicates what the .NET CLR does, so that we can be sure the case works even when running
    // tests on mono.

    [TestFixture]
    public class GenericTypeConstraintExpressionTests
    {
        IDatumConverterFactory datumConverterFactory;
        IExpressionConverterFactory expressionConverterFactory;
        IQueryConverter queryConverter;

        [OneTimeSetUp]
        public void TestFixtureSetUp()
        {
            datumConverterFactory = new AggregateDatumConverterFactory(
                PrimitiveDatumConverterFactory.Instance,
                DataContractDatumConverterFactory.Instance,
                new TestInterfaceDatumConverterFactory()
            );
            expressionConverterFactory = new RethinkDb.Expressions.DefaultExpressionConverterFactory();
            queryConverter = new QueryConverter(datumConverterFactory, expressionConverterFactory);
        }

        private class TestInterfaceDatumConverterFactory : AbstractDatumConverterFactory
        {
            public override bool TryGet<T>(IDatumConverterFactory rootDatumConverterFactory, out IDatumConverter<T> datumConverter)
            {
                datumConverter = null;
                if (rootDatumConverterFactory == null)
                    throw new ArgumentNullException("rootDatumConverterFactory");

                if (typeof(T) != typeof(ITestInterface))
                    return false;

                datumConverter = (IDatumConverter<T>)new TestInterfaceDatumConverter();
                return true;
            }
        }

        private class TestInterfaceDatumConverter : AbstractReferenceTypeDatumConverter<ITestInterface>, IObjectDatumConverter
        {
            public override ITestInterface ConvertDatum(Datum datum)
            {
                throw new System.NotImplementedException();
            }

            public override Datum ConvertObject(ITestInterface value)
            {
                throw new System.NotImplementedException();
            }

            public string GetDatumFieldName(System.Reflection.MemberInfo memberInfo)
            {
                return "number";
            }
        }

        private static void AssertFunctionIsGetFieldSomeNumberSingleParameter(Term expr)
        {
            var funcTerm =
                new Term() {
                type = Term.TermType.FUNC,
                args = {
                    new Term() {
                        type = Term.TermType.MAKE_ARRAY,
                        args = {
                            new Term() {
                                type = Term.TermType.DATUM,
                                datum = new Datum() {
                                    type = Datum.DatumType.R_NUM,
                                    r_num = 2,
                                }
                            }
                        }
                    },
                    new Term() {
                        type = Term.TermType.GET_FIELD,
                        args = {
                            new Term() {
                                type = Term.TermType.VAR,
                                args = {
                                    new Term() {
                                        type = Term.TermType.DATUM,
                                        datum = new Datum() {
                                            type = Datum.DatumType.R_NUM,
                                            r_num = 2,
                                        }
                                    }
                                }
                            },
                            new Term() {
                                type = Term.TermType.DATUM,
                                datum = new Datum() {
                                    type = Datum.DatumType.R_STR,
                                    r_str = "number",
                                }
                            },
                        }
                    },
                }
            };
            expr.ShouldBeEquivalentTo(funcTerm);
        }

        private static void AssertFunctionIsGetFieldSomeNumberDoubleParameter(Term expr)
        {
            var funcTerm =
                new Term() {
                type = Term.TermType.FUNC,
                args = {
                    new Term() {
                        type = Term.TermType.MAKE_ARRAY,
                        args = {
                            new Term() {
                                type = Term.TermType.DATUM,
                                datum = new Datum() {
                                    type = Datum.DatumType.R_NUM,
                                    r_num = 3,
                                }
                            },
                            new Term() {
                                type = Term.TermType.DATUM,
                                datum = new Datum() {
                                    type = Datum.DatumType.R_NUM,
                                    r_num = 4,
                                }
                            }
                        }
                    },
                    new Term() {
                        type = Term.TermType.GET_FIELD,
                        args = {
                            new Term() {
                                type = Term.TermType.VAR,
                                args = {
                                    new Term() {
                                        type = Term.TermType.DATUM,
                                        datum = new Datum() {
                                            type = Datum.DatumType.R_NUM,
                                            r_num = 3,
                                        }
                                    }
                                }
                            },
                            new Term() {
                                type = Term.TermType.DATUM,
                                datum = new Datum() {
                                    type = Datum.DatumType.R_STR,
                                    r_str = "number",
                                }
                            },
                        }
                    },
                }
            };
            expr.ShouldBeEquivalentTo(funcTerm);
        }

        public interface ITestInterface
        {
            double SomeNumber { get; set; }
        }

        [DataContract]
        public class TestObject : ITestInterface
        {
            [DataMember(Name = "id", EmitDefaultValue = false)]
            public string Id  { get; set; }

            [DataMember(Name = "number")]
            public double SomeNumber { get; set; }
        }

        [Test]
        public void DirectIntentionalCastToExactSameTypeSingleParameter()
        {
            var expr = ExpressionUtils.CreateFunctionTerm<TestObject, double>(
                queryConverter,
                o => ((ITestInterface)o).SomeNumber);
            AssertFunctionIsGetFieldSomeNumberSingleParameter(expr);
        }

        [Test]
        public void IndirectCastByTypeConstraintSingleParameter()
        {
            DoIndirectCastByTypeConstraintSingleParameter<TestObject>();
        }

        private void DoIndirectCastByTypeConstraintSingleParameter<T>() where T : ITestInterface
        {
            var expr = ExpressionUtils.CreateFunctionTerm<T, double>(
                queryConverter,
                o => o.SomeNumber);
            AssertFunctionIsGetFieldSomeNumberSingleParameter(expr);
        }

        [Test]
        public void DirectCastWithTypeConstraintSingleParameter()
        {
            DoIndirectCastByTypeConstraintSingleParameter<TestObject>();
        }

        private void DoDirectCastWithTypeConstraintSingleParameter<T>() where T : ITestInterface
        {
            var expr = ExpressionUtils.CreateFunctionTerm<T, double>(
                queryConverter,
                o => ((ITestInterface)o).SomeNumber);
            AssertFunctionIsGetFieldSomeNumberSingleParameter(expr);
        }

        [Test]
        public void DirectIntentionalCastToExactSameTypeDoubleParameter()
        {
            var expr = ExpressionUtils.CreateFunctionTerm<TestObject, TestObject, double>(
                queryConverter,
                (o1, o2) => ((ITestInterface)o1).SomeNumber);
            AssertFunctionIsGetFieldSomeNumberDoubleParameter(expr);
        }

        [Test]
        public void IndirectCastByTypeConstraintDoubleParameter()
        {
            DoIndirectCastByTypeConstraintDoubleParameter<TestObject>();
        }

        private void DoIndirectCastByTypeConstraintDoubleParameter<T>() where T : ITestInterface
        {
            var expr = ExpressionUtils.CreateFunctionTerm<T, T, double>(
                queryConverter,
                (o1, o2) => o1.SomeNumber);
            AssertFunctionIsGetFieldSomeNumberDoubleParameter(expr);
        }

        [Test]
        public void DirectCastWithTypeConstraintDoubleParameter()
        {
            DoIndirectCastByTypeConstraintDoubleParameter<TestObject>();
        }

        private void DoDirectCastWithTypeConstraintDoubleParameter<T>() where T : ITestInterface
        {
            var expr = ExpressionUtils.CreateFunctionTerm<T, T, double>(
                queryConverter,
                (o1, o2) => ((ITestInterface)o1).SomeNumber);
            AssertFunctionIsGetFieldSomeNumberDoubleParameter(expr);
        }
    }
}

