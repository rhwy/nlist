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
	public class ThenDoJoinedListElementTaskTests
	{
		[Test]
		public void it_is_available_on_joinedLists ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.Source);
			var joinedList = wrapped.OnlyIn (SampleData.Modified) as JoinedListElement<User>;

			Check.ThatCode (() => joinedList.Do<User> (_ => {
			})).DoesNotThrow ();
		}

		[Test]
		public void it_can_accept_a_delegate_defining_each_element_action ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.Source);
			var joinedList = wrapped.OnlyIn (SampleData.Modified) as JoinedListElement<User>;
			Action<User> onEachAction = (user) => {
				Check.That (SampleData.Source).Contains (user);
			};

			Check.ThatCode (() => joinedList.Do (onEachAction)).DoesNotThrow ();
		}
	}

	[TestFixture]
	public class OnlyInJoinedListElementTests
	{
		[Test]
		public void when_call_OnlyIn_it_stores_other_list_in_other ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.Source);
			wrapped.OnlyIn (SampleData.Modified);

			Check.That (wrapped.Other).IsEqualTo (SampleData.Modified);
		}

		[Test]
		public void when_call_OnlyIn_then_return_type_is_a_joinedList_of_onlyin_type ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.Source);
			var joinList = wrapped.OnlyIn (SampleData.Modified);

			Check.That (joinList).InheritsFrom<IEnumerable<User>> ();
			Check.That (joinList).InheritsFrom<JoinedListElement<User>> ();
			Check.That (joinList).InheritsFrom<OnlyInJoinedListElement<User,object>> ();
		}

		[Test]
		public void when_enumerate_onlyinjoinlist_it_should_be_filtered ()
		{
			var wrapped = new ListElementsWrapper<int> (new []{ 1, 2, 3 });
			var joinList = wrapped.OnlyIn (new List<int>{ 1, 3, 4 });

			Check.That (joinList).ContainsExactly (4);
		}

		[Test]
		public void when_call_onlyin_with_second_parameter_it_is_the_filter_key ()
		{
			var sut = ForElements
				.In (SampleData.Source)
				.OnlyIn (SampleData.Modified, x => x.Id);

			Check.That (sut.Properties ("Id")).ContainsExactly (5);
		}
	}
}
