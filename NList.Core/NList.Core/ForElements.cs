using System;
using System.Collections.Generic;

namespace NList.Core
{
	public class ForElements
	{
		public static ListElementsWrapper<T> In<T> (IEnumerable<T> source)
		{
			return new ListElementsWrapper<T> (source);
		}

		public static dynamic NotIn<T> (List<T> source)
		{
			throw new NotImplementedException ();
		}
	}
}

