using System;
using System.Linq;
using System.Collections.Generic;

namespace NList.Core
{
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
			return from item in items
			       join otherItem in other on getKey (item)
				equals getKey (otherItem) into tempItems
			       from temp in tempItems.DefaultIfEmpty<T> ()
			       where ReferenceEquals (null, temp) || temp.Equals (default(T))
			       select item;
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
			Func<T, TKey> getKey)
		{
			return from item in items
			       join otherItem in other on getKey (item)
				equals getKey (otherItem) into tempItems
			       from ti in tempItems
			       select ti;
		}
	}
}
