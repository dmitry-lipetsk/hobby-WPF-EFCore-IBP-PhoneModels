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
///  Interaction logic for App.xaml
/// </summary>
public partial class App:Application
{
 private void Application_DispatcherUnhandledException
                             (object sender,
                              System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
 {
  if(sm_cUnhandledExceptions!=0)
  {
   MessageBox.Show
    ("Recursice unhandled application exceptions!",
     "Crictical Error",
     MessageBoxButton.OK);
  }
  else
  {
   ++sm_cUnhandledExceptions;

   Debug.Assert(!Object.ReferenceEquals(e,null));
   Debug.Assert(!Object.ReferenceEquals(e.Exception,null));

   var errWnd
    =new AppErrorWindow
      (/*ownerWindow*/null,
       e.Exception,
       "Application Error",
       "An application error occurred.");;

   errWnd.ShowDialog();

   --sm_cUnhandledExceptions;
  }//else

  Application.Current.Shutdown();

  e.Handled=true;
 }//Application_DispatcherUnhandledException

 //-----------------------------------------------------------------------
 static long sm_cUnhandledExceptions;
};//class App

////////////////////////////////////////////////////////////////////////////////
}//namespace MVVM
