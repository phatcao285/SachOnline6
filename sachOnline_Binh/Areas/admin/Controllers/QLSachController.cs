using System;
using System.Linq;
using System.Web.Mvc;
using sachOnline_Binh.Models;
using System.Collections.Generic;
using System.Web;
using SachOnline;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace sachOnline_Binh.Areas.admin.Controllers
{
    public class QLSachController : Controller
    {
        DataClassesSachOnlineDataContext dt = new DataClassesSachOnlineDataContext();
        // GET: Admin/ChuDe
        public ActionResult Index()
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
        [HttpGet]
        public JsonResult DsSach()
        {
            try
            {
                var dsS = (from s in dt.SACHes
                           select new
                           {
                               MaS = s.MaSach,
                               TenS = s.TenSach,
                               MoTa = s.MoTa, // Add the additional fields here
                               AnhBia = s.AnhBia, // Add the additional fields here
                               NgayCapNhat = s.NgayCapNhat, // Add the additional fields here
                               SoLuong = s.SoLuongBan, // Add the additional fields here
                               GiaBan = s.GiaBan, // Add the additional fields here
                               ChuDe = s.CHUDE.TenChuDe, // Add the additional fields here
                               NhaXuatBan = s.NHAXUATBAN.TenNXB // Add the additional fields here
                           }).ToList();
                return Json(new { code = 200, dsS = dsS, msg = "Lấy danh sách Sách thành công" },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách sách thất bại" + ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Detail(int maS)
        {
            try
            {
                var s = (from sach in dt.SACHes
                          where (sach.MaSach == maS)
                          select new
                          {
                              MaS = sach.MaSach,
                              TenSach = sach.TenSach
                          }).SingleOrDefault();
                //dt.CHUDEs.SingleOrDefault(c => c.MaCD == maCD);
                return Json(new { code = 200, sach = s, msg = "Lấy thông tin sách thành công." }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin sách thất bại." + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddSach(string strTenS, string strMoTa, string strAnhBia, DateTime NgayCapNhat, int SoLuong, decimal GiaBan, int MaChuDe, int MaNXB)
        {
            try
            {
                var s = new SACH();
                s.TenSach = strTenS;
                s.MoTa = strMoTa;
                s.AnhBia = strAnhBia;
                s.NgayCapNhat = NgayCapNhat;
                s.SoLuongBan = SoLuong;
                s.GiaBan = GiaBan;
                s.MaCD = MaChuDe;
                s.MaNXB = MaNXB;

                dt.SACHes.InsertOnSubmit(s);
                dt.SubmitChanges();

                return Json(new { code = 200, msg = "Thêm sách thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm sách thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult Update(int maS, string strTenS)
        {
            try
            {
                var s = dt.SACHes.SingleOrDefault(c => c.MaSach == maS);
                s.TenSach = strTenS;
                dt.SubmitChanges();

                return Json(new { code = 200, msg = "Sửa sách thành công. " }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Sửa sách thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Delete(int maS)
        {
            try
            {
                var s = dt.SACHes.SingleOrDefault(c => c.MaSach == maS);
                dt.SACHes.DeleteOnSubmit(s);
                dt.SubmitChanges();

                return Json(new { code = 200, msg = "Xóa sách thành công." }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa sách thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
