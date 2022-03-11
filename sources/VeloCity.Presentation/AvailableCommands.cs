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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint;
using DustInTheWind.VeloCity.Presentation.Commands.OpenDatabase;
using DustInTheWind.VeloCity.Presentation.Commands.PresentSprints;
using DustInTheWind.VeloCity.Presentation.Commands.PresentTeam;
using DustInTheWind.VeloCity.Presentation.Commands.Vacations;
using DustInTheWind.VeloCity.Presentation.Infrastructure;

namespace DustInTheWind.VeloCity.Presentation
{
    public class AvailableCommands : IEnumerable<CommandInfo>
    {
        private readonly List<CommandInfo> commandInfos;

        public AvailableCommands()
        {
            commandInfos = new List<CommandInfo>()
            {
                new CommandInfo
                {
                    Name = "sprint",
                    DescriptionLines = new List<string>()
                    {
                        "Makes an analysis of the sprint and displays the result.",
                        "usage:",
                        "  sprint",
                        "  sprint [sprint-number]",
                        "  sprint [sprint-number] -exclude [sprint-number[,sprint-number[...]]]",
                        ""
                    },
                    Type = typeof(AnalyzeSprintCommand)
                },
                new CommandInfo
                {
                    Name = "sprints",
                    DescriptionLines = new List<string>()
                    {
                        "Displays an overview of the last sprints.",
                        "usage:",
                        "  sprints",
                        "  sprints [sprint-count]",
                        ""
                    },
                    Type = typeof(PresentSprintsCommand)
                },
                new CommandInfo
                {
                    Name = "vacations",
                    DescriptionLines = new List<string>()
                    {
                        "Displays the vacation days for the specified team member.",
                        "usage:",
                        "  vacations [person-name]",
                        ""
                    },
                    Type = typeof(VacationsCommand)
                },
                new CommandInfo
                {
                    Name = "team",
                    DescriptionLines = new List<string>()
                    {
                        "Displays the composition of the team (team members).",
                        "usage:",
                        "  team",
                        "  team -date [date]",
                        "  team -start-date [date] -end-date [date]",
                        ""
                    },
                    Type = typeof(PresentTeamCommand)
                },
                new CommandInfo
                {
                    Name = "db",
                    DescriptionLines = new List<string>()
                    {
                        "Opens the database in a text editor.",
                        "usage:",
                        "  db",
                        ""
                    },
                    Type = typeof(OpenDatabaseCommand)
                }
            };
        }

        public CommandInfo this[string commandName] => commandInfos.FirstOrDefault(x => x.Name == commandName);

        public IEnumerator<CommandInfo> GetEnumerator()
        {
            return commandInfos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}