////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using Microsoft.Win32;
using System.IO;

namespace MVVM{
////////////////////////////////////////////////////////////////////////////////
//class MainWindow

public partial class MainWindow:Window
{
 private void Helper__Cmd__AddPhone__Execute(object? parameter)
 {
  var c=Helper__GetModel();

  Phone phone=new Phone();

  c.Phones.Add(phone);

  ListBox__Phones.SelectedItem=phone;

  ListBox__Phones.ScrollIntoView(phone);
 }//Helper__Cmd__AddPhone__Execute

 //-----------------------------------------------------------------------
 private void Helper__Cmd__DelPhone__Execute(object? parameter)
 {
  Debug.Assert(!Object.ReferenceEquals(null,parameter));
  Debug.Assert(parameter is Phone);

  var c=Helper__GetModel();

  var phone=(Phone)parameter;

  Debug.Assert(c.Phones.Contains(phone));

  c.Phones.Remove(phone);
 }//Helper__Cmd__DelPhone__Execute

 //-----------------------------------------------------------------------
 private bool Helper__Cmd__DelPhone__CanExecute(object? parameter)
 {
  if(Object.ReferenceEquals(parameter,null))
   return false;

  Debug.Assert(parameter is Phone);

#if DEBUG
  var c=Helper__GetModel();

  Debug.Assert(c.Phones.Contains((Phone)parameter));
#endif

  return true;
 }//Helper__Cmd__DelPhone__CanExecute

 //-----------------------------------------------------------------------
 private void Helper__Cmd__NewWindow__Execute(object? parameter)
 {
  var newWindow
   =new MainWindow(Helper__GetModel());

  newWindow.ListBox__Phones.SelectedItem
   =this.ListBox__Phones.SelectedItem;

  //Clone window size
  newWindow.Width
   =this.Width;

  newWindow.Height
   =this.Height;

  newWindow.Show();

  newWindow.ListBox__Phones.ScrollIntoView(newWindow.ListBox__Phones.SelectedItem);
 }//Helper__Cmd__NewWindow__Execute

 //-----------------------------------------------------------------------
 private void Helper__Cmd__Save__Execute(object? parameter)
 {
  var model
   =Helper__GetModel();

  var prevCursor=this.Cursor;

  this.Cursor=Cursors.Wait;

  try
  {
   try
   {
    model.SaveChanges();
   }
   finally
   {
    this.Cursor=prevCursor;
   }//finally
  }
  catch(Exception exc)
  {
   var errWnd
    =new AppErrorWindow
      (/*ownerWindow*/this,
       exc,
       "Save Error",
       "An save error occurred.");

   errWnd.Owner=this;

   errWnd.ShowDialog();
  }//catch
 }//Helper__Cmd__Save__Execute

 //-----------------------------------------------------------------------
 private bool Helper__Cmd__Save__CanExecute(object? parameter)
 {
  return Helper__ModelIsChanged();
 }//Helper__Cmd__Save__CanExecute

 //-----------------------------------------------------------------------
 private void Helper__Cmd__AddPhoneImage__Execute(object? parameter)
 {
  Debug.Assert(!Object.ReferenceEquals(parameter,null));
  Debug.Assert(parameter is Phone);

  OpenFileDialog openFileDialog=new OpenFileDialog();

  openFileDialog.Filter=AppConsts.c_ImageFilter;

  if(openFileDialog.ShowDialog()!=true)
   return;

  byte[] imageData;

  try
  {
   imageData=File.ReadAllBytes(openFileDialog.FileName);
  }
  catch(Exception e)
  {
   MessageBox.Show(e.Message,e.Source,MessageBoxButton.OK,MessageBoxImage.Error);

   return;
  }//catch

  var phone=(Phone)parameter;

  phone.Image=imageData;
 }//Helper__Cmd__AddPhoneImage__Execute

 //-----------------------------------------------------------------------
 private bool Helper__Cmd__AddPhoneImage__CanExecute(object? parameter)
 {
  if(Object.ReferenceEquals(parameter,null))
   return false;

  Debug.Assert(parameter is Phone);

#if DEBUG
  var c=Helper__GetModel();

  Debug.Assert(c.Phones.Contains((Phone)parameter));
#endif

  return true;
 }//Helper__Cmd__AddPhoneImage__CanExecute

 //-----------------------------------------------------------------------
 private void Helper__Cmd__DelPhoneImage__Execute(object? parameter)
 {
  Debug.Assert(!Object.ReferenceEquals(parameter,null));
  Debug.Assert(parameter is Phone);

  var phone=(Phone)parameter;

  phone.Image=null;
 }//Helper__Cmd__DelPhoneImage__Execute

 //-----------------------------------------------------------------------
 private bool Helper__CmdHelper__PhoneHasImage(object? parameter)
 {
  if(Object.ReferenceEquals(parameter,null))
   return false;

  Debug.Assert(parameter is Phone);

  var phone=(Phone)parameter;

#if DEBUG
  var c=Helper__GetModel();

  Debug.Assert(c.Phones.Contains(phone));
#endif

 if(Object.ReferenceEquals(phone.Image,null))
  return false;

  return true;
 }//Helper__CmdHelper__PhoneHasImage

 //-----------------------------------------------------------------------
 private void Helper__Cmd__PastePhoneImage__Execute(object? parameter)
 {
  Debug.Assert(!Object.ReferenceEquals(parameter,null));
  Debug.Assert(parameter is Phone);

  Debug.Assert(Clipboard.ContainsImage());

  BitmapSource
   bitmapSource
    =Clipboard.GetImage();

  if(Object.ReferenceEquals(bitmapSource,null))
  {
   //WTF?
   Debug.Assert(false);

   MessageBox.Show
    ("Clipboard contains image with an unsupported format.",
     "Error",
     MessageBoxButton.OK,
     MessageBoxImage.Error);

   return;
  }//if

  var phone
   =(Phone)parameter;

  var imageData
   =ImageUtils.BitmapSourceToBinary
     (bitmapSource);

  //Expected
  Debug.Assert(!Object.ReferenceEquals(imageData,null));

  phone.Image=imageData;
 }//Helper__Cmd__PastePhoneImage__Execute

 //-----------------------------------------------------------------------
 private bool Helper__Cmd__PastePhoneImage__CanExecute(object? parameter)
 {
  if(Object.ReferenceEquals(parameter,null))
   return false;

  Debug.Assert(parameter is Phone);

  var phone=(Phone)parameter;

#if DEBUG
  var c=Helper__GetModel();

  Debug.Assert(c.Phones.Contains(phone));
#endif

  if(Clipboard.ContainsImage())
   return true;

  return false;
 }//Helper__Cmd__PastePhoneImage__CanExecute

 //-----------------------------------------------------------------------
 private void Helper__Cmd__CopyPhoneImage__Execute(object? parameter)
 {
  Debug.Assert(!Object.ReferenceEquals(parameter,null));
  Debug.Assert(parameter is Phone);

  var phone=(Phone)parameter;

  Debug.Assert(!Object.ReferenceEquals(phone.Image,null));

  BitmapSource?
   imageSource
    =ImageUtils.BinaryToBitmapSource
      (phone.Image);

  //expected
  Debug.Assert(!Object.ReferenceEquals(imageSource,null));

  Clipboard.SetImage(imageSource);
 }//Helper__Cmd__CopyPhoneImage__Execute
};//class MainWindow

////////////////////////////////////////////////////////////////////////////////
}//namespace MVVM
