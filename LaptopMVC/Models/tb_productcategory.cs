﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LaptopMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    public partial class tb_productcategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_productcategory()
        {
            this.tb_product = new HashSet<tb_product>();
        }
    
        public int id { get; set; }
        [DisplayName("Tên danh mục"), Required(ErrorMessage ="Vui lòng nhập tên danh mục")]
        public string title { get; set; }
        [DisplayName("Ảnh bìa"), Required(ErrorMessage ="Vui lòng chọn ảnh bìa danh mục")]
        public string images { get; set; }
        [DisplayName("Ngày tạo")]
        public System.DateTime createdate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_product> tb_product { get; set; }
    }
}
