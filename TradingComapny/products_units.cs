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
    
    public partial class products_units
    {
        public int product_ID { get; set; }
        public string unit { get; set; }
    
        public virtual product product { get; set; }
    }
}
