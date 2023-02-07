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

using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using Autofac;
using DustInTheWind.VeloCity.JsonFiles;
using DustInTheWind.VeloCity.Ports.SettingsAccess;
using DustInTheWind.VeloCity.Wpf.Presentation.MainArea.Main;

namespace DustInTheWind.VeloCity.Wpf.Bootstrapper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : global::System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IContainer container = Setup.BuildContainer();

            SetCurrentCulture(container);
            OpenDatabase(container);

            MainWindow mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();

            MainWindow = mainWindow;

            base.OnStartup(e);
        }

        private static void SetCurrentCulture(IComponentContext container)
        {
            IConfig config = container.Resolve<IConfig>();

            CultureInfo.CurrentCulture = config.Culture;

            string currentCultureTag = CultureInfo.CurrentCulture.IetfLanguageTag;
            XmlLanguage xmlLanguage = XmlLanguage.GetLanguage(currentCultureTag);
            FrameworkPropertyMetadata frameworkPropertyMetadata = new(xmlLanguage);
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), frameworkPropertyMetadata);
        }

        private static void OpenDatabase(IComponentContext container)
        {
            JsonDatabase jsonDatabase = container.Resolve<JsonDatabase>();
            jsonDatabase.Open();
        }
    }
}