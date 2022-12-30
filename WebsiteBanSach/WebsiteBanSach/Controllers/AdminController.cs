using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanSach.Models;
namespace WebsiteBanSach.Controllers
{
    public class AdminController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult SignUp(Admin ad)
        {
            if (ModelState.IsValid)
            {
                // Chen du lieu vao bang khach hang
                db.Admins.Add(ad);
                //Luu vao csdl
                db.SaveChanges();
                Response.Write("<script>alert('Đăng ký thành công!')</script>"); //works great
            }


            return View();
        }

        [HttpGet]
        public ActionResult LogIn()
        {

            return View();
        }
        [HttpPost]

        public ActionResult LogIn(FormCollection f)
        {
            string sTaikhoan = f["txtTaiKhoan"].ToString();
            string sMatkhau = f.Get("txtMatKhau").ToString();
            Admin ad = db.Admins.SingleOrDefault(n => n.TaiKhoan == sTaikhoan && n.MatKhau == sMatkhau);


            if (ad != null)
            {
                Session["adName"] = ad.TaiKhoan;
                Session["AdTaiKhoan"] = ad;
                Response.Write("<script>alert('Bạn đã đăng nhập!')</script>");
                return RedirectToAction("Index", "Product");
            }
            else
            {
                Response.Write("<script>alert('Đăng Nhập Thất Bại! Tài khoản hoặc mật khẩu không chính xác')</script>"); //works great
            }


            return View();

        }
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("LogIn");
        }
    }
}