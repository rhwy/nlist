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

namespace ReList.Core.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using NFluent;
    using NUnit.Framework;
    using ReList.Core;
    using SampleData;

    [TestFixture]
	public class ThenDoJoinedListElementTaskTests
	{
		[Test]
		public void it_is_available_on_joinedLists ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.ListsOfUsers.Source);
			var joinedList = wrapped.OnlyIn (SampleData.ListsOfUsers.Modified) as JoinedListElement<User>;

			Check.ThatCode (() => joinedList.Do (_ => 1)).DoesNotThrow ();
		}

		[Test]
		public void it_can_accept_a_delegate_defining_each_element_action ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.ListsOfUsers.Source);
			var joinedList = wrapped.OnlyIn (SampleData.ListsOfUsers.Modified) as JoinedListElement<User>;

			Action<User> onEachAction = (user) => Check.That (SampleData.ListsOfUsers.Modified).Contains (user);

			Check.ThatCode (() => joinedList.Do (onEachAction)).DoesNotThrow ();
		}

		[Test]
		public void eachAction_delegate_is_called_for_each_element ()
		{
			JoinedListElement<User> joinedList = ForElements
				.In (SampleData.ListsOfUsers.Source)
				.AlsoIn (SampleData.ListsOfUsers.Modified, x => x.Id);

			var nbElements = joinedList.Count ();
			var accumulationCounter = 0;

			joinedList.Do (u => {
				accumulationCounter++;
			});

			Check.That (accumulationCounter).IsEqualTo (nbElements);
		}

		[Test]
		public void each_element_of_joinedList_is_passed_to_eachAction_delegate ()
		{
			JoinedListElement<User> joinedList = ForElements
				.In (SampleData.ListsOfUsers.Source)
				.AlsoIn (SampleData.ListsOfUsers.Modified, x => x.Id);

			var accumulator = "";

			joinedList.Do (u => {
				if (!string.IsNullOrEmpty (accumulator))
					accumulator += ",";
				accumulator += u.Id;
			});

			Check.That (accumulator).IsEqualTo ("1,3,4");
		}

		[Test]
		public void when_delegate_on_Do_action_is_function_then_it_returns_a_list ()
		{
			JoinedListElement<User> joinedList = ForElements
				.In (SampleData.ListsOfUsers.Source)
				.AlsoIn (SampleData.ListsOfUsers.Modified, x => x.Id);

			var reduced = joinedList.Do (u => u.Id);
			Check.That (reduced as IEnumerable).IsNotNull ();
			Check.That (reduced).ContainsExactly (1, 3, 4);
		    Check.That(reduced).IsInstanceOf<DoProjectionJoinedListElement<User,int>>();
		}

		[Test]
		public void the_second_method_provided_is_the_error_callback ()
		{
			JoinedListElement<User> joinedList = ForElements
				.In (SampleData.ListsOfUsers.Source)
				.AlsoIn (SampleData.ListsOfUsers.Modified, x => x.Id);

			var errors = new List<string> ();

			joinedList.Do (
				user => {
					if (user.Id == 3 || user.Id == 4)
						throw new Exception ("do");
				},
				(user, error) => errors.Add (error.Message));

			Check.That (errors).HasSize (2);
			Check.That (string.Join ("", errors)).IsEqualTo ("dodo");
		}

		[Test]
		public void when_error_callback_provided_with_function_selector_in_do_it_returns_only_succeeded_items ()
		{

			
		    Func<User, dynamic> projection = user =>
		    {
		        if (user.Id == 3 || user.Id == 4)
		            throw new Exception("do");

		        return user.Id;
		    };
            var errors = new List<string>();
            List<User> listOfFailedElements = new List<User>();

		    Action<User, Exception> onError = (user, error) =>
		    {
		        errors.Add(error.Message);
                listOfFailedElements.Add(user);
		    };

			var filtered = ForElements
				.In (SampleData.ListsOfUsers.Source)
				.AlsoIn(SampleData.ListsOfUsers.Modified, x => x.Id)
                .Do (
			        @select: projection,
                    onerror: onError);

		    var sut = filtered.ToList();
		    Check.That (sut).IsNotNull ();
			Check.That (sut).HasSize (1);
			Check.That (errors).HasSize (2);
			Check.That (string.Join (",", errors)).IsEqualTo ("do,do");
            Check.That(listOfFailedElements).HasSize(2);
            Check.That(listOfFailedElements.Properties("Id")).ContainsExactly(3,4);
		}

	    [Test]
	    public void it_has_a_then_method_to_chain_thenDolists()
	    {
	        Func<User, dynamic> getOnlyEven = user =>
		    {
		        if (user.Id % 2 != 0)
		            throw new Exception("notEven!");

		        return user.Id;
		    };
            var errors = new List<string>();
            var listOfFailedElements = new List<User>();

		    Action<User, Exception> onError = (user, error) =>
		    {
		        errors.Add(error.Message);
                listOfFailedElements.Add(user);
		    };

			var filtered = ForElements
				.In (SampleData.ListsOfUsers.Source)
				.AlsoIn(SampleData.ListsOfUsers.Modified, x => x.Id)
                .Do (
                    @select: getOnlyEven,
                    onerror: onError)
                .Then((u) =>
                {
                    Check.That(u).IsEqualTo(4);
                });

	        Check.That(filtered).ContainsExactly(4);
	        Check.That(listOfFailedElements.Properties("Id")).ContainsExactly(1, 3);
	    }
	}
}
