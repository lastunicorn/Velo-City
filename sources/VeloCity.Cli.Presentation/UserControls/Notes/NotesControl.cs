﻿// VeloCity
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

using DustInTheWind.ConsoleTools.Controls;

namespace DustInTheWind.VeloCity.Cli.Presentation.UserControls.Notes;

internal class NotesControl : BlockControl
{
    public List<NoteBase> Notes { get; set; }

    public bool ShowTitle { get; set; } = true;

    public NotesControl()
    {
        Margin = "0 1 0 0";
        ForegroundColor = ConsoleColor.DarkYellow;
    }

    protected override void DoDisplayContent(ControlDisplay display)
    {
        if (Notes.Count == 0)
            return;

        if (ShowTitle)
            display.WriteRow("Notes:");

        foreach (NoteBase note in Notes)
            display.WriteRow($"  - {note}");
    }

    public IEnumerable<string> ToLines()
    {
        return Notes
            .SelectMany(x => x.ToLines());
    }

    public override string ToString()
    {
        if (Notes.Count == 0)
            return string.Empty;

        return string.Join(Environment.NewLine, Notes);
    }
}