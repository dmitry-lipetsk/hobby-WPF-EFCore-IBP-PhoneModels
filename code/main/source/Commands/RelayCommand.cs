////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Windows.Input;

namespace MVVM{
////////////////////////////////////////////////////////////////////////////////

delegate void Delegate_RelayCommand_Execute
 (object? paramerer);

delegate bool Delegate_RelayCommand_CanExecute
 (object? paramerer);

////////////////////////////////////////////////////////////////////////////////
//class RelayCommand

sealed class RelayCommand:ICommand
{
 public event EventHandler? CanExecuteChanged
 {
  add
  {
   CommandManager.RequerySuggested+=value;
  }

  remove
  {
   CommandManager.RequerySuggested-=value;
  }
 }//CanExecuteChanged

 //-----------------------------------------------------------------------
 public RelayCommand(Delegate_RelayCommand_Execute     execute,
                     Delegate_RelayCommand_CanExecute? canExecute=null)
 {
  Debug.Assert(!Object.ReferenceEquals(execute,null));

  m_execute = execute;
  m_canExecute = canExecute;
 }//RelayCommand

 //-----------------------------------------------------------------------
 public bool CanExecute(object? parameter)
 {
  if(!Object.ReferenceEquals(m_canExecute,null))
  {
   if(!m_canExecute(parameter))
    return false;
  }//if

  return true;
 }//CanExecute

 //-----------------------------------------------------------------------
 public void Execute(object? parameter)
 {
  m_execute(parameter);
 }//Execute

 //-----------------------------------------------------------------------
 private readonly Delegate_RelayCommand_Execute m_execute;

 private readonly Delegate_RelayCommand_CanExecute? m_canExecute;
};//class RelayCommand

////////////////////////////////////////////////////////////////////////////////
}//namespace MVVM
