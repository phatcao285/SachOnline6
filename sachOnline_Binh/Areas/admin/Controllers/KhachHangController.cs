using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sachOnline_Binh.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Runtime.CompilerServices;

namespace sachOnline_Binh.Areas.admin.Controllers
{
    // GET: admin/Home
    public class KhachHangController : Controller
    {
        DataClassesSachOnlineDataContext dt = new DataClassesSachOnlineDataContext();
        // GET: Admin/Sach
        public ActionResult Index()
        {
            // Kiểm tra trạng thái đăng nhập của người dùng
            if (Session["Admin"] != null && !string.IsNullOrWhiteSpace(Session["Admin"].ToString()))
            {
                // Người dùng đã đăng nhập, hiển thị trang quản trị
                var customers = dt.KHACHHANGs.ToList();

                return View(customers);
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng về trang chủ
                return RedirectToAction("Login", "Home");
            }
        }
    }
}