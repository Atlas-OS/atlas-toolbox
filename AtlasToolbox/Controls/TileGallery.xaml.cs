using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AtlasToolbox.Controls
{
    public sealed partial class TileGallery : UserControl
    {
        public TileGallery()
        {
            this.InitializeComponent();
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
