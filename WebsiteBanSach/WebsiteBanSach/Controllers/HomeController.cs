using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanSach.Models;

namespace WebsiteBanSach.Controllers
{
    public class HomeController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();

        // GET: Home
        public ActionResult Index()
        {
            List<Sach> saches = db.Saches.OrderBy(n => n.NgayCapNhap).ToList();
            List<Sach> sachmoi = new List<Sach>();
            List<Sach> sachbanchay = new List<Sach>();
            int i = saches.Count - 1;
            while (i > saches.Count - 5)
            {
                sachmoi.Add(saches[i]);
                i--;
            }
            ViewBag.SachMoi = sachmoi;
            var chiTietDonHang = db.ChiTietDonHangs.GroupBy(n => n.MaSach).Select(grp => new { MaSach = grp.Key, total = grp.Sum(s => s.SoLuong) }).OrderBy(n => n.total).ToList();
            var items = new Dictionary<int, int>();
            foreach (var number in chiTietDonHang)
            {
                items.Add(number.MaSach, number.total.Value);
            }
            int index = chiTietDonHang.Count - 1;
            int length = index - 4;
            if (chiTietDonHang.Count >= 5)
            {
                while (index > length)
                {
                    var x = chiTietDonHang[index];
                    sachbanchay.Add(saches.Find(n => n.MaSach == x.MaSach));
                    index--;
                }
            }
            else if (chiTietDonHang.Count > 0)
            {
                while (index >= 0)
                {
                    var x = chiTietDonHang[index];
                    sachbanchay.Add(saches.Find(n => n.MaSach == x.MaSach));
                    index--;
                }
            }

            ViewBag.SachBanChay = sachbanchay;
            ViewBag.ChitietDonHang = items;
            return View(sachbanchay);
        }
        public ActionResult LienHe()
        {
            return View();
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult HuongDan()
        {
            return View();
        }
        public ActionResult TraCuuDonHang(FormCollection f)
        {
            string txtTimKiemDonHang = f["txtTimKiemDonHang"].ToString();
            if (txtTimKiemDonHang == "")
            {
                return RedirectToAction("Index");

            }
            else
            {
                List<DonHang> kqtk = db.DonHangs.Where(n => n.SoDienThoai.Contains(txtTimKiemDonHang)).ToList();
                if (kqtk.Count == 0)
                {
                    ViewBag.ThongBaoTraCuu = "Tra cứu đơn hàng theo mã đơn " + txtTimKiemDonHang;
                    kqtk = db.DonHangs.Where(n => n.MaDonHang.ToString().Contains(txtTimKiemDonHang)).ToList();
                    if (kqtk.Count == 0)
                    {
                        Response.Write("<script>alert('Không tìm thấy đơn hàng nào!!!');window.location = 'Index';</script>"); //works great

                    }

                }
                else
                {
                    ViewBag.ThongBaoTraCuu = "Tra cứu đơn hàng cho số điện thoại " + txtTimKiemDonHang;

                }
                foreach (DonHang d in kqtk)
                {
                    if (d.TinhTrangGiaoHang < 0)
                    {
                        kqtk.Remove(d);
                    }
                }



                return View(kqtk);
            }

        }
    }
}