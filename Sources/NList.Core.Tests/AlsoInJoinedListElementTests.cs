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

using NList.Core.Tests.SampleData;

namespace NList.Core.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using NFluent;
	using NUnit.Framework;

	[TestFixture]
	public class AlsoInJoinedListElementTests
	{
		[Test]
		public void when_call_alsoIn_it_stores_other_list_in_other ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.ListsOfUsers.Source);
			wrapped.AlsoIn (SampleData.ListsOfUsers.Modified);

			Check.That (wrapped.Other).IsEqualTo (SampleData.ListsOfUsers.Modified);
		}

		[Test]
		public void when_call_alsoIn_then_return_type_is_a_joinedList_of_onlyin_type ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.ListsOfUsers.Source);
			var joinList = wrapped.AlsoIn (SampleData.ListsOfUsers.Modified);

			Check.That (joinList).InheritsFrom<IEnumerable<User>> ();
			Check.That (joinList).InheritsFrom<JoinedListElement<User>> ();
			Check.That (joinList).InheritsFrom<AlsoInJoinedListElement<User,object>> ();
		}

		[Test]
		public void when_enumerate_alsoinjoinlist_it_should_be_filtered ()
		{
			var wrapped = new ListElementsWrapper<int> (new []{ 1, 2, 3 });
			var joinList = wrapped.AlsoIn (new List<int>{ 1, 3, 4 });

			Check.That (joinList).ContainsExactly (1, 3);
		}

		[Test]
		public void when_call_alsoin_with_second_parameter_it_is_the_filter_key ()
		{
			var sut = ForElements
				.In (SampleData.ListsOfUsers.Source)
				.AlsoIn (SampleData.ListsOfUsers.Modified, x => x.Id);

			Check.That (sut.Properties ("Id")).ContainsExactly (1, 3, 4);
		}
	}
}
