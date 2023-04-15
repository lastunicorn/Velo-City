// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

using System.Text.RegularExpressions;

namespace DustInTheWind.VeloCity.Domain;

internal class PersonNameParser
{
    private static readonly Regex Regex = new(@"^\s*(\w*)\s*(.*?)\s*(\w*)\s*(?:\((.*)\))?\s*$", RegexOptions.Multiline);
    
    private readonly Match match;

    public string FirstName
    {
        get
        {
            string value = match.Groups[1].Value;

            return string.IsNullOrEmpty(value)
                ? null
                : value;
        }
    }

    public string MiddleName
    {
        get
        {
            string value = match.Groups[2].Value;

            return string.IsNullOrEmpty(value)
                ? null
                : value;
        }
    }

    public string LastName
    {
        get
        {
            string value = match.Groups[3].Value;

            return string.IsNullOrEmpty(value)
                ? null
                : value;
        }
    }

    public string Nickname
    {
        get
        {
            string value = match.Groups[4].Value;

            return string.IsNullOrEmpty(value)
                ? null
                : value;
        }
    }

    public PersonNameParser(string text)
    {
        match = Regex.Match(text);
    }
}