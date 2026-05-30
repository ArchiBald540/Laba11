using Microsoft.EntityFrameworkCore;

namespace Laba11;

public class DataContext : DbContext
{
    public DataContext() { }
    
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
    public DbSet<Note> Notes => Set<Note>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.EnableSensitiveDataLogging();
        }
        base.OnConfiguring(optionsBuilder);
    }
}