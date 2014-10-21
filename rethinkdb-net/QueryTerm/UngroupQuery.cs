using RethinkDb.Spec;
using System;
using System.Linq.Expressions;

namespace RethinkDb.QueryTerm
{
    public class UngroupQuery<TKey, TValue> : ISequenceQuery<UngroupObject<TKey, TValue>>
    {
        private readonly IGroupingQuery<TKey, TValue> groupingQuery;

        public UngroupQuery(IGroupingQuery<TKey, TValue> groupingQuery)
        {
            this.groupingQuery = groupingQuery;
        }

        public Term GenerateTerm(IDatumConverterFactory datumConverterFactory)
        {
            var term = new Term()
            {
                type = Term.TermType.UNGROUP,
            };
            term.args.Add(groupingQuery.GenerateTerm(datumConverterFactory));
            return term;
        }
    }
}