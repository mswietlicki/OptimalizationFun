using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalizationFun
{
    public static class IntExtentions
    {

    }

    class Program
    {
        static void Main(string[] args)
        {
            int[] ints;
            using (new TimeMetrics("Ints generation."))
            {
                ints = GenerateInts(10000000).ToArray();
            }


            using (new TimeMetrics("GetMaxWithFor."))
                RunManyTimes(() => GetMaxWithFor(ints));

            using (new TimeMetrics("GetMaxWithForParallel."))
                RunManyTimesParalel(() => GetMaxWithFor(ints));

            using (new TimeMetrics("GetMaxLinq."))
                RunManyTimes(() => GetMaxLinq(ints));

            using (new TimeMetrics("GetMaxParalelLinq."))
                RunManyTimes(() => GetMaxParalelLinq(ints));

            using (new TimeMetrics("GetMaxGeneric."))
                RunManyTimes(() => GetMaxGeneric(ints));

            using (new TimeMetrics("GetMaxWith2Maxes."))
                RunManyTimes(() => GetMaxWith2Maxes(ints));

            using (new TimeMetrics("GetMaxWith2MaxesParalel."))
                RunManyTimesParalel(() => GetMaxWith2Maxes(ints));

            using (new TimeMetrics("GetMaxWith2MaxesParalelVoid."))
                RunManyTimesParalelVoid(() => GetMaxWith2Maxes(ints));


            Console.ReadLine();
        }

        static int GetMaxGeneric(IEnumerable<int> ints)
        {
            var max = int.MinValue;
            foreach (var i in ints)
            {
                max = Math.Max(max, i);
            }
            return max;
        }

        static int GetMaxLinq(IEnumerable<int> ints)
        {
            return ints.Max();
        }

        static int GetMaxParalelLinq(IEnumerable<int> ints)
        {
            return ints.AsParallel().Max();
        }

        static int GetMaxWithFor(int[] ints)
        {
            var n = ints.Count();
            var max = int.MinValue;

            for (var i = 0; i < n; i++)
            {
                max = Math.Max(max, ints[i]);
            }
            return max;
        }

        static int GetMaxWith2Maxes(int[] ints)
        {
            var n = ints.Count();
            int max1 = int.MinValue, max2 = int.MinValue;

            for (var i = 0; i < n; i += 2)
            {
                max1 = Math.Max(max1, ints[i]);
                max2 = Math.Max(max2, ints[i + 1]);
            }
            return Math.Max(max1, max2);
        }


        private static IEnumerable<int> GenerateInts(int n)
        {
            var r = new Random((int)DateTime.Now.Ticks);
            return Enumerable.Range(0, n).Select(_ => r.Next());
        }


        private const int Times = 100;
        private static T RunManyTimes<T>(Func<T> func)
        {
            var t = default(T);
            for (var i = 0; i < Times; i++)
            {
                t = func();
            }
            return t;
        }

        private static T RunManyTimesParalel<T>(Func<T> func)
        {
            var t = default(T);
            Parallel.ForEach(Enumerable.Range(0, Times), _ => t = func());
            return t;
        }

        private static void RunManyTimesParalelVoid<T>(Func<T> func)
        {
            Parallel.ForEach(Enumerable.Range(0, Times), _ => func());
        }

    }
}
