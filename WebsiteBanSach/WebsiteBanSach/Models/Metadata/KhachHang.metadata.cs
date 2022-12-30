using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// using 2 thu vien thiet ke class metadata
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanSach.Models
{
    [MetadataTypeAttribute(typeof(KhachHangMetadata))]
    public partial class KhachHang
    {
        internal sealed class KhachHangMetadata
        {
            [Display(Name = "Mã Khách Hàng")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public int MaKH { get; set; }
            [Display(Name = "Họ Tên")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public string HoTen { get; set; }
            [Display(Name = "Tài Khoản")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public string TaiKhoan { get; set; }
            [Display(Name = "Mật Khẩu")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public string MatKhau { get; set; }
            public string Email { get; set; }
            [Display(Name = "Địa Chỉ")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public string DiaChi { get; set; }
            [Display(Name = "Số điện thoại")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public string DienThoai { get; set; }

            [Display(Name = "Giới Tính")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public string GioiTinh { get; set; }
            [Display(Name = "Ngày Sinh")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public Nullable<System.DateTime> NgaySinh { get; set; }
        }
    }
}