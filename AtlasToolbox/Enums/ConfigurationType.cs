using System;
using System.ComponentModel;

namespace AtlasToolbox.Enums
{
    public enum ConfigurationType
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
        [Description("Software")]
        Software,
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
        WindowsUpdate,
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
            switch (value)
            {
                case ConfigurationType.General:
                    return App.GetValueFromItemList("GeneralConfig");

                case ConfigurationType.Interface:
                    return App.GetValueFromItemList("Interface");

                case ConfigurationType.Windows:
                    return App.GetValueFromItemList("Windows");

                case ConfigurationType.Advanced:
                    return App.GetValueFromItemList("Advanced");

                case ConfigurationType.Security:
                    return App.GetValueFromItemList("Security");

                case ConfigurationType.Troubleshooting:
                    return App.GetValueFromItemList("Troubleshooting");
                default: return null;
            }
        }
    }
}
