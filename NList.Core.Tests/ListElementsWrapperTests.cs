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

		[Test]
		public void when_call_NotIn_it_stores_other_list_in_other ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.Source);
			wrapped.NotIn (SampleData.Modified);

			Check.That (wrapped.Other).IsEqualTo (SampleData.Modified);
		}

		[Test]
		public void when_call_NotIn_then_return_type_is_a_joinedList_of_notin_type ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.Source);
			var joinList = wrapped.NotIn (SampleData.Modified);

			Check.That (joinList).InheritsFrom<IEnumerable<User>> ();
			Check.That (joinList).InheritsFrom<JoinedListElement<User>> ();
			Check.That (joinList).InheritsFrom<NotInJoinedListElement<User,object>> ();
		}

		[Test]
		public void when_enumerate_notinjoinlist_it_should_be_filtered ()
		{
			var wrapped = new ListElementsWrapper<int> (new []{ 1, 2, 3 });
			var joinList = wrapped.NotIn (new List<int>{ 1, 3 });

			Check.That (joinList).ContainsExactly (2);
		}

		[Test]
		public void when_call_notin_with_second_parameter_it_is_the_filter_key ()
		{
			var sut = ForElements
				.In (SampleData.Source)
				.NotIn (SampleData.Modified, x => x.Id);

			Check.That (sut.Properties ("Id")).ContainsExactly (2);
		}

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

