using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sachOnline_Binh.Areas.admin.Controllers
{
    public class MenuController : Controller
    {
        DataClassesSachOnlineDataContext dt = new DataClassesSachOnlineDataContext();
        // GET: admin/Menu
        public ActionResult Index()
        {
            // Kiểm tra trạng thái đăng nhập của người dùng
            if (Session["Admin"] != null && !string.IsNullOrWhiteSpace(Session["Admin"].ToString()))
            {
                // Người dùng đã đăng nhập, hiển thị trang quản trị
                var listMenu = dt.MENUs.Where(m => m.ParentId == null).OrderBy(m => m.OrderNumber).ToList();
                int[] a = new int[listMenu.Count()];
                for (int i = 0; i < listMenu.Count(); i++)
                {
                    var l = dt.MENUs.Where(m => m.ParentId == listMenu[i].Id);
                    a[i] = l.Count();
                }
                ViewBag.lst = a;
                List<CHUDE> cd = dt.CHUDEs.ToList();
                ViewBag.ChuDe = cd;
                List<TRANGTIN> tt = dt.TRANGTINs.ToList();
                ViewBag.TrangTin = tt;
                return View(listMenu);
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng về trang chủ
                return RedirectToAction("Login", "Home");
            }
        }
        [ChildActionOnly]
        public ActionResult ChildMenu(int parentId)
        {
            // Kiểm tra trạng thái đăng nhập của người dùng
            if (Session["Admin"] != null && !string.IsNullOrWhiteSpace(Session["Admin"].ToString()))
            {
                // Người dùng đã đăng nhập, hiển thị trang quản trị
                List<MENU> lst = new List<MENU>();
                lst = dt.MENUs.Where(m => m.ParentId == parentId).OrderBy(m => m.OrderNumber).ToList();
                ViewBag.Count = lst.Count();
                int[] a = new int[lst.Count()];
                for (int i = 0; i < lst.Count(); i++)
                {
                    var l = dt.MENUs.Where(m => m.ParentId == lst[i].Id);
                    a[i] = l.Count();
                }
                ViewBag.lst = a;
                return PartialView("ChildMenu", lst);
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng về trang chủ
                return RedirectToAction("Login", "Home");
            }
        }
        [ChildActionOnly]
        public ActionResult ChildMenu1(int parentId)
        {
            // Kiểm tra trạng thái đăng nhập của người dùng
            if (Session["Admin"] != null && !string.IsNullOrWhiteSpace(Session["Admin"].ToString()))
            {
                // Người dùng đã đăng nhập, hiển thị trang quản trị
                List<MENU> lst = new List<MENU>();
                lst = dt.MENUs.Where(m => m.ParentId == parentId).OrderBy(m => m.OrderNumber).ToList();
                ViewBag.Count = lst.Count();
                int[] a = new int[lst.Count()];
                for (int i = 0; i < lst.Count(); i++)
                {
                    var l = dt.MENUs.Where(m => m.ParentId == lst[i].Id);
                    a[i] = l.Count();
                }
                ViewBag.lst = a;
                return PartialView("ChildMenu1", lst);
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng về trang chủ
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddMenu(FormCollection f)
        {
            if (!String.IsNullOrEmpty(f["ThemChuDe"]))
            {
                MENU m = new MENU();
                var cd = dt.CHUDEs.Where(c => c.MaCD == int.Parse(f["MaCD"].ToString())).SingleOrDefault();
                m.MenuName = cd.TenChuDe;
                m.MenuLink = "sach-theo-chu-de-" + cd.MaCD;
                if (!String.IsNullOrEmpty(f["ParentID"]))
                {
                    m.ParentId = int.Parse(f["ParentID"]);
                }
                else
                {
                    m.ParentId = null;
                }
                m.OrderNumber = int.Parse(f["Number"]);
                List<MENU> l = null;
                if (m.ParentId != null)
                {
                    l = dt.MENUs.Where(k => k.ParentId == null && k.OrderNumber >= m.OrderNumber).ToList();
                }
                else
                {
                    l = dt.MENUs.Where(k => k.ParentId == m.ParentId && k.OrderNumber >= m.OrderNumber).ToList();
                }
                for (int i = 0; i < l.Count; i++)
                {
                    l[i].OrderNumber++;
                }
                dt.MENUs.InsertOnSubmit(m);
                dt.SubmitChanges();
            }
            else if (!String.IsNullOrEmpty(f["ThemTrang"]))
            {
                MENU m = new MENU();
                var trang = dt.TRANGTINs.Where(t => t.MaTT == int.Parse(f["MaTT"].ToString())).SingleOrDefault();
                m.MenuName = trang.TenTrang;
                m.MenuLink = trang.MetaTitle;
                if (!String.IsNullOrEmpty(f["ParentID"]))
                {
                    m.ParentId = int.Parse(f["ParentID"]);
                }
                else
                {
                    m.ParentId = null;
                }
                m.OrderNumber = int.Parse(f["Number1"]);

                List<MENU> l = null;
                if (m.ParentId == null)
                {
                    l = dt.MENUs.Where(k => k.ParentId == null && k.OrderNumber >= m.OrderNumber).ToList();
                }
                else
                {
                    l = dt.MENUs.Where(k => k.ParentId == m.ParentId && k.OrderNumber >= m.OrderNumber).ToList();
                }
                for (int i = 0; i < l.Count; i++)
                {
                    l[i].OrderNumber++;
                }
                dt.MENUs.InsertOnSubmit(m);
                dt.SubmitChanges();
            }
            else if (!String.IsNullOrEmpty(f["ThemLink"]))
            {
                MENU m = new MENU();
                m.MenuName = f["TenMenu"];
                m.MenuLink = f["Link"];
                if (!String.IsNullOrEmpty(f["ParentID"]))
                {
                    m.ParentId = int.Parse(f["ParentID"]);
                }
                else
                {
                    m.ParentId = null;
                }
                m.OrderNumber = int.Parse(f["Number2"]);
                List<MENU> l = null;
                if (m.ParentId == null)
                {
                    l = dt.MENUs.Where(k => k.ParentId == null && k.OrderNumber >= m.OrderNumber).ToList();

                }
                else
                {
                    l = dt.MENUs.Where(k => k.ParentId == m.ParentId && k.OrderNumber >= m.OrderNumber).ToList();
                }
                for (int i = 0;i < l.Count;i++)
                {
                    l[i].OrderNumber++;
                }
                dt.MENUs.InsertOnSubmit(m);
                dt.SubmitChanges();
            }
            return Redirect("~/admin/Menu/Index");
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            List<MENU> submn = dt.MENUs.Where(m => m.ParentId == id).ToList();
            if (submn.Count() > 0)
            {
                return Json(new { code = 500, msg = "Còn menu con, không xóa được." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var mn = dt.MENUs.SingleOrDefault(m => m.Id == id);
                List<MENU> l = null;
                if (mn.ParentId == null)
                {
                    l = dt.MENUs.Where(k => k.ParentId == null && k.OrderNumber > mn.OrderNumber).ToList();
                }
                else
                {
                    l = dt.MENUs.Where(k => k.ParentId == mn.ParentId && k.OrderNumber > mn.OrderNumber).ToList();
                }
                for (int i = 0; i < l.Count; i++)
                {
                    l[i].OrderNumber--;
                }
                dt.MENUs.DeleteOnSubmit(mn);
                dt.SubmitChanges();
                return Json(new { code = 200, msg = "Xóa thành công." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Update(int id)
        {
            try
            {
                var mn = (from m in dt.MENUs
                          where (m.Id == id)
                          select new
                          {
                              Id = m.Id,
                              MenuName = m.MenuName,
                              MenuLink = m.MenuLink,
                              OrderNumber = m.OrderNumber,
                          }).SingleOrDefault();
                return Json(new { code = 200, mn = mn, msg = "Lấy thông tin thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin thất bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Update(int id, string strTenMenu, string strLink, int STT)
        {
            try
            {
                var mn = dt.MENUs.SingleOrDefault(m => m.Id == id);
                List<MENU> l = null;
                if (STT < mn.OrderNumber)
                {
                    if (mn.ParentId == null)
                    {
                        l = dt.MENUs.Where(m => m.ParentId == null && m.OrderNumber >= STT && m.OrderNumber < mn.OrderNumber).ToList();
                    }
                    else
                    {
                        l = dt.MENUs.Where(m => m.ParentId == mn.ParentId && m.OrderNumber >= STT && m.OrderNumber < mn.OrderNumber).ToList();
                    }
                    for (int i = 0; i < l.Count; i++)
                    {
                        l[i].OrderNumber++;
                    }
                }
                else if (STT > mn.OrderNumber)
                {
                    if (mn.ParentId == null)
                    {
                        l = dt.MENUs.Where(m => m.ParentId == null && m.OrderNumber > mn.OrderNumber && m.OrderNumber <= STT).ToList();
                    }
                    else
                    {
                        l = dt.MENUs.Where(m => m.ParentId == mn.ParentId && m.OrderNumber > mn.OrderNumber && m.OrderNumber <= STT).ToList();
                    }
                    for (int i = 0; i <= l.Count; i++)
                    {
                        l[i].OrderNumber--;
                    }
                }
                    mn.MenuName = strTenMenu;
                    mn.MenuLink = strLink;
                    mn.OrderNumber = STT;
                    dt.SubmitChanges();
                    return Json(new { code = 200, msg = "Sửa menu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Sửa menu thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}