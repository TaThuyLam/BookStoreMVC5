using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanSach.Models;
using PagedList;

namespace WebsiteBanSach.Controllers
{
    public class CategoryController : Controller
    {
        private QuanLyBanSachEntities db = new QuanLyBanSachEntities();

        // GET: ChuDe

        public ActionResult Index(int? page)
        {
            //Kiem tra Dang Nhap
            if (Session["AdTaiKhoan"] == null || Session["AdTaiKhoan"].ToString() == "")
            {
                Response.Write("<script>alert('Bạn cần đăng nhập vào tài khoản Admin');window.location.pathname = '/Admin/Login';</script>"); //works great
            }
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            ViewBag.Lable = "MANAGE CATEGORY";
            ViewBag.Count = db.ChuDes.ToList().Count() + " Records Found";
            return View(db.ChuDes.ToList().OrderBy(n => n.MaChuDe).ToPagedList(pageNumber, pageSize));
        }



        // GET: ChuDe/Details/5
        public ActionResult Details(int? maChuDe)
        {
            if (maChuDe == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuDe chuDe = db.ChuDes.Find(maChuDe);
            if (chuDe == null)
            {
                return HttpNotFound();
            }
            return View(chuDe);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.Lable = "MANAGE CATEGORY / CREATE";
            return View();
        }

        // POST: Publisher/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ChuDe chuDe)
        {

            //Them vao database
            if (ModelState.IsValid)
            {
                db.ChuDes.Add(chuDe);
                db.SaveChanges();
                Response.Write("<script>alert('Created Successfully!')</script>");
            }
            return RedirectToAction("Index");
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? maChuDe)
        {
            //Lay doi tuong sach theo ma
            ChuDe chu = db.ChuDes.SingleOrDefault(n => n.MaChuDe == maChuDe);
            if (chu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.Lable = "MANAGE CATEGORY / EDIT";
            return View(chu);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ChuDe chuDe)
        {
            //Them vao database
            if (ModelState.IsValid)
            {
                db.Entry(chuDe).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            Response.Write("<script>alert('Đã lưu chỉnh sửa của bạn ');</script>"); //works great
            return RedirectToAction("Index");
        }

        // GET: Category/Delete/5

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

        // POST: Category/Delete/5
        /* [HttpPost, ActionName("Delete")]
         [ValidateAntiForgeryToken]*/
        public ActionResult DeleteConfirmed(int? maChuDe)
        {
            Response.Write("<script>alert('Đã lưu chỉnh sửa của bạn ');</script>");
            ChuDe chuDe = db.ChuDes.Find(maChuDe);
            List<Sach> saches = db.Saches.Where(n => n.MaChuDe == maChuDe).ToList();
            if (saches.Count > 0)
            {
                foreach (Sach sache in saches)
                {
                    sache.MaNXB = null;
                    db.Entry(sache).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            db.ChuDes.Remove(chuDe);
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