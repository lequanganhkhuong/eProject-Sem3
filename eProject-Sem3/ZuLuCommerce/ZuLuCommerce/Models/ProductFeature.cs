//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZuLuCommerce.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductFeature
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal ScreenSize { get; set; }
        public string StorageType { get; set; }
        public string StorageCap { get; set; }
        public string Graphic { get; set; }
        public string Processor { get; set; }
        public string OS { get; set; }
        public decimal BatteryLife { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
