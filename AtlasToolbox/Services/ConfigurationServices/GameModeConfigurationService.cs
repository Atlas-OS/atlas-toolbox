using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class GameModeConfigurationService : IRoutable
    {
        private const string GAME_BAR_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\GameBar";

        private const string ALLOW_AUTO_GAME_MODE_VALUE_NAME = @"AllowAutoGameMode";
        private const string AUTO_GAME_MODE_ENABLED_VALUE_NAME = @"AutoGameModeEnabled";

        private readonly ConfigurationStore _gameModeConfigurationStore;

        public GameModeConfigurationService(
            [FromKeyedServices("GameMode")] ConfigurationStore gameModeConfigurationStore)
        {
            _gameModeConfigurationStore = gameModeConfigurationStore;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(GAME_BAR_KEY_NAME, ALLOW_AUTO_GAME_MODE_VALUE_NAME, 0);
            RegistryHelper.SetValue(GAME_BAR_KEY_NAME, AUTO_GAME_MODE_ENABLED_VALUE_NAME, 0);

            _gameModeConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(GAME_BAR_KEY_NAME, ALLOW_AUTO_GAME_MODE_VALUE_NAME);
            RegistryHelper.DeleteValue(GAME_BAR_KEY_NAME, AUTO_GAME_MODE_ENABLED_VALUE_NAME);

            _gameModeConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            bool[] checks =
            {
                RegistryHelper.IsMatch(GAME_BAR_KEY_NAME, ALLOW_AUTO_GAME_MODE_VALUE_NAME, null),
                RegistryHelper.IsMatch(GAME_BAR_KEY_NAME, AUTO_GAME_MODE_ENABLED_VALUE_NAME, null)
            };

            return checks.All(x => x);
        }
    }
}
