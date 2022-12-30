using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanSach.Models;
using PagedList;

namespace WebsiteBanSach.Controllers
{
    public class PublisherController : Controller
    {
        private QuanLyBanSachEntities db = new QuanLyBanSachEntities();

        // GET: NhaXuanBan

        public ActionResult Index(int? page)
        {
            //Kiem tra Dang Nhap
            if (Session["AdTaiKhoan"] == null || Session["AdTaiKhoan"].ToString() == "")
            {
                Response.Write("<script>alert('Bạn cần đăng nhập vào tài khoản Admin');window.location.pathname = '/Admin/Login';</script>"); //works great
            }
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            ViewBag.Lable = "MANAGE PUBLISHER";
            ViewBag.Count = db.NhaXuatBans.ToList().Count() + " Records Found";
            return View(db.NhaXuatBans.ToList().OrderBy(n => n.MaNXB).ToPagedList(pageNumber, pageSize));
        }



        // GET: NhaXuanBan/Details/5
        public ActionResult Details(int? maNXB)
        {
            if (maNXB == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaXuatBan nhaXuatBan = db.NhaXuatBans.Find(maNXB);
            if (nhaXuatBan == null)
            {
                return HttpNotFound();
            }
            return View(nhaXuatBan);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.Lable = "MANAGE PUBLISHERS / CREATE";
            return View();
        }

        // POST: Publisher/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(NhaXuatBan nha)
        {
          
            //Them vao database
            if (ModelState.IsValid)
            {
                db.NhaXuatBans.Add(nha);
                db.SaveChanges();
                Response.Write("<script>alert('Created Successfully!')</script>");
            }
            return RedirectToAction("Index");
        }

        // GET: Publisher/Edit/5
        public ActionResult Edit(int? maNXB)
        {
            //Lay doi tuong sach theo ma
            NhaXuatBan nhaXuatBan = db.NhaXuatBans.SingleOrDefault(n => n.MaNXB == maNXB);
            if (nhaXuatBan == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.Lable = "MANAGE PUBLISHER / EDIT";
            return View(nhaXuatBan);
        }

        // POST: Publisher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(NhaXuatBan nha)
        {
            //Them vao database
            if (ModelState.IsValid)
            {
                db.Entry(nha).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            Response.Write("<script>alert('Đã lưu chỉnh sửa của bạn ');</script>"); //works great
            return RedirectToAction("Index");
        }

        // GET: Publisher/Delete/5

        public ActionResult Delete(int? maNXB)
        {
            if (maNXB == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Find(maNXB);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // POST: Product/Delete/5
        /* [HttpPost, ActionName("Delete")]
         [ValidateAntiForgeryToken]*/
        public ActionResult DeleteConfirmed(int? maNXB)
        {
            Response.Write("<script>alert('Đã lưu chỉnh sửa của bạn ');</script>");
            NhaXuatBan nha = db.NhaXuatBans.Find(maNXB);
            List<Sach> saches = db.Saches.Where(n => n.MaNXB == maNXB).ToList();
            if(saches.Count > 0)
            {
                foreach (Sach sache in saches)
                {
                    sache.MaNXB = null;
                    db.Entry(sache).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            db.NhaXuatBans.Remove(nha);
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

    }
}
