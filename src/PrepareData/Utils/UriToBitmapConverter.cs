﻿using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace PrepareData.Utils
{
    public class UriToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            if (parameter is int)
            {
                bi.DecodePixelWidth = (int) parameter;
            }
            if (parameter is string)
            {
                int val;
                if (int.TryParse((string) parameter,out val))
                {
                    bi.DecodePixelWidth = val;
                }
            }
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.UriSource = new Uri(value.ToString());
            bi.EndInit();
            return bi;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}