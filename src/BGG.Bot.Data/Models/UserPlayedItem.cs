using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Data.Models
{
  public class UserPlayedItem
  {
      public int UserId { get; set; }
      public User User { get; set; }
      public int BggId { get; set; }
      public Item Item { get; set; }
      public float Rating { get; set; }
  }
}
