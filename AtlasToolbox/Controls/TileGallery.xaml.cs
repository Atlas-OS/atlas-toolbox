using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// Taken from https://github.com/microsoft/WinUI-Gallery/blob/1f88f9327e23ca658b45d4df0530a663eddfbe11/WinUIGallery/Controls/TileGallery.xaml.cs
namespace AtlasToolbox.Controls
{
    public sealed partial class TileGallery : UserControl
    {
        public TileGallery()
        {
            this.InitializeComponent();
            SetText();
        }

        private void SetText()
        {
            DocumentationTile.Title = App.GetValueFromItemList("Tile_DocumentationTitle");
            DocumentationTile.Description = App.GetValueFromItemList("Tile_DocumentationDescription");
            GithubTile.Title = App.GetValueFromItemList("Tile_GithubTitle");
            GithubTile.Description = App.GetValueFromItemList("Tile_GithubDescription");
            DiscordTile.Title = App.GetValueFromItemList("Tile_DiscordTitle");
            DiscordTile.Description = App.GetValueFromItemList("Tile_DiscordDescription");
            KofiTile.Title = App.GetValueFromItemList("Tile_KofiTitle");
            KofiTile.Description = App.GetValueFromItemList("Tile_KofiDescription");
        }

        private void scroller_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (e.FinalView.HorizontalOffset < 1)
            {
                ScrollBackBtn.Visibility = Visibility.Collapsed;
            }
            else if (e.FinalView.HorizontalOffset > 1)
            {
                ScrollBackBtn.Visibility = Visibility.Visible;
            }

            if (e.FinalView.HorizontalOffset > scroller.ScrollableWidth - 1)
            {
                ScrollForwardBtn.Visibility = Visibility.Collapsed;
            }
            else if (e.FinalView.HorizontalOffset < scroller.ScrollableWidth - 1)
            {
                ScrollForwardBtn.Visibility = Visibility.Visible;
            }
        }

        private void ScrollBackBtn_Click(object sender, RoutedEventArgs e)
        {
            scroller.ChangeView(scroller.HorizontalOffset - scroller.ViewportWidth, null, null);
            // Manually focus to ScrollForwardBtn since this button disappears after scrolling to the end.          
            ScrollForwardBtn.Focus(FocusState.Programmatic);
        }

        private void ScrollForwardBtn_Click(object sender, RoutedEventArgs e)
        {
            scroller.ChangeView(scroller.HorizontalOffset + scroller.ViewportWidth, null, null);

            // Manually focus to ScrollBackBtn since this button disappears after scrolling to the end.    
            ScrollBackBtn.Focus(FocusState.Programmatic);
        }

        private void scroller_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateScrollButtonsVisibility();
        }

        private void UpdateScrollButtonsVisibility()
        {
            if (scroller.ScrollableWidth > 0)
            {
                ScrollForwardBtn.Visibility = Visibility.Visible;
            }
            else
            {
                ScrollForwardBtn.Visibility = Visibility.Collapsed;
            }
        }
    }
}
