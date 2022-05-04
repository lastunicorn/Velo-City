// Velo City
// Copyright (C) 2022 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;

namespace DustInTheWind.VeloCity.Presentation.Infrastructure
{
    internal static class StringExtensions
    {
        public static IEnumerable<string> ToLowerCaseWords(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                yield break;

            int startIndex = -1;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (!char.IsLetter(c))
                {
                    if (startIndex != -1)
                    {
                        int wordLength = i - startIndex;
                        yield return text.Substring(startIndex, wordLength).ToLower();
                        startIndex = -1;
                    }
                }
                else if (char.IsUpper(c))
                {
                    if (startIndex != -1)
                    {
                        int wordLength = i - startIndex;
                        yield return text.Substring(startIndex, wordLength).ToLower();
                    }

                    startIndex = i;
                }
            }

            if (startIndex != -1)
                yield return text.Substring(startIndex).ToLower();
        }
    }
}