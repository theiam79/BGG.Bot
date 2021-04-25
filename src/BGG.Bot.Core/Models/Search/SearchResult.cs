using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BGG.Bot.Core.Models.Search
{
  [XmlRoot("items")]
  public class SearchResult
  {
    [XmlAttribute("total")]
    public string Total;

    [XmlAttribute("termsofuse")]
    public string TermsOfUse;

    [XmlElement("item")]
    public Item[] Items;
  }
}
