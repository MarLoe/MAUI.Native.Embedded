using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MAUI.Native.Embedded.Tasks;

public sealed class UpdateXCAssetsImageset : Task
{
    [Required]
    public ITaskItem[] Items { get; set; } = [];

    [Required]
    public string Name { get; set; } = "";

    [Required]
    public string ImageName { get; set; } = "";

    public override bool Execute()
    {
        if (Items.Length == 0)
        {
            Log.LogMessage(MessageImportance.Low, $@"No {nameof(Items)} to update.");
            return true;
        }

        try
        {
            if (!Name.EndsWith(".imageset", StringComparison.InvariantCultureIgnoreCase))
            {
                Name += ".imageset";
            }

            foreach (var item in Items)
            {
                var imagesetFile = Path.Combine(item.ItemSpec, Name, "Contents.json");
                if (!File.Exists(imagesetFile))
                {
                    Log.LogError(null, ErrorCodes.ImagesetNotFound, null, null, 0, 0, 0, 0, $@"Imageset with name {Name} was not found in {imagesetFile}");
                    continue;
                }

                var original = File.ReadAllText(imagesetFile);
                var updated = original
                    .Replace("{filename}", ImageName);

                if (original != updated)
                {
                    File.WriteAllText(imagesetFile, updated);
                }
            }
        }
        catch (Exception ex)
        {
            Log.LogErrorFromException(ex);
        }

        return !Log.HasLoggedErrors;
    }

}
