using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Data.Models
{
  public class UserCollectionItem
  {
    public int UserId { get; set; }
    public User User { get; set; }
    public int BggId{ get; set; }
    public CollectionItem CollectionItem { get; set; }
    public bool Owned { get; set; } = false;
    public bool PreviouslyOwned { get; set; } = false;
    public bool ForTrade { get; set; } = false;
    public bool Want { get; set; } = false;
    public bool WantToPlay { get; set; } = false;
    public bool WantToBuy { get; set; } = false;
    public bool WishList { get; set; } = false;
    public WishlistPriority WishlistPriority { get; set; } = WishlistPriority.None;
    public bool PreOrdered { get; set; } = false;
    public float Rating { get; set; }
    public DateTime LastModified { get; set; }
  }

  public enum WishlistPriority
  {
    None = 0,
    MustHave = 1,
    LoveToHave = 2,
    LikeToHave = 3,
    ThinkingAboutIt = 4,
    DontBuy = 5
  }
}
