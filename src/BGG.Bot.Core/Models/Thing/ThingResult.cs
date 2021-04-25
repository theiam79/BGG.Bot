using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BGG.Bot.Core.Models.Thing
{
	[XmlRoot("items")]
	public class ThingResult
	{
		[XmlElement("item")]
		public Thing[] Things { get; set; }
	}
}
