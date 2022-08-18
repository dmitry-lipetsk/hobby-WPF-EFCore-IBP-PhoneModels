////////////////////////////////////////////////////////////////////////////////

using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.Definitions;

using System.Text;

using Clipboard
 =System.Windows.Forms.Clipboard;
using System.Configuration;

namespace simple_app_controller{
////////////////////////////////////////////////////////////////////////////////
//class HelperServices

static class HelperServices
{
 //-----------------------------------------------------------------------
 public static string GetCnStr()
 {
  return Helper__ReadConfValue("cn_str");
 }//GetCnStr

 //-----------------------------------------------------------------------
 private static string Helper__ReadConfValue(string PropName)
 {
  var a=System.Reflection.Assembly.GetExecutingAssembly();

  var l=a.Location;

  var c=ConfigurationManager.OpenExeConfiguration(l);

  var s=c.AppSettings.Settings;

  var k=s[PropName];

  if(Object.ReferenceEquals(k,null))
  {
   string msg
    =string.Format("Configuration property {0} not defined",PropName);

   throw new ApplicationException(msg);
  }//if

  string v=k.Value;

  if(Object.ReferenceEquals(k,null))
  {
   string msg
    =string.Format("Configuration property {0} exists, but without value",PropName);

   throw new ApplicationException(msg);
  }//if

  return v;
 }//Helper__ReadConfValue

 //-----------------------------------------------------------------------
 public static void LogWindowInfo(OpCtx opCtx,AutomationElement window)
 {
  opCtx.Logger.Send("window1.ProcessId    : {0}",window.Properties.ProcessId);
  opCtx.Logger.Send("window1.NativeHandle : {0}",window.Properties.NativeWindowHandle);
 }//LogWindowInfo

 //-----------------------------------------------------------------------
 public static AutomationElement GetWindowParent(OpCtx opCtx,AutomationElement window)
 {
  opCtx.Logger.Send("Try get window parent ...");

  for(int nPass=1;;++nPass)
  {
   AutomationElement parentWindow=window.Parent;

   if(Object.ReferenceEquals(parentWindow,null))
   {
    opCtx.Logger.Send("No window parent [{0}]!",nPass);
   }
   else
   {
    if(string.CompareOrdinal(window.Name,parentWindow.Name)==0)
     return parentWindow;

    opCtx.Logger.Send("Unexpected window parent [{0}]: \"{1}\"!",nPass,parentWindow.Name);
   }//if

   if(nPass==50)
    throw new ApplicationException("Getting of Window Parent is failed!");

   HelperServices.Wait_UntilInputIsProcessed(opCtx);
  }//for nPass
 }//GetWindowParent

 //-----------------------------------------------------------------------
 public static string Clipboard__GetText(OpCtx opCtx)
 {
  opCtx.Logger.Send("Try read clipboard ...");

  for(int nPass=1;;++nPass)
  {
   HelperServices.Wait_UntilInputIsProcessed(opCtx);

   try
   {
    string clipboardData=Clipboard.GetText();

    opCtx.Logger.Send("Clipboard.Text: {0}",clipboardData);

    return clipboardData;
   }
   catch(Exception e)
   {
    opCtx.Logger.Send("Reading failure [{0}]... {1}",nPass,e.Message);
   }

   if(nPass==50)
    throw new ApplicationException("Getting of clipboard data is failed!");
  }//for nPass
 }//Clipboard__GetText

 //-----------------------------------------------------------------------
 public static void Clipboard__SetText(OpCtx opCtx,string text)
 {
  opCtx.Logger.Send("Set text to clipboard [{0}]",text);

  Clipboard.SetText(text);
 }//Clipboard__SetText

 //-----------------------------------------------------------------------
 public static void Clipboard__Clear(OpCtx opCtx)
 {
  opCtx.Logger.Send("Clear clipboard");

  Clipboard.Clear();
 }//Clipboard__Clear

 //-----------------------------------------------------------------------
 public static FlaUI.Core.Application LaunchStoreApp(OpCtx opCtx,string appID)
 {
  opCtx.Logger.Send("Launch store application [{0}] ...",appID);

  var app=FlaUI.Core.Application.LaunchStoreApp(appID);

  opCtx.Logger.Send("Done!");

  return app;
 }//LaunchStoreApp

 //-----------------------------------------------------------------------
 public static FlaUI.Core.Application LaunchApp(OpCtx opCtx,string appEXE)
 {
  opCtx.Logger.Send("Launch application [{0}] ...",appEXE);

  var app=FlaUI.Core.Application.Launch(appEXE);

  opCtx.Logger.Send("Done!");

  return app;
 }//LaunchApp

 //-----------------------------------------------------------------------
 public static void Wait_UntilInputIsProcessed(OpCtx opCtx)
 {
  opCtx.Logger.Send("Wait.UntilInputIsProcessed...");

  Wait.UntilInputIsProcessed();
 }//Wait_UntilInputIsProcessed

 //-----------------------------------------------------------------------
 public static void Thread_Sleep(OpCtx opCtx,int milliSeconds)
 {
  opCtx.Logger.Send("Thread.Sleep({0})...",milliSeconds);

  Thread.Sleep(milliSeconds);
 }//Thread_Sleep

 //-----------------------------------------------------------------------
 public static void CheckData(object actual,object expected)
 {
  if(object.Equals(actual,expected))
   return;

  StringBuilder sb=new StringBuilder();

  string errMsg
   =string.Format
     ("Wrong data [{0}].Expected [{1}]",
      HelperServices.WrapData(actual),
      HelperServices.WrapData(expected));

  throw new ApplicationException(errMsg);
 }//CheckData

 //-----------------------------------------------------------------------
 public static string GetPropertyNameValue(OpCtx opCtx,AutomationElement window,string elementID)
 {
  var element=GetElement(opCtx,window,elementID);

  opCtx.Logger.Send("Try read [{0}] Properties.Name.Value",elementID);

  var result=element.Properties.Name.Value;

  opCtx.Logger.Send("[{0}] contains [{1}]",elementID,result);

  return result;
 }//GetPropertyNameValue

 //-----------------------------------------------------------------------
 public static void SelectItem(OpCtx opCtx,AutomationElement window,params string[] elementIDs)
 {
  var element=HelperServices.GetElement(opCtx,window,elementIDs);

  opCtx.Logger.Send("Select element [{0}]",element.Properties.AutomationId.Value);

  //TODO: it need to check an exist of support for invoke.

  element.Patterns.SelectionItem.Pattern.Select();
 }//SelectItem

 //-----------------------------------------------------------------------
 public static void Invoke(OpCtx opCtx,AutomationElement window,params string[] elementIDs)
 {
  var element=HelperServices.GetElement(opCtx,window,elementIDs);

  opCtx.Logger.Send("Invoke element [{0}]",element.Properties.AutomationId.Value);

  //TODO: it need to check an exist of support for invoke.

  element.Patterns.Invoke.Pattern.Invoke();
 }//Invoke

 //-----------------------------------------------------------------------
 public static AutomationElement GetElement(OpCtx opCtx,AutomationElement window,params string[] elementIDs)
 {
  var result=HelperServices.TryGetElement(opCtx,window,elementIDs);

  if(Object.ReferenceEquals(result,null))
   throw new ApplicationException("Element not found!");

  return result;
 }//GetElement

 //-----------------------------------------------------------------------
 public static AutomationElement? TryGetElement(OpCtx opCtx,AutomationElement window,params string[] elementIDs)
 {
  System.Diagnostics.Debug.Assert(!Object.ReferenceEquals(window,null));
  System.Diagnostics.Debug.Assert(!Object.ReferenceEquals(elementIDs,null));

  foreach(var elementID in elementIDs)
  {
   opCtx.Logger.Send("Try find element [{0}] ...",elementID);

   var obj=window.FindFirstDescendant(cf=>cf.ByAutomationId(elementID));

   if(Object.ReferenceEquals(obj,null))
   {
    opCtx.Logger.Send("  not found");

    continue;
   }//if

   return obj;
  }//foreach n

  return null;
 }//TryGetElement

 //-----------------------------------------------------------------------
 public static Button GetButton(OpCtx opCtx,AutomationElement window,params string[] elementIDs)
 {
  var result=HelperServices.TryGetButton(opCtx,window,elementIDs);

  if(Object.ReferenceEquals(result,null))
   throw new ApplicationException("Button not found!");

  return result;
 }//GetButton

 //-----------------------------------------------------------------------
 public static Button? TryGetButton(OpCtx opCtx,AutomationElement window,params string[] elementIDs)
 {
  System.Diagnostics.Debug.Assert(!Object.ReferenceEquals(window,null));
  System.Diagnostics.Debug.Assert(!Object.ReferenceEquals(elementIDs,null));

  foreach(var elementID in elementIDs)
  {
   opCtx.Logger.Send("Try find element [{0}] ...",elementID);

   var obj=window.FindFirstDescendant(cf=>cf.ByAutomationId(elementID));

   if(Object.ReferenceEquals(obj,null))
   {
    opCtx.Logger.Send("  not found");

    continue;
   }//if

   if(obj.Properties.ControlType!=ControlType.Button)
   {
    opCtx.Logger.Send("  wrong type [{0}]",obj.Properties.ControlType);

    continue;
   }//if

   return obj.AsButton();
  }//foreach n

  return null;
 }//TryGetButton

 //-----------------------------------------------------------------------
 public static TextBox GetTextBox(OpCtx opCtx,AutomationElement window,params string[] elementIDs)
 {
  System.Diagnostics.Debug.Assert(!Object.ReferenceEquals(window,null));
  System.Diagnostics.Debug.Assert(!Object.ReferenceEquals(elementIDs,null));

  foreach(var elementID in elementIDs)
  {
   opCtx.Logger.Send("Try find TextBox [{0}] ...",elementID);

   var obj=window.FindFirstDescendant(cf=>cf.ByAutomationId(elementID));

   if(Object.ReferenceEquals(obj,null))
   {
    opCtx.Logger.Send("  not found");

    continue;
   }//if

   if(obj.Properties.ControlType!=ControlType.Text)
   {
    opCtx.Logger.Send("  wrong type [{0}]",obj.Properties.ControlType);

    continue;
   }//if

   return obj.AsTextBox();
  }//foreach n

  throw new ApplicationException("TextBox not found!");
 }//GetTextBox

 //-----------------------------------------------------------------------
 public static TextBox GetEdit(OpCtx opCtx,AutomationElement window,params string[] elementIDs)
 {
  System.Diagnostics.Debug.Assert(!Object.ReferenceEquals(window,null));
  System.Diagnostics.Debug.Assert(!Object.ReferenceEquals(elementIDs,null));

  foreach(var elementID in elementIDs)
  {
   opCtx.Logger.Send("Try find edit [{0}] ...",elementID);

   var obj=window.FindFirstDescendant(cf=>cf.ByAutomationId(elementID));

   if(Object.ReferenceEquals(obj,null))
   {
    opCtx.Logger.Send("  not found");

    continue;
   }//if

   if(obj.Properties.ControlType!=ControlType.Edit)
   {
    opCtx.Logger.Send("  wrong type [{0}]",obj.Properties.ControlType);

    continue;
   }//if

   return obj.AsTextBox();
  }//foreach n

  throw new ApplicationException("Edit not found!");
 }//GetEdit

 //-----------------------------------------------------------------------
 public static ListBox GetListBox(OpCtx opCtx,AutomationElement window,params string[] elementIDs)
 {
  System.Diagnostics.Debug.Assert(!Object.ReferenceEquals(window,null));
  System.Diagnostics.Debug.Assert(!Object.ReferenceEquals(elementIDs,null));

  foreach(var elementID in elementIDs)
  {
   opCtx.Logger.Send("Try find list [{0}] ...",elementID);

   var obj=window.FindFirstDescendant(cf=>cf.ByAutomationId(elementID));

   if(Object.ReferenceEquals(obj,null))
   {
    opCtx.Logger.Send("  not found");

    continue;
   }//if

   if(obj.Properties.ControlType!=ControlType.List)
   {
    opCtx.Logger.Send("  wrong type [{0}]",obj.Properties.ControlType);

    continue;
   }//if

   return obj.AsListBox();
  }//foreach n

  throw new ApplicationException("ListBox not found!");
 }//GetListBox

 //-----------------------------------------------------------------------
 public static string WrapData(object? data)
 {
  if(Object.ReferenceEquals(data,null))
   return "null";

  switch(Type.GetTypeCode(data.GetType()))
  {
   case TypeCode.String:
    return HelperServices.WrapData__string((string)data);
  }

  var s=data.ToString();

  if(Object.ReferenceEquals(s,null))
   return "##null";

  return s;
 }//WrapData

 //-----------------------------------------------------------------------
 public static string WrapData__string(string data)
 {
  var sb=new StringBuilder();

  sb.Append('\"');

  foreach(var c in data)
  {
   switch(c)
   {
    case '\0': sb.Append("\\0");  break;
    case '\\': sb.Append("\\\\"); break;
    case '\n': sb.Append("\\n");  break;
    case '\r': sb.Append("\\r");  break;
    case '\t': sb.Append("\\t");  break;
    case '\"': sb.Append("\"");   break;

    default  : sb.Append(c); break;
   }//switch
  }//WrapData__string

  sb.Append('\"');

  return sb.ToString();
 }//WrapData__string
};//class HelperServices

////////////////////////////////////////////////////////////////////////////////
}//namespace simple_app_controller
