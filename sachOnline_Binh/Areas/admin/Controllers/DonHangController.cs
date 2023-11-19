using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PagedList;
using PagedList.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;


namespace sachOnline_Binh.Areas.admin.Controllers
{
    public class DonHangController : Controller
    {
        DataClassesSachOnlineDataContext dt = new DataClassesSachOnlineDataContext();
        // GET: admin/DonHang
        public ActionResult Index(int? page)
        {
            // Kiểm tra trạng thái đăng nhập của người dùng
            if (Session["Admin"] != null && !string.IsNullOrWhiteSpace(Session["Admin"].ToString()))
            {
                // Người dùng đã đăng nhập, hiển thị trang quản trị
                int pageNumber = page ?? 1;
                int pageSize = 10; // Số lượng đơn hàng hiển thị trên mỗi trang

                // Sort the orders by MaDonHang in ascending order
                var donHangs = dt.DONDATHANGs.OrderBy(dh => dh.MaDonHang)
                                               .ToPagedList(pageNumber, pageSize);

                return View(donHangs);
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng về trang chủ
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult DsDonHang(int id)
        {
            // Kiểm tra trạng thái đăng nhập của người dùng
            if (Session["Admin"] != null && !string.IsNullOrWhiteSpace(Session["Admin"].ToString()))
            {
                // Người dùng đã đăng nhập, hiển thị trang quản trị
                var ctdonHang = dt.DONDATHANGs.SingleOrDefault(dh => dh.MaDonHang == id);

                if (ctdonHang == null)
                {
                    return HttpNotFound();
                }

                return View(ctdonHang);
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng về trang chủ
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult ChiTietDonHang(int id)
        {
            // Kiểm tra trạng thái đăng nhập của người dùng
            if (Session["Admin"] != null && !string.IsNullOrWhiteSpace(Session["Admin"].ToString()))
            {
                // Người dùng đã đăng nhập, hiển thị trang quản trị
                var ctdonHang = dt.CHITIETDATHANGs.Where(dh => dh.MaDonHang == id);

                if (ctdonHang == null)
                {
                    return HttpNotFound();
                }

                return View(ctdonHang);
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng về trang chủ
                return RedirectToAction("Login", "Home");
            }
        }
    }
}