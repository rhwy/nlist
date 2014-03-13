using System;
using System.Collections.Generic;

namespace NList.Core
{
	public class NotInJoinedListElementScalar<T> : NotInJoinedListElement<T,object>
	{
		public NotInJoinedListElementScalar (IEnumerable<T> source, IEnumerable<T> other)
			: base (source, other, x => x)
		{

		}
	}
}
