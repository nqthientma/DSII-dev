using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Evp.Ds.Data.Extension
{
    public static class ReadOnlyCollectionExtension
    {
        public static IList<T> ToReadOnlyCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ReadOnlyCollection<T>(enumerable.ToList());
        }
    }
}