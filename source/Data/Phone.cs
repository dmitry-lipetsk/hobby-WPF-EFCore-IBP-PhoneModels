////////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVVM{
////////////////////////////////////////////////////////////////////////////////
//class Phone

[Table("PHONE")]
public class Phone
{
 [Key]
 public long? ID { get; set; }

 [Column("TITLE", TypeName="VARCHAR(64) CHARACTER SET UTF8")]
 public string? Title { get; set; }

 [Column("COMPANY", TypeName="VARCHAR(64) CHARACTER SET UTF8")]
 public string? Company { get; set; }

 //-----------------------------------------------------------------------
 [Column("PRICE", TypeName="INTEGER")]
 public int? Price { get; set; }
};//class Phone

////////////////////////////////////////////////////////////////////////////////
}//namespace MVVM
