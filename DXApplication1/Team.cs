using System.Collections.Generic;

namespace DXApplication1
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IList<Employee> Members { get; set; }
    }

    public static class Teams
    {
        public static Team Team1 { get; } = new Team { Id = 1, Name = "Team 1", Members = new List<Employee> { Employees.Employee1, Employees.Employee2 } };
        public static Team Team2 { get; } = new Team { Id = 2, Name = "Team 2", Members = new List<Employee> { Employees.Employee2 } };
        public static Team Team3 { get; } = new Team { Id = 3, Name = "Team 3", Members = new List<Employee> { Employees.Employee3 } };
        public static Team Team4 { get; } = new Team { Id = 4, Name = "Team 4", Members = new List<Employee> { } };

        public static IList<Team> AllTeams { get; } = new List<Team> { Team1, Team2, Team3, Team4 };
    }
}