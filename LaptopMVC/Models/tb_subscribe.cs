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
    
    public partial class tb_subscribe
    {
        public int id { get; set; }
        [DisplayName("Email")]
        public string email { get; set; }
        [DisplayName("Ngày tạo")]
        public System.DateTime createdate { get; set; }
    }
}
