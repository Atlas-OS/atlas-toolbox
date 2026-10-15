using AtlasToolbox.Utils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AtlasToolbox.Atlas
{
    /// <summary>
    /// Applies a toggle state from the interface: asks first when Atlas has a warning for
    /// it, says why when it fails, and finishes what the state asks for afterwards.
    /// Runs on the interface thread and never throws.
    /// </summary>
    public static class ToggleActions
    {
        // One dialog at a time: WinUI refuses a second, which would silently cancel a
        // change waiting for its warning to be answered.
        private static readonly SemaphoreSlim DialogGate = new(1, 1);

        // While a profile applies several changes, one restart prompt comes at the end.
        private static int _batchDepth;
        private static bool _restartWanted;

        /// <summary>Runs several changes, asking about a restart once, after all of them.</summary>
        public static async Task RunBatchAsync(Func<Task> changes)
        {
            _batchDepth++;
            try
            {
                await changes();
            }
            finally
            {
                if (--_batchDepth == 0 && _restartWanted)
                {
                    _restartWanted = false;
                    await PromptRestartAsync();
                }
            }
        }

        public static async Task<bool> ApplyAsync(CatalogToggle toggle, CatalogState state, bool? interactive = null)
        {
            // A run in a window shows Atlas's own warning; a silent one can't, so it is
            // asked here.
            bool silent = !(interactive ?? (toggle.NoStateRecord || state.Work.User));
            if (silent && !string.IsNullOrWhiteSpace(toggle.Warning) &&
                !await ConfirmAsync(App.GetValueFromItemList("ToggleWarningTitle"), toggle.Warning))
            {
                return false;
            }

            ToggleResult result;
            try
            {
                result = await ToggleRunner.RunAsync(toggle, state, interactive);
            }
            catch (Exception ex)
            {
                App.logger.Error(ex, $"[TOGGLE] {toggle.Name} {state.Name} could not start");
                result = new ToggleResult(false, -1, ex.Message, false, AsUser: false);
            }

            if (result.Succeeded)
            {
                await FinishAsync(state, result);
            }
            else if (!result.Interactive)
            {
                // A run in a window already showed why it stopped.
                await ShowAsync(App.GetValueFromItemList("ToggleFailedTitle"), result.Message ?? App.GetValueFromItemList("ToggleFailedDesc"));
            }
            AtlasContract.Refresh();
            return result.Succeeded;
        }

        /// <summary>Runs an action toggle (one that records no choice) by name.</summary>
        public static async Task<bool> RunActionAsync(string toggleName, string stateName, bool? interactive = null)
        {
            CatalogToggle toggle = AtlasContract.Catalog.Find(toggleName);
            CatalogState state = toggle?.State(stateName);
            if (state is null)
            {
                App.logger.Error($"[TOGGLE] This Atlas has no {toggleName} {stateName}");
                await ShowAsync(App.GetValueFromItemList("ToggleFailedTitle"), App.GetValueFromItemList("ToggleUnavailable"));
                return false;
            }
            return await ApplyAsync(toggle, state, interactive);
        }

        /// <summary>
        /// Runs a launcher from the Atlas folder as the signed-in user, as opening it there
        /// does. <paramref name="launcher"/> is relative to AtlasDesktop.
        /// </summary>
        public static async Task RunLauncherAsync(string launcher)
        {
            try
            {
                await ToggleRunner.RunLauncherAsUserAsync(Path.Combine(AtlasPaths.AtlasDesktop, launcher));
            }
            catch (Exception ex)
            {
                // A launcher that ran shows its own result; this one never started.
                App.logger.Error(ex, $"[TOGGLE] {launcher} could not start");
                await ShowAsync(App.GetValueFromItemList("ToggleFailedTitle"), ex.Message);
            }
            AtlasContract.Refresh();
        }

        /// <summary>
        /// Clears the list of apps the fault tolerant heap applies its mitigations to, with
        /// the command Microsoft documents for it.
        /// </summary>
        public static async Task ResetFaultTolerantHeapAsync()
        {
            ProcessStartInfo start = new(Path.Combine(AtlasPaths.System32, "rundll32.exe"), "fthsvc.dll,FthSysprepSpecialize")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            try
            {
                using Process process = Process.Start(start);
                await process.WaitForExitAsync();
                App.logger.Info($"[FTH] Reset exited {process.ExitCode}");
            }
            catch (Exception ex)
            {
                App.logger.Error(ex, "[FTH] Reset failed");
                await ShowAsync(App.GetValueFromItemList("ToggleFailedTitle"), ex.Message);
            }
        }

        /// <summary>
        /// What a state asks for after it applied. A run in a window, and any run as the
        /// signed-in user, has done this itself; Atlas leaves restarting File Explorer to
        /// an elevated caller, which does it as the signed-in user.
        /// </summary>
        private static async Task FinishAsync(CatalogState state, ToggleResult result)
        {
            if (result.Interactive) return;
            switch (state.Reboot)
            {
                case "RestartExplorer" when !result.AsUser:
                    CatalogToggle restart = AtlasContract.Catalog.Find("RestartExplorer");
                    ToggleResult restarted = null;
                    try
                    {
                        if (restart?.State("Run") is CatalogState run) restarted = await ToggleRunner.RunAsync(restart, run, interactive: false);
                    }
                    catch (Exception ex)
                    {
                        App.logger.Warn(ex, "[TOGGLE] Couldn't restart File Explorer");
                    }
                    if (restarted?.Succeeded != true) await PromptSignOutAsync();
                    break;
                case "Recommend":
                case "Prompt":
                    if (_batchDepth > 0) _restartWanted = true;
                    else await PromptRestartAsync();
                    break;
            }
        }

        private static Task PromptRestartAsync() => AskAsync(
            App.GetValueFromItemList("RestartPC"), App.GetValueFromItemList("RestartPCDesc"),
            App.GetValueFromItemList("Restart"), ComputerStateHelper.RestartComputer);

        private static Task PromptSignOutAsync() => AskAsync(
            App.GetValueFromItemList("RelogApply"), App.GetValueFromItemList("RelogApplyDesc"),
            App.GetValueFromItemList("RelogBtn"), ComputerStateHelper.LogOffComputer);

        private static async Task AskAsync(string title, string text, string action, Action onAccept)
        {
            if (await ShowDialogAsync(title, text, action, App.GetValueFromItemList("Later"), ContentDialogButton.Primary)) onAccept();
        }

        private static Task<bool> ConfirmAsync(string title, string text) =>
            ShowDialogAsync(title, text, App.GetValueFromItemList("Continue"), App.GetValueFromItemList("Cancel"), ContentDialogButton.Close);

        private static Task ShowAsync(string title, string text) =>
            ShowDialogAsync(title, text, null, App.GetValueFromItemList("Ok"), ContentDialogButton.Close);

        /// <summary>Shows one dialog after any open one closes; true when its action was chosen.</summary>
        private static async Task<bool> ShowDialogAsync(string title, string text, string action, string close, ContentDialogButton defaultButton)
        {
            await DialogGate.WaitAsync();
            try
            {
                ContentDialog dialog = new()
                {
                    XamlRoot = App.XamlRoot,
                    Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    Title = title,
                    Content = new TextBlock { Text = text, TextWrapping = TextWrapping.Wrap },
                    CloseButtonText = close,
                    DefaultButton = defaultButton,
                };
                if (action is not null) dialog.PrimaryButtonText = action;
                return await dialog.ShowAsync() == ContentDialogResult.Primary;
            }
            catch (Exception ex)
            {
                // Without an answer, nothing is changed.
                App.logger.Warn(ex, $"[TOGGLE] Couldn't show: {title}");
                return false;
            }
            finally
            {
                DialogGate.Release();
            }
        }
    }
}
