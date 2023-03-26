// VeloCity
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
using System.Runtime.Serialization;

namespace DustInTheWind.VeloCity.Wpf.Application.CloseSprint;

[Serializable]
internal class InternalException : Exception
{
    private static string DefaultMessage = "An unexpected internal error occurred.";

    public InternalException()
        : base(DefaultMessage)
    {
    }

    public InternalException(string message)
        : base(BuildMessage(message))
    {
    }

    public InternalException(string message, Exception innerException)
        : base(BuildMessage(message), innerException)
    {
    }

    protected InternalException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    private static string BuildMessage(string message)
    {
        return message == null
            ? DefaultMessage
            : $"{DefaultMessage} {message}";
    }
}