using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BGG.Bot.Core.Models.Search
{
  public class Item
  {
    [XmlAttribute("type")]
    public string Type;

    [XmlAttribute("id")]
    public string Id;

    [XmlElement("name")]
    public Name Name;

    [XmlElement("yearpublished")]
    public ValueElement YearPublished;
  }
}
