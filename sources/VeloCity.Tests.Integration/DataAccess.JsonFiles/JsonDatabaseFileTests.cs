using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.JsonFiles;
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.DataAccess.JsonFiles;

public class JsonDatabaseFileTests
{
    [Fact]
    public void OpenEmptyFile()
    {
        JsonDatabaseFile jsonDatabaseFile = new(@"TestData\DataAccess.JsonFiles\JsonDatabaseFileTests\01-empty.json");
        jsonDatabaseFile.Open();

        jsonDatabaseFile.Document.DatabaseInfo.Should().BeNull();
        jsonDatabaseFile.Document.Sprints.Should().BeNull();
        jsonDatabaseFile.Document.TeamMembers.Should().BeNull();
        jsonDatabaseFile.Document.OfficialHolidays.Should().BeNull();
    }

    [Fact]
    public void DatabaseInfo()
    {
        JsonDatabaseFile jsonDatabaseFile = new(@"TestData\DataAccess.JsonFiles\JsonDatabaseFileTests\02-database-info.json");
        jsonDatabaseFile.Open();

        Version expectedVersion = new(2, 0, 0);
        jsonDatabaseFile.Document.DatabaseInfo.DatabaseVersion.Should().Be(expectedVersion);
    }

    [Fact]
    public void OneSprint()
    {
        JsonDatabaseFile jsonDatabaseFile = new(@"TestData\DataAccess.JsonFiles\JsonDatabaseFileTests\03-one-sprint.json");
        jsonDatabaseFile.Open();

        jsonDatabaseFile.Document.Sprints.Should().HaveCount(1);
        jsonDatabaseFile.Document.Sprints[0].Id.Should().Be(5);
        jsonDatabaseFile.Document.Sprints[0].Number.Should().Be(7);
        jsonDatabaseFile.Document.Sprints[0].Name.Should().Be("Dummy Name");
        jsonDatabaseFile.Document.Sprints[0].StartDate.Should().Be(new DateTime(2022, 02, 21));
        jsonDatabaseFile.Document.Sprints[0].EndDate.Should().Be(new DateTime(2022, 03, 06));
        jsonDatabaseFile.Document.Sprints[0].Goal.Should().Be("The goal of the sprint");
        jsonDatabaseFile.Document.Sprints[0].CommitmentStoryPoints.Should().Be(27);
        jsonDatabaseFile.Document.Sprints[0].ActualStoryPoints.Should().Be(24);
        jsonDatabaseFile.Document.Sprints[0].State.Should().Be(JSprintState.Closed);
        jsonDatabaseFile.Document.Sprints[0].Comments.Should().Be("some comments");
    }

    [Fact]
    public void OneTeamMember()
    {
        JsonDatabaseFile jsonDatabaseFile = new(@"TestData\DataAccess.JsonFiles\JsonDatabaseFileTests\04-one-team-member.json");
        jsonDatabaseFile.Open();

        jsonDatabaseFile.Document.TeamMembers.Should().HaveCount(1);
        jsonDatabaseFile.Document.TeamMembers[0].Id.Should().Be(7);
        jsonDatabaseFile.Document.TeamMembers[0].FirstName.Should().Be("Laura");
        jsonDatabaseFile.Document.TeamMembers[0].MiddleName.Should().Be("Allison");
        jsonDatabaseFile.Document.TeamMembers[0].LastName.Should().Be("Amberson");
        jsonDatabaseFile.Document.TeamMembers[0].Nickname.Should().Be("Alli");
        jsonDatabaseFile.Document.TeamMembers[0].Comments.Should().Be("This is the team member");
        
        jsonDatabaseFile.Document.TeamMembers[0].Employments[0].StartDate.Should().Be(new DateTime(2022, 02, 21));
        jsonDatabaseFile.Document.TeamMembers[0].Employments[0].EndDate.Should().Be(new DateTime(2027, 01, 25));
        jsonDatabaseFile.Document.TeamMembers[0].Employments[0].HoursPerDay.Should().Be((HoursValue)4);
        jsonDatabaseFile.Document.TeamMembers[0].Employments[0].WeekDays.Should().Equal(JDayOfWeek.Tuesday, JDayOfWeek.Wednesday, JDayOfWeek.Thursday, JDayOfWeek.Friday);
        jsonDatabaseFile.Document.TeamMembers[0].Employments[0].Country.Should().Be("RO");
        
        jsonDatabaseFile.Document.TeamMembers[0].VacationDays[0].Recurrence.Should().Be(JVacationRecurrence.Once);
        jsonDatabaseFile.Document.TeamMembers[0].VacationDays[0].Date.Should().Be(new DateTime(2022, 01, 20));
        jsonDatabaseFile.Document.TeamMembers[0].VacationDays[0].Comments.Should().Be("first vacation");
        
        jsonDatabaseFile.Document.TeamMembers[0].VacationDays[1].StartDate.Should().Be(new DateTime(2022, 03, 28));
        jsonDatabaseFile.Document.TeamMembers[0].VacationDays[1].EndDate.Should().Be(new DateTime(2022, 03, 31));
        jsonDatabaseFile.Document.TeamMembers[0].VacationDays[1].Comments.Should().Be("daily one");
    }
}