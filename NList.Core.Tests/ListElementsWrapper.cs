using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NList.Core.Tests
{

	public class ListElementsWrapper<T> : IEnumerable<T>
	{
		public IEnumerable<T> Source { get; private set; }

		public ListElementsWrapper (IEnumerable<T> source)
		{
			Source = source;
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
