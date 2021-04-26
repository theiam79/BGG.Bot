using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.ResultSelection
{
  public static class IndexedResultHelpers
  {
    public static List<IndexedResult<T>> ToIndexedResults<T>(this IEnumerable<T> results)
    {
      var i = 1;
      return results.Select(r => new IndexedResult<T>(i++, r)).ToList();
    }
  }
}
