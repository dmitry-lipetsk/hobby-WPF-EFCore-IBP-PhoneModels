////////////////////////////////////////////////////////////////////////////////

using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

using xdb=lcpi.data.oledb;

namespace simple_app_controller{
////////////////////////////////////////////////////////////////////////////////

internal class Program
{
 [STAThread]
 public static int Main()
 {
  int errCount=0;

  try
  {
   var logger=new Logger();

   errCount+=Exec(logger);
  }
  catch(Exception exc)
  {
   ++errCount;

   for(var e=exc;e!=null;e=e.InnerException)
   {
    Console.WriteLine("ERROR: {0} - {1}",e.Source,e.Message);
   }
  }//catch

  return errCount;
 }//Main

 //-----------------------------------------------------------------------
 public static int Exec(Logger logger)
 {
  int errCount=0;

  try
  {
   var opCtx=new OpCtx(logger);

   var app=HelperServices.LaunchApp(opCtx,"PhoneModels.exe");

   using var automation=new UIA3Automation();

   //Wait initialization
   for(int nPass=1;;++nPass)
   {
    HelperServices.Thread_Sleep(opCtx,100);

    var x
     =HelperServices.TryGetElement
        (opCtx,
         app.GetMainWindow(automation),
         Controllers.MainWindowController.ElementIDs.Close);

    if(!Object.ReferenceEquals(x,null))
     break;

    if(nPass==10)
     throw new ApplicationException("Attach to window failed");
   }//for nPass

   var window=app.GetMainWindow(automation);

   HelperServices.LogWindowInfo(opCtx,window);

   var mainWindow1=new Controllers.MainWindowController(opCtx,window);

   //---------------------------------------
   Test__0001(opCtx,mainWindow1);
   Test__0002(opCtx,mainWindow1);

   //---------------------------------------
   mainWindow1.Close();

   //---------------------------------------
   opCtx.Logger.Send("OK. Go home!");

   HelperServices.Thread_Sleep(opCtx,5000);
  }
  catch(Exception exc)
  {
   ++errCount;

   for(var e=exc;e!=null;e=e.InnerException)
   {
    logger.Send("ERROR: {0} - {1}",e.Source,e.Message);
   }
  }//catch

  return errCount;
 }//Exec

 //-----------------------------------------------------------------------
 private static void Test__0001(OpCtx opCtx,Controllers.MainWindowController mainWindow)
 {
  var listBox__Phones=mainWindow.GetCtrl__ListBox__Phones();

  var nPhones=listBox__Phones.Items.Length;

  opCtx.Logger.Send("Current phone count: {0}.",nPhones);

  //---------------------------------------
  mainWindow.Invoke__AddPhone();

  HelperServices.CheckData
   (listBox__Phones.Items.Length,
    nPhones+1);

  //---------------------------------------
  mainWindow.Invoke__DelPhone();

  HelperServices.CheckData
   (listBox__Phones.Items.Length,
    nPhones);
 }//Test__0001

 //-----------------------------------------------------------------------
 private static void Test__0002(OpCtx opCtx,Controllers.MainWindowController mainWindow)
 {
  const string c_data__MODEL       ="MY_PHONE!";
  const string c_data__COMPANY     ="MY_COMPANY!";
  const int    c_data__PRICE       =1;

  mainWindow.Invoke__AddPhone();

  mainWindow.GetCtrl__TextBox__SelectedPhone__Model()
   .Text=c_data__MODEL;

  mainWindow.GetCtrl__TextBox__SelectedPhone__Company()
   .Text=c_data__COMPANY;

  mainWindow.GetCtrl__TextBox__SelectedPhone__Price()
   .Text=c_data__PRICE.ToString();

  HelperServices.CheckData
   (mainWindow.GetCtrl__Button__Save().IsEnabled,
    true);

  //----------------------------------------------------------------------
  mainWindow.Invoke__Save();

  HelperServices.CheckData
   (mainWindow.GetCtrl__Button__Save().IsEnabled,
    false);

  //----------------------------------------------------------------------
  var newID
   =mainWindow.GetTextBlock__SelectedPhone__ID();

  //----------------------------------------------------------------------
  opCtx.Logger.Send("Check database state");

  using var cn=new xdb.OleDbConnection(HelperServices.GetCnStr());

  cn.Open();

  using var tr=cn.BeginTransaction();

  using var cmd=cn.CreateCommand();

  cmd.Transaction=tr;

  cmd.CommandText="select * from phone where id=:id";

  cmd["id"].Value=newID;

  using var rd=cmd.ExecuteReader();

  HelperServices.CheckData
   (rd.Read(),
    true);

  HelperServices.CheckData
   (rd["TITLE"],
    c_data__MODEL);

  HelperServices.CheckData
   (rd["COMPANY"],
    c_data__COMPANY);

  HelperServices.CheckData
   (rd["PRICE"],
    c_data__PRICE);

  HelperServices.CheckData
   (rd["ID"],
    Convert.ToInt64(newID));

  HelperServices.CheckData
   (rd["IMAGE"],
    DBNull.Value);

  //----------------------------------------------------------------------
  mainWindow.Invoke__DelPhone();

  HelperServices.CheckData
   (mainWindow.GetCtrl__Button__Save().IsEnabled,
    true);

  mainWindow.Invoke__Save();

  HelperServices.CheckData
   (mainWindow.GetCtrl__Button__Save().IsEnabled,
    false);

  //----------------------------------------------------------------------
  tr.CommitRetaining();

  opCtx.Logger.Send("Check database state");

  rd.Dispose();

  using var rd2=cmd.ExecuteReader();

  HelperServices.CheckData
   (rd2.Read(),
    false);

  tr.Commit();
 }//Test__0002
}//class Program

////////////////////////////////////////////////////////////////////////////////
}//namespace simple_app_controller
