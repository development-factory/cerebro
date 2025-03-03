using System.Globalization;

namespace Cerebro.Utils;

internal static class GlobalizationUtils
{
    public static void ConfigureCurrentCulture()
    {
        var cultureInfo = new CultureInfo("ro-RO");

        cultureInfo.NumberFormat.CurrencySymbol = "RON";
        cultureInfo.NumberFormat.CurrencyDecimalDigits = 0;

        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
    }
}
