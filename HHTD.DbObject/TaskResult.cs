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
    
    public partial class TaskResult
    {
        public int id { get; set; }
        public Nullable<int> tid { get; set; }
        public string content { get; set; }
        public Nullable<decimal> charge { get; set; }
        public Nullable<short> checkout { get; set; }
        public Nullable<System.DateTime> completetime { get; set; }
        public Nullable<int> createuser { get; set; }
    }
}