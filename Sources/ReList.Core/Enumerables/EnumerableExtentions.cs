// The MIT License (MIT)
//
// Copyright (c) 2014 Rui Carvalho
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
//	The above copyright notice and this permission notice shall be included in all
//	copies or substantial portions of the Software.
//
//	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//	SOFTWARE.
// ==============================================================================

namespace ReList.Core.Enumerables
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface INotInListHelper<TSource,TOther,TKey>
    {
        IEnumerable<TSource> Items { get; set; }
        IEnumerable<TOther> Other { get; set; }
        Func<TSource, TKey> JoinItemsKey { get; set; }
        Func<TOther, TKey> JoinOthersKey { get; set; }

        IEnumerable<TSource> GetExpression();
    }

    public class NotInListHelper<T, TKey> : INotInListHelper<T, T, TKey>
    {
        public IEnumerable<T> Items { get; set; }
        public IEnumerable<T> Other { get; set; }
        public Func<T, TKey> JoinItemsKey { get; set; }
        public Func<T, TKey> JoinOthersKey { get; set; }

        public NotInListHelper(
            IEnumerable<T> items,
            IEnumerable<T> other,
            Func<T, TKey> joinKey = null
            )
        {
            Items = items;
            Other = other;
            JoinItemsKey = joinKey ?? (x => (TKey)Convert.ChangeType(x, typeof(TKey)));
            JoinOthersKey = JoinItemsKey;
        }

        public IEnumerable<T> GetExpression()
        {
            return from item in Items
                   join otherItem in Other on JoinItemsKey(item)
                equals JoinOthersKey(otherItem) into tempItems
                   from temp in tempItems.DefaultIfEmpty<T>()
                   where ReferenceEquals(null, temp) || temp.Equals(default(T))
                   select item;
        }
    }

    public static class EnumerableExtentions
	{
		public static IEnumerable<T> Except<T, U, TKey> (
			IEnumerable<T> items, 
			IEnumerable<U> other, 
			Func<T, TKey> getKeyLeft,
			Func<U, TKey> getKeyRight
		)
		{
			return from item in items
			       join otherItem in other on getKeyLeft (item)
				equals getKeyRight (otherItem) into tempItems
			       from temp in tempItems.DefaultIfEmpty<U> ()
			       where ReferenceEquals (null, temp) || temp.Equals (default(U))
			       select item;
		}

		public static IEnumerable<T> Except<T, TKey> (
			IEnumerable<T> items, 
			IEnumerable<T> other, 
			Func<T, TKey> getKey)
		{
			return new NotInListHelper<T,TKey>(items,other,getKey).GetExpression();
		}

		public static IEnumerable<T> Same<T> (
			IEnumerable<T> items, 
			IEnumerable<T> other)
		{
			return Same<T,T> (items, other, x => x);
		}

		public static IEnumerable<T> Same<T, TKey> (
			IEnumerable<T> items, 
			IEnumerable<T> other, 
			Func<T, TKey> joinKey)
		{
			return from item in items
			       join otherItem in other on joinKey (item)
				equals joinKey (otherItem) into tempItems
			       from ti in tempItems
			       select ti;
		}

		public static IEnumerable<dynamic> ModifiedWithProjection<T, TKey> (
			IEnumerable<T> items, 
			IEnumerable<T> other, 
			Func<T, TKey> joinKey,
			Func<T,dynamic> projector,
			params Func<T, dynamic>[] filterKeys
		)
		{
			return from item in items
			       join otherItem in other on joinKey (item)
				equals joinKey (otherItem) into tempItems
			       from ti in tempItems
			       where buildAndFilterWhereClause (item, ti, filterKeys)
			       select projector (ti);
		}

        public static IEnumerable<T> Modified<T, TKey>(
            IEnumerable<T> items,
            IEnumerable<T> other,
            Func<T, TKey> joinKey,
            params Func<T, dynamic>[] filterKeys
        )
        {
            return from item in items
                   join otherItem in other on joinKey(item)
                equals joinKey(otherItem) into tempItems
                   from ti in tempItems
                   where buildAndFilterWhereClause(item, ti, filterKeys)
                   select ti;
        }

		private static bool buildAndFilterWhereClause<T> (T key1, T key2, params Func<T, dynamic>[] filterKeys)
		{
			bool result = true;
			if (filterKeys.Any ()) {
				foreach (var item in filterKeys) {
					result = result && item (key1) != item (key2);
				}
				return result;
			}
			return true;
		}
	}
}