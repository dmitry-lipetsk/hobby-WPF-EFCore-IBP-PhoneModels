////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace MVVM{
////////////////////////////////////////////////////////////////////////////////
//class Converter__BinaryToImageSource

class Converter__BinaryToImageSource:IValueConverter
{
 public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
 {
  Debug.Assert(!Object.ReferenceEquals(targetType,null));
  Debug.Assert(targetType==typeof(ImageSource));

  if(value==null)
   return null;

  Debug.Assert(value is byte[]);

  ImageSource?
   result
    =ImageUtils.BinaryToImageSource((byte[])value);

  //expected
  Debug.Assert(!Object.ReferenceEquals(result,null));

  return result;
 }//Convert

 //-----------------------------------------------------------------------         
 public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
 {
  return DependencyProperty.UnsetValue;
 }//ConvertBack
};//class Converter__BinaryToImageSource

////////////////////////////////////////////////////////////////////////////////
}//namespace MVVM
