using System;
using System.Collections.Generic;

namespace NList.Core
{
	public class AlsoInJoinedListElementScalar<T> : AlsoInJoinedListElement<T,object>
	{
		public AlsoInJoinedListElementScalar (IEnumerable<T> source, IEnumerable<T> other)
			: base (source, other, x => x)
		{

		}
	}
}
