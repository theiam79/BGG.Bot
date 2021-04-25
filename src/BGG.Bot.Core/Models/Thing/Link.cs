using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BGG.Bot.Core.Models.Thing
{
	public class Link
	{
		[XmlAttribute("type")]
		public string Type { get; set; }

		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlAttribute("value")]
		public string Value { get; set; }
	}
}
