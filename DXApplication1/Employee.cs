using System.Collections.Generic;

namespace DXApplication1
{
    public class Employee
    {
        public string Name { get; set; }
        public IList<Team> MemberOfTeams { get; set; }
    }

    public static class Employees
    {
        public static Employee Employee1 { get; } = new Employee { Name = "Joe Doe", MemberOfTeams = new List<Team> { Teams.Team1 } };
        public static Employee Employee2 { get; } = new Employee { Name = "Jane Doe", MemberOfTeams = new List<Team> { Teams.Team1, Teams.Team2 } };
        public static Employee Employee3 { get; } = new Employee { Name = "Tom Burow", MemberOfTeams = new List<Team> { Teams.Team3 } };
        public static Employee Employee4 { get; } = new Employee { Name = "Anne Will", MemberOfTeams = new List<Team> { } };
    }
}