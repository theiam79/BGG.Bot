using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Core.Models
{
  public class CommandResult
  {
    public bool Success { get; set; }
    public string Message { get; set; }

    public CommandResult(bool success, string message = "")
    {
      Success = success;
      Message = message;
    }
  }
}
