using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MAUI.Native.Embedded.Tasks;

public sealed class UpdateXCAssetsColorset : Task
{
    [Required]
    public ITaskItem[] Items { get; set; } = [];

    [Required]
    public string Color { get; set; } = "#FFFFFF";

    public string DarkColor { get; set; } = "#000000";

    public override bool Execute()
    {
        var colorsets = Items.Where(i => i.ItemSpec.Contains(".colorset", StringComparison.CurrentCultureIgnoreCase)).ToArray();
        if (colorsets.Length == 0)
        {
            Log.LogError($@"No .colorset found to update. {nameof(Items)} = '{string.Join(";", Items.Select(i => i.ItemSpec))}'");
            return false;
        }

        try
        {
            ParseColor(Color, out var lcr, out var lcg, out var lcb, out var lca);
            ParseColor(DarkColor, out var dcr, out var dcg, out var dcb, out var dca);

            foreach (var item in colorsets)
            {
                var path = item.ItemSpec;
                if (!File.Exists(path))
                {
                    continue;
                }

                var original = File.ReadAllText(path);
                var updated = original
                    .Replace("{lcr}", lcr)
                    .Replace("{lcg}", lcg)
                    .Replace("{lcb}", lcb)
                    .Replace("{lca}", lca)
                    .Replace("{dcr}", dcr)
                    .Replace("{dcg}", dcg)
                    .Replace("{dcb}", dcb)
                    .Replace("{dca}", dca);

                if (original != updated)
                {
                    File.WriteAllText(path, updated);
                }
            }
        }
        catch (Exception ex)
        {
            Log.LogError(ex.Message);
        }

        return !Log.HasLoggedErrors;
    }

    private static void ParseColor(string color, out string r, out string g, out string b, out string a)
    {
        var colorValue = ColorTranslator.FromHtml(color);
        r = ToFloatString(colorValue.R);
        g = ToFloatString(colorValue.G);
        b = ToFloatString(colorValue.B);
        a = ToFloatString(colorValue.A);
    }

    private static string ToFloatString(byte v) =>
        (v / 255.0).ToString("0.000", CultureInfo.InvariantCulture);
}
