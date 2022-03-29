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

using System;
using System.Text;

namespace DustInTheWind.VeloCity.Domain
{
    public class PersonName : IComparable<PersonName>
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Nickname { get; set; }

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
                        sb.Append(" ");

                    sb.Append(MiddleName);
                }

                if (LastName != null)
                {
                    if (sb.Length > 0)
                        sb.Append(" ");

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
                        sb.Append(" ");

                    sb.Append(MiddleName);
                }

                if (LastName != null)
                {
                    if (sb.Length > 0)
                        sb.Append(" ");

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

        public string NickNameOrFullName => Nickname ?? FullName;

        public string ShortName => Nickname ?? FirstName ?? MiddleName ?? LastName;

        public static PersonName Parse(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            string[] parts = text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            PersonName personName = new();

            if (parts.Length == 1)
            {
                personName.FirstName = parts[0];
            }
            else if (parts.Length == 2)
            {
                personName.FirstName = parts[0];
                personName.LastName = parts[1];
            }
            else if (parts.Length == 3)
            {
                personName.FirstName = parts[0];
                personName.MiddleName = parts[1];
                personName.LastName = parts[2];
            }

            return personName;
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

        public static implicit operator PersonName(string personName)
        {
            return Parse(personName);
        }

        public static implicit operator string(PersonName personName)
        {
            return personName.ToString();
        }

        public int CompareTo(PersonName other)
        {
            if (ReferenceEquals(this, other))
                return 0;

            if (ReferenceEquals(null, other))
                return 1;

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
    }
}