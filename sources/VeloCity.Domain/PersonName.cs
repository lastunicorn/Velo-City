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

using System.Text;

namespace DustInTheWind.VeloCity.Domain;

public readonly partial struct PersonName : IComparable<PersonName>, IEquatable<PersonName>
{
    public string FirstName { get; init; }

    public string MiddleName { get; init; }

    public string LastName { get; init; }

    public string Nickname { get; init; }

    public string FullName
    {
        get
        {
            StringBuilder sb = new();

            if (FirstName != null)
                sb.Append(FirstName);

            if (MiddleName != null)
            {
                if (sb.Length > 0)
                    sb.Append(' ');

                sb.Append(MiddleName);
            }

            if (LastName != null)
            {
                if (sb.Length > 0)
                    sb.Append(' ');

                sb.Append(LastName);
            }

            return sb.ToString();
        }
    }

    public string FullNameWithNickname
    {
        get
        {
            StringBuilder sb = new();

            if (FirstName != null)
                sb.Append(FirstName);

            if (MiddleName != null)
            {
                if (sb.Length > 0)
                    sb.Append(' ');

                sb.Append(MiddleName);
            }

            if (LastName != null)
            {
                if (sb.Length > 0)
                    sb.Append(' ');

                sb.Append(LastName);
            }

            if (Nickname != null)
            {
                string value = sb.Length > 0
                    ? $" ({Nickname})"
                    : Nickname;

                sb.Append(value);
            }

            return sb.ToString();
        }
    }

    public string ShortName => Nickname ?? FirstName ?? MiddleName ?? LastName;

    public static PersonName Parse(string text)
    {
        if (text == null) throw new ArgumentNullException(nameof(text));

        PersonNameParser parser = new(text);

        return new PersonName
        {
            FirstName = parser.FirstName,
            MiddleName = parser.MiddleName,
            LastName = parser.LastName,
            Nickname = parser.Nickname
        };
    }

    public bool Contains(string text)
    {
        return (FirstName != null && FirstName.Contains(text, StringComparison.InvariantCultureIgnoreCase)) ||
               (MiddleName != null && MiddleName.Contains(text, StringComparison.InvariantCultureIgnoreCase)) ||
               (LastName != null && LastName.Contains(text, StringComparison.InvariantCultureIgnoreCase)) ||
               (Nickname != null && Nickname.Contains(text, StringComparison.InvariantCultureIgnoreCase));
    }

    public override string ToString()
    {
        return FullNameWithNickname;
    }

    public int CompareTo(PersonName other)
    {
        int firstNameComparison = string.Compare(FirstName, other.FirstName, StringComparison.Ordinal);
        if (firstNameComparison != 0)
            return firstNameComparison;

        int middleNameComparison = string.Compare(MiddleName, other.MiddleName, StringComparison.Ordinal);
        if (middleNameComparison != 0)
            return middleNameComparison;

        int lastNameComparison = string.Compare(LastName, other.LastName, StringComparison.Ordinal);
        if (lastNameComparison != 0)
            return lastNameComparison;

        return string.Compare(Nickname, other.Nickname, StringComparison.Ordinal);
    }

    public bool Equals(PersonName other)
    {
        return FirstName == other.FirstName && MiddleName == other.MiddleName && LastName == other.LastName && Nickname == other.Nickname;
    }

    public override bool Equals(object obj)
    {
        return obj is PersonName other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FirstName, MiddleName, LastName, Nickname);
    }
}