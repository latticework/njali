using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jali.Core
{
    public static class JaliCoreExtensions
    {
        public static void AddRange<T>(this ICollection<T> sequence, IEnumerable<T> range)
        {
            var list = sequence as List<T>;
            if (list != null)
            {
                list.AddRange(range);
            }
            else
            {
                foreach (var element in range)
                {
                    sequence.Add(element);
                }
            }
        }
    }
}
