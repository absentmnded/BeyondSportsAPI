using BeyondSportsAPI.Models;

namespace BeyondSportsAPI.Data

{
    public class DbInitializer
    {
        public static void Initialize(BeyondSportsDbContext context)
        {
            if (context.Players.Any())
            {
                return;   // DB has been seeded
            }

            var players = new Player[]
            {
                new Player{Name="Steven Bergwijn",BirthDate=DateTime.Parse("1997-10-08"),Height=178},
                new Player{Name="Kenneth Taylor",BirthDate=DateTime.Parse("2002-05-16"),Height=182}
            };

            context.Players.AddRange(players);
            context.SaveChanges();

            var teams = new Team[]
            {
                new Team{Name="Ajax Amsterdam"},
                new Team{Name="FC Barcelona"}

            };

            context.Teams.AddRange(teams);
            context.SaveChanges();
        }

    }
}
