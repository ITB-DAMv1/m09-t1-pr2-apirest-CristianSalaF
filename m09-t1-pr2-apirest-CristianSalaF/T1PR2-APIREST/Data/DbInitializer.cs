using Microsoft.EntityFrameworkCore;
using T1PR2_APIREST.Context;
using T1PR2_APIREST.Models;

namespace T1PR2_APIREST.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(AppDbContext context, IServiceProvider serviceProvider)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if we already have games in the database
            if (await context.Games.AnyAsync())
            {
                return;
            }

            var games = new List<Game>
            {
                new Game
                {
                    Title = "Pixel Paradise",
                    Description = "A colorful adventure where players explore vibrant worlds, solve puzzles, and make friends with quirky characters.",
                    DeveloperTeamName = "Rainbow Studios"
                },
                new Game
                {
                    Title = "Animal Kingdom",
                    Description = "Build and manage your own wildlife sanctuary while learning about different animal species and their habitats.",
                    DeveloperTeamName = "Wild Creations"
                },
                new Game
                {
                    Title = "Space Explorers",
                    Description = "Embark on an educational journey through our solar system, discovering planets and astronomical phenomena.",
                    DeveloperTeamName = "Cosmic Games"
                },
                new Game
                {
                    Title = "Melody Makers",
                    Description = "A musical game that teaches kids how to create songs and understand music theory through fun challenges.",
                    DeveloperTeamName = "Harmony Interactive"
                },
                new Game
                {
                    Title = "Puzzle Quest",
                    Description = "A collection of brain-teasing puzzles that promote critical thinking and problem-solving skills.",
                    DeveloperTeamName = "Mind Games Co."
                },
                new Game
                {
                    Title = "Garden Guardians",
                    Description = "Plant, grow, and harvest virtual crops while learning about sustainable farming and ecology.",
                    DeveloperTeamName = "Green Thumb Games"
                },
                new Game
                {
                    Title = "Robot Engineers",
                    Description = "Build and program robots to complete missions, introducing basic coding concepts in a fun way.",
                    DeveloperTeamName = "Tech Tots"
                },
                new Game
                {
                    Title = "History Heroes",
                    Description = "Travel through time to experience important historical events and meet famous figures from the past.",
                    DeveloperTeamName = "Time Travelers Inc."
                },
                new Game
                {
                    Title = "Un-LIT",
                    Description = "A relaxing puzzle game that is easy to understand, yet hard to master.",
                    DeveloperTeamName = "Csf91",
                    ImageUrl = "https://img.itch.zone/aW1nLzE2MzU0NjEzLnBuZw==/315x250%23c/vBYhG4.png"
                },
                new Game
                {
                    Title = "Ocean Odyssey",
                    Description = "Dive deep into the ocean to discover marine life and learn about underwater ecosystems.",
                    DeveloperTeamName = "Deep Blue Games"
                }
            };

            await context.Games.AddRangeAsync(games);
            await context.SaveChangesAsync();
        }
    }
}