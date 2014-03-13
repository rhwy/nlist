using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NList.Core.Tests
{
	public class NotInJoinedListElementScalar<T> : NotInJoinedListElement<T,object>
	{
		public NotInJoinedListElementScalar (IEnumerable<T> source, IEnumerable<T> other)
			: base (source, other, x => x)
		{

		}
	}

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

	public class JoinedListElement<T> : ListElementsWrapper<T>
	{
		public JoinedListElement (IEnumerable<T> source, IEnumerable<T> other)
			: base (source)
		{
			Source = source;
			Other = other;
		}
	}

	public class ListElementsWrapper<T> : IEnumerable<T>
	{
		public IEnumerable<T> Source { get; protected set; }

		public IEnumerable<T> Other { get; protected set; }

		public ListElementsWrapper (IEnumerable<T> source)
		{
			Source = source;
		}

		public JoinedListElement<T> NotIn (List<T> other)
		{
			Other = other;
			return new NotInJoinedListElementScalar<T> (Source, Other);
		}

		public JoinedListElement<T> NotIn<Tkey> (List<T> other, Func<T,Tkey> key)
		{
			Other = other;
			return new NotInJoinedListElement<T,Tkey> (Source, Other, key);
		}

		protected virtual IEnumerable<T> definedEnumerableList ()
		{
			return Source;
		}

		#region IEnumerable implementation

		public IEnumerator<T> GetEnumerator ()
		{
			return definedEnumerableList ().GetEnumerator ();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		#endregion

	}
}
