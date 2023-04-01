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

using DustInTheWind.VeloCity.Ports.DataAccess;

namespace DustInTheWind.VeloCity.JsonFiles;

public class TooBigDatabaseVersionException : DataAccessException
{
    private const string DefaultMessage = "The database json file in newer than expected. Please upgrade the VeloCity application. Actual database version: {0}. Current version of VeloCity works with database version: {1}.";

    public TooBigDatabaseVersionException(Version actualVersion, Version expectedVersion)
        : base(string.Format(DefaultMessage, actualVersion, expectedVersion))
    {
    }
}