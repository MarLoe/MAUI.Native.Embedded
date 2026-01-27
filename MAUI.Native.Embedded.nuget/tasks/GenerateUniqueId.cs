using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MAUI.Native.Embedded.Tasks
{
    public sealed class GenerateUniqueId : Task
    {
        [Required]
        public ITaskItem[] Items { get; set; } = [];

        public string[] Exclude { get; set; } = [];

        [Output]
        public ITaskItem[] OutputItems { get; set; } = [];

        public override bool Execute()
        {
            foreach (var item in Items)
            {
                // If UniqueId already exists, skip
                var existing = item.GetMetadata("UniqueId");
                if (!string.IsNullOrEmpty(existing))
                    continue;

                var seed = string.Join(";",
                    item.CloneCustomMetadata().Keys
                        .OfType<string>()
                        .Where(k => !k.StartsWith("_", StringComparison.Ordinal))
                        .Where(k => Exclude == null || !Exclude.Contains(k, StringComparer.Ordinal))
                        .Distinct(StringComparer.Ordinal)
                        .OrderBy(k => k, StringComparer.Ordinal)
                        .Select(k => $"{k}={item.GetMetadata(k)}"));
                var hash = MD5.HashData(Encoding.UTF8.GetBytes(seed));

#if NET9_0_OR_GREATER
                item.SetMetadata("UniqueId", Convert.ToHexStringLower(hash));
#else
                item.SetMetadata("UniqueId", BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant());
#endif
            }

            OutputItems = Items;

            return true;
        }
    }
}
