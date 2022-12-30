using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// using 2 thu vien thiet ke class metadata
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanSach.Models
{
    [MetadataTypeAttribute(typeof(SachMetadata))]
    public partial class Sach
    {
        internal sealed class SachMetadata
        {
            [Display(Name = "Mã Sách")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public int MaSach { get; set; }
            [Display(Name = "Tên Sách")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public string TenSach { get; set; }
            [Display(Name = "Giá Bán")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public Nullable<decimal> GiaBan { get; set; }
            [Display(Name = "Mô Tả")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public string MoTa { get; set; }
            [Display(Name = "Ảnh Bìa")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public string AnhBia { get; set; }
            [Display(Name = "Ngày Cập Nhập")]//Thuộc tính Display dùng để dặt tên lại cho cột
            [DataType(DataType.DateTime)]
            [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd H:mm:ss tt}", ApplyFormatInEditMode =true)]
            public Nullable<System.DateTime> NgayCapNhap { get; set; }
            [Display(Name = "Số Lượng Tồn")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public Nullable<int> SoLuongTon { get; set; }
            [Display(Name = "Mã NXB")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public Nullable<int> MaNXB { get; set; }
            [Display(Name = "Mã Chủ Đề")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public Nullable<int> MaChuDe { get; set; }
            [Display(Name = "Mới")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public Nullable<int> Moi { get; set; }
        }
    }
}