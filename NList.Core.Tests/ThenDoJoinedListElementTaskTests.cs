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

			Check.ThatCode (() => joinedList.Do (_ => {
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

		[Test]
		public void eachAction_delegate_is_called_for_each_element ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.Source);
			var joinedList = wrapped.AlsoIn (SampleData.Modified) as JoinedListElement<User>;
			var nbElements = joinedList.Count ();
			var accumulationCounter = 0;

			joinedList.Do (u => accumulationCounter++);

			Check.That (accumulationCounter).IsEqualTo (nbElements);
		}
	}
	
}
