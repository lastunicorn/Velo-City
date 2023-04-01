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

namespace DustInTheWind.VeloCity.Cli.Presentation.UserControls;

internal class WorkDaysControl
{
    public List<DateTime> Days { get; set; }

    public void Display()
    {
        Console.WriteLine($"Work Days: {Days.Count} days");

        for (int i = 0; i < Days.Count; i++)
        {
            DateTime dateTime = Days[i];
            int dayIndex = i + 1;
            Console.WriteLine($"  - day {dayIndex:D2}: {dateTime:d} ({dateTime:dddd})");
        }
    }
}