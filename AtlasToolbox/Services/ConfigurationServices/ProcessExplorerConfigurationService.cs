using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AtlasToolbox.Commands.ConfigurationButtonsCommand;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class ProcessExplorerConfigurationService : IRoutable
    {
        private readonly ConfigurationStore _configurationStore;

        public ProcessExplorerConfigurationService(
            [FromKeyedServices("ProcessExplorer")] ConfigurationStore configurationStore)
        {
            _configurationStore = configurationStore;
        }

        public void Disable()
        {
            ICommand command = new UninstallProcessExplorer();
            command.Execute(this);

            _configurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            ICommand command = new InstallProcessExplorer();
            command.Execute(this);

            _configurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
           return RegistryHelper.IsMatch(@"HKLM\SOFTWARE\AtlasOS\Services\ProcessExplorer", "state", 1);
        }
    }
}
