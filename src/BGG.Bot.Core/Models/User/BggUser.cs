using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BGG.Bot.Core.Models.User
{
  [XmlRoot("user")]
  public class BggUser
  {
    [XmlAttribute("id")]
    public string Id { get; set; }
    [XmlAttribute("name")]
    public string Name { get; set; }
    [XmlAttribute("termsofuse")]
    public string TermsOfUse { get; set; }
  }
}
