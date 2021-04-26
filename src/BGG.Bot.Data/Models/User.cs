using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Data.Models
{
  public class User
  {
    public int UserId { get; set; }
    public ulong DiscordId { get; set; }
    public string BggUsername { get; set; }
    public ICollection<UserCollectionItem> UserCollectionItems { get; set; }
  }
}
