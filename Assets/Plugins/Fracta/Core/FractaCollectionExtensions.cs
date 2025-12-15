using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Fracta.Core
{
    public static class FractaCollectionExtensions
    {
        public static bool TryToFind<T>(this List<T> list, Predicate<T> predicate, out T result)
        {
            result = list.Find(predicate);
            if (result == null) return false;
            return true;
        }

        public static T GetRandom<T>(this T[] array)
        {
            var random = Random.Range(0, array.Length);
            return array[random];
        }
    }
}