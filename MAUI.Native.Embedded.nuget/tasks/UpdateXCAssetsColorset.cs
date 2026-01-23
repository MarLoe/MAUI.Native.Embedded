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
    public string Name { get; set; } = "";

    [Required]
    public string Color { get; set; } = "#FFFFFF";

    public string DarkColor { get; set; } = "#000000";

    public override bool Execute()
    {
        if (Items.Length == 0)
        {
            Log.LogMessage(MessageImportance.Low, $@"No {nameof(Items)} to update.");
            return true;
        }

        try
        {
            if (!Name.EndsWith(".colorset", StringComparison.InvariantCultureIgnoreCase))
            {
                Name += ".colorset";
            }

            if (ParseColor(Color, out var lcr, out var lcg, out var lcb, out var lca) &&
                ParseColor(DarkColor, out var dcr, out var dcg, out var dcb, out var dca))
            {
                foreach (var item in Items)
                {
                    var colorsetFile = Path.Combine(item.ItemSpec, Name, "Contents.json");
                    if (!File.Exists(colorsetFile))
                    {
                        Log.LogError(null, ErrorCodes.ColorsetNotFound, null, null, 0, 0, 0, 0, $@"Colorset with name {Name} was not found in {item.ItemSpec}");
                        continue;
                    }

                    var original = File.ReadAllText(colorsetFile);
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
                        File.WriteAllText(colorsetFile, updated);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.LogErrorFromException(ex);
        }

        return !Log.HasLoggedErrors;
    }

    private bool ParseColor(string color, out string r, out string g, out string b, out string a)
    {
        try
        {
            var colorValue = ColorTranslator.FromHtml(color);
            r = ToFloatString(colorValue.R);
            g = ToFloatString(colorValue.G);
            b = ToFloatString(colorValue.B);
            a = ToFloatString(colorValue.A);
            return true;
        }
        catch (Exception ex)
        {
            r = g = b = a = string.Empty;
            Log.LogError(null, ErrorCodes.CouldNotParseColor, null, null, 0, 0, 0, 0, $@"Failed to parse color ({color}): {ex.Message}");
        }
        return false;
    }

    private static string ToFloatString(byte v) =>
        (v / 255.0).ToString("0.000", CultureInfo.InvariantCulture);
}
