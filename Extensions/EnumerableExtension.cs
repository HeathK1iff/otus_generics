
using System.Collections;

namespace otus_generics.Extensions;

public static class EnumerableExtension
{
    public static T GetMax<T>(this IEnumerable collection, Func<T, float> convertToNumber) where T : class
    {
        if (convertToNumber == null)
        {
            throw new ArgumentNullException(nameof(convertToNumber));
        }

        (T Item, float Value) itemWithMaxValue = (null, float.MinValue);

        foreach (T item in collection)
        {
            float value = convertToNumber.Invoke(item);
            
            if (itemWithMaxValue.Value < value)
            {
                itemWithMaxValue = (item, value);
            }
        }

        return itemWithMaxValue.Item;
    }
}