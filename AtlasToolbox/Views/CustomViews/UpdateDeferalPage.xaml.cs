using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using AtlasToolbox.Commands.ConfigurationButtonsCommand;
using AtlasToolbox.Utils;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Web.WebView2.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AtlasToolbox.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UpdateDeferralPage : Page
    {
        public UpdateDeferralPage()
        {
            this.InitializeComponent();
            try
            {
                FeatureBox.Value = int.Parse(RegistryHelper.GetValue("HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\WindowsUpdate", "DeferFeatureUpdatesPeriodInDays").ToString());
                QualityBox.Value = int.Parse(RegistryHelper.GetValue("HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\WindowsUpdate", "DeferQualityUpdatesPeriodInDays").ToString());
            }
            catch
            {
                FeatureBox.Value = 0;
                QualityBox.Value = 0;
            }
            LoadText();
        }

        private void LoadText()
        {
            FeatureHeader.Text = App.GetValueFromItemList("FeatureHeader");
            QualityHeader.Text = App.GetValueFromItemList("QualityHeader");
        }
        public async void ShowUpdateDeferralPrompt()
        {
            await DispatcherQueue.EnqueueAsync(async () =>
            {
                ContentDialog dialog = new ContentDialog();

                dialog.XamlRoot = App.XamlRoot;
                dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                dialog.Title = App.GetValueFromItemList("UpdateDeferralPromptTitle");
                dialog.Content = this;
                dialog.PrimaryButtonText = App.GetValueFromItemList("Set");
                dialog.CloseButtonText = App.GetValueFromItemList("Cancel");
                dialog.DefaultButton = ContentDialogButton.Primary;


                try
                {
                    var result = await dialog.ShowAsync();
                    if (result == ContentDialogResult.Primary)
                    {
                        if (FeatureBox.Value is double.NaN) FeatureBox.Value = 0;
                        if (QualityBox.Value is double.NaN) QualityBox.Value = 0;
                        SetUpdateDeferralConfigurationButton.SetUpdateDeferral(FormatDoubleInt(FeatureBox.Value), FormatDoubleInt(QualityBox.Value));
                    }
                }
                catch
                { App.logger.Error("Program tried to open more than one ContentDialog"); }
            });
        }

        private int FormatDoubleInt(double value)
        {
            string valueString = value.ToString();
            string[] valueArr = valueString.Split('.');
            return int.Parse(valueArr[0]);
        }
    }
}
