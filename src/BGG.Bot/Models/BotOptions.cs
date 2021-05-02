using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Models
{
  public class BotOptions
  {
    [Required(AllowEmptyStrings = false, ErrorMessage = "You must set a command prefix")]
    public string CommandPrefix { get; set; }
  }
}
