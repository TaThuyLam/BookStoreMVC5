using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanSach.Models;

namespace WebsiteBanSach.Controllers
{
    public class ChuDeController : Controller
    {

        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        // GET: ChuDe Partial//
        public PartialViewResult ChuDePartial()
        {                
            return PartialView(db.ChuDes.Take(7).ToList());

        }
        public PartialViewResult Filter_CD()
        {

            return PartialView(db.ChuDes.Take(7).ToList());

        }
        //Sachtheo chu de
        public ViewResult SachTheoChuDe(int MaChuDe)
        {
            //Kiem Tra ChuDe co ton ai khong
            ChuDe chude = db.ChuDes.SingleOrDefault(n => n.MaChuDe == MaChuDe);
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Sach> listsach = db.Saches.Where(n => n.MaChuDe == MaChuDe).OrderBy(n=>n.GiaBan).ToList();
            if (listsach.Count==0)
            {
                ViewBag.Sach = "Khong co sach thuoc chu de nay";
            }
            if (listsach.Count > 0)
            {
                ViewBag.LabelChuDe = chude.TenChuDe.ToString();
            }
            return View(listsach);

        }
        //HienThi tat ca chu de
        public ViewResult DanhMucChuDe()
        {
            return View(db.ChuDes.ToList());
        }
    }
}