////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace MVVM{
////////////////////////////////////////////////////////////////////////////////
//class Converter__IconToImageSource

sealed class Converter__IconToImageSource:IValueConverter
{
 public object? Convert(object value,Type targetType,object parameter,CultureInfo culture)
 {
  Debug.Assert(!Object.ReferenceEquals(value,null));
  Debug.Assert(value is Icon);
  Debug.Assert(!Object.ReferenceEquals(targetType,null));
  Debug.Assert(targetType==typeof(ImageSource));

  var icon=value as Icon;

  if(Object.ReferenceEquals(icon,null))
  {
   Trace.TraceWarning("Attempted to convert {0} instead of Icon object in IconToImageSourceConverter",value);

   return null;
  }//if

  ImageSource imageSource
   =Imaging.CreateBitmapSourceFromHIcon
     (icon.Handle,
      Int32Rect.Empty,
      BitmapSizeOptions.FromEmptyOptions());

  return imageSource;
 }//Convert

 //-----------------------------------------------------------------------         
 public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
 {
  return DependencyProperty.UnsetValue;
 }//ConvertBack
};//class Converter__IconToImageSource

////////////////////////////////////////////////////////////////////////////////
}//namespace MVVM
