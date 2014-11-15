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
    using System.Collections.Generic;
    using System.Linq;
    using NFluent;
    using NUnit.Framework;
    using ReList.Core.Enumerables;
    using SampleData;

    [TestFixture]
    public class NotInListHelperWithSameTypeTests
    {
        protected IEnumerable<User> Left;
        protected IEnumerable<User> Right;

        [SetUp]
        public void when_we_have_two_list_of_the_same_type()
        {
            Left = ListsOfUsers.Source;
            Right = ListsOfUsers.Modified;
        }

        [Test]
        public void ensure_it_is_base_contract_with_same_types()
        {
            var sut = new NotInListHelper<User,User>(Left, Right);
            Check.That(sut).InheritsFrom<INotInListHelper<User, User,User>>();
        }
        [Test]
        public void when_ctor_properties_are_set()
        {
            Func<User, int> selector = x => x.Id;
            var sut = new NotInListHelper<User,int>(Left, Right, selector);
            Check.That(sut).IsNotNull();
            Check.That(sut.Items).IsEqualTo(Left);
            Check.That(sut.Other).IsEqualTo(Right);
            Check.That(sut.JoinItemsKey).IsEqualTo(selector);
        }

        [Test]
        public void when_ctor_with_no_selector_then_it_uses_idendity_selector()
        {
            var sut = new NotInListHelper<User,User>(Left, Right);
            Func<User, User> identitySelector = x => x;
            Check.That(sut).IsNotNull();
			Check.That(sut.JoinItemsKey.ToString()).IsEqualTo(identitySelector.ToString());
        }


        [Test]
        public void it_can_filter_lists_with_same_types()
        {
            
            var elementsFromLeftNotInRight =
                EnumerableExtentions.Except(
                    Left,
                    Right,
                    x => x.Id
                );

            Check.That(elementsFromLeftNotInRight.Properties("Id")).ContainsExactly(2);
        }

        [Test]
        public void it_can_filter_lists_with_different_types()
        {
            var elementsFromLeftNotInRight =
                EnumerableExtentions.Except<User,int,int>(
                    Left,
                    Right.Select(x=>x.Id),
                    x => x.Id,
                    x => x
                );

            Check.That(elementsFromLeftNotInRight.Properties("Id")).ContainsExactly(2);
        }

    }

	[TestFixture]
	public class EnumerableExtensionsTests
	{
		
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

		[Test]
		public void it_can_define_a_projection_function_for_modified ()
		{
			var left = new[]{ new {a = 1,b = 10}, new {a = 2,b = 20} };
			var right = new[]{ new {a = 1,b = 10}, new {a = 2,b = 21}, new {a = 3,b = 30} };

			var elementsModified = 
				EnumerableExtentions.ModifiedWithProjection (
					left,
					right,
					x => x.a, //checks for existence on this key
					x => (x.b * 100).ToString (),  //define the projection
                    x => x.b //once item is present, it uses this key to compare
				);

			Check.That (elementsModified).ContainsExactly ("2100");

		}

		[Test]
		public void it_can_get_elements_in_both_lists_and_look_for_one_difference ()
		{
			var left = new[]{ new {a = 1,b = 10}, new {a = 2,b = 20} };
			var right = new[]{ new {a = 1,b = 10}, new {a = 2,b = 21}, new {a = 3,b = 30} };

			var elementsModified = 
				EnumerableExtentions.Modified (
					left,
					right,
					x => x.a, //checks for existence on this key
					x => x.b  //once item is present, it uses this key to compare
				);
			//then here only a1 and a2 are present in both, but only a2 has it's b key modified

			Check.That (elementsModified.Properties ("a")).ContainsExactly (2);
		}

		[Test]
		public void it_can_get_elements_in_both_lists_and_look_for_two_differences ()
		{
			var left = new[]{ new {a = 1,b = 10, c = 100}, new {a = 2,b = 20, c = 200} };
			var right = new[] { 
				new {a = 1,b = 11, c = 101}, 
				new {a = 2,b = 21, c = 200}, 
				new {a = 3,b = 30, c = 300}
			};

			var elementsModified = 
				EnumerableExtentions.ModifiedWithProjection (
					left,
					right,
					x => x.a, //checks for existence on this key
					//once item is present, you can look for other properties:
					x => x.a,
					x => x.b, 
					x => x.c  
				);
			//then here, only a1 and a2 are present in both lists, 
			//a1 and a2 have their b key modified but only a1 has b and c keys modified

			Check.That (elementsModified).ContainsExactly (1);
		}
	}
}