using System;
using System.IO;
using System.Security.AccessControl;

namespace AtlasToolbox.Utils
{
    /// <summary>
    /// Folders the elevated Toolbox writes and reads its own data in. Under ProgramData
    /// every user may create files, so each folder from AtlasOS down gets an access list of
    /// its own: SYSTEM and Administrators change it, users only read. A folder that is a
    /// link is refused, so nothing can redirect the Toolbox's writes.
    /// </summary>
    public static class ProtectedFolder
    {
        private const string Sddl = "O:BAG:BAD:P(A;OICI;FA;;;SY)(A;OICI;FA;;;BA)(A;OICI;0x1200a9;;;BU)";

        /// <summary>Creates or secures <paramref name="path"/> and the folders above it up to <paramref name="root"/>.</summary>
        public static string Ensure(string root, string path)
        {
            string fullRoot = Path.GetFullPath(root).TrimEnd('\\');
            string fullPath = Path.GetFullPath(path).TrimEnd('\\');
            if (!fullPath.StartsWith(fullRoot, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("The folder isn't inside the Toolbox's data.", nameof(path));
            }
            string parent = Path.GetDirectoryName(fullRoot);
            foreach (string part in fullPath[(parent.Length + 1)..].Split('\\'))
            {
                parent = Path.Combine(parent, part);
                Secure(parent);
            }
            return fullPath;
        }

        private static void Secure(string path)
        {
            DirectorySecurity security = new();
            security.SetSecurityDescriptorSddlForm(Sddl);
            DirectoryInfo directory = new(path);
            if (!directory.Exists)
            {
                directory.Create(security);
                directory.Refresh();
            }
            if (directory.Attributes.HasFlag(FileAttributes.ReparsePoint))
            {
                throw new IOException($"'{path}' is a link, so the Toolbox doesn't use it.");
            }
            directory.SetAccessControl(security);
        }
    }
}
