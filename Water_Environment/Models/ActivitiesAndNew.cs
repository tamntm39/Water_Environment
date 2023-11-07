//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Water_Environment.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;
    using System.Web.Mvc;

    public partial class ActivitiesAndNew
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ActivitiesAndNew()
        {
            this.Comments = new HashSet<Comment>();
        }
    
        public int id { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateOn { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public int ViewCount { get; set; }
        public string Img { get; set; }
        public string ShortDescription { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }

     
    }
}
