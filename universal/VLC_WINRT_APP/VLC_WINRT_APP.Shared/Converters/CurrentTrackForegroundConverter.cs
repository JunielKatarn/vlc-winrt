﻿using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using VLC_WINRT_APP.ViewModels;
using VLC_WINRT_APP.ViewModels.MusicVM;

namespace VLC_WINRT_APP.Converters
{
    public class CurrentTrackForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is MusicLibraryVM.TrackItem
                && ((MusicLibraryVM.TrackItem) value).Path == Locator.MusicPlayerVM.CurrentTrackItem.Path)
            {
                return App.Current.Resources["MainColor"] as SolidColorBrush;
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
