using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebsiteBanSach.Models;
using System.Net;
using System.Data.Entity;

namespace WebsiteBanSach.Controllers
{
    public class NguoiDungController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        // GET: NguoiDung        
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                List<KhachHang> khachHangs = db.KhachHangs.Where(n => n.HoTen.Contains(kh.HoTen)).ToList();
                if(khachHangs.Count==0)
                {
                    // Chen du lieu vao bang khach hang
                    db.KhachHangs.Add(kh);
                    //Luu vao csdl
                    db.SaveChanges();
                    TempData["message"] = "Đăng ký thành công!!";
                    return RedirectToAction("DangNhap", "NguoiDung");
                }    
                else
                {
                    Response.Write("<script>alert('Tài khoản đã tồn tại!!!');</script>");
                }    
               
            }
            return View();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {

            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string sTaikhoan = f["txtTaiKhoan"].ToString();
            string sMatkhau = f.Get("txtMatKhau").ToString();
            var kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == sTaikhoan && n.MatKhau == sMatkhau);

            if (kh != null)
            {
                Session["name"] = kh.HoTen;
                Session["TaiKhoan"] = kh;
                return RedirectToAction("Index", "Home");
            }
            else
            {

            }
            ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu không đúng";
            return View();

        }

        //Chinh sua Profile
        //[HttpGet, Authorize]
        //DangXuat
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        //Tao Partial NguoiDung
        public ActionResult KhachHangPartial()
        {
            return PartialView();
        }
        [HttpGet]
        public ActionResult ChinhSua(int maKh)
        {
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH == maKh);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<string> gt = new List<string>() { "Nam", "Nữ" };
            ViewBag.GioiTinh = new SelectList(gt, kh.GioiTinh);
            return View(kh);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSua(KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                var kh = db.KhachHangs.Find(khachHang.MaKH);
                Session["name"] = kh.HoTen;
                TempData["message"] = "Cập nhật thành công";

            }
            List<string> gt = new List<string>() { "Nam", "Nữ" };
            ViewBag.GioiTinh = new SelectList(gt, khachHang.GioiTinh);

            return View(khachHang);
        }
        public ActionResult LichSuMuaHang(int? maKH)
        {
            List<DonHang> donHangs = db.DonHangs.Where(n => n.MaKH == maKH).ToList();
            if (donHangs.Count < 0)
            {
                donHangs = new List<DonHang>();
            }
            return View(donHangs);
        }
        public ActionResult HuyDon(int? maDH)
        {
            DonHang donHang = db.DonHangs.SingleOrDefault(n => n.MaDonHang == maDH);
            if (donHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else if(donHang.TinhTrangGiaoHang == 1)
            {
                ViewBag.ThongBao = "Bạn không thể hủy khi đơn hàng đang giao!!!";
            }
            else
            {
                donHang.TinhTrangGiaoHang = -2;
                db.Entry(donHang);
                db.SaveChanges();

            }
            
            return RedirectToAction("LichSuMuaHang", "NguoiDung",new {@maKH=donHang.MaKH});
        }
        public ActionResult Voucher()
        {
            return View(db.VOUCHERs.ToList());
        }
    }
    // POST: KhachHangs/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    // 



}
