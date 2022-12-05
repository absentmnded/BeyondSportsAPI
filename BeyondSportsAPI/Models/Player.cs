using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BeyondSportsAPI.Models
{
    public class Player : PlayerBase
    {
        public Player() { }
        public Player(PlayerBase basePlayer)
        { 
            Name = basePlayer.Name;
            BirthDate = basePlayer.BirthDate;
            Height = basePlayer.Height;
            IdTeam = basePlayer.IdTeam;
        }
        public long Id { get; set; }
    }

    // TRO class to prevent setting Id through POST requests
    public class PlayerBase
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }
        [Range(150, 220)]
        public long Height { get; set; }
        public long? IdTeam { get; set; }
    }

    // This class created to be able to quickly assign multiple players to a team
    public class PlayersOnTheTeam
    {
        [Required]
        public long IdTeam { get; set; }
        public long[]? IdsPlayer { get; set; }
    }

}
