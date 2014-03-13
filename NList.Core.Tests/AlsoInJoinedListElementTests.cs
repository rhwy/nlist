using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using NFluent;

namespace NList.Core.Tests
{
	[TestFixture]
	public class AlsoInJoinedListElementTests
	{
		[Test]
		public void when_call_alsoIn_it_stores_other_list_in_other ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.Source);
			wrapped.AlsoIn (SampleData.Modified);

			Check.That (wrapped.Other).IsEqualTo (SampleData.Modified);
		}

		[Test]
		public void when_call_alsoIn_then_return_type_is_a_joinedList_of_onlyin_type ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.Source);
			var joinList = wrapped.AlsoIn (SampleData.Modified);

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
				.In (SampleData.Source)
				.AlsoIn (SampleData.Modified, x => x.Id);

			Check.That (sut.Properties ("Id")).ContainsExactly (1, 3, 4);
		}
	}
}
