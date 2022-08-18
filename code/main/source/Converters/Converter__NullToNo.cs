////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace MVVM{
////////////////////////////////////////////////////////////////////////////////
//class Converter__NullToNo

class Converter__NullToNo:IValueConverter
{
 public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
 {
  if(value==null)
   return "[No]";

  var result
   =value.ToString();

  if(Object.ReferenceEquals(result,null))
   return string.Empty;

  return result;
 }//Convert

 //-----------------------------------------------------------------------         
 public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
 {
  return DependencyProperty.UnsetValue;
 }//ConvertBack
};//class Converter__NullToNo

////////////////////////////////////////////////////////////////////////////////
}//namespace MVVM
