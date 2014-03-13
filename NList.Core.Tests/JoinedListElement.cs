using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NList.Core.Tests
{

	public class JoinedListElement<T> : ListElementsWrapper<T>
	{
		public JoinedListElement (IEnumerable<T> source, IEnumerable<T> other)
			: base (source)
		{
			Source = source;
			Other = other;
		}
	}
	
}