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

using Newtonsoft.Json;

namespace DustInTheWind.VeloCity.JsonFiles;

internal class JsonDatabaseDocument
{
    public JDatabaseInfo DatabaseInfo { get; set; }

    public List<JSprint> Sprints { get; set; }

    public List<JTeamMember> TeamMembers { get; set; }

    public List<JOfficialHoliday> OfficialHolidays { get; set; }

    public static JsonDatabaseDocument Parse(string json)
    {
        return JsonConvert.DeserializeObject<JsonDatabaseDocument>(json);
    }

    public string Serialize()
    {
        JsonSerializerSettings settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        return JsonConvert.SerializeObject(this, settings);
    }
}