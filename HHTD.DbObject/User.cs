//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HHTD.DbObject
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int id { get; set; }
        public string loginname { get; set; }
        public string loginpw { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public Nullable<short> type { get; set; }
        public Nullable<short> isDel { get; set; }
    }
}
