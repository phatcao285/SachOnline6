using SachOnline.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sachOnline_Binh.Areas.admin.Controllers
{
    public class TrangTinController : Controller
    {
        DataClassesSachOnlineDataContext dt = new DataClassesSachOnlineDataContext();
        // GET: admin/TrangTin
        public ActionResult Index()
        {
            // Kiểm tra trạng thái đăng nhập của người dùng
            if (Session["Admin"] != null && !string.IsNullOrWhiteSpace(Session["Admin"].ToString()))
            {
                // Người dùng đã đăng nhập, hiển thị trang quản trị
                return View(dt.TRANGTINs.ToList());
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng về trang chủ
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            // Kiểm tra trạng thái đăng nhập của người dùng
            if (Session["Admin"] != null && !string.IsNullOrWhiteSpace(Session["Admin"].ToString()))
            {
                // Người dùng đã đăng nhập, hiển thị trang quản trị
                return View();
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng về trang chủ
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(TRANGTIN tt)
        {
            if (ModelState.IsValid)
            {
                tt.MetaTitle = tt.TenTrang.RemoveDiacritics().Replace(" ", "-");
                tt.NgayTao = DateTime.Now;
                dt.TRANGTINs.InsertOnSubmit(tt);
                dt.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            // Kiểm tra trạng thái đăng nhập của người dùng
            if (Session["Admin"] != null && !string.IsNullOrWhiteSpace(Session["Admin"].ToString()))
            {
                // Người dùng đã đăng nhập, hiển thị trang quản trị
                var tt = dt.TRANGTINs.Where(t => t.MaTT == id);
                return View(tt.SingleOrDefault());
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng về trang chủ
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                var tt = dt.TRANGTINs.Where(t => t.MaTT == int.Parse(f["MaTT"])).SingleOrDefault();
                tt.TenTrang = f["TenTrang"];
                tt.NoiDung = f["NoiDung"];
                tt.NgayTao = Convert.ToDateTime(f["NgayTao"]);
                tt.MetaTitle = f["TenTrang"].RemoveDiacritics().Replace(" ", "-");
                dt.SubmitChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            // Kiểm tra trạng thái đăng nhập của người dùng
            if (Session["Admin"] != null && !string.IsNullOrWhiteSpace(Session["Admin"].ToString()))
            {
                // Người dùng đã đăng nhập, hiển thị trang quản trị
                var tt = from t in dt.TRANGTINs where t.MaTT == id select t;
                return View(tt.SingleOrDefault());
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng về trang chủ
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var tt = (from t in dt.TRANGTINs where t.MaTT == id select t).SingleOrDefault();
            dt.TRANGTINs.DeleteOnSubmit(tt);
            dt.SubmitChanges();
            return RedirectToAction("Index");
        }

    }
}