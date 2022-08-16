////////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM{
////////////////////////////////////////////////////////////////////////////////
//class App

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App:Application
{
 private void Application_DispatcherUnhandledException(object sender,System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
 {
  Debug.Assert(!Object.ReferenceEquals(e,null));
  Debug.Assert(!Object.ReferenceEquals(e.Exception,null));

  var msgBuilder
   =new StringBuilder();

  for(var exc=e.Exception;!Object.ReferenceEquals(exc,null);exc=exc.InnerException)
  {
   if(msgBuilder.Length!=0)
   {
    msgBuilder.Append(Environment.NewLine);
   }

   msgBuilder.AppendFormat("------------------------- [{0}]",exc.Source);
   msgBuilder.Append(Environment.NewLine);
   msgBuilder.Append(exc.Message);
  }//for

  string errorMessage
   =string.Format
     ("An application error occurred.\n\n"
      +"Error:{0}\n\n"
      +"Application will be closed.",
       msgBuilder.ToString());

  var r1
   =MessageBox.Show
     (errorMessage,
       "Application UnhandledException Error",
       MessageBoxButton.OK,
       MessageBoxImage.Error);

  Application.Current.Shutdown();

  e.Handled=true;
 }//Application_DispatcherUnhandledException
};//class App

////////////////////////////////////////////////////////////////////////////////
}//namespace MVVM
