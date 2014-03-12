using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NList.Core.Tests
{
	public class ForElements
	{
		public static ListElementsWrapper<T> In<T> (IEnumerable<T> source)
		{
			return new ListElementsWrapper<T> (source);
		}

		public static dynamic NotIn (List<User> source)
		{
			throw new NotImplementedException ();
		}
	}
}

