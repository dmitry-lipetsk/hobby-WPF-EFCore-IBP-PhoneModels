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
//class Converter__NullToFalse

class Converter__NullToFalse:IValueConverter
{
 public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
 {
  if(Object.ReferenceEquals(value,null))
   return false;

  return true;
 }//Convert

 //-----------------------------------------------------------------------         
 public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
 {
  return DependencyProperty.UnsetValue;
 }//ConvertBack
};//class Converter__NullToFalse

////////////////////////////////////////////////////////////////////////////////
}//namespace MVVM
