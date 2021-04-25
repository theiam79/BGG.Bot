using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BGG.Bot.Core.Models.Thing
{
	public class Poll
	{
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("title")]
		public string Title { get; set; }

		[XmlAttribute("totalvotes")]
		public int TotalVotes { get; set; }

		[XmlElement("results")]
		public SubPoll[] SubPolls { get; set; }
	}
}
