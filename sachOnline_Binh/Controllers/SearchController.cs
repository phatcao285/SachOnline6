using sachOnline_Binh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer.Symbols;
using PagedList;
using PagedList.Mvc;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Data.SqlClient;

namespace sachOnline_Binh.Controllers
{
    public class SearchController : Controller
    {
        DataClassesSachOnlineDataContext dt = new DataClassesSachOnlineDataContext();
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(string strSearch = null) // trên form strSearch
        {
            ViewBag.Search = strSearch;
            if (!string.IsNullOrEmpty(strSearch))
            {
                //var results = from s in dt.SACHes where s.TenSach.Contains(strSearch) select s;
                //var results = dt.SACHes;
                //var results = dt.SACHes.ToList();
                //var results = dt.SACHes.Select(s => s);
                //var kq = from ….
                //var kq = db.SACHes… return View(kq);

                var results = from s in dt.SACHes where s.TenSach.Contains(strSearch) || s.CHUDE.TenChuDe.Contains(strSearch) || s.NHAXUATBAN.TenNXB.Contains(strSearch) select s;
                return View("Search", results);
            }
            return View();
        }
        public ActionResult SearchTheoDanhMuc(string strSearch = null, int maCD = 0)
        {
            // 1. Lưu từ khóa tìm kiếm
            ViewBag.Search = strSearch;

            //2.Tạo câu truy cơ bản
            var kq = dt.SACHes.Select(b => b);
            //3. Tìm kiếm theo searchString
            if (!String.IsNullOrEmpty(strSearch))
                kq = kq.Where(b => b.TenSach.Contains(strSearch));

            //4. Tìm kiếm theo MaCD
            if (maCD != 0)
            {
                kq = kq.Where(b => b.CHUDE.MaCD == maCD);
            }
            //5. Tạo danh sách danh mục để hiển thị ở giao diện View thông qua DropDownList
            ViewBag.MaCD = new SelectList(dt.CHUDEs, "MaCD", "TenChuDe"); // danh sách Chủ đề
            ViewBag.cd = dt.CHUDEs.ToList();
            return View(kq.ToList());
        }
        public ActionResult Group()
        {
            //var kq = from s in db.SACHes group s by s.MaCD;
            var kq = dt.SACHes.GroupBy(s => s.MaCD);

            return View(kq);
        }
        public ActionResult ThongKe()
        {
            var kq = from s in dt.SACHes
                     join cd in dt.CHUDEs on s.MaCD equals cd.MaCD
                     group s by new { cd.MaCD, cd.TenChuDe} into g
                     select new ReportInfo
                     {
                         Id = g.Key.MaCD.ToString(),
                         Name = g.Key.TenChuDe,
                         Count = g.Count(),
                         Sum = g.Sum(n => n.SoLuongBan),
                         Max = g.Max(n => n.SoLuongBan),
                         Min = g.Min(n => n.SoLuongBan),
                         Avg = Convert.ToDecimal(g.Average(n => n.SoLuongBan)),
                     };
            return View(kq);
        }
        public ActionResult SearchPhanTrang(int? page, string strSearch = null)
        {
            ViewBag.Search = strSearch;

            if (!string.IsNullOrEmpty(strSearch))
            {
                int iSize = 3;
                int iPageNumber = (page ?? 1);
                var kq = from s in dt.SACHes where s.TenSach.Contains(strSearch) || s.MoTa.Contains(strSearch) select s;
                return View(kq.ToPagedList(iPageNumber, iSize));
            }
            return View();
        }
        public ActionResult SearchPhanTrangTuyChon(int? size, int? page, string strSearch = null)
        {
            //1 List để lấy nguồn cho Combobox chọn số lượng sản phẩm
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "3", Value = "3" });
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "25", Value = "25" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });

            //1.1 Giữ trạng thái kích thước trang được chọn trên DropDownList
            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }

            //1.2. Tạo các biến ViewBag
            ViewBag.size = items; // ViewBag DropDownList
            ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại
            ViewBag.Search = strSearch;

            int iSize = (size ?? 3); // Mặc định 3 item trên 1 page
            int iPageNumber = (page ?? 1);
            var kq = from s in dt.SACHes select s;
            if (!string.IsNullOrEmpty(strSearch))
                kq = kq.Where(s => s.TenSach.Contains(strSearch) || s.MoTa.Contains(strSearch));
            return View(kq.ToPagedList(iPageNumber, iSize));
        }
        public ActionResult SearchPhanTrangSapXep(int? page, string sortProperty, string sortOrder = "", string strSearch = null)
        {
            ViewBag.Search = strSearch;
            if (!string.IsNullOrEmpty(strSearch))
            {
                int iSize = 3;
                int iPageNumber = (page ?? 1);
                //Giamr gt bieens sortorder
                if (sortOrder == "") ViewBag.SortOrder = "desc";
                if (sortOrder == "desc") ViewBag.SortOrder = "";
                if (sortOrder == "") ViewBag.SortOrder = "asc";
                //tao thuoc tinh sap xep gia tri mac dinh ten sach
                if (String.IsNullOrEmpty(sortProperty)) 
                    sortProperty = "TenSach";
                ViewBag.SortProperty = sortProperty;
                //truy van
                var kq = from s in dt.SACHes where s.TenSach.Contains(strSearch) || s.MoTa.Contains(strSearch) select s;
                //sap xep tang giam bang phuong thuc orderby su dung thu vien dynamic linq
                if (sortOrder == "desc")
                {
                    kq = kq.OrderBy(sortProperty + " desc");
                }
                else
                {
                    kq = kq.OrderBy(sortProperty);
                }
                return View(kq.ToPagedList(iPageNumber, iSize));
            }
            return View();
        }

    }
}