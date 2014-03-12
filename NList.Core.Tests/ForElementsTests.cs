using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using NFluent;

namespace NList.Core.Tests
{

	[TestFixture]
	public class ForElementsTests
	{
		[Test]
		public void when_call_In_it_returns_a_wrapper_of_the_source ()
		{
			var wrapped = ForElements.In (SampleData.Source);

			Check.That (wrapped).IsNotNull ();
			Check.That (wrapped).IsInstanceOf<ListElementsWrapper<User>> ();
		}

		[Test]
		public void when_call_In_it_set_Source_property_with_source ()
		{
			var wrapped = ForElements.In (SampleData.Source);
			Check.That (wrapped.Source).IsEqualTo (SampleData.Source);
		}
	}

}
