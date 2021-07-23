using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DXApplication1
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TeamMember
    {
        public int? TeamId { get; set; }

        [DisplayName("Team")]
        public string TeamName { get; set; }
        public int MemberId { get; set; }

        [DisplayName("Employee")]
        public string MemberName { get; set; }
    }

    public static class Teams
    {
        public static Team Team1 { get; } = new Team { Id = 1, Name = "Team 1", };
        public static Team Team2 { get; } = new Team { Id = 2, Name = "Team 2", };
        public static Team Team3 { get; } = new Team { Id = 3, Name = "Team 3", };
        public static Team Team4 { get; } = new Team { Id = 4, Name = "Team 4", };

        public static IList<Team> AllTeams { get; } = new List<Team> { Team1, Team2, Team3, Team4 };

        private static IList<TeamMember> _AllTeamMembers;
        public static IList<TeamMember> AllTeamMembers
            => _AllTeamMembers ?? (_AllTeamMembers = (from e in Employees.AllEmployees
                                                      from t in e.MemberOf.DefaultIfEmpty()
                                                      select new TeamMember
                                                      {
                                                          MemberId = e.Id,
                                                          MemberName = e.Name,
                                                          TeamId = t?.Id,
                                                          TeamName = t?.Name,
                                                      }).ToList());
    }
}