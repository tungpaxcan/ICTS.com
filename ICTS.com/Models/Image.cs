//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ICTS.com.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Image
    {
        public int Id { get; set; }
        public Nullable<int> IdProduct { get; set; }
        public string Image1 { get; set; }
        public string Name { get; set; }
        public string Meta { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifileDate { get; set; }
        public string ModifileBy { get; set; }
        public Nullable<bool> Status { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
