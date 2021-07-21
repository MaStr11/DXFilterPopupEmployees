using System.Collections.Generic;

namespace DXApplication1
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Team> MemberOf { get; set; }
    }

    public static class Employees
    {
        public static Employee Employee1 { get; } = new Employee { Id = 1, Name = "Joe Doe", MemberOf = new List<Team> { Teams.Team1 } };
        public static Employee Employee2 { get; } = new Employee { Id = 2, Name = "Jane Doe", MemberOf = new List<Team> { Teams.Team1, Teams.Team2 } };
        public static Employee Employee3 { get; } = new Employee { Id = 3, Name = "Tom Burow", MemberOf = new List<Team> { Teams.Team3 } };
        public static Employee Employee4 { get; } = new Employee { Id = 4, Name = "Anne Will", MemberOf = new List<Team> { } };

        public static IList<Employee> AllEmployees { get; } = new List<Employee> { Employee1, Employee2, Employee3, Employee4, };
    }
}