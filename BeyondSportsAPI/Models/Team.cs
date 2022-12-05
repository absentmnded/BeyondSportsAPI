using System.ComponentModel.DataAnnotations;


namespace BeyondSportsAPI.Models
{
    public class Team : TeamBase
    {
        public Team(TeamBase baseTeam)
        {
            Name = baseTeam.Name;
            Players = baseTeam.Players;
        }
        public Team() { }
        public long Id { get; set; }
    }

    public class TeamBase
    {
        [Required]
        public string? Name { get; set; }
        public List<Player>? Players { get;internal set; }
    }

}
