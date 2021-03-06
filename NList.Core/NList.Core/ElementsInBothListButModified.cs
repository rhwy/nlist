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
using System.Linq;

namespace NList.Core
{
	using System;
	using System.Collections.Generic;

	public class ElementsInBothListButModified<T,TKey> : AlsoInJoinedListElement<T,TKey>
	{
		public IEnumerable<Func<T,dynamic>> Comparators { get; private set; }

		public ElementsInBothListButModified (
			IEnumerable<T> source, IEnumerable<T> other, Func<T,TKey> joinKey, params Func<T,dynamic>[] comparators)
			: base (source, other, joinKey)
		{
			if (!comparators.Any ())
				Comparators = new List<Func<T, dynamic>> (){ x => x };
			else {
				Comparators = new List<Func<T,dynamic>> (comparators ?? (new Func<T,dynamic>[]{ x => x }));
			}
		}

		protected override IEnumerable<T> definedEnumerableList ()
		{
			IEnumerable<T> elementsInBoth = EnumerableExtentions.Modified<T,TKey> (
				                                Source,
				                                Other,
				                                JoinKey,
				                                Comparators.ToArray ());

			return elementsInBoth;
		}
	}
}
