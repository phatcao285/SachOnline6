using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using PagedList;
using sachOnline_Binh;
using sachOnline_Binh.Models;

namespace sachOnline_Binh.Controllers
{
    public class SachOnlineController : Controller
    {
        //Tao 1 doi tuong chua toan bo CSDL tu bdSachOnline
        DataClassesSachOnlineDataContext dt = new DataClassesSachOnlineDataContext();
        /// <summary>
        /// LaySachMoi
        /// </summary>
        /// <param name="count">int</param>
        /// <returns>List</returns>
        private List<SACH> LaySachMoi(int count)
        {
 	        return dt.SACHes.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        // GET: SachOnline
        public ActionResult Index(int ? page)
        {
            int iSize = 6; 
            int iPageNumber = (page ?? 1);

            // Lấy danh sách sách phân trang
            var kq = from s in dt.SACHes select s;
            return View(kq.ToPagedList(iPageNumber, iSize));
        }
        [ChildActionOnly]
        public ActionResult NavPartial()
        {
            List<MENU> lst = new List<MENU>();
            lst = dt.MENUs.Where(m => m.ParentId == null).OrderBy(m => m.OrderNumber).ToList();
            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count; i++)
            {
                var l = dt.MENUs.Where(m => m.ParentId == lst[i].Id);
                a[i] = l.Count();
            }
            ViewBag.lst = a;
            return PartialView(lst);
        }
        public ActionResult SliderPartial()
        {
            return PartialView();
        }
        public ActionResult ChuDePartial()
        {
            var t = from cd in dt.CHUDEs select cd;
            return PartialView(t);
        }
        public ActionResult NhaXuatBanPartial()
        {
            var nxban = from nxb in dt.NHAXUATBANs select nxb;
            return PartialView(nxban);
        }
        public ActionResult SachBanNhieuPartial()
        {
            var sachBanNhieu = (from sbn in dt.SACHes
                                orderby sbn.SoLuongBan descending
                                select sbn).Take(6).ToList();
            return PartialView(sachBanNhieu);
        }
        public ActionResult FooterPartial()
        {
            return PartialView();
        }
        [HttpGet]
        public ActionResult Search(string keyword)
        {
            var results = from s in dt.SACHes where s.TenSach.Contains(keyword) select s;
            return View("Search", results);
        }
        public ActionResult ChiTietSach(int id)
        {
            var sach = from s in dt.SACHes where s.MaSach == id select s;
            return View(sach.Single());
        }
        public ActionResult SachTheoChuDe(int ? page)
        {
            var maCD = Request.QueryString["MaCD"];
            ViewBag.MaCD = maCD;
            int iSize = 3; // Số lượng sách trên mỗi trang
            int iPageNumber = (page ?? 1); // Trang mặc định là 1 nếu không có page

            // Lấy danh sách sách phân trang
            var kq = from s in dt.SACHes where s.MaCD == int.Parse(maCD) select s;
            return View(kq.ToPagedList(iPageNumber, iSize));
        }

        public ActionResult SachTheoNhaXuatBan(int ? page)
        {
            var maNXB = Request.QueryString["MaNXB"];
            ViewBag.MaNXB = maNXB;
            int iSize = 3; // Số lượng sách trên mỗi trang
            int iPageNumber = (page ?? 1); // Trang mặc định là 1 nếu không có page

            // Lấy danh sách sách phân trang
            var kq = from s in dt.SACHes where s.MaNXB == int.Parse(maNXB) select s;
            return View(kq.ToPagedList(iPageNumber, iSize));
        }
        public ActionResult LoginLogout()
        {
            return PartialView("LoginLogoutPartial");
        }
        [ChildActionOnly]
        public ActionResult LoadChildMenu(int parentId)
        {
            List<MENU> lst = new List<MENU>();
            lst = dt.MENUs.Where(m => m.ParentId == parentId).OrderBy(m => m.OrderNumber).ToList();
            ViewBag.Count = lst.Count();
            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count; i++)
            {
                var l = dt.MENUs.Where(m => m.ParentId == lst[i].Id);
                a[i] = l.Count();
            }
            ViewBag.lst = a;
            return PartialView("LoadChildMenu", lst);
        }
        //public ActionResult TrangTin(string metatitle)
        //{
        //    var tt = (from t in dt.TRANGTINs where t.MetaTitle == metatitle select t).Single();
        //    return View(tt);
        //}
    } 
}