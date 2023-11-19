using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sachOnline_Binh.Models
{
    public class GioHang
    {
        DataClassesSachOnlineDataContext dt = new DataClassesSachOnlineDataContext();
        public int iMaSach { set; get; }
        public string sTenSach { set; get; }
        public string sAnhBia { set; get; }
        public Double dDonGia { set; get; }
        public int iSoLuong { set; get; }
        public Double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }
        //Khoi tao gio hang theo MaSach duoc truyen vao voi SoLuong mac dinh la 1
        public GioHang(int ms)
        {
            iMaSach = ms;
            SACH s = dt.SACHes.Single(n => n.MaSach == iMaSach);
            sTenSach = s.TenSach;
            sAnhBia = s.AnhBia;
            dDonGia = double.Parse(s.GiaBan.ToString());
            iSoLuong = 1;
        }
    }
}