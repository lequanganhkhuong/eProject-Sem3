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
    
    public partial class Comment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Comment()
        {
            this.Comments1 = new HashSet<Comment>();
        }
    
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public Nullable<System.DateTime> DateComment { get; set; }
    
        public virtual Account Account { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments1 { get; set; }
        public virtual Comment Comment1 { get; set; }
        public virtual Product Product { get; set; }
    }
}