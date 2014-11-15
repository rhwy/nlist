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
    using System.Linq;

    public class JoinedListElement<T> : ListElementsWrapper<T>
	{
		public JoinedListElement (IEnumerable<T> source, IEnumerable<T> other)
			: base (source)
		{
			Source = source;
			Other = other;
		}
        public DoProjectionJoinedListElement<T, TProjection> Do<TProjection>(Func<T, TProjection> select, Action<T, Exception> onerror = null)
		{
            return new DoProjectionJoinedListElement<T, TProjection>(this, select, onerror);
		}

        public void Do(Action<T> doOnEachAction, Action<T, Exception> onerror = null)
        {
            Func<T,dynamic> nullSelect = (t) =>
            {
                doOnEachAction(t);
                return null;
            };
            var thenDoHelper = new  DoProjectionJoinedListElement<T,dynamic>(this, nullSelect, onerror);

            var applyEnumerable = thenDoHelper.ToList();
        }
	}
}
