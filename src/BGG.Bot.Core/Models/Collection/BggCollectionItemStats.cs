using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BGG.Bot.Core.Models.Collection
{
  public class BggCollectionItemStats
  {
    [XmlAttribute("minplayers")]
    public int MinPlayers { get; set; }
    [XmlAttribute("maxplayers")]
    public int MaxPlayers { get; set; }
    [XmlAttribute("minplaytime")]
    public int MinPlaytime { get; set;}
    [XmlAttribute("maxplaytime")]
    public int MaxPlaytime { get; set; }
    [XmlElement("rating")]
    public ValueElement Rating { get; set; }
  }
}
