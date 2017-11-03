using System.Collections.Generic;
using System.Linq;

namespace Apocalibs.Extensions.Enumerables
{
    public static class EnumerableExtensions
    {
        public struct EnumeratedItem<T>
        {
            public EnumeratedItem(int index, T item)
            {
                Index = index;
                Item = item;
            }

            public int Index { get; }
            public T Item { get; }

            public static implicit operator T(EnumeratedItem<T> enumeratedItem) => enumeratedItem.Item;
        }

        public static IEnumerable<EnumeratedItem<T>> EnumerateWithIndex<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Select((item, index) => new EnumeratedItem<T>(index, item));
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, T item)
        {
            return enumerable.Except(new[] { item });
        }


    }
}
