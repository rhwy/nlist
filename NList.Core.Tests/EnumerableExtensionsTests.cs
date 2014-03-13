// The MIT License (MIT)
//
// Copyright (c) 2014 Rui Carvalho
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
//	The above copyright notice and this permission notice shall be included in all
//	copies or substantial portions of the Software.
//
//	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//	SOFTWARE.
// ==============================================================================
namespace NList.Core.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using NFluent;
	using NUnit.Framework;

	[TestFixture]
	public class EnumerableExtensionsTests
	{
		[Test]
		public void it_can_filter_lists_with_same_types ()
		{
			var left = SampleData.Source;
			var right = SampleData.Modified;

			var elementsFromLeftNotInRight = 
				EnumerableExtentions.Except (
					left,
					right,
					x => x.Id
				);

			Check.That (elementsFromLeftNotInRight.Properties ("Id")).ContainsExactly (2);
		}

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

		[Test]
		public void it_can_get_elements_of_same_type_in_both_lists ()
		{
			var left = new[]{ 1, 2, 3 };
			var right = new[]{ 2, 3, 4 };

			var elementsInBothLists = 
				EnumerableExtentions.Same (
					left,
					right);

			Check.That (elementsInBothLists).ContainsExactly (2, 3);
		}
	}
}
