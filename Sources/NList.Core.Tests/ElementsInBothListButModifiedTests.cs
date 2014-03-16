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
	public class ElementsInBothListButModifiedTests
	{
		[Test]
		public void it_is_available_in_alsoin_joinedlist ()
		{
			var wrapped = new ListElementsWrapper<User> (SampleData.SampleData.Source);
			var joinList = wrapped.AlsoIn (SampleData.SampleData.Modified);
			var changed = joinList.ModifiedBy (x => x);
			Check.That (changed).IsNotEqualTo (null);
			Check.That (changed).InheritsFrom<ElementsInBothListButModified<User,object>> ();
		}

		[Test]
		public void when_call_modifiedby_without_param_it_compare_objects ()
		{
			var unmodifiedUser = new User (1, "a");
			var left = new List<User>{ unmodifiedUser, new User (2, "b") };
			var right = new List<User>{ unmodifiedUser, new User (2, "b") };
			var changed = ForElements.In (left).AlsoIn (right, x => x.Id)
				.ModifiedBy ();
			Check.That (changed.Properties ("Id")).ContainsExactly (2);
		}

		[Test]
		public void when_call_modifiedby_with_1param_it_should_compare_on_that_key ()
		{
			var left = new List<User>{ new User (1, "a"), new User (2, "b") };
			var right = new List<User>{ new User (1, "b"), new User (2, "b") };
			var changed = ForElements.In (left).AlsoIn (right, x => x.Id)
				.ModifiedBy (x => x.Name);
			Check.That (changed.Properties ("Id")).ContainsExactly (1);
		}

		[Test]
		public void when_call_modifiedby_with_2params_it_should_compare_on_both_keys ()
		{
			var left = new[]{ new {a = 1,b = 10, c = 100}, new {a = 2,b = 20, c = 200} };
			var right = new[] { 
				new {a = 1,b = 11, c = 101}, 
				new {a = 2,b = 21, c = 200}, 
				new {a = 3,b = 30, c = 300}
			};
			var modified = ForElements.In (left).AlsoIn (right, x => x.a)
				.ModifiedBy (
				               x => x.b,
				               x => x.c
			               );
			Check.That (modified.Properties ("a")).ContainsExactly (1);
		}
	}
}
