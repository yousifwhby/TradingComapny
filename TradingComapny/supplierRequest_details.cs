//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TradingComapny
{
    using System;
    using System.Collections.Generic;
    
    public partial class supplierRequest_details
    {
        public int supplierRequest_ID { get; set; }
        public int product_ID { get; set; }
        public string store_name { get; set; }
        public int input_quantity { get; set; }
        public System.DateTime Production_date { get; set; }
    
        public virtual product product { get; set; }
        public virtual supplier_requests supplier_requests { get; set; }
        public virtual WareHouse WareHouse { get; set; }
    }
}