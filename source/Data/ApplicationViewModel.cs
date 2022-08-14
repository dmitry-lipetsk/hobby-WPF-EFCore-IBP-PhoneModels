////////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Reflection.Metadata;

using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace MVVM{
////////////////////////////////////////////////////////////////////////////////
//class ApplicationViewModel

sealed class ApplicationViewModel:DbContext
{
 public DbSet<Phone> dbset__Phones { get; set; }

 //-----------------------------------------------------------------------
 public ObservableCollection<Phone> Phones
 {
  get
  {
   Debug.Assert(!Object.ReferenceEquals(this.dbset__Phones,null));

   return this.dbset__Phones.Local.ToObservableCollection();
  }
 }//Phones

 //-----------------------------------------------------------------------
 protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
 {
  optionsBuilder.UseLcpiOleDb(Helper__GetCnStr());

  optionsBuilder.UseLazyLoadingProxies();
 }//OnConfiguring

 //-----------------------------------------------------------------------
 public override int SaveChanges(bool acceptAllChangesOnSuccess)
 {
  var r=base.SaveChanges(acceptAllChangesOnSuccess);

  //TODO: Is it correct?

  foreach(var p in this.Phones)
  {
   //It's expected!
   Debug.Assert(p.ID.HasValue);

   p.ResetState();
  }//for p

  return r;
 }//SaveChanges

 //-----------------------------------------------------------------------
 public static string Helper__GetCnStr()
 {
  return Helper__ReadConfValue(AppConsts.c_CfgParam__cn_str);
 }//Helper__GetCnStr

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
};//class ApplicationViewModel

////////////////////////////////////////////////////////////////////////////////
}//namespace MVVM
