////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.IO;

namespace MVVM{
////////////////////////////////////////////////////////////////////////////////
//class MainWindow

/// <summary>
///  Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow:Window
{
 public MainWindow()
  :this(Helper__CreateModel())
 {
 }//MainWindow

 //-----------------------------------------------------------------------
 private MainWindow(ApplicationViewModel model)
 {
  Debug.Assert(!Object.ReferenceEquals(model,null));

  //----------------------------------------
  m_model=model;

  //----------------------------------------
  m_addPhoneCommand
   =new RelayCommand
     (Helper__Cmd__AddPhone__Execute);

  m_delPhoneCommand
    =new RelayCommand
      (Helper__Cmd__DelPhone__Execute,
       Helper__Cmd__DelPhone__CanExecute);

  m_saveCommand
   =new RelayCommand
     (Helper__Cmd__Save__Execute,
      Helper__Cmd__Save__CanExecute);

  m_newWindowCommand
   =new RelayCommand
     (Helper__Cmd__NewWindow__Execute);

  m_addPhoneImageCommand
   =new RelayCommand
     (Helper__Cmd__AddPhoneImage__Execute,
      Helper__Cmd__AddPhoneImage__CanExecute);

  m_delPhoneImageCommand
   =new RelayCommand
     (Helper__Cmd__DelPhoneImage__Execute,
      Helper__CmdHelper__PhoneHasImage);

  m_pastePhoneImageCommand
   =new RelayCommand
     (Helper__Cmd__PastePhoneImage__Execute,
      Helper__Cmd__PastePhoneImage__CanExecute);

  m_copyPhoneImageCommand
   =new RelayCommand
     (Helper__Cmd__CopyPhoneImage__Execute,
      Helper__CmdHelper__PhoneHasImage);

  //----------------------------------------
  InitializeComponent();

  this.Helper__AttachToModel();

  //TODO: It looks very ugly! Does it need to move in another place?
  if(m_model.Phones.Count!=0)
   ListBox__Phones.SelectedItem=m_model.Phones[0];
 }//MainWindow - model

 //-----------------------------------------------------------------------
 public ICommand Command__AddPhone
 {
  get
  {
   return m_addPhoneCommand;
  }//get
 }//Command__AddPhone

 //-----------------------------------------------------------------------
 public ICommand Command__DelPhone
 {
  get
  {
   return m_delPhoneCommand;
  }//get
 }//Command__DelPhone

 //-----------------------------------------------------------------------
 public ICommand Command__Save
 {
  get
  {
   return m_saveCommand;
  }//get
 }//Command__Save

 //-----------------------------------------------------------------------
 public ICommand Command__NewWindow
 {
  get
  {
   return m_newWindowCommand;
  }//get
 }//Command__NewWindow

 //-----------------------------------------------------------------------
 public ICommand Command__AddPhoneImage
 {
  get
  {
   return m_addPhoneImageCommand;
  }//get
 }//Command__AddPhoneImage

 //-----------------------------------------------------------------------
 public ICommand Command__DelPhoneImage
 {
  get
  {
   return m_delPhoneImageCommand;
  }//get
 }//Command__DelPhoneImage

 //-----------------------------------------------------------------------
 public ICommand Command__PastePhoneImage
 {
  get
  {
   return m_pastePhoneImageCommand;
  }//get
 }//Command__PastePhoneImage

 //-----------------------------------------------------------------------
 public ICommand Command__CopyPhoneImage
 {
  get
  {
   return m_copyPhoneImageCommand;
  }//get
 }//Command__CopyPhoneImage

 //Event handlers --------------------------------------------------------
 private void Helper__EvModel__Saved(object? sender,SavedChangesEventArgs args)
 {
  Helper__RefreshDisplayData();
 }//Helper__EvModel__Saved

 //-----------------------------------------------------------------------
 private void MainWindow1_Closing(object sender,CancelEventArgs e)
 {
  int cWindows=0;

  foreach(var win in App.Current.Windows)
  {
   if(Helper__Closing__IsOurWindow(win))
    ++cWindows;

   if(cWindows>1)
    break;
  }//foreach win

  Debug.Assert(cWindows>0);

  if(cWindows>1)
   return;

  if(!Helper__ModelIsChanged())
   return;

  var msgBoxResult
   =MessageBox.Show("Changes didn't save. Continue exit?","Warning!",MessageBoxButton.YesNo);

  if(msgBoxResult==MessageBoxResult.Yes)
   return;

  e.Cancel=true;
 }//MainWindow1_Closing

 //-----------------------------------------------------------------------
 private static bool Helper__Closing__IsOurWindow(object? win)
 {
  if(win is MainWindow)
   return true;

  return false;
 }//Helper__Closing__IsOurWindow

 //-----------------------------------------------------------------------
 private void MainWindow1_Closed(object sender,EventArgs e)
 {
  Debug.Assert(!Object.ReferenceEquals(m_model,null));

  Helper__DetachFromModel();
 }//MainWindow1_Closed

 //Helper methods --------------------------------------------------------
 private static ApplicationViewModel Helper__CreateModel()
 {
  var model=new ApplicationViewModel();

  model.dbset__Phones.Load();

#if DEBUG
  //
  // I don't understand why it works. It's probably a miracle.
  //
  // The answer: these are backing fields, Carl.
  //

  foreach(var p in model.Phones)
  {
   Debug.Assert(p.ID.HasValue);
   Debug.Assert(p.State==PhoneState.NotChanged);
  }
#endif

  return model;
 }//Helper__CreateModel

 //-----------------------------------------------------------------------
 private ApplicationViewModel Helper__GetModel()
 {
  Debug.Assert(!Object.ReferenceEquals(m_model,null));

  return m_model;
 }//Helper__GetModel

 //-----------------------------------------------------------------------
 private void Helper__AttachToModel()
 {
  m_model.SavedChanges+=Helper__EvModel__Saved;

  this.DataContext=m_model;
 }//Helper__AttachToModel

 //-----------------------------------------------------------------------
 private bool Helper__ModelIsChanged()
 {
  return Helper__GetModel().ChangeTracker.HasChanges();
 }//Helper__ModelIsChanged

 //-----------------------------------------------------------------------
 private void Helper__DetachFromModel()
 {
  DataContext=null;

  m_model.SavedChanges-=Helper__EvModel__Saved;
 }//Helper__DetachFromModel

 //-----------------------------------------------------------------------
 void Helper__RefreshDisplayData()
 {
  var model=Helper__GetModel();

  var curPhone=(Phone)ListBox__Phones.SelectedItem;

  //Refresh visibled data
  this.DataContext=null;

  this.DataContext=model;

  ListBox__Phones.SelectedItem=curPhone;
 }//Helper__RefreshDisplayData

 //Private data ----------------------------------------------------------
 private readonly ApplicationViewModel m_model;

 //Commands --------------------------------------------------------------
 private readonly ICommand m_addPhoneCommand;

 private readonly ICommand m_delPhoneCommand;

 private readonly ICommand m_saveCommand;

 private readonly ICommand m_newWindowCommand;

 private readonly ICommand m_addPhoneImageCommand;

 private readonly ICommand m_delPhoneImageCommand;

 private readonly ICommand m_pastePhoneImageCommand;

 private readonly ICommand m_copyPhoneImageCommand;
};//class MainWindow

////////////////////////////////////////////////////////////////////////////////
}//namespace MVVM
