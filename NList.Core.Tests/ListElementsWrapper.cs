using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NList.Core.Tests
{
	public class NotInJoinedListElement<T> : JoinedListElement<T>
	{
		public NotInJoinedListElement (IEnumerable<T> source, IEnumerable<T> other)
			: base (source, other)
		{
			
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
			return new NotInJoinedListElement<T> (Source, Other);
		}

		#region IEnumerable implementation

		public IEnumerator<T> GetEnumerator ()
		{
			return Source.GetEnumerator ();
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
