using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanSach.Models
{
    [MetadataTypeAttribute(typeof(DonHangMetadata))]
    public partial class DonHang
    {
        internal sealed class DonHangMetadata
        {
            [Display(Name = "Mã Đơn Hàng")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public int MaDonHang { get; set; }
            [Display(Name = "Đã Thanh Toán")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public Nullable<int> DaThanhToan { get; set; }
            [Display(Name = "Tình Trạng Giao Hàng")]//Thuộc tính Display dùng để dặt tên lại cho cột

            public Nullable<int> TinhTrangGiaoHang { get; set; }
            [Display(Name = "Ngày Đặt")]//Thuộc tính Display dùng để dặt tên lại cho cột
            [DataType(DataType.DateTime)]
            [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd H:mm:ss tt}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> NgayDat { get; set; }
            [Display(Name = "Ngày Giao")]//Thuộc tính Display dùng để dặt tên lại cho cột
            [DataType(DataType.DateTime)]
            [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd H:mm:ss tt}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> NgayGiao { get; set; }
            [Display(Name = "Mã Khách Hàng")]//Thuộc tính Display dùng để dặt tên lại cho cột
            public Nullable<int> MaKH { get; set; }
        }
    }
}