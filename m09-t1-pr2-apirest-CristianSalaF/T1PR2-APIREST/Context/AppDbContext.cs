using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using T1PR2_APIREST.Models;

namespace T1PR2_APIREST.Context
{
    /// <summary>
    /// Database context for the application.
    /// This class inherits from IdentityDbContext to manage user authentication and authorization.
    /// It also includes DbSet properties for the Game and Vote entities.
    /// </summary>
    public class AppDbContext : IdentityDbContext<User>
    {
        /// <summary>
        /// Constructor for the AppDbContext class.
        /// This constructor takes DbContextOptions as a parameter and passes it to the base class constructor.
        /// It is used to configure the database context with specific options such as connection string, provider, etc.
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet for the Game entity.
        /// This represents the games in the database.
        /// </summary>
        public DbSet<Game> Games { get; set; }

        /// <summary>
        /// DbSet for the Vote entity.
        /// This represents the votes cast by users for games in the database. 
        /// </summary>
        public DbSet<Vote> Votes { get; set; }

        /// <summary>
        /// Configures the model for the database context.
        /// This method is called when the model for the context is being created.
        /// It allows for custom configurations and relationships between entities.
        /// This includes defining relationships between Game, User, and Vote entities.
        /// It also sets up a unique constraint to ensure that each user can only vote once per game.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Game>()
                .HasMany(g => g.Votes)
                .WithOne(v => v.Game)
                .HasForeignKey(v => v.GameId);

            builder.Entity<User>()
                .HasMany(u => u.Votes)
                .WithOne(v => v.User)
                .HasForeignKey(v => v.UserId);

            builder.Entity<Vote>()
                .HasIndex(v => new { v.UserId, v.GameId })
                .IsUnique();
        }
    }
}
