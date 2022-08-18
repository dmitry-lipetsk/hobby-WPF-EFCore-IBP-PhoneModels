////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows;

namespace MVVM{
////////////////////////////////////////////////////////////////////////////////
//class AppErrorWindow

/// <summary>
///  Interaction logic for AppErrorWindow.xaml
/// </summary>
public partial class AppErrorWindow:Window
{
 public AppErrorWindow(Window?   ownerWindow,
                       Exception exc,
                       string    captionText,
                       string    baseDescriptionText)
 {
  Debug.Assert(!Object.ReferenceEquals(exc,null));

  m_Exceptions=new List<Exception>();

  for(var e=exc;!Object.ReferenceEquals(e,null);e=e.InnerException)
  {
   m_Exceptions.Add(e);
  }//for

  InitializeComponent();

  this.ComboBox__ErrorSources.SelectedIndex=0;

  this.Title=captionText;

  this.TextBlock__BaseDescription.Text=baseDescriptionText;

  this.Owner=ownerWindow;

  if(!Object.ReferenceEquals(ownerWindow,null))
  {
   this.Left
    =ownerWindow.Left+(ownerWindow.Width-this.Width)/2;

   this.Top
    =ownerWindow.Top+(ownerWindow.Height-this.Height)/2;
  }//if
 }//AppErrorWindow

 //-----------------------------------------------------------------------
 public List<Exception> Exceptions
 {
  get
  {
   return m_Exceptions;
  }
 }//Exceptions

 //-----------------------------------------------------------------------
 private void Button_Click(object sender,RoutedEventArgs e)
 {
  this.DialogResult=true;
 }//Button_Click

 //-----------------------------------------------------------------------
 private List<Exception> m_Exceptions;
};//class AppErrorWindow

////////////////////////////////////////////////////////////////////////////////
}//namespace
