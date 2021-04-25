using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BGG.Bot.Core.Models
{
	public class ValueElement
	{
		[XmlAttribute("value")]
		public string Value { get; set; }
	}
}
