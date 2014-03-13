using System;
using System.Collections.Generic;

namespace NList.Core
{
	public class NotInJoinedListElement<T,TKey> : JoinedListElement<T>
	{
		public Func<T,TKey> JoinKey { get; protected set; }

		public NotInJoinedListElement (IEnumerable<T> source, IEnumerable<T> other, Func<T,TKey> joinKey)
			: base (source, other)
		{
			JoinKey = joinKey;
		}

		protected override IEnumerable<T> definedEnumerableList ()
		{
			return EnumerableExtentions.Except (
				Source,
				Other,
				JoinKey
			);
		}
	}
}
