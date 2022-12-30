using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanSach.Models;

namespace WebsiteBanSach.Controllers
{
    public class OrderController : Controller
    {
        private QuanLyBanSachEntities db = new QuanLyBanSachEntities();


        public ActionResult Index(int? tt)
        {
            //Kiem tra Dang Nhap
            List<DonHang> ds = new List<DonHang>();

            if (Session["AdTaiKhoan"] == null || Session["AdTaiKhoan"].ToString() == "")
            {
                Response.Write("<script>alert('Bạn cần đăng nhập vào tài khoản Admin');window.location = 'Admin/Login';</script>"); //works great
            }

            // Lọc kết quả
            //  var idTrangThai = tt;
            if (tt == null)
            {
                ds = db.DonHangs.ToList();
            }
            else
            {
                ds = db.DonHangs.Where(n => n.TinhTrangGiaoHang == tt).ToList();
            }

            ViewBag.UnPrepared = db.DonHangs.Where(n => n.TinhTrangGiaoHang == 0).ToList().Count();
            ViewBag.UnShipping = db.DonHangs.Where(n => n.TinhTrangGiaoHang == 1).ToList().Count();
            ViewBag.UnCancel = db.DonHangs.Where(n => n.TinhTrangGiaoHang == -1).ToList().Count();
            return View(ds.OrderBy(n => n.MaDonHang).ToList());
        }

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // GET: Order/Create
        public ActionResult Create()
        {

            return View();//(db.Saches.ToList().OrderBy(n => n.MaSach));
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                donHang.NgayDat = DateTime.Now;
                donHang.LoaiKH = "Admin";
                donHang.TinhTrangGiaoHang = 0;
                db.DonHangs.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(donHang);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen", donHang.MaKH);
            return View(donHang);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDonHang,DaThanhToan,TinhTrangGiaoHang,NgayDat,NgayGiao,MaKH")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen", donHang.MaKH);
            return View(donHang);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            db.DonHangs.Remove(donHang);
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
        public void CapNhapDonHang(int MaSP, int SoLuong, int MaDH)
        {
            //Kiem Tra MaSP
            ChiTietDonHang chiTietDon = db.ChiTietDonHangs.SingleOrDefault(n => n.MaDonHang == MaDH && n.MaSach == MaSP);
            if (ModelState.IsValid)
            {
                chiTietDon.SoLuong = SoLuong;
                db.Entry(chiTietDon).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                Response.Write("<script>alert('Cập nhật đơn hàng không thành công ');</script>"); //works great
            }
        }
        public ActionResult PreparedOrder(int id)
        {
            DonHang don = db.DonHangs.SingleOrDefault(n => n.MaDonHang == id);

            if (ModelState.IsValid)
            {

                don.TinhTrangGiaoHang = 1;
                db.Entry(don).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Thanks for Feedback!";

            }
            else
            {
            }
            return RedirectToAction("Edit", new { @id = id });
        }
        public ActionResult FinishOrder(int id)
        {
            DonHang don = db.DonHangs.SingleOrDefault(n => n.MaDonHang == id);

            if (ModelState.IsValid)
            {

                don.TinhTrangGiaoHang = 3;
                don.NgayGiao = DateTime.Now;
                don.DaThanhToan = Convert.ToInt32(don.TongTien);
                db.Entry(don).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Thanks for Feedback!";

            }
            else
            {
            }
            return RedirectToAction("Edit", new { @id = id });
        }
        public ActionResult CancelOrder(int id)
        {
            DonHang don = db.DonHangs.SingleOrDefault(n => n.MaDonHang == id);

            if (ModelState.IsValid)
            {

                don.TinhTrangGiaoHang = -2;
                don.NgayHuyDon = DateTime.Now;
                db.Entry(don).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Thanks for Feedback!";



            }
            else
            {
            }
            return RedirectToAction("Edit", new { @id = id });
        }
    }
}
