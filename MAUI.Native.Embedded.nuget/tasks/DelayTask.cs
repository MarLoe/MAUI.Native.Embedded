using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MAUI.Native.Embedded.Tasks
{
    public sealed class Delay : Task
    {
        [Required]
        public string Timeout { get; set; } = string.Empty;

        public override bool Execute()
        {
            if (int.TryParse(Timeout, out var delay))
            {
                Thread.Sleep(delay);
                return true;
            }

            return false;
        }
    }
}
