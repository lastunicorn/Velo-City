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
using System.Collections.Generic;
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Controls;

namespace DustInTheWind.VeloCity.Presentation.UserControls
{
    internal class NotesControl : BlockControl
    {
        public List<NoteBase> Notes { get; set; }

        public NotesControl()
        {
            Margin = "0 1 0 0";
            ForegroundColor = ConsoleColor.DarkYellow;
        }

        protected override void DoDisplayContent(ControlDisplay display)
        {
            CustomConsole.WriteLine(ConsoleColor.DarkYellow, "Notes:");

            foreach (NoteBase note in Notes)
                CustomConsole.WriteLine(ConsoleColor.DarkYellow, $"  - {note}");

            //display.WriteRow("Notes:");

            //foreach (NoteBase note in Notes)
            //{
            //    display.StartRow();
            //    display.Write("  - ");
            //    note.Display();
            //    display.EndRow();
            //}
        }
    }
}