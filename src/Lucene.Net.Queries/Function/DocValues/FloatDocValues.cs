﻿using Lucene.Net.Util.Mutable;
using System;

namespace Lucene.Net.Queries.Function.DocValues
{
    /*
     * Licensed to the Apache Software Foundation (ASF) under one or more
     * contributor license agreements.  See the NOTICE file distributed with
     * this work for additional information regarding copyright ownership.
     * The ASF licenses this file to You under the Apache License, Version 2.0
     * (the "License"); you may not use this file except in compliance with
     * the License.  You may obtain a copy of the License at
     *
     *     http://www.apache.org/licenses/LICENSE-2.0
     *
     * Unless required by applicable law or agreed to in writing, software
     * distributed under the License is distributed on an "AS IS" BASIS,
     * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     * See the License for the specific language governing permissions and
     * limitations under the License.
     */

    /// <summary>
    /// Abstract <see cref="FunctionValues"/> implementation which supports retrieving <see cref="float"/> values.
    /// Implementations can control how the <see cref="float"/> values are loaded through <see cref="FloatVal(int)"/>
    /// </summary>
    public abstract class FloatDocValues : FunctionValues
    {
        protected readonly ValueSource m_vs;

        public FloatDocValues(ValueSource vs)
        {
            this.m_vs = vs;
        }

        public override byte ByteVal(int doc)
        {
            return (byte)FloatVal(doc);
        }

        public override short ShortVal(int doc)
        {
            return (short)FloatVal(doc);
        }

        public override abstract float FloatVal(int doc);

        public override int IntVal(int doc)
        {
            return (int)FloatVal(doc);
        }

        public override long LongVal(int doc)
        {
            return (long)FloatVal(doc);
        }

        public override double DoubleVal(int doc)
        {
            return (double)FloatVal(doc);
        }

        public override string StrVal(int doc)
        {
            return Convert.ToString(FloatVal(doc));
        }

        public override object ObjectVal(int doc)
        {
            return Exists(doc) ? FloatVal(doc) : (float?)null;
        }

        public override string ToString(int doc)
        {
            return m_vs.GetDescription() + '=' + StrVal(doc);
        }

        public override ValueFiller GetValueFiller()
        {
            return new ValueFillerAnonymousInnerClassHelper(this);
        }

        private class ValueFillerAnonymousInnerClassHelper : ValueFiller
        {
            private readonly FloatDocValues outerInstance;

            public ValueFillerAnonymousInnerClassHelper(FloatDocValues outerInstance)
            {
                this.outerInstance = outerInstance;
                mval = new MutableValueFloat();
            }

            private readonly MutableValueFloat mval;

            public override MutableValue Value
            {
                get
                {
                    return mval;
                }
            }

            public override void FillValue(int doc)
            {
                mval.Value = outerInstance.FloatVal(doc);
                mval.Exists = outerInstance.Exists(doc);
            }
        }
    }
}