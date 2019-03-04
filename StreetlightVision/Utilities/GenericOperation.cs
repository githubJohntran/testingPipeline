using StreetlightVision.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetlightVision.Utilities
{
    public class GenericOperation<T>
    {
        public static T Retry(Func<T> action, Func<T, bool> condition, int second)
        {
            T result;
            var startTime = DateTime.Now;
            while (true)
            {
                Wait.ForSeconds(2);
                result = action();
                if (condition(result))
                    return result;

                var executeTime = DateTime.Now;
                var diff = (executeTime - startTime).TotalSeconds;
                if (diff > second)
                    break;
            }
            return result;
        }
    }

    public static class SLVCustomExtension
    {
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static List<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            int length = source.Count();
            if (count > length) throw new OverflowException(string.Format("The count '{0}' is larger than list count '{1}'", count, length));

            return source.Shuffle().Take(count).ToList();
        }

        public static List<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }

    public static class AsyncExtension
    {
        public static T ToSync<T>(this Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
    }
}
