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
    public DbSet<UserCollectionItem> UserCollectionItems { get; set; }
  }
}
