using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string _lastButtonId;
        private DispatcherTimer _buttonTimer;

        public MainPage()
        {
            this.InitializeComponent();

            var applicationView = ApplicationView.GetForCurrentView();
            applicationView.TryEnterFullScreenMode();

            if (_buttonTimer == null)
            {
                _buttonTimer = new DispatcherTimer();
                _buttonTimer.Interval = TimeSpan.FromMilliseconds(1000);
                _buttonTimer.Tick += (sender, e) =>
                {
                    _buttonTimer.Stop();
                    _lastButtonId = string.Empty;
                };
            }

        }

        private void _buttonTimer_Tick(object sender, object e)
        {
            throw new NotImplementedException();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".avi");
            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                var stream = await file.OpenStreamForReadAsync();
                _pickFileButton.Visibility = Visibility.Collapsed;
                _mediaElement.SetSource(stream.AsRandomAccessStream(), "video/mp4");
            }
        }

        private void _mediaElement_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("ME tapped");
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("grid tapped");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var buttonId = (sender as Button)?.Content as string;
            if (buttonId == null) return;

            if (buttonId == "2" && _lastButtonId == "1")
            {
                _mediaElement.AreTransportControlsEnabled = !_mediaElement.AreTransportControlsEnabled;
                _pickFileButton.Visibility = _mediaElement.AreTransportControlsEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
            _lastButtonId = buttonId;
            _buttonTimer.Stop();
            _buttonTimer.Start();
        }
    }
}
