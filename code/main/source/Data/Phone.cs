﻿////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVVM{
////////////////////////////////////////////////////////////////////////////////
//enum PhoneState

public enum PhoneState
{
 NotChanged=0,
 IsNew=1,
 IsChanged=2,
};//enum PhoneState

////////////////////////////////////////////////////////////////////////////////
//class Phone

[Table("PHONE")]
public class Phone:INotifyPropertyChanged
{
 public event PropertyChangedEventHandler? PropertyChanged;

 //-----------------------------------------------------------------------
 [NotMapped]
 public PhoneState State
 {
  get
  {
   if(!m_ID.HasValue)
    return PhoneState.IsNew;

   if(m_IsChanged)
    return PhoneState.IsChanged;

   return PhoneState.NotChanged;
  }//get
 }//State

 //-----------------------------------------------------------------------
 [Key]
 [BackingField(nameof(m_ID))]
 public long? ID
 {
  get
  {
   return m_ID;
  }//get

  private set //disable direct access
  {
   Debug.Assert(false);

   throw new ApplicationException("[BUG CHECK] This call isn't applicable.");
  }//set
 }//ID

 //-----------------------------------------------------------------------
 [Column("TITLE", TypeName="VARCHAR(64) CHARACTER SET UTF8")]
 [BackingField(nameof(m_Title))]
 public string? Title
 {
  get
  {
   return m_Title;
  }//get

  set
  {
   m_Title=value;

   Helper__OnPropertyChanged(/*"Title"*/);
  }//set
 }//Title

 //-----------------------------------------------------------------------
 [Column("COMPANY", TypeName="VARCHAR(64) CHARACTER SET UTF8")]
 [BackingField(nameof(m_Company))]
 public string? Company
 {
  get
  {
   return m_Company;
  }//get

  set
  {
   m_Company=value;

   Helper__OnPropertyChanged(/*"Company"*/);
  }//set
 }//Company

 //-----------------------------------------------------------------------
 [Column("PRICE", TypeName="INTEGER")]
 [BackingField(nameof(m_Price))]
 public int? Price
 {
  get
  {
   return m_Price;
  }//get

  set
  {
   m_Price=value;

   Helper__OnPropertyChanged(/*"Price"*/);
  }//set
 }//Price

 //-----------------------------------------------------------------------
 [Column("IMAGE", TypeName="BLOB SUB_TYPE BINARY")]
 [BackingField(nameof(m_Image))]
 public byte[]? Image
 {
  get
  {
   return m_Image;
  }//get

  set
  {
   m_Image=value;

   Helper__OnPropertyChanged(/*"Image"*/);
  }//set
 }//Price

 //-----------------------------------------------------------------------
 private void Helper__OnPropertyChanged([CallerMemberName] string prop = "")
 {
  if(Object.ReferenceEquals(PropertyChanged,null))
   return;

  bool prevIsChanged=m_IsChanged;

  if(m_ID.HasValue)
   m_IsChanged=true;

  PropertyChanged(this,new PropertyChangedEventArgs(prop));

  if(prevIsChanged!=m_IsChanged)
   PropertyChanged(this,new PropertyChangedEventArgs(nameof(State)));
 }//Helper__OnPropertyChanged

 //-----------------------------------------------------------------------
 public void ResetState()
 {
  m_IsChanged=false;
 }//ResetState

 //-----------------------------------------------------------------------
 private bool m_IsChanged=false;

 private long?    m_ID;
 private string?  m_Title;
 private string?  m_Company;
 private int?     m_Price;

 private byte[]?  m_Image;
};//class Phone

////////////////////////////////////////////////////////////////////////////////
}//namespace MVVM
