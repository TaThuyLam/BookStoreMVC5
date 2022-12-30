using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebsiteBanSach.Models;

namespace WebsiteBanSach.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        #region Gio Hang 
        // GET: GioHang
        //Lay Gio Hang

        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                //Neu Gio hang chua ton tai thi tien hanh khoi tao list GioHang
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        //Them GioHang
        public ActionResult ThemGioHang(int iMaSach, string strUrl)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == iMaSach);
            if (sach==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lay ra Session GioHang
            List<GioHang> lstGioHang = LayGioHang();
            //Kiem Tra Sach nay da ton tai trong Session[GioHang]
            GioHang SanPham = lstGioHang.Find(n => n.iMaSach == iMaSach);
            if (SanPham == null)
            {
                SanPham = new GioHang(iMaSach);
                //Add san pham moi them
                lstGioHang.Add(SanPham);
                return Redirect(strUrl);
            }
            else
            {
                SanPham.iSoLuong++;
                return Redirect(strUrl);
            }
        }
        //Cap Nhap GioHang
        public ActionResult CapNhapGioHang(int iMaSP, FormCollection f )
        {
            //Kiem Tra MaSP
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == iMaSP);
            //Neu get sai masp thi se tra ve trang loi 404
            if (sach==null)
            {
                Response.StatusCode = 404;
                return null;

            }
            //Lay GioHAng ra tu Session
            List <GioHang> lstGioHang = LayGioHang();
            bool kiemtra = true;
            foreach(GioHang gh in lstGioHang)
            {
                if(gh.iSoLuong<=0)
                { kiemtra = false; }
            }
            //Kiem tra san pham co ton tai trong session[GioHang]
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSP);
            //Neu ton tai san pham trong Gio Hang thi cho phep sua soLuong
            if(sanpham!=null)
            {
                sanpham.iSoLuong = int.Parse( f["txtSoLuong"].ToString());
                if (sanpham.iSoLuong <= 0)
                {
                    return RedirectToAction("SuaGioHang");
                }
            }
           if(kiemtra == false)
            {
                return RedirectToAction("SuaGioHang");
            }
            return RedirectToAction("GioHang");
        }
        //Xoa GioHang
        public ActionResult XoaGioHang(int iMaSP)
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.RemoveAll(n => n.iMaSach == iMaSP);
            Session["GioHang"] = lstGioHang;
            return RedirectToAction("GioHang");

            /* //Kiem Tra MaSP
             Sach sach = db.Sach.SingleOrDefault(n => n.MaSach == iMaSP);
             //Neu get sai masp thi se tra ve trang loi 404
             if (sach == null)
             {
                 Response.StatusCode = 404;
                 return null;

             }
             //Lay GioHAng ra tu Session
             List<GioHang> lstGioHang = LayGioHang();
             //Kiem tra san pham co ton tai trong session[GioHang]
             GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSP);
             //Neu ton tai san pham trong Gio Hang thi cho phep sua soLuong
             if (sanpham != null)
             {
                 lstGioHang.RemoveAll(n=>n.iMaSach==iMaSP);

             }*/
        }
        //Xay Dung Trang Gio Hang
        public ActionResult GioHang()
        {
            List < GioHang > lstGioHang = new List<GioHang>();  
            if (Session["GioHang"] != null)
            {
                lstGioHang = LayGioHang();
            }
           foreach(GioHang hang in lstGioHang)
            {
                Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == hang.iMaSach);
                if (hang.iSoLuong>sach.SoLuongTon)
                {
                   
                    Response.Write("<script>alert('Số lượng quyển "+sach.TenSach+" chỉ còn "+sach.SoLuongTon +" cuốn')</script>");
                    return View(lstGioHang);

                }
            }    
         
            return View(lstGioHang);
        }
        //Xay dung tinh tong so luong va tong tien

        //Tinh Tong So Luong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang!=null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
                if (iTongSoLuong < 0)
                {
                    iTongSoLuong = 0;
                }
            }
            
            return iTongSoLuong;
        }
        // Tính tổng mặt hàng
        private int TongMatHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            return lstGioHang.Count();
            
        }
        //Tinh Tong Thanh Tien 
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.ThanhTien);
            }
            return dTongTien;
        }
        //Tao Partial GioHang
        public ActionResult GioHangPartial()
        {
            if (TongSoLuong()==0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongMatHang = TongMatHang();
            return PartialView();
        }
        //XAy dung View cho nguoi dung chinh sua gio hang
        public ActionResult SuaGioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();
         

            return View(lstGioHang);
        }
        #endregion
        #region Dat Hang
        //Xay dung chuc nang dat hang 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DatHang(DonHang dh)
        {
            KhachHang kh = Session["TaiKhoan"] as KhachHang;
            List<GioHang> gh = LayGioHang();

             //Kiem tra Dang Nhap
             if(kh!=null)
                 {
                        dh.MaKH = kh.MaKH;
                dh.LoaiKH = "Khách hàng";
                 }
            else
            {
                dh.LoaiKH = "Khách hàng vãng lai";
            }
             //Them don dat hang 
             if (gh.Count()==0)
                 {
                     return RedirectToAction("Index", "Home");
                 }
            //Them Don Hang
                double a = 0;
                 dh.NgayDat = DateTime.Now;
                 db.DonHangs.Add(dh);

            //Them Chi tiet don hang 
            foreach (var item in gh)
            {
                a = item.dDonGia * item.iSoLuong;
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDonHang = dh.MaDonHang;
                ctdh.MaSach = item.iMaSach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = item.dDonGia.ToString();
                db.ChiTietDonHangs.Add(ctdh);
                // Cập nhật số lượng tồn
                Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == item.iMaSach);
                sach.SoLuongTon = sach.SoLuongTon - item.iSoLuong;

                //Thuc hien cap nhap trong model
                db.Entry(sach).State = System.Data.Entity.EntityState.Modified;

                Session["GioHang"] = new List<GioHang>();

            }
            
                // Cập nhật đơn hàng
                dh.TongTien = a;
                dh.TinhTrangGiaoHang = 0;
                db.SaveChanges();
                TempData["message"] = "Bạn đã đặt hàng thành công!!!";
                 return RedirectToAction("Index", "Home");
           
        }

        public ActionResult ThanhToan()
        {
            List<GioHang> gh = LayGioHang();
            if (gh.Count() == 0)
            {
                Response.Write("<script>alert('Bạn cần chọn sản phẩm để đặt!!!');window.location.pathname = '/Home/Index';</script>"); //works great
            }
            foreach (GioHang hang in gh)
            {
                Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == hang.iMaSach);
                if (hang.iSoLuong > sach.SoLuongTon)
                {
                    return RedirectToAction("GioHang", "GioHang");

                }
            }
            KhachHang kh = Session["TaiKhoan"] as KhachHang;
            if (kh == null)
            {
                return View();
            }   
            else
            {
                DonHang dh = new DonHang();
                dh.TenKH=kh.HoTen;
                dh.SoDienThoai = kh.DienThoai;
                dh.DiaChi = kh.DiaChi;
                return View(dh);
            }    
            
        }
        #endregion

    }
}