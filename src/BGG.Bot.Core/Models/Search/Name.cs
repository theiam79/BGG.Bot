using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BGG.Bot.Core.Models.Search
{
  public class Name
  {
    [XmlAttribute("type")]
    public string Type;

    [XmlAttribute("value")]
    public string Value;

  }

}
