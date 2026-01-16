using System;
using System.Globalization;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MAUI.Native.Embedded.Tasks;

public sealed class UpdateXCAssetsColorset : Task
{
    [Required]
    public ITaskItem[] Items { get; set; } = [];

    [Required]
    public string Color { get; set; } = string.Empty;

    public string DarkColor { get; set; } = "#000000";

    public override bool Execute()
    {
        if (Items.Length == 0)
        {
            return false;
        }

        try
        {
            ParseColor(Color, out var lcr, out var lcg, out var lcb, out var lca);
            ParseColor(DarkColor, out var dcr, out var dcg, out var dcb, out var dca);

            foreach (var item in Items)
            {
                var path = item.ItemSpec;

                if (!path.Contains(".colorset", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                if (!File.Exists(path))
                {
                    continue;
                }

                var json = File.ReadAllText(path)
                    .Replace("{lcr}", lcr)
                    .Replace("{lcg}", lcg)
                    .Replace("{lcb}", lcb)
                    .Replace("{lca}", lca)
                    .Replace("{dcr}", dcr)
                    .Replace("{dcg}", dcg)
                    .Replace("{dcb}", dcb)
                    .Replace("{dca}", dca);

                File.WriteAllText(path, json);
            }
        }
        catch (Exception ex)
        {
            Log.LogError(ex.Message);
            return false;
        }

        return !Log.HasLoggedErrors;
    }

    private static void ParseColor(string color, out string r, out string g, out string b, out string a)
    {
        if (string.IsNullOrWhiteSpace(color) || !color.StartsWith("#"))
        {
            throw new Exception($"Color must be #RRGGBB or #RRGGBBAA, got '{color}'.");
        }

        var hex = color.TrimStart('#');
        if (hex.Length != 6 && hex.Length != 8)
        {
            throw new Exception($"Color must be #RRGGBB or #RRGGBBAA, got '{color}'.");
        }

        byte rr = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
        byte gg = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
        byte bb = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
        byte aa = hex.Length == 8 ? byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber) : (byte)255;

        r = ToFloat(rr);
        g = ToFloat(gg);
        b = ToFloat(bb);
        a = ToFloat(aa);
    }

    private static string ToFloat(byte v) =>
        (v / 255.0).ToString("0.000", CultureInfo.InvariantCulture);
}
