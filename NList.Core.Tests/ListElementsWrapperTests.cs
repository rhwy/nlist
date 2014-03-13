using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using NFluent;

namespace NList.Core.Tests
{
	[TestFixture]
	public class ListElementsWrapperTests
	{
		[Test]
		public void it_is_enumerable ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.Source);
			Check.That (wrapped).InheritsFrom<IEnumerable<User>> ();
		}

		[Test]
		public void when_enumerate_it_should_be_equal_to_source ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.Source);
			var enumerated = (wrapped as IEnumerable<User>).ToList ();

			Check.That (enumerated).ContainsExactly (SampleData.Source);
		}
	}
}

