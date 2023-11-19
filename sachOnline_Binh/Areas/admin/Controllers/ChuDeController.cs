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
    public class ChuDeController : Controller
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
        public JsonResult DsChuDe()
        {
            try
            {
                var dsCD = (from cd in dt.CHUDEs
                            select new
                            {
                                MaCD = cd.MaCD,
                                TenCD = cd.TenChuDe
                            }).ToList();
                return Json(new { code = 200, dsCD = dsCD, msg = "Lấy danh sách chủ đề thành công" },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách chủ đề thất bại" + ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Detail(int maCD)
        {
            try
            {
                var cd = (from s in dt.CHUDEs
                          where (s.MaCD == maCD)
                          select new
                          {
                              MaCD = s.MaCD,
                              TenChuDe = s.TenChuDe
                          }).SingleOrDefault();
                //dt.CHUDEs.SingleOrDefault(c => c.MaCD == maCD);
                return Json(new { code = 200, cd = cd, msg = "Lấy thông tin chủ đề thành công." }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin chủ đề thất bại." + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddChuDe(string strTenCD)
        {
            try
            {
                var cd = new CHUDE();
                cd.TenChuDe = strTenCD;
                dt.CHUDEs.InsertOnSubmit(cd);
                dt.SubmitChanges();

                return Json(new { code = 200, msg = "Thêm chủ đề để thành công." }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm chủ đề thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(int maCD, string strTenCD)
        {
            try
            {
                var cd = dt.CHUDEs.SingleOrDefault(c => c.MaCD == maCD);
                cd.TenChuDe = strTenCD;
                dt.SubmitChanges();

                return Json(new { code = 200, msg = "Sửa chủ đề thành công. " }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Sửa chủ đề thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Delete(int maCD)
        {
            try
            {
                var cd = dt.CHUDEs.SingleOrDefault(c => c.MaCD == maCD);
                dt.CHUDEs.DeleteOnSubmit(cd);
                dt.SubmitChanges();

                return Json(new { code = 200, msg = "Xóa chủ đề thành công." }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa chủ đề thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
