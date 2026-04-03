using Microsoft.EntityFrameworkCore;

namespace Auth.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
}