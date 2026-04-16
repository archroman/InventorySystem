using System.Globalization;

namespace Inventory.UI.Utils
{
    public static class NumberFormatter
    {
        public static string Format(int value)
        {
            return Format((long)value);
        }

        private static string Format(long value)
        {
            if (value < 1000)
                return value.ToString();

            if (value < 1_000_000)
                return (value / 1000f).ToString("0.#", CultureInfo.InvariantCulture) + "K";

            if (value < 1_000_000_000)
                return (value / 1_000_000f).ToString("0.#", CultureInfo.InvariantCulture) + "M";

            return (value / 1_000_000_000f).ToString("0.#", CultureInfo.InvariantCulture) + "B";
        }
    }
}