using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.ResultSelection
{
  public class IndexedResult<T>
  {
    public int Index { get; set; }
    public T Result { get; set; }
    public IndexedResult(int index, T result)
    {
      Index = index;
      Result = result;
    }
  }
}
