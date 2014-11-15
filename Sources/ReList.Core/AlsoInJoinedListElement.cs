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

namespace ReList.Core
{
    using System;
    using System.Collections.Generic;
    using Enumerables;

    public class AlsoInJoinedListElement<T,TKey> : JoinedListElement<T>
	{
		public Func<T,TKey> JoinKey { get; protected set; }

		public AlsoInJoinedListElement (IEnumerable<T> source, IEnumerable<T> other, Func<T,TKey> joinKey)
			: base (source, other)
		{
			JoinKey = joinKey;
		}

		public ElementsInBothListButModified<T,TKey> ModifiedBy (params Func<T,dynamic>[] comparators)
		{
			return new ElementsInBothListButModified<T,TKey> (Source, Other, JoinKey, comparators);
		}

		protected override IEnumerable<T> definedEnumerableList ()
		{
			return EnumerableExtentions.Same (
				Source,
				Other,
				JoinKey
			);
		}
	}
}