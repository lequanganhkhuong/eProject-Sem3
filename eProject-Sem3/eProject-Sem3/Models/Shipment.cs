//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eProject_Sem3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Shipment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shipment()
        {
            this.Orders = new HashSet<Order>();
        }
    
        public int Id { get; set; }
        public int StatusId { get; set; }
        public Nullable<System.DateTime> ShippedDate { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public decimal ShippingFee { get; set; }
        public int ShipperId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ShipmentStatus ShipmentStatus { get; set; }
        public virtual Shipper Shipper { get; set; }
    }
}
