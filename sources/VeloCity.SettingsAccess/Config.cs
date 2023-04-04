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
using DustInTheWind.VeloCity.Ports.SettingsAccess;
using Microsoft.Extensions.Configuration;

namespace DustInTheWind.VeloCity.SettingsAccess;

public class Config : IConfig
{
    private readonly CultureProperty cultureProperty;
    private readonly ErrorMessageLevelProperty errorMessageLevelProperty;
    private readonly DatabaseLocationProperty databaseLocationProperty;
    private readonly DatabaseEditorProperty databaseEditorProperty;
    private readonly DatabaseEditorArgumentsProperty databaseEditorArgumentsProperty;
    private readonly DataGridStyleProperty dataGridStyleProperty;
    private readonly AnalysisLookBackProperty analysisLookBackProperty;

    public CultureInfo Culture => cultureProperty.Value;

    public ErrorMessageLevel ErrorMessageLevel => errorMessageLevelProperty.Value;

    public string DatabaseLocation => databaseLocationProperty.Value;

    public string DatabaseEditor => databaseEditorProperty.Value;

    public string DatabaseEditorArguments => databaseEditorArgumentsProperty.Value;

    public DataGridStyle DataGridStyle => dataGridStyleProperty.Value;

    public uint AnalysisLookBack => analysisLookBackProperty.Value;

    public Config()
    {
        try
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            cultureProperty = new CultureProperty(config);
            errorMessageLevelProperty = new ErrorMessageLevelProperty(config);
            databaseLocationProperty = new DatabaseLocationProperty(config);
            databaseEditorProperty = new DatabaseEditorProperty(config);
            databaseEditorArgumentsProperty = new DatabaseEditorArgumentsProperty(config);
            dataGridStyleProperty = new DataGridStyleProperty(config);
            analysisLookBackProperty = new AnalysisLookBackProperty(config);
        }
        catch (Exception ex)
        {
            throw new ConfigurationOpenException(ex);
        }
    }

    public List<ConfigItem> GetAllValuesRaw()
    {
        return new List<ConfigItem>
        {
            cultureProperty.Raw,
            errorMessageLevelProperty.Raw,
            databaseLocationProperty.Raw,
            databaseEditorProperty.Raw,
            databaseEditorArgumentsProperty.Raw,
            dataGridStyleProperty.Raw,
            analysisLookBackProperty.Raw
        };
    }
}