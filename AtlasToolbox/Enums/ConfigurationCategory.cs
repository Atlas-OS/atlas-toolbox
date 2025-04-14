using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AtlasToolbox.Enums
{
    public enum ConfigurationCategory
    {
        [Description("General Configuration")]
        General,
        [Description("Interface Tweaks")]
        Interface,
        [Description("Windows Settings")]
        Windows,
        [Description("Advanced Configuration")]
        Advanced,
        [Description("Security")]
        Security,
        [Description("Troubleshooting")]
        Troubleshooting,
        ContextMenuSubMenu,
        AiSubMenu,
        ServicesSubMenu,
        CpuIdleSubMenu,
        BootConfigurationSubMenu,
        FileExplorerSubMenu,
        StartMenuSubMenu,
        BootConfigAppearance,
        BootConfigBehavior,
        DriverConfigurationSubMenu,
        NvidiaDisplayContainerSubMenu,
        CoreIsolationSubMenu,
        DefenderSubMenu,
        MitigationsSubMenu,
        TroubleshootingNetwork,
        FileSharingSubMenu,
    }

    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the description of an Enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
