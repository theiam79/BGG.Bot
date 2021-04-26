using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Data.Models
{
  public class CollectionItem
  {
    public int CollectionItemId { get; set; }
    public int BggId { get; set; }
    public string Name { get; set; }
    public ICollection<UserCollectionItem> UserCollectionItems { get; set; }
  }
}
