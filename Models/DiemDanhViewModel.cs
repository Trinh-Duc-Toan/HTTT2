using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HocPhi.Models
{
    public class DiemDanhViewModel
    {
        
        public string MaHocSinh { get; set; }
        public string TenHocSinh { get; set; }
        public string GioiTinh { get; set; }
        public Nullable<System.DateTime> NamSinh { get; set; }

        public string TenPhuHuynh { get; set; }
        public string DienThoai { get; set; }
        public string DiaChiLienHe { get; set; }

        public bool? TrangThaiDiemDanh { get; set; }
    }
}