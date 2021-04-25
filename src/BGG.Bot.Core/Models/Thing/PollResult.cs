using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BGG.Bot.Core.Models.Thing
{
	public class PollResult
	{
		[XmlAttribute("value")]
		public string Value { get; set; }

		[XmlAttribute("numvotes")]
		public int NumVotes { get; set; }
	}
}
