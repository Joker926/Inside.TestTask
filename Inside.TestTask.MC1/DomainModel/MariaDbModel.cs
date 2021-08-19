
using Microsoft.EntityFrameworkCore;

namespace Inside.TestTask.MC1.DomainModel;
public class Session
{
    public int Id { get; set; }
    public ICollection<Message> Messages {get;set;}
}

public class Message
{
    public int Id { get; set; }

    public int Session_Id { get; set; }
    public Session Session { get; set; }
    public DateTime MC1_timestamp { get; set; }
    public DateTime MC2_timestamp { get; set; }
    public DateTime MC3_timestamp { get; set; }
    public DateTime End_timestamp { get; set; }
}

public class MariaDbContext: DbContext
{
    public MariaDbContext(string connectionString) : base()
    {
        _connectionString = connectionString;
    }
    private string _connectionString;// = "server=localhost;port=3306;database=EFCoreMySQL;user=root;password=my-secret-pw";//

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var serverVersion = ServerVersion.AutoDetect(_connectionString);
        optionsBuilder.UseMySql(_connectionString, serverVersion);
    }

    public DbSet<Session> Sessions { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Session>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<Session>()
            .Property(s => s.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Message>()
            .Property(m => m.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Message>()
            .HasOne<Session>(m => m.Session)
            .WithMany(s => s.Messages)
            .HasForeignKey(m => m.Session_Id);

    }
}
