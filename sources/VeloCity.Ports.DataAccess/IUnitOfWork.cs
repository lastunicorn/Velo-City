// VeloCity
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

using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Ports.DataAccess;

public interface IUnitOfWork
{
    public WarningException DatabaseWarning { get; }

    public Exception DatabaseError { get; }

    public IOfficialHolidayRepository OfficialHolidayRepository { get; }

    public ISprintRepository SprintRepository { get; }

    public ITeamMemberRepository TeamMemberRepository { get; }

    Task SaveChanges();
}