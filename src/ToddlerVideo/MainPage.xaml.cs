using System;
using System.Diagnostics;
using System.IO;
using Windows.Storage.Pickers;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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

            InitButtonTimer();
            _lockAndPlayButton.Visibility = Visibility.Collapsed;
        }

        private void InitButtonTimer()
        {
            if (_buttonTimer != null) return;

            _buttonTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(2000) };
            _buttonTimer.Tick += (sender, e) =>
            {
                _buttonTimer.Stop();
                _lastButtonId = string.Empty;
            };
        }

        private async void PickFileButtonClick(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker { SuggestedStartLocation = PickerLocationId.VideosLibrary };
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".avi");
            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                var stream = await file.OpenStreamForReadAsync();
                _playButtonsPanel.Visibility = Visibility.Collapsed;
                _lockAndPlayButton.Visibility = Visibility.Visible;
                _mediaElement.SetSource(stream.AsRandomAccessStream(), "video/mp4");
            }
        }


        private void NumberButtonClick(object sender, RoutedEventArgs e)
        {
            var buttonId = (sender as Button)?.Content as string;
            if (buttonId == null) return;

            if (buttonId == "2" && _lastButtonId == "1")
            {
                _mediaElement.AreTransportControlsEnabled = !_mediaElement.AreTransportControlsEnabled;
                _playButtonsPanel.Visibility = _mediaElement.AreTransportControlsEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
            _lastButtonId = buttonId;
            _buttonTimer.Stop();
            _buttonTimer.Start();
        }

        private void _lockAndPlayButton_OnClick(object sender, RoutedEventArgs e)
        {
            LockAndPlay();
        }

        private void LockAndPlay()
        {
            if (_mediaElement.Source != null)
            {
                _mediaElement.Play();
            }
            _playButtonsPanel.Visibility = Visibility.Collapsed;
            _mediaElement.AreTransportControlsEnabled = false;

            var applicationView = ApplicationView.GetForCurrentView();
            applicationView.TryEnterFullScreenMode();
        }
    }
}
