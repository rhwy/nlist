using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NList.Core.Tests
{
	public class OnlyInJoinedListElementScalar<T> : OnlyInJoinedListElement<T,object>
	{
		public OnlyInJoinedListElementScalar (IEnumerable<T> source, IEnumerable<T> other)
			: base (source, other, x => x)
		{

		}
	}
	
}
