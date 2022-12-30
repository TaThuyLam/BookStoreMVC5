using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanSach.Models;

namespace WebsiteBanSach.Controllers
{
    public class UserController : Controller
    {
        private QuanLyBanSachEntities db = new QuanLyBanSachEntities();

        public ActionResult LoyalCustomer(int? page)
        {
            //Kiem tra Dang Nhap
            if (Session["AdTaiKhoan"] == null || Session["AdTaiKhoan"].ToString() == "")
            {
                Response.Write("<script>alert('Bạn cần đăng nhập vào tài khoản Admin');window.location.pathname = '/Admin/Login';</script>"); //works great
            }
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            ViewBag.Lable = "MANAGE USERS / LOYAL CUSTOMERS";
            ViewBag.Count = db.KhachHangs.ToList().Count() + " Records Found";
            return View(db.KhachHangs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Admin(int? page)
        {
            //Kiem tra Dang Nhap
            if (Session["AdTaiKhoan"] == null || Session["AdTaiKhoan"].ToString() == "")
            {
                Response.Write("<script>alert('Bạn cần đăng nhập vào tài khoản Admin');window.location.pathname = '/Admin/Login';</script>"); //works great
            }
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            ViewBag.Lable = "MANAGE USERS / ADMINS";
            ViewBag.Count = db.Admins.ToList().Count() + " Records Found";
            return View(db.Admins.ToList().OrderBy(n => n.MaAD).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult VisitorCustomer(int? page)
        {
            //Kiem tra Dang Nhap
            if (Session["AdTaiKhoan"] == null || Session["AdTaiKhoan"].ToString() == "")
            {
                Response.Write("<script>alert('Bạn cần đăng nhập vào tài khoản Admin');window.location.pathname = '/Admin/Login';</script>"); //works great
            }
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            ViewBag.Lable = "MANAGE USERS / NORMAL CUSTOMERS";
            ViewBag.Count = db.DonHangs.Where(n=>n.MaKH==null).ToList().Count() + " Records Found";
            return View(db.DonHangs.Where(n => n.MaKH == null).ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khach = db.KhachHangs.Find(id);
            if (khach == null)
            {
                return HttpNotFound();
            }
            return View(khach);
        }

        // GET: Product/Create
        public ActionResult CreateLoyalCustomer()
        {
            ViewBag.Lable = "MANAGE USERS / LOYAL CUSTOMER / CREATE";
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateLoyalCustomer(KhachHang khach)
        {
          
            //Them vao database
            if (ModelState.IsValid)
            {
               
                db.KhachHangs.Add(khach);
                db.SaveChanges();
                Response.Write("<script>alert('Created Successfully!')</script>");
            }
            return RedirectToAction("LoyalCustomer");
        }

        // GET: Product/Edit/5
        public ActionResult EditLoyalCustomer(int? MaKH)
        {
            //Lay doi tuong sach theo ma
            KhachHang khach = db.KhachHangs.SingleOrDefault(n => n.MaKH == MaKH);
            if (khach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua data vao dropdownlist
            ViewBag.Lable = "MANAGE USERS / LOYAL CUSTOMER / EDIT";
            return View(khach);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditLoyalCustomer(KhachHang khach)
        {
            //Them vao database
            if (ModelState.IsValid)
            {
                //Thuc hien cap nhap trong model
                db.Entry(khach).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            //Dua data vao dropdownlist
            Response.Write("<script>alert('Đã lưu chỉnh sửa của bạn ');</script>"); //works great
            return View(khach);
        }

        // GET: Product/Delete/5

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khach = db.KhachHangs.Find(id);
            if (khach == null)
            {
                return HttpNotFound();
            }
            return View(khach);
        }

        // POST: Product/Delete/5
        /* [HttpPost, ActionName("Delete")]
         [ValidateAntiForgeryToken]*/
        public ActionResult DeleteConfirmed(int id)
        {
            Response.Write("<script>alert('Đã lưu chỉnh sửa của bạn ');</script>");
            KhachHang khach = db.KhachHangs.Find(id);
            db.KhachHangs.Remove(khach);
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
                return RedirectToAction("Index" + s);
        }
        [HttpPost]
        public ActionResult KetQuaTimKiemAd(FormCollection f, int? page)
        {

            string sTuKhoa = f["txtTimKiemAd"].ToString();
            ViewBag.STuKhoa = sTuKhoa;
            List<KhachHang> kqtk = db.KhachHangs.Where(n => n.HoTen.Contains(sTuKhoa)).ToList();
            //Phan Trang
            int pageNumber = (page ?? 1);
            int pageSize = 5;

            if (kqtk.Count == 0)
            {
                Response.Write("<script>alert('Không tìm thấy khách hàng! Quay về trang quản lý');window.location = 'Index';</script>"); //works great

            }
            ViewBag.ThongBao = "Đã tìm thấy " + kqtk.Count + " Kết quả";
            ViewBag.Lable = "MANAGE USERS";
            ViewBag.Count = db.KhachHangs.ToList().Count() + " Records Found";
            return View(kqtk.OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize)); ;
        }
        [HttpGet]
        public ActionResult KetQuaTimKiemAd(string sTuKhoa, int? page)
        {
            ViewBag.STuKhoa = sTuKhoa;
            List<KhachHang> kqtk = db.KhachHangs.Where(n => n.HoTen.Contains(sTuKhoa)).ToList();
            //Phan Trang
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            if (kqtk.Count == 0)
            {
                Response.Write("<script>alert('Không tìm thấy khách hàng!')</script>"); //works great

            }
            ViewBag.ThongBao = "Đã tìm thấy " + kqtk.Count + " Kết quả";

            return View(kqtk.OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));
        }
    }
}
