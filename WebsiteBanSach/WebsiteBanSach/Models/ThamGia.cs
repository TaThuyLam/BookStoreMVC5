//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebsiteBanSach.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ThamGia
    {
        public int MaSach { get; set; }
        public int MaTacGia { get; set; }
        public string VaiTro { get; set; }
        public string ViTri { get; set; }
    
        public virtual Sach Sach { get; set; }
        public virtual TacGia TacGia { get; set; }
    }
}
