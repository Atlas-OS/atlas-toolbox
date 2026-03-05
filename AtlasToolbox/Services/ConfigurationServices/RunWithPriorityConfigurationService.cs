using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class RunWithPriorityConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\RunWithPriority";
        private const string STATE_VALUE_NAME = "state";

        private const string PRIORITY_KEY_NAME = @"HKCR\exefile\shell\Priority";
        private const string MUI_VERB_VALUE_NAME = "MUIVerb";
        private const string SUB_COMMANDS_VALUE_NAME = "SubCommands";

        private const string ONE_FLYOUT_KEY_NAME = @"HKCR\exefile\Shell\Priority\shell\001flyout";
        private const string ONE_COMMAND_KEY_NAME = @"HKCR\exefile\Shell\Priority\shell\001flyout\command";
        private const string TWO_FLYOUT_KEY_NAME = @"HKCR\exefile\Shell\Priority\shell\002flyout";
        private const string TWO_COMMAND_KEY_NAME = @"HKCR\exefile\Shell\Priority\shell\002flyout\command";
        private const string THREE_FLYOUT_KEY_NAME = @"HKCR\exefile\Shell\Priority\shell\003flyout";
        private const string THREE_COMMAND_KEY_NAME = @"HKCR\exefile\Shell\Priority\shell\003flyout\command";
        private const string FOUR_FLYOUT_KEY_NAME = @"HKCR\exefile\Shell\Priority\shell\004flyout";
        private const string FOUR_COMMAND_KEY_NAME = @"HKCR\exefile\Shell\Priority\shell\004flyout\command";
        private const string FIVE_FLYOUT_KEY_NAME = @"HKCR\exefile\Shell\Priority\shell\005flyout";
        private const string FIVE_COMMAND_KEY_NAME = @"HKCR\exefile\Shell\Priority\shell\005flyout\command";
        private const string SIX_FLYOUT_KEY_NAME = @"HKCR\exefile\Shell\Priority\shell\006flyout";
        private const string SIX_COMMAND_KEY_NAME = @"HKCR\exefile\Shell\Priority\shell\006flyout\command";


        private readonly ConfigurationStore _runWithPriorityConfigurationService;

        public RunWithPriorityConfigurationService(
            [FromKeyedServices("RunWithPriority")] ConfigurationStore runWithPriorityConfigurationService)
        {
            _runWithPriorityConfigurationService = runWithPriorityConfigurationService;
        }

        public void Disable()
        {
            RegistryHelper.DeleteKey(PRIORITY_KEY_NAME);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\Context Menus\Run With Priority\Remove Run With Priority In Context Menu (default).cmd");

        }

        public void Enable()
        {
            RegistryHelper.SetValue(PRIORITY_KEY_NAME, MUI_VERB_VALUE_NAME, "Run with priority");
            RegistryHelper.SetValue(PRIORITY_KEY_NAME, SUB_COMMANDS_VALUE_NAME, "");
            RegistryHelper.SetValue(ONE_FLYOUT_KEY_NAME, "", "Realtime");
            RegistryHelper.SetValue(ONE_COMMAND_KEY_NAME, "", "powershell start -file 'cmd' -args '/c start \"\"\"Realtime App\"\"\" /Realtime \"\"\"%1\"\"\"' -verb runas");
            RegistryHelper.SetValue(TWO_FLYOUT_KEY_NAME, "", "High");
            RegistryHelper.SetValue(TWO_COMMAND_KEY_NAME, "", "cmd /c start \"\" /High \"%1\\");
            RegistryHelper.SetValue(THREE_FLYOUT_KEY_NAME, "", "Above normal");
            RegistryHelper.SetValue(THREE_COMMAND_KEY_NAME, "", "cmd /c start \"\" /AboveNormal \"%1\"");
            RegistryHelper.SetValue(FOUR_FLYOUT_KEY_NAME, "", "Normal");
            RegistryHelper.SetValue(FOUR_COMMAND_KEY_NAME, "", "cmd /c start \"\" /Normal \"%1\"");
            RegistryHelper.SetValue(FIVE_FLYOUT_KEY_NAME, "", "Below normal");
            RegistryHelper.SetValue(FIVE_COMMAND_KEY_NAME, "", "cmd /c start \"\" /BelowNormal \"%1\"");
            RegistryHelper.SetValue(SIX_FLYOUT_KEY_NAME, "", "Low");
            RegistryHelper.SetValue(SIX_COMMAND_KEY_NAME, "", "cmd /c start \"\" /Low \"%1\"");

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\Context Menus\Run With Priority\Add Run With Priority In Context Menu.cmd");
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
