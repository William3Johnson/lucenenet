﻿using System;
using System.Text;

namespace Lucene.Net.Support
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Causes this character sequence to be replaced by the reverse of
        /// the sequence. If there are any surrogate pairs included in the
        /// sequence, these are treated as single characters for the
        /// reverse operation. Thus, the order of the high-low surrogates
        /// is never reversed.
        /// <para/>
        /// Let <c>n</c> be the character length of this character sequence
        /// (not the length in <see cref="char"/> values) just prior to
        /// execution of the <see cref="Reverse"/> method. Then the
        /// character at index <c>k</c> in the new character sequence is
        /// equal to the character at index <c>n-k-1</c> in the old
        /// character sequence.
        /// <para/>
        /// Note that the reverse operation may result in producing
        /// surrogate pairs that were unpaired low-surrogates and
        /// high-surrogates before the operation. For example, reversing
        /// "&#92;uDC00&#92;uD800" produces "&#92;uD800&#92;uDC00" which is
        /// a valid surrogate pair.
        /// </summary>
        /// <param name="text">this <see cref="StringBuilder"/></param>
        /// <returns>a reference to this <see cref="StringBuilder"/>.</returns>
        public static StringBuilder Reverse(this StringBuilder text)
        {
            bool hasSurrogate = false;
            int codePointCount = text.Length;
            int n = text.Length - 1;
            for (int j = (n - 1) >> 1; j >= 0; --j)
            {
                char temp = text[j];
                char temp2 = text[n - j];
                if (!hasSurrogate)
                {
                    hasSurrogate = (temp >= Character.MIN_SURROGATE && temp <= Character.MAX_SURROGATE)
                        || (temp2 >= Character.MIN_SURROGATE && temp2 <= Character.MAX_SURROGATE);
                }
                text[j] = temp2;
                text[n - j] = temp;
            }
            if (hasSurrogate)
            {
                // Reverse back all valid surrogate pairs
                for (int i = 0; i < text.Length - 1; i++)
                {
                    char c2 = text[i];
                    if (char.IsLowSurrogate(c2))
                    {
                        char c1 = text[i + 1];
                        if (char.IsHighSurrogate(c1))
                        {
                            text[i++] = c1;
                            text[i] = c2;
                        }
                    }
                }
            }

            return text;
        }

        /// <summary>
        /// Returns the number of Unicode code points in the specified text
        /// range of this <see cref="StringBuilder"/>. The text range begins at the specified
        /// <paramref name="beginIndex"/> and extends to the <see cref="char"/> at
        /// index <c>endIndex - 1</c>. Thus the length (in
        /// <see cref="char"/>s) of the text range is
        /// <c>endIndex-beginIndex</c>. Unpaired surrogates within
        /// this sequence count as one code point each.
        /// </summary>
        /// <param name="text">this <see cref="StringBuilder"/></param>
        /// <param name="beginIndex">the index to the first <see cref="char"/> of the text range.</param>
        /// <param name="endIndex">the index after the last <see cref="char"/> of the text range.</param>
        /// <returns>the number of Unicode code points in the specified text range.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// if the <paramref name="beginIndex"/> is negative, or <paramref name="endIndex"/>
        /// is larger than the length of this sequence, or
        /// <paramref name="beginIndex"/> is larger than <paramref name="endIndex"/>.
        /// </exception>
        public static int CodePointCount(this StringBuilder text, int beginIndex, int endIndex)
        {
            if (beginIndex < 0 || endIndex > text.Length || beginIndex > endIndex)
            {
                throw new IndexOutOfRangeException();
            }
            return Character.CodePointCountImpl(text.GetChars(), beginIndex, endIndex - beginIndex);
        }

        /// <summary>
        /// Copies the array from the <see cref="StringBuilder"/> into a new array
        /// and returns it.
        /// </summary>
        /// <param name="text">this <see cref="StringBuilder"/></param>
        /// <returns></returns>
        public static char[] GetChars(this StringBuilder text)
        {
            char[] chars = new char[text.Length];
            text.CopyTo(0, chars, 0, text.Length);
            return chars;
        }

        /// <summary>
        /// Appends the string representation of the <paramref name="codePoint"/>
        /// argument to this sequence.
        /// 
        /// <para>
        /// The argument is appended to the contents of this sequence.
        /// The length of this sequence increases by <see cref="Character.CharCount(int)"/>.
        /// </para>
        /// <para>
        /// The overall effect is exactly as if the argument were
        /// converted to a <see cref="char"/> array by the method
        /// <see cref="Character.ToChars(int)"/> and the character in that array
        /// were then <see cref="StringBuilder.Append(char[])">appended</see> to this 
        /// <see cref="StringBuilder"/>.
        /// </para>
        /// </summary>
        /// <param name="text">This <see cref="StringBuilder"/>.</param>
        /// <param name="codePoint">a Unicode code point</param>
        /// <returns>a reference to this object.</returns>
        public static StringBuilder AppendCodePoint(this StringBuilder text, int codePoint)
        {
            text.Append(Character.ToChars(codePoint));
            return text;
        }
    }
}