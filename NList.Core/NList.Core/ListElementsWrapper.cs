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

using System;
using System.Collections.Generic;

namespace NList.Core
{
	public class ListElementsWrapper<T> : IEnumerable<T>
	{
		public IEnumerable<T> Source { get; protected set; }

		public IEnumerable<T> Other { get; protected set; }

		public ListElementsWrapper (IEnumerable<T> source)
		{
			Source = source;
		}

		public JoinedListElement<T> NotIn (List<T> other)
		{
			Other = other;
			return new NotInJoinedListElementScalar<T> (Source, Other);
		}

		public JoinedListElement<T> NotIn<Tkey> (List<T> other, Func<T,Tkey> key)
		{
			Other = other;
			return new NotInJoinedListElement<T,Tkey> (Source, Other, key);
		}

		public JoinedListElement<T> OnlyIn (List<T> other)
		{
			Other = other;
			return new OnlyInJoinedListElementScalar<T> (Source, Other);
		}

		public JoinedListElement<T> OnlyIn<Tkey> (List<T> other, Func<T,Tkey> key)
		{
			Other = other;
			return new OnlyInJoinedListElement<T,Tkey> (Source, Other, key);
		}

		public JoinedListElement<T> AlsoIn (List<T> other)
		{
			Other = other;
			return new AlsoInJoinedListElementScalar<T> (Source, Other);
			;
		}

		public JoinedListElement<T> AlsoIn<Tkey> (List<T> other, Func<T,Tkey> key)
		{
			Other = other;
			return new AlsoInJoinedListElement<T,Tkey> (Source, Other, key);
		}

		protected virtual IEnumerable<T> definedEnumerableList ()
		{
			return Source;
		}

		#region IEnumerable implementation

		public IEnumerator<T> GetEnumerator ()
		{
			return definedEnumerableList ().GetEnumerator ();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		#endregion

	}
}
