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
    using System.Collections;
    using System.Collections.Generic;

    public class ThenDoJoinedListElement<T> : IEnumerable<T>
    {
        public IEnumerable<T> PreviousDoJoinedListElementValue { get; set; }

        public ThenDoJoinedListElement(
            IEnumerable<T> previousDoJoinedListElement, 
            Func<T, dynamic> actionOnEach, 
            Action<T, Exception> errorActionOnEach = null)
           
        {
            PreviousDoJoinedListElementValue = previousDoJoinedListElement;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return PreviousDoJoinedListElementValue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class DoProjectionJoinedListElement<TSource,TProjection> : IEnumerable<TProjection>
    {
        public JoinedListElement<TSource> JoinedSource { get; private set; }
        public Func<TSource, TProjection> ActionOnEach { get; private set; }
        public Action<TSource, Exception> ErrorActionOnEach { get; private set; }

        public DoProjectionJoinedListElement(
            JoinedListElement<TSource> joinedListSource,
            Func<TSource, TProjection> actionOnEach,
            Action<TSource, Exception> errorActionOnEach = null)
           
        {
            JoinedSource = joinedListSource;
            ActionOnEach = actionOnEach;
            ErrorActionOnEach = errorActionOnEach;
        }


        protected virtual IEnumerable<TProjection> DefinedEnumerableList()
        {
            //Func<TSource, TProjection> transformOrNull = (s) => (TProjection)Convert.ChangeType(s,typeof(TProjection));
            return 
                JoinedSource
                .Select(element => SafeExecute(element, ActionOnEach, ErrorActionOnEach))
                .Where(current => current != null);
        }

        public ThenDoJoinedListElement<TProjection> Then(Func<TProjection, dynamic> select, Action<TProjection, Exception> onerror = null)
        {
            var currentList = this.ToList();
            return new ThenDoJoinedListElement<TProjection>(currentList,select,onerror);
            
        }
        public ThenDoJoinedListElement<TProjection> Then(Action<TProjection> select, Action<TProjection, Exception> onerror = null)
        {
            var currentList = this.ToList();
            Func<TProjection, dynamic> voidFunctionWrapper = (item) =>
            {
                select(item);
                return null;
            };
            return new ThenDoJoinedListElement<TProjection>(currentList, voidFunctionWrapper, onerror);
        }

        protected TProjection SafeExecute(TSource item, Func<TSource, TProjection> actionOnEach, Action<TSource, Exception> errorActionOnEach = null)
        {
            if (errorActionOnEach != null)
            {
                dynamic validResult = null;
                try
                {
                    validResult = actionOnEach(item);
                }
                catch (Exception ex)
                {
                    errorActionOnEach(item, ex);
                }
                return validResult;
            }
            return actionOnEach(item);
        }


        public IEnumerator<TProjection> GetEnumerator()
        {
            return DefinedEnumerableList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}