using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using NFluent;

namespace NList.Core.Tests
{

	[TestFixture]
	public class EnumerableExtensionsTests
	{
		[Test]
		public void it_can_filter_lists_with_different_types ()
		{
			var left = SampleData.Source;
			var right = SampleData.Modified.Select (x => x.Id);

			var elementsFromLeftNotInRight = 
				EnumerableExtentions.Except (
					left,
					right,
					x => x.Id,
					x => x
				);

			Check.That (elementsFromLeftNotInRight.Properties ("Id")).ContainsExactly (2);
		}
	}

}
