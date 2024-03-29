﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;

namespace FlyingColorsDesktop.Views
{
	internal class ByteImageConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (targetType != typeof(ImageSource))
				throw new InvalidOperationException("The target must be ImageSource or derived types");

			if (value != null && value is List<Byte>)
			{
				List<Byte> bytes = value as List<Byte>;
				if (bytes.Count > 0)
				{
					MemoryStream stream = new MemoryStream(bytes.ToArray());
					BitmapImage image = new BitmapImage();
					image.BeginInit();
					image.StreamSource = stream;
					image.EndInit();
					return image;
				}
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

