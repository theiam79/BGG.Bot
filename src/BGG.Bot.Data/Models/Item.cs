using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Data.Models
{
  public class Item
  {
    public int ItemId { get; set; }
    public int BggId { get; set; }
    public string Name { get; set; }
    [ForeignKey("BggId")]
    public ICollection<UserCollectionItem> UserCollectionItems { get; set; }
    [ForeignKey("BggId")]
    public ICollection<UserPlayedItem> UserPlayedItems { get; set; }
  }
}
