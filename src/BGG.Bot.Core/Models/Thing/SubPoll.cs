using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BGG.Bot.Core.Models.Thing
{
	public class SubPoll
	{
		[XmlAttribute("numplayers")]
		public string NumPlayers { get; set; }

		[XmlElement("result")]
		public PollResult[] PollResults { get; set; }
	}
}
