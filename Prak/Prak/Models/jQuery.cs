//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class jQuery
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public jQuery()
    {
        this.jWorkList = new HashSet<jWorkList>();
    }

    public int QueryId { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
    ApplyFormatInEditMode = true)]
    public Nullable<System.DateTime> DateOut { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
    ApplyFormatInEditMode = true)]
    public Nullable<System.DateTime> DateIn { get; set; }
    public System.DateTime DateModification { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
    ApplyFormatInEditMode = true)]
    public Nullable<System.DateTime> DeadLine { get; set; }
    public string Text { get; set; }
    public int StateId { get; set; }
    public string PersonId { get; set; }
    public string PersonSpId { get; set; }

    public virtual AspNetUsers AspNetUsers { get; set; }
    public virtual AspNetUsers AspNetUsers1 { get; set; }
    public virtual hState hState { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<jWorkList> jWorkList { get; set; }
}
