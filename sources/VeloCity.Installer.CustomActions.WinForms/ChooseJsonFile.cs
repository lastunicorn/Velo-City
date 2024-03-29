﻿// Velo City
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

using System.Threading;
using System.Windows.Forms;

namespace DustInTheWind.VeloCity.Installer.CustomActions.WinForms
{
    public class ChooseJsonFile
    {
        private volatile string fileName;

        public string FileName => fileName;

        public void Run()
        {
            fileName = null;

            Thread task = new Thread(GetFile);
            task.SetApartmentState(ApartmentState.STA);
            task.Start();
            task.Join();
        }

        private void GetFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON File (*.json)|*.json|All Files|*.*"
            };

            fileName = openFileDialog.ShowDialog() == DialogResult.OK
                ? openFileDialog.FileName
                : null;
        }
    }
}