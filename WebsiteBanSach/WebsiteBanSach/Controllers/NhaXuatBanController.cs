using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanSach.Models;

namespace WebsiteBanSach.Controllers
{
    public class NhaXuatBanController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        // GET: NhaXuatBan Partial//
        public PartialViewResult NhaXuatBanPartial()
        {

            return PartialView(db.NhaXuatBans.Take(7).OrderBy(x => x.TenNXB).ToList());

        }
        public PartialViewResult Filter_NXB()
        {

            return PartialView(db.NhaXuatBans.Take(7).ToList());

        }
        //Hien Thi Sach Theo Nha Xuat Ban
        public ViewResult SachTheoNXB(int manxb)
        {
            //Kiem Tra ChuDe co ton ai khong
            NhaXuatBan nxb = db.NhaXuatBans.SingleOrDefault(n => n.MaNXB == manxb);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Sach> listsach = db.Saches.Where(n => n.MaNXB == manxb).OrderBy(n => n.GiaBan).ToList();
            if (listsach.Count == 0)
            {
                ViewBag.Sach = "Khong co sach thuoc nxb nay";
            }
            return View(listsach);
        }
        //Hien thi tac ca nha xuat ban
        public ViewResult DanhMucNXB()
        {
            return View(db.NhaXuatBans.ToList());
        }
    }
}