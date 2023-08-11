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
    public partial class tb_news
    {
        public int id { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập tên tin tức"), DisplayName("Tên tin tức")]
        public string title { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập mô tả"), DisplayName("Mô tả")]
        public string description { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập chi tiết tin tức"), DisplayName("Chi tiết tin tức")]
        public string detail { get; set; }
        [Required(ErrorMessage ="Vui lòng chọn ảnh"), DisplayName("Ảnh bìa")]
        public string images { get; set; }
        [DisplayName("Ngày tạo")]
        public System.DateTime createdate { get; set; }
        [DisplayName("Ngày sửa đổi/cập nhật")]
        public System.DateTime modifieddate { get; set; }
        [DisplayName("Hiển thị")]
        public bool isActive { get; set; }
    }
}
