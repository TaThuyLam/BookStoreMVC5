using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using WebsiteBanSach.Models;

namespace WebsiteBanSach.Controllers
{
    public class SachController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        // GET: Sach, Sach Moi Partial//
        public PartialViewResult SachMoiPartial()
        {
            var listSachMoi = db.Saches.Where(n => n.Moi == 1).Take(3).ToList();
            return PartialView(listSachMoi);

        }
        public ViewResult XemChiTiet(int MaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach==MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.TenChuDe = db.ChuDes.Single(n => n.MaChuDe == sach.MaChuDe).TenChuDe;
            ViewBag.TenNXB = db.NhaXuatBans.Single(n => n.MaNXB == sach.MaNXB).TenNXB;
            return View(sach);
        }
        //Tat ca Sach
        public ActionResult Tatca(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 16;
            
            return View(db.Saches.ToList().OrderBy(n => n.TenSach).ToPagedList(pageNumber, pageSize));
        }
        //Timkiem
        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection f, int? page)
        {
            
            string sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.STuKhoa = sTuKhoa;
            List<Sach> kqtk = db.Saches.Where(n => n.TenSach.Contains(sTuKhoa)).ToList();
            //Phan Trang
            int pageNumber = (page ?? 1);
          
            int pageSize = 16;
          
            if (kqtk.Count==0)
            {
                Response.Write("<script>alert('Không tìm thấy sách! Quay về trang sách');window.location = 'Tatca';</script>"); //works great
 
            }
            ViewBag.ThongBao = "Đã tìm thấy " + kqtk.Count + " Kết quả";
            
            return View(kqtk.OrderBy(n => n.TenSach).ToPagedList(pageNumber, pageSize)); ;
        }
        [HttpGet]
        public ActionResult KetQuaTimKiem(string sTuKhoa, int? page)
        {
            ViewBag.STuKhoa = sTuKhoa;
            List<Sach> kqtk = db.Saches.Where(n => n.TenSach.Contains(sTuKhoa)).ToList();
            //Phan Trang
          
           
            int pageNumber = (page ?? 1)  ;
            int pageSize = 16;
            if (kqtk.Count == 0)
            {
                Response.Write("<script>alert('Đã thêm sách vào giỏ hàng');window.location = 'Tatca';</script>"); //works great

            }
        
            ViewBag.ThongBao = "Đã tìm thấy " + kqtk.Count + " Kết quả";

            return View(kqtk.OrderBy(n => n.TenSach).ToPagedList(pageNumber, pageSize)); 
        }
    }
}