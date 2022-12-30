using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsiteBanSach.Models;

namespace WebsiteBanSach.Controllers
{
    public class ProductController : Controller
    {
        private QuanLyBanSachEntities db = new QuanLyBanSachEntities();

        // GET: Product

        public ActionResult Index(int? page)
        {
            //Kiem tra Dang Nhap
            if (Session["AdTaiKhoan"] == null || Session["AdTaiKhoan"].ToString() == "")
            {
                Response.Write("<script>alert('Bạn cần đăng nhập vào tài khoản Admin');window.location.pathname = '/Admin/Login';</script>"); //works great
            }
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            ViewBag.Lable = "MANAGE PRODUCTS";
            ViewBag.Count = db.Saches.ToList().Count() + " Records Found";
            return View(db.Saches.Where(n => n.Moi == 1).ToList().OrderBy(n => n.MaSach).ToPagedList(pageNumber, pageSize));
        }



        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.Lable = "MANAGE PRODUCTS / CREATE";
            ViewBag.MaChuDe = new SelectList(db.ChuDes, "MaChuDe", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Sach sach)
        {
            //Dua data vao dropdownlist
            ViewBag.MaChuDe = new SelectList(db.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            //Kiểm tra dữ liệu
            if (sach.TenSach == "" || sach.SoLuongTon == null || sach.GiaBan == null)
            {
                TempData["unsuccess"] = "Error!!!";
                return View();
            }
            else if(sach.AnhBia==null)
                {
                TempData["unsuccess"] = "Error!!!";
                return View();
            }
            else
            {     
                //Them vao database
                sach.NgayCapNhap = DateTime.Now;
                sach.Moi = 1;
                db.Saches.Add(sach);
                db.SaveChanges();
                TempData["success"] = "Created Sucessfully!!!";
                 return RedirectToAction("Index"); ;
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? MaSach)
        {
            //Lay doi tuong sach theo ma
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua data vao dropdownlist
            ViewBag.MaChuDe = new SelectList(db.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe", sach.MaChuDe);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            ViewBag.Lable = "MANAGE PRODUCTS / EDIT";
            return View(sach);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Sach sach)
        {
            //Them vao database
            if (ModelState.IsValid)
            {
                //Thuc hien cap nhap trong model
                sach.Moi = 1;
                db.Entry(sach).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            //Dua data vao dropdownlist
            ViewBag.MaChuDe = new SelectList(db.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe", sach.MaChuDe);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            Response.Write("<script>alert('Đã lưu chỉnh sửa của bạn ');</script>"); //works great
            return RedirectToAction("Index");
        }

        // GET: Product/Delete/5

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // POST: Product/Delete/5
        /* [HttpPost, ActionName("Delete")]
         [ValidateAntiForgeryToken]*/
        public ActionResult DeleteConfirmed(int? id)
        {
            Response.Write("<script>alert('Đã lưu chỉnh sửa của bạn ');</script>");
            Sach sach = db.Saches.Find(id);
            List<ChiTietDonHang> chis = db.ChiTietDonHangs.Where(n => n.MaSach == id).ToList();
            if (chis.Count > 0)
            {
                db.Saches.Remove(sach);
            }
            else
            {
                sach.Moi = 0;
                db.Entry(sach);
            }
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult DeleteAll(string s)
        {
            if (s == null)
            {
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Index");
        }
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
            ViewBag.Lable = "MANAGE PRODUCTS";
            ViewBag.Count = kqtk.Count + " Records Found";
            return View(kqtk.OrderBy(n => n.TenSach).ToPagedList(pageNumber, pageSize)); ;
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


    }
}
