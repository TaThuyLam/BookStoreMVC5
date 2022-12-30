using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanSach.Models;

namespace WebsiteBanSach.Controllers
{
    public class ReportController : Controller
    {
        private QuanLyBanSachEntities db = new QuanLyBanSachEntities();

        
        // GET: Report
        public ActionResult Index()
        {
                DateTime from = new DateTime( DateTime.Now.Year,DateTime.Now.Month,01 );
              DateTime to =  new DateTime( DateTime.Now.Year, DateTime.Now.Month, LaySoNgay(DateTime.Now.Month, DateTime.Now.Year));
            List<DonHang> ds,donHangs = new List<DonHang>();
            ds = db.DonHangs.ToList();
            foreach (DonHang a in ds)
            {
                if (a.NgayDat >= from && a.NgayDat <= to)
                {
                    donHangs.Add(a);
                }
            }
            ViewBag.DateFrom = from;
            ViewBag.DateTo=to;
            return View(donHangs);
        }
        public ActionResult Report(DateTime from, DateTime to)
        {
            List<DonHang> ds, donHangs = new List<DonHang>();
            ds = db.DonHangs.ToList();
            foreach (DonHang a in ds)
            {
                if (a.NgayDat >= from && a.NgayDat <= to)
                {
                    donHangs.Add(a);
                }
            }
            ViewBag.DateFrom = from;
            ViewBag.DateTo = to;
            return View(donHangs);
        }

        public int LaySoNgay(int thang, int nam)
        {
            while (nam < 0 || thang < 0) ;//neu Nam, tháng < 0 yeu cau nhap lai
            switch (thang)
            {
                // thang 1, 3, 5, 6, 8, 10 , 12 có 31 ngay
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                // thang 4, 6, 9, 11 co 30 ngay
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                // thang 2 có 28 ngay neu nam nhuan co 29 ngay
                case 2:
                    if ((nam % 4 == 0 && nam % 100 != 0) || (nam % 400 == 0))//kiem tra nam nhuan
                    {
                        return 29;
                    }
                    else
                    {
                        return 28;
                    }
                default:
                    return 0;
            }
        }
    }
}