﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ClassyficatorTester
{
    public static class LinqExtensions
    {
        public static Tuple<List<T>, List<T>> Split<T>(this IEnumerable<T> src, double percent)
        {
            if (percent < 0 || percent > 1)
                throw new ArgumentException("Value should be between 0.0 and 1.0.", nameof(percent));

            var list = src as IList<T> ?? src.ToList();
            var count = list.Count;
            var take = (int) Math.Round(count * percent);
            return new Tuple<List<T>, List<T>>(list.Take(take).ToList(), list.Skip(take).ToList());
        }

        public static List<T> Shuffle<T>(this IEnumerable<T> src)
        {
            return src.OrderBy(arg => Guid.NewGuid()).ToList();
        }
    }
}