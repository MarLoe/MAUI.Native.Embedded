using System;
using System.Drawing;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MAUI.Native.Embedded.Tasks;

public sealed class PrepareMauiNativeSplashScreen : Task
{
    [Required]
    public ITaskItem[] MauiSplashScreen { get; set; } = [];

    [Output]
    public ITaskItem[] MauiNativeSplashScreen { get; set; } = [];

    public override bool Execute()
    {
        if (MauiSplashScreen.Length == 0)
        {
            Log.LogMessage(MessageImportance.Low, $@"No {nameof(MauiSplashScreen)} to prepare.");
            return true;
        }

        try
        {
            MauiNativeSplashScreen = [.. MauiSplashScreen.Where(i => i.MetadataNames.OfType<string>().Any(n => HasDarkMode(n)))];
            foreach (var item in MauiNativeSplashScreen)
            {
                var color = UpdateMetadata(item, "Color");
                var tintColor = UpdateMetadata(item, "TintColor");
                UpdateMetadata(item, "DarkColor", color);
                UpdateMetadata(item, "DarkTintColor", tintColor);
            }
        }
        catch (Exception ex)
        {
            Log.LogErrorFromException(ex);
        }

        return !Log.HasLoggedErrors;
    }

    private string UpdateMetadata(ITaskItem item, string metadataName, string defaultValue = "")
    {
        var color = item.GetMetadata(metadataName);
        if (!TryParseColor(color, out var hexColor) && !string.IsNullOrWhiteSpace(defaultValue))
        {
            hexColor = defaultValue;
        }

        if (!string.IsNullOrWhiteSpace(hexColor))
        {
            item.SetMetadata(metadataName, hexColor);
        }

        return hexColor;
    }

    private bool TryParseColor(string color, out string hexColor)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(color))
            {
                hexColor = string.Empty;
                return false;
            }

            var colorValue = ColorTranslator.FromHtml(color);
            hexColor = $@"#{colorValue.A:X2}{colorValue.R:X2}{colorValue.G:X2}{colorValue.B:X2}";
            return true;
        }
        catch (Exception ex)
        {
            hexColor = "";
            Log.LogError(null, ErrorCodes.CouldNotParseColor, null, null, 0, 0, 0, 0, $@"Failed to parse color ({color}): {ex.Message}");
        }
        return false;
    }

    private static bool HasDarkMode(string metadataName) =>
        metadataName.Equals("DarkColor", StringComparison.InvariantCultureIgnoreCase) ||
        metadataName.Equals("DarkTintColor", StringComparison.InvariantCultureIgnoreCase);
}
