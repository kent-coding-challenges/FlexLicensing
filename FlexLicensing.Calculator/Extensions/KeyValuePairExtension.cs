using System.Collections.Generic;
using System.Linq;

namespace FlexLicensing.Calculator.Extensions
{
    /// <summary>
    ///     Provides extension methods to KeyValuePair<T1, T2>.
    /// </summary>
    public static class KeyValuePairExtension
    {
        /// <summary>
        ///     Extension method to get the KeyValuePair with maximum value.
        /// </summary>
        /// <param name="sequence">
        ///     Enumerable of KeyValuePair<T, uint> to get the max value from.
        /// </param>
        /// <returns>
        ///     Returns the KeyValuePair with maximum value if sequence is not empty, null otherwise.
        /// </returns>
        public static KeyValuePair<T, uint>? MaxByValue<T>
            (this IEnumerable<KeyValuePair<T, uint>> sequence)
        {
            // Return null if sequence is empty.
            if (sequence == null || sequence.Count() == 0)
                return null;

            // Return KeyValuePair with the highest value.
            var max = sequence.Aggregate((left, right) => left.Value > right.Value ? left : right);
            return max;
        }
    }
}