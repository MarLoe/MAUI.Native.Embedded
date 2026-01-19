using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MAUI.Native.Embedded.Tasks
{
    public sealed class ReplaceInFile : Task
    {
        [Required]
        public ITaskItem[] Items { get; set; } = [];

        [Required]
        public string Pattern { get; set; } = string.Empty;

        [Required]
        public string Replacement { get; set; } = string.Empty;

        public override bool Execute()
        {
            foreach (var item in Items)
            {
                var path = item.ItemSpec;
                if (!File.Exists(path))
                {
                    Log.LogWarning($"File not found: {path}");
                    continue;
                }

                var original = File.ReadAllText(path);
                var updated = Regex.Replace(original, Pattern, Replacement, RegexOptions.Singleline);

                if (original != updated)
                {
                    File.WriteAllText(path, updated);
                }
            }

            return true;
        }
    }
}
