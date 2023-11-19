using System;
using System.Linq;
using System.Web.Mvc;
using sachOnline_Binh.Models;

namespace sachOnline_Binh.Areas.admin.Controllers
{
    public class NhaXuatBanController : Controller
    {
        DataClassesSachOnlineDataContext dt = new DataClassesSachOnlineDataContext();

        // GET: admin/NhaXuatBan
        public ActionResult Index()
        {
            // Kiểm tra trạng thái đăng nhập của người dùng
            if (Session["Admin"] != null && !string.IsNullOrWhiteSpace(Session["Admin"].ToString()))
            {
                // Người dùng đã đăng nhập, hiển thị trang quản trị
                var nhaxuatbans = dt.NHAXUATBANs.ToList();
                return View(nhaxuatbans);
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng về trang chủ
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult ChiTiet(int id)
        {
            // Kiểm tra trạng thái đăng nhập của người dùng
            if (Session["Admin"] != null && !string.IsNullOrWhiteSpace(Session["Admin"].ToString()))
            {
                // Người dùng đã đăng nhập, hiển thị trang quản trị
                var nxb = dt.NHAXUATBANs.FirstOrDefault(n => n.MaNXB == id);
                return View(nxb);
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng về trang chủ
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpGet]
        public ActionResult Sua(int id)
        {
            // Kiểm tra trạng thái đăng nhập của người dùng
            if (Session["Admin"] != null && !string.IsNullOrWhiteSpace(Session["Admin"].ToString()))
            {
                // Người dùng đã đăng nhập, hiển thị trang quản trị
                var nxb = dt.NHAXUATBANs.FirstOrDefault(n => n.MaNXB == id);
                return View(nxb);
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng về trang chủ
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult Sua(NHAXUATBAN nxb)
        {
            if (ModelState.IsValid)
            {
                var existingNxb = dt.NHAXUATBANs.FirstOrDefault(n => n.MaNXB == nxb.MaNXB);
                if (existingNxb != null)
                {
                    existingNxb.TenNXB = nxb.TenNXB;
                    existingNxb.DiaChi = nxb.DiaChi;
                    existingNxb.DienThoai = nxb.DienThoai;
                    dt.SubmitChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(nxb);
        }

        [HttpGet]
        public ActionResult ThemMoi()
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
        [ValidateAntiForgeryToken]
        public ActionResult ThemMoi(NHAXUATBAN nxb)
        {
            if (ModelState.IsValid)
            {
                dt.NHAXUATBANs.InsertOnSubmit(nxb);
                dt.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(nxb);
        }

        [HttpGet]
        public ActionResult Xoa(int id)
        {
            // Kiểm tra trạng thái đăng nhập của người dùng
            if (Session["Admin"] != null && !string.IsNullOrWhiteSpace(Session["Admin"].ToString()))
            {
                // Người dùng đã đăng nhập, hiển thị trang quản trị
                var nxb = dt.NHAXUATBANs.FirstOrDefault(n => n.MaNXB == id);
                return View(nxb);
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng về trang chủ
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult XoaConfirmed(int id)
        {
            try
            {
                var nxb = dt.NHAXUATBANs.FirstOrDefault(n => n.MaNXB == id);
                if (nxb != null)
                {
                    dt.NHAXUATBANs.DeleteOnSubmit(nxb);
                    dt.SubmitChanges();
                }
            }
            catch
            {
                // Handle errors
            }
            return RedirectToAction("Index");
        }

        public void Validate(string TenNXB, string DienThoai)
        {
            if (string.IsNullOrEmpty(TenNXB))
            {
                ModelState.AddModelError("TenNXB", "Tên không được để trống.");
            }
            else if (dt.NHAXUATBANs.Any(n => n.TenNXB == TenNXB))
            {
                ModelState.AddModelError("TenNXB", "Tên nhà xuất bản đã tồn tại.");
            }
            else if (string.IsNullOrEmpty(DienThoai))
            {
                ModelState.AddModelError("DienThoai", "Số điện thoại không được để trống.");
            }
        }
    }
}
