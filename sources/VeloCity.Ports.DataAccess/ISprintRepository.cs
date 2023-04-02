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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Ports.DataAccess;

public interface ISprintRepository
{
    Task<IEnumerable<Sprint>> GetAll();

    Task<Sprint> Get(int id);

    Task<Sprint> GetByNumber(int number);

    Task<IEnumerable<Sprint>> GetClosedSprintsBefore(int sprintNumber, uint count);

    Task<IEnumerable<Sprint>> GetClosedSprintsBefore(int sprintNumber, uint count, IEnumerable<int> excludedSprints);

    Task<Sprint> GetLast();

    Task<IEnumerable<Sprint>> GetLast(int count);

    Task<Sprint> GetLastInProgress();
    
    Task<IEnumerable<Sprint>> GetLastClosed(uint count);

    Task<Sprint> GetLastClosed();

    Task<IEnumerable<Sprint>> Get(DateTime startDate, DateTime endDate);

    Task<DateInterval?> GetDateIntervalFor(int sprintNumber);

    Task<bool> IsAnyInProgress();

    Task<bool> IsFirstNewSprint(int sprintId);

    void Add(Sprint sprint);
}