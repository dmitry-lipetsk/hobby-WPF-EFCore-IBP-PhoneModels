////////////////////////////////////////////////////////////////////////////////

using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace simple_app_controller.Controllers{
////////////////////////////////////////////////////////////////////////////////
//class MainWindowController

sealed class MainWindowController
{
 public static class ElementIDs
 {
  public const string Close
   ="Close";

  public const string ListBox__Phones
   ="ListBox__Phones";

  public const string Button__AddPhone
   ="Button__AddPhone";

  public const string Button__DelPhone
   ="Button__DelPhone";
 
  public const string Button__Save
   ="Button__Save";
 
  public const string TextBox__SelectedPhone__Model
   ="TextBox__SelectedPhone__Model";
 
  public const string TextBox__SelectedPhone__Company
   ="TextBox__SelectedPhone__Company";
 
  public const string TextBox__SelectedPhone__Price
   ="TextBox__SelectedPhone__Price";
 
  public const string TextBlock__SelectedPhone__ID
   ="TextBlock__SelectedPhone__ID";
 };//class ElementIDs

 //-----------------------------------------------------------------------
 public MainWindowController(OpCtx             opCtx,
                             AutomationElement mainWindow)
 {
  m_opCtx=opCtx;

  m_mainWindow=mainWindow;
 }

 //-----------------------------------------------------------------------
 public void Close()
 {
  Helper__MainWindowElement_Invoke(ElementIDs.Close);
 }//Close

 //-----------------------------------------------------------------------
 public ListBox GetCtrl__ListBox__Phones()
 {
  return HelperServices.GetListBox(m_opCtx,m_mainWindow,ElementIDs.ListBox__Phones);
 }//GetCtrl__ListBox__Phones

 //-----------------------------------------------------------------------
 public Button GetCtrl__Button__AddPhone()
 {
  return HelperServices.GetButton(m_opCtx,m_mainWindow,ElementIDs.Button__AddPhone);
 }//GetCtrl__Button__AddPhone

 //-----------------------------------------------------------------------
 public Button GetCtrl__Button__DelPhone()
 {
  return HelperServices.GetButton(m_opCtx,m_mainWindow,ElementIDs.Button__DelPhone);
 }//GetCtrl__Button__DelPhone

 //-----------------------------------------------------------------------
 public Button GetCtrl__Button__Save()
 {
  return HelperServices.GetButton(m_opCtx,m_mainWindow,ElementIDs.Button__Save);
 }//GetCtrl__Button__Save

 //-----------------------------------------------------------------------
 public TextBox GetCtrl__TextBox__SelectedPhone__Model()
 {
  return HelperServices.GetEdit(m_opCtx,m_mainWindow,ElementIDs.TextBox__SelectedPhone__Model);
 }//GetCtrl__TextBox__SelectedPhone__Model

 //-----------------------------------------------------------------------
 public TextBox GetCtrl__TextBox__SelectedPhone__Company()
 {
  return HelperServices.GetEdit(m_opCtx,m_mainWindow,ElementIDs.TextBox__SelectedPhone__Company);
 }//GetCtrl__TextBox__SelectedPhone__Company

 //-----------------------------------------------------------------------
 public TextBox GetCtrl__TextBox__SelectedPhone__Price()
 {
  return HelperServices.GetEdit(m_opCtx,m_mainWindow,ElementIDs.TextBox__SelectedPhone__Price);
 }//GetCtrl__TextBox__SelectedPhone__Price

 //-----------------------------------------------------------------------
 public TextBox GetCtrl__TextBlock__SelectedPhone__ID()
 {
  return HelperServices.GetTextBox(m_opCtx,m_mainWindow,ElementIDs.TextBlock__SelectedPhone__ID);
 }//GetCtrl__TextBlock__SelectedPhone__ID

 //-----------------------------------------------------------------------
 public void Invoke__AddPhone()
 {
  Helper__Invoke(GetCtrl__Button__AddPhone());  
 }//Invoke__AddPhone

 //-----------------------------------------------------------------------
 public void Invoke__DelPhone()
 {
  Helper__Invoke(GetCtrl__Button__DelPhone());  
 }//Invoke__DelPhone

 //-----------------------------------------------------------------------
 public void Invoke__Save()
 {
  Helper__Invoke(GetCtrl__Button__Save());  
 }//Invoke__Save

 //-----------------------------------------------------------------------
 public string GetTextBlock__SelectedPhone__ID()
 {
  var element=this.GetCtrl__TextBlock__SelectedPhone__ID();

  Helper__Log("Try to read [{0}] value...",element.Properties.AutomationId);

  string text=element.Properties.Name.Value;

  Helper__Log("Value of [{0}] is [{1}]",element.Properties.AutomationId,text);

  const string c_prefix="ID: ";

  HelperServices.CheckData
   (text.StartsWith(c_prefix),
    true);

  var text2=text.Remove(0,c_prefix.Length);

  Helper__Log("Clear value of [{0}] is [{1}]",element.Properties.AutomationId,text2);

  return text2;
 }//GetTextBlock__SelectedPhone__ID

 //-----------------------------------------------------------------------
 private void Helper__Invoke(Button button)
 {
  Helper__Log("Invoke {0}",button.Properties.AutomationId);

  button.Invoke();
 }//Helper__Invoke

 //-----------------------------------------------------------------------
 private void Helper__MainWindowElement_Invoke(params string[] ids)
 {
  HelperServices.Invoke(m_opCtx,m_mainWindow,ids);
 }//Helper__MainWindowElement_Invoke

 //-----------------------------------------------------------------------
 private void Helper__Log(string formatStr,params object[] args)
 {
  m_opCtx.Logger.Send(formatStr,args);
 }//Helper__Log

 //-----------------------------------------------------------------------
 private readonly OpCtx             m_opCtx;
 private readonly AutomationElement m_mainWindow;
};//class MainWindowController

////////////////////////////////////////////////////////////////////////////////
}//namespace simple_app_controller.Controllers