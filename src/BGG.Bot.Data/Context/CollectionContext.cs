using BGG.Bot.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Data.Context
{
  public class CollectionContext : DbContext
  {
    public CollectionContext(DbContextOptions<CollectionContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<UserCollectionItem> UserCollectionItem { get; set; }
    public DbSet<UserPlayedItem> UserPlayedItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<UserCollectionItem>().HasKey(uci => new { uci.UserId, uci.BggId });

      modelBuilder.Entity<UserCollectionItem>()
        .HasOne(uci => uci.Item)
        .WithMany(i => i.UserCollectionItems)
        .HasForeignKey(uci => uci.BggId)
        .HasPrincipalKey(i => i.BggId);

      modelBuilder.Entity<UserPlayedItem>().HasKey(upi => new { upi.UserId, upi.BggId });

      modelBuilder.Entity<UserPlayedItem>()
        .HasOne(upi => upi.Item)
        .WithMany(i => i.UserPlayedItems)
        .HasForeignKey(upi => upi.BggId)
        .HasPrincipalKey(i => i.BggId);

      modelBuilder.Entity<Item>()
        
        .HasAlternateKey(ci => ci.BggId);

      modelBuilder.Entity<User>()
        .Property(u => u.BggUsername)
        .UseCollation("NOCASE");
    }
  }
}
