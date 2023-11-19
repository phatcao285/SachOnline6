using sachOnline_Binh.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Security.Policy;

namespace sachOnline_Binh.Controllers
{
    public class GioHangController : Controller
    {
        DataClassesSachOnlineDataContext dt = new DataClassesSachOnlineDataContext();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int ms, string url)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.Find(n => n.iMaSach == ms);
            if (sp == null)
            {
                sp = new GioHang(ms);
                lstGioHang.Add(sp);
            }
            else
            {
                sp.iSoLuong++;
            }
            return Redirect(url);
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }
        public ActionResult Giohang()
        {
            List<GioHang> lstGiohang = LayGioHang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGiohang);
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        //Xoa Giohang
        public ActionResult XoaSPKhoiGiohang(int iMaSach)
        {
            //Lay gio hang tu Session
            List<GioHang> lstGiohang = LayGioHang();
            //Kiem tra san pham da co trong Session["Giohang"]
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.iMaSach == iMaSach);
            //Neu ton tai thi cho sua SoLuong
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMaSach == iMaSach);
                if (lstGiohang.Count == 0)
                {
                    return RedirectToAction("Index", "SachOnline");
                }
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGiohang(int iMaSach, FormCollection f)
        {
            //Lay gio hang tu Session
            List<GioHang> lstGiohang = LayGioHang();
            //Kiem tra san pham da co trong Session["Giohang"]
            GioHang sp = lstGiohang.SingleOrDefault(n => n.iMaSach == iMaSach);
            //Neu ton tai thi cho sua Soluong
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGiohang()
        {
            //Lay gio hang tu Session
            List<GioHang> lstGiohang = LayGioHang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "SachOnline");
        }
        //Hien thi View DatHang de cap nhat cac thong tin cho Don hang
        [HttpGet]
        public ActionResult DatHang()
        {
            //Kiểm tra đăng nhập chưa
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User", new{Url = "/GioHang/GioHang"});
            }
            //Kiểm tra có hàng trong giỏ chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            //Lấy hàng từ Session
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            // Lấy ngày đặt hàng
            DateTime ngayDatHang = DateTime.Now;

            // Kiểm tra ngày giao hàng từ form
            if (DateTime.TryParse(f["NgayGiao"], out DateTime ngayGiaoHang) && ngayGiaoHang > ngayDatHang)
            {

                // Ngày giao hợp lệ, tiếp tục xử lý đơn hàng

                // Trước khi sử dụng biến lstGioHang, hãy đảm bảo rằng bạn đã khai báo nó ở phía trên
                List<GioHang> lstGioHang = LayGioHang();

                // Truy vấn dữ liệu từ bảng SANPHAM dựa trên danh sách sản phẩm trong giỏ hàng
                var maSachList = lstGioHang.Select(item => item.iMaSach).ToList();
                var sanPhamList = dt.SACHes.Where(s => maSachList.Contains(s.MaSach)).ToList();

                // Tạo một chuỗi hoặc HTML để hiển thị thông tin sản phẩm trong email
                string sanPhamInfo = "CHI TIẾT SẢN PHẨM\n";
                foreach (var sanPham in sanPhamList)
                {
                    sanPhamInfo += "Tên sách: " + sanPham.TenSach + "\n";
                    // Hiển thị ảnh bìa
                    //sanPhamInfo += $"<img src='{sanPham.AnhBia}' alt='Ảnh bìa' />\n";
                    sanPhamInfo += "Giá bán 1 quyển: " + sanPham.GiaBan + "VND" + "\n";
                    sanPhamInfo += "Số lượng: " + lstGioHang.First(item => item.iMaSach == sanPham.MaSach).iSoLuong + "\n"; // Lấy số lượng từ giỏ hàng
                    // Thêm các thông tin khác của sản phẩm
                    // Ví dụ: sanPhamInfo += "Giá: " + sanPham.Gia + "\n";
                    sanPhamInfo += "\n";
                }

                // Thêm đơn hàng
                DONDATHANG ddh = new DONDATHANG();
                KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
                //List<GioHang> lstGioHang = LayGioHang();
                ddh.MaKH = kh.MaKH;
                ddh.NgayDat = ngayDatHang; // Sử dụng ngày đặt hàng
                ddh.NgayGiao = ngayGiaoHang; // Sử dụng ngày giao hàng từ form
                ddh.TinhTrangGiaoHang = 1;
                ddh.DaThanhToan = false;
                dt.DONDATHANGs.InsertOnSubmit(ddh);
                dt.SubmitChanges();
                // Tính toán tổng tiền
                decimal tongTien = lstGioHang.Sum(item => (decimal)item.dThanhTien);
                // Thêm chi tiết đơn hàng
                foreach (var item in lstGioHang)
                {
                    CHITIETDATHANG ctdh = new CHITIETDATHANG();
                    ctdh.MaDonHang = ddh.MaDonHang;
                    ctdh.MaSach = item.iMaSach;
                    ctdh.SoLuong = item.iSoLuong;
                    ctdh.DonGia = (decimal)item.dDonGia;
                    dt.CHITIETDATHANGs.InsertOnSubmit(ctdh);
                }
                dt.SubmitChanges();

                // Gửi email thông báo cho khách hàng
                try
                {
                    var mail = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential("2124802010313@student.tdmu.edu.vn", "Binhlam1103@"),
                        EnableSsl = true
                    };
                    var message = new MailMessage();
                    message.From = new MailAddress("2124802010313@student.tdmu.edu.vn");
                    message.ReplyToList.Add("2124802010313@student.tdmu.edu.vn");
                    message.To.Add(new MailAddress(kh.Email)); // Sử dụng email của khách hàng
                    message.Subject = "Xác nhận đặt hàng thành công";
                    message.Body = "Cảm ơn bạn đã đặt hàng tại cửa hàng chúng tôi. Đơn hàng của bạn đã được xác nhận.";
                    message.Body += "\nMã đơn hàng: " + ddh.MaDonHang;
                    message.Body += "\nHọ tên khách hàng: " + kh.HoTen;
                    message.Body += "\nNgày đặt: " + ddh.NgayDat;
                    message.Body += "\nNgày giao: " + ddh.NgayGiao;
                    message.Body += "\nTổng tiền: " + tongTien.ToString() + "VND" + "\n";
                    message.Body += "\nCHI TIẾT ĐƠN HÀNG";
                    // Bạn có thể thêm chuỗi sanPhamInfo vào message.Body của email
                    message.Body += sanPhamInfo;

                    mail.Send(message);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Có lỗi khi gửi email. Vui lòng thử lại sau.";
                    return View("DatHang");
                }

                // Đặt lại giỏ hàng
                Session["GioHang"] = null;
                return RedirectToAction("XacNhanDonHang", "GioHang");
            }
            else
            {
                // Ngày giao không hợp lệ, hiển thị thông báo lỗi cho người dùng
                ViewBag.ErrorMessage = "Ngày giao hàng phải sau ngày đặt hàng. Vui lòng chọn lại.";
                List<GioHang> lstGioHang = LayGioHang();
                ViewBag.TongSoLuong = TongSoLuong();
                ViewBag.TongTien = TongTien();
                return View(lstGioHang);
            }
        }


        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}