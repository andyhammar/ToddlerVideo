using System;
using System.Diagnostics;
using System.IO;
using Windows.Security.Authentication.Web.Core;
using Windows.Storage.Pickers;
using Windows.System.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

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
        private DisplayRequest _request;

        public MainPage()
        {
            this.InitializeComponent();

            InitButtonTimer();
            _lockAndPlayButton.Visibility = Visibility.Collapsed;
            _mediaElement.CurrentStateChanged += _mediaElement_CurrentStateChanged;
        }

        private void _mediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateLockScreenPrevention(_mediaElement.CurrentState);
            }
            catch (Exception exception)
            {
                Debug.WriteLine("error preventing lock screen:  " + exception);
            }
        }

        private void UpdateLockScreenPrevention(MediaElementState currentState)
        {
            if (currentState != MediaElementState.Playing)
            {
                if (_request == null) return;

                _request.RequestRelease();
                _request = null;
                return;
            }

            if (_request != null) return;

            _request = new DisplayRequest();
            _request.RequestActive();
        }


        private void InitButtonTimer()
        {
            if (_buttonTimer != null) return;

            _buttonTimer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(2000)};
            _buttonTimer.Tick += (sender, e) =>
            {
                _buttonTimer.Stop();
                _lastButtonId = string.Empty;
            };
        }

        private async void PickFileButtonClick(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId
                .VideosLibrary
            };
=======
            var picker = new FileOpenPicker {SuggestedStartLocation = PickerLocationId.VideosLibrary};
>>>>>>> preventing lock screen when playing
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".avi");
            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                var stream = await file.OpenStreamForReadAsync();
                _mediaElement.SetSource(stream.AsRandomAccessStream(), "video/mp4");
                _lockAndPlayButton.Visibility = Visibility.Visible;

                LockAndPlay();
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
            if (_mediaElement == null) return;

            _mediaElement.Play();

            _playButtonsPanel.Visibility = Visibility.Collapsed;
            _mediaElement.AreTransportControlsEnabled = false;

            var applicationView = ApplicationView.GetForCurrentView();
            applicationView.TryEnterFullScreenMode();
        }
    }
}
