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
    public DbSet<CollectionItem> CollectionItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<UserCollectionItem>().HasKey(uci => new { uci.UserId, uci.BggId });

      //modelBuilder.Entity<CollectionItem>().HasMany<UserCollectionItem>(ci => ci.UserCollectionItems).WithMany(uci => uci.).HasForeignKey(f => f.BggId).HasPrincipalKey(f => f.BggId);
      modelBuilder.Entity<UserCollectionItem>()
        .HasOne(uci => uci.CollectionItem)
        .WithMany(ci => ci.UserCollectionItems)
        .HasForeignKey(uci => uci.BggId)
        .HasPrincipalKey(ci => ci.BggId);

      modelBuilder.Entity<CollectionItem>()
        .HasAlternateKey(ci => ci.BggId);

      //modelBuilder.Entity<User>()
      //  .HasMany(u => u.UserCollectionItems)
      //  .WithOne(u => u.User)
      //  .OnDelete(DeleteBehavior.casc)
    }
  }
}
