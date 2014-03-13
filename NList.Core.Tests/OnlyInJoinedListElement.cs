using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NList.Core.Tests
{
	public class OnlyInJoinedListElement<T,TKey> : JoinedListElement<T>
	{
		public Func<T,TKey> JoinKey { get; protected set; }

		public OnlyInJoinedListElement (IEnumerable<T> source, IEnumerable<T> other, Func<T,TKey> joinKey)
			: base (source, other)
		{
			JoinKey = joinKey;
		}

		protected override IEnumerable<T> definedEnumerableList ()
		{
			return EnumerableExtentions.Except (
				Other,
				Source,
				JoinKey
			);
		}
	}
	
}
