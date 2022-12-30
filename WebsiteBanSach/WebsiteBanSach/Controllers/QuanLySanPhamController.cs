using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanSach.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebsiteBanSach.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
       
        //Chinh sua san pham 
        [HttpGet]
        public ActionResult ChinhSua(int MaSach)
        {
            //Lay doi tuong sach theo ma
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua data vao dropdownlist
            ViewBag.MaChuDe = new SelectList(db.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe",sach.MaChuDe);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB",sach.MaNXB);

            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(Sach sach)
        {

           
            //Them vao database
            if(ModelState.IsValid)
            {
                //Thuc hien cap nhap trong model
                db.Entry(sach).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            //Dua data vao dropdownlist
            ViewBag.MaChuDe = new SelectList(db.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe", sach.MaChuDe);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            Response.Write("<script>alert('Đã lưu chỉnh sửa của bạn ');</script>"); //works great
            return View(sach);
             
        }
        //Hien thi
        public ActionResult HienThi(int MaSach)
        {
            //Lay doi tuong sach theo ma
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        //Xóa sản phẩm
        [HttpGet]
        public ActionResult Xoa(int MaSach)
        {

            //Lay doi tuong sach theo ma
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
            
        }
        [HttpPost, ActionName("Xoa")]
        public ActionResult XacNhanXoa(int MaSach)
        {

            //Lay doi tuong sach theo ma
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Saches.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        //Timkiem
        [HttpPost]
        public ActionResult KetQuaTimKiemAd(FormCollection f, int? page)
        {

            string sTuKhoa = f["txtTimKiemAd"].ToString();
            ViewBag.STuKhoa = sTuKhoa;
            List<Sach> kqtk = db.Saches.Where(n => n.TenSach.Contains(sTuKhoa)).ToList();
            //Phan Trang
            int pageNumber = (page ?? 1);
            int pageSize = 5;

            if (kqtk.Count == 0)
            {
                Response.Write("<script>alert('Không tìm thấy sách! Quay về trang quản lý');window.location = 'Index';</script>"); //works great

            }
            ViewBag.ThongBao = "Đã tìm thấy " + kqtk.Count + " Kết quả";

            return View(kqtk.OrderBy(n => n.TenSach).ToPagedList(pageNumber, pageSize)); 
        }
        [HttpGet]
        public ActionResult KetQuaTimKiemAd(string sTuKhoa, int? page)
        {
            ViewBag.STuKhoa = sTuKhoa;
            List<Sach> kqtk = db.Saches.Where(n => n.TenSach.Contains(sTuKhoa)).ToList();
            //Phan Trang
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            if (kqtk.Count == 0)
            {
                Response.Write("<script>alert('Không tìm thấy sách!')</script>"); //works great

            }
            ViewBag.ThongBao = "Đã tìm thấy " + kqtk.Count + " Kết quả";

            return View(kqtk.OrderBy(n => n.TenSach).ToPagedList(pageNumber, pageSize)); 
        }
       
      
        //Quan Ly Don Hang 
        #region DonHang
        public ActionResult QuanLyGioHang(int? tt)
        {
            //Kiem tra Dang Nhap
            List<DonHang> ds,kq = new List<DonHang>();
            ds = db.DonHangs.ToList();

            if (Session["AdTaiKhoan"] == null || Session["AdTaiKhoan"].ToString() == "")
            {
                Response.Write("<script>alert('Bạn cần đăng nhập vào tài khoản Admin');window.location = 'Admin/Login';</script>"); //works great
            }
            // Lọc kết quả
                var idTrangThai = tt;
                if (idTrangThai == 1)
                {
                    foreach (DonHang dh in ds)
                    {
                        if (dh.DaThanhToan == 1)
                        {
                            kq.Add(dh);
                        }
                    }
                ViewBag.Loc = "Đã giao";
                }
                else
                {
                    if (idTrangThai == 0)
                    {
                        foreach (DonHang dh in ds)
                        {
                            if (dh.DaThanhToan == null)
                            {
                                kq.Add(dh);
                            }
                        }
                    ViewBag.Loc = "Đang giao";
                }
                    else
                    {
                        kq = ds;
                    ViewBag.Loc = "Tất cả";
                }


                }
            
            return View(kq.OrderBy(n => n.MaDonHang).ToPagedList(1, kq.Count));
        }
        //Chinh sua san pham 
        [HttpGet]
        public ActionResult ChinhSuaDonHang(int MaDonHang)
        {
            //Lay doi tuong sach theo ma
            DonHang dh = db.DonHangs.SingleOrDefault(n => n.MaDonHang == MaDonHang);
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
          
           

            return View(dh);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSuaDonHang(DonHang dh)
        {


            //Them vao database
            if (ModelState.IsValid)
            {
                //Thuc hien cap nhap trong model
                db.Entry(dh).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
          
            Response.Write("<script>alert('Đã lưu chỉnh sửa của bạn ');</script>"); //works great
            return View(dh);

        }
        //Xem Chi Tiet Don Hang
        public ActionResult  XemChiTiet(int MaDonHang)
        {
            //Lay doi tuong sach theo ma
            List<ChiTietDonHang> ctdh = db.ChiTietDonHangs.Where(n => n.MaDonHang == MaDonHang).ToList();
            if (ctdh.Count == 0)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ctdh);
        }
        //Xóa don hang
        [HttpGet]
        public ActionResult XoaDonHang(int MaDonHang)
        {

            //Lay doi tuong sach theo ma
            DonHang dh = db.DonHangs.SingleOrDefault(n => n.MaDonHang == MaDonHang);
            List<ChiTietDonHang> ctdh = db.ChiTietDonHangs.Where(n => n.MaDonHang == MaDonHang).ToList();
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dh);

        }
        [HttpPost, ActionName("XoaDonHang")]
        public ActionResult XacNhanXoaDonHang(int MaDonHang)
        {

            //Lay doi tuong sach theo ma
            DonHang dh = db.DonHangs.SingleOrDefault(n => n.MaDonHang == MaDonHang);
           List<ChiTietDonHang> ctdh = db.ChiTietDonHangs.Where(n => n.MaDonHang == MaDonHang).ToList();
            foreach(ChiTietDonHang ctdhitem in ctdh)
            {
                if (ctdhitem != null)
                {
                    db.ChiTietDonHangs.Remove(ctdhitem);
                }

            }
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
         
            db.DonHangs.Remove(dh);
           
            db.SaveChanges();
            return RedirectToAction("QuanLyGioHang");

        }
        #endregion
        //Quan Ly NXB
        #region NXB
      
        public ActionResult QuanLyNXB(int? page)
        {
            //Kiem tra Dang Nhap

            if (Session["AdTaiKhoan"] == null || Session["AdTaiKhoan"].ToString() == "")
            {
                Response.Write("<script>alert('Bạn cần đăng nhập vào tài khoản Admin');window.location = 'Admin/Login';</script>"); //works great
            }
          /*  else
            {
                if (page == null)
                {
                    Response.Write("<script>alert('Bạn đã đăng nhập!')</script>"); //works great
                }
            }*/
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(db.NhaXuatBans.ToList().OrderBy(n => n.MaNXB).ToPagedList(pageNumber, pageSize));
        }
        //Them Moi NXB
        [HttpGet]
        public ActionResult ThemNXB()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemNXB(NhaXuatBan NXB)
        {

            
            //Them vao database
            if (ModelState.IsValid)
            {
               
                db.NhaXuatBans.Add(NXB);
                db.SaveChanges();
                Response.Write("<script>alert('Tạo thành công!');</script>"); //works great
            }
            return View();
        }
        //Chinh sua NXB
        [HttpGet]
        public ActionResult ChinhSuaNXB(int MaNXB)
        {
            //Lay doi tuong sach theo ma
            NhaXuatBan nxb = db.NhaXuatBans.SingleOrDefault(n => n.MaNXB == MaNXB);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
          

            return View(nxb);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSuaNXB(NhaXuatBan nxb)
        {


            //Them vao database
            if (ModelState.IsValid)
            {
                //Thuc hien cap nhap trong model
                db.Entry(nxb).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
          
            Response.Write("<script>alert('Đã lưu chỉnh sửa của bạn ');</script>"); //works great
            return View(nxb);

        }
        //Xóa NXB
        [HttpGet]
        public ActionResult XoaNXB(int MaNXB)
        {

            //Lay doi tuong sach theo ma
            NhaXuatBan nxb = db.NhaXuatBans.SingleOrDefault(n => n.MaNXB == MaNXB);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nxb);

        }
        [HttpPost, ActionName("XoaNXB")]
        public ActionResult XacNhanXoaNXB(int MaNXB)
        {

            //Lay doi tuong sach theo ma
            NhaXuatBan nxb = db.NhaXuatBans.SingleOrDefault(n => n.MaNXB == MaNXB);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NhaXuatBans.Remove(nxb);
            db.SaveChanges();
            return RedirectToAction("QuanLyNXB");

        }
        #endregion
        //QuanLyChuDe
        #region ChuDe
        public ActionResult QuanLyChuDe(int? page)
        {
            //Kiem tra Dang Nhap

            if (Session["AdTaiKhoan"] == null || Session["AdTaiKhoan"].ToString() == "")
            {
                Response.Write("<script>alert('Bạn cần đăng nhập vào tài khoản Admin');window.location = 'Admin/Login';</script>"); //works great
            }
          /*  else
            {
                if (page == null)
                {
                    Response.Write("<script>alert('Bạn đã đăng nhập!')</script>"); //works great
                }
            }*/
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(db.ChuDes.ToList().OrderBy(n => n.MaChuDe).ToPagedList(pageNumber, pageSize));
        }
        //Them Moi ChuDe
        [HttpGet]
        public ActionResult ThemChuDe()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemChuDe(ChuDe cd)
        {


            //Them vao database
            if (ModelState.IsValid)
            {

                db.ChuDes.Add(cd);
                db.SaveChanges();
                Response.Write("<script>alert('Tạo thành công!');</script>"); //works great
            }
            return View();
        }
        //Chinh sua ChuDe
        [HttpGet]
        public ActionResult ChinhSuaChuDe(int MaChuDe)
        {
            //Lay doi tuong sach theo ma
            ChuDe cd = db.ChuDes.SingleOrDefault(n => n.MaChuDe == MaChuDe);
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }


            return View(cd);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSuaChuDe(ChuDe cd)
        {


            //Them vao database
            if (ModelState.IsValid)
            {
                //Thuc hien cap nhap trong model
                db.Entry(cd).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            Response.Write("<script>alert('Đã lưu chỉnh sửa của bạn ');</script>"); //works great
            return View(cd);

        }
        //Xóa Chu De
        [HttpGet]
        public ActionResult XoaChuDe(int MaChuDe)
        {

            //Lay doi tuong sach theo ma
            ChuDe cd = db.ChuDes.SingleOrDefault(n => n.MaChuDe == MaChuDe);
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(cd);

        }
        [HttpPost, ActionName("XoaChuDe")]
        public ActionResult XacNhanXoaChuDe(int MaChuDe)
        {

            //Lay doi tuong sach theo ma
            ChuDe cd = db.ChuDes.SingleOrDefault(n => n.MaChuDe == MaChuDe);
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.ChuDes.Remove(cd);
            db.SaveChanges();
            return RedirectToAction("QuanLyChuDe");

        }
        #endregion
   

    }

}



