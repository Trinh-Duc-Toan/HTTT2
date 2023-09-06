using HocPhi.Common;
using HocPhi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HocPhi.Areas.Admin.Controllers
{
    public class ThongKeController : Controller

    {
        HocPhiEntities db = new HocPhiEntities();
        // GET: Admin/ThongKe
        [AjaxOnly]
        [HttpGet]
        public ActionResult DiemDanh()
        {
            using (HocPhiEntities context = new HocPhiEntities())
            {
                var dsLop = (from lop in context.Lops
                             join hs in context.HocSinhs on lop.MaLop equals hs.MaLop
                             join dd in context.DiemDanhs on hs.MaHocSinh equals dd.HocSinh
                             group dd by
                             new
                             {
                                 IDLop = lop.MaLop,
                                 TenLopP = lop.TenLop,

                             }
                             into g
                             select new
                             {
                                 g.Key.IDLop,
                                 TenLopHoc
                                 = g.Key.TenLopP,
                                 Mindate = g.Min(x => x.NgayDiemDanh)
                             }).ToList();

                return Json(dsLop, JsonRequestBehavior.AllowGet);

            }
              
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ThongKeBienLai()
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            ViewBag.Lop_MaL = new SelectList(db.Lops.ToList().OrderBy(n => n.MaLop), "TenLop", "TenLop");
            return View();
        }

        public ActionResult KQ()
        {
            return View();
        }


        [HttpPost]
        public ActionResult KQ(FormCollection f)
        {
            string TenLop = f["TenLop"].ToString();
            DateTime Ngay = Convert.ToDateTime(f["NgayNop"]);
            bool TrangThai = Convert.ToBoolean(f["TrangThai"]);
            DateTime NgayEnd = Convert.ToDateTime(f["NgayNopEnd"]);
            ViewBag.CheckResult = db.BienLais.Where(x => x.HocSinh.Lop.TenLop == TenLop)
                .Where(x => x.NgayNop >= Ngay)
                .Where(x => x.NgayNop <= NgayEnd)
                .ToList();

            return View();
        }
        [HttpGet]
        public ActionResult ThongKeDoanhThu()
        {
            //ViewBag.Lop_MaL = new SelectList(db.Lops.ToList().OrderBy(n => n.MaLop), "TenLop", "TenLop");
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            return View();

        }
        public ActionResult DoanhThu()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoanhThu(FormCollection f)
        {
            int MaBienLai = Convert.ToInt32(f["MaBienLai"]);
       
            DateTime Ngay = Convert.ToDateTime(f["NgayNop"]);
            bool TrangThai = Convert.ToBoolean(f["TrangThai"]);
            DateTime NgayEnd = Convert.ToDateTime(f["NgayNopEnd"]);
            ViewBag.Result = db.BienLais.Where(x => x.NgayNop >= Ngay)
                .Where(x => x.NgayNop <= NgayEnd)
                .Where(x => x.TrangThai == true)
                .ToList();

            return View();


        }

        [HttpGet]
        public ActionResult ThongKeCongNo()
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            //ViewBag.Lop_MaL = new SelectList(db.Lops.ToList().OrderBy(n => n.MaLop), "TenLop", "TenLop");

            return View();

        }
        public ActionResult CongNo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CongNo(FormCollection f)
        {
            int MaBienLai = Convert.ToInt32(f["MaBienLai"]);

            DateTime Ngay = Convert.ToDateTime(f["NgayNop"]);
            bool TrangThai = Convert.ToBoolean(f["TrangThai"]);
            DateTime NgayEnd = Convert.ToDateTime(f["NgayNopEnd"]);
            ViewBag.Result = db.BienLais.Where(x => x.NgayNop >= Ngay)
                .Where(x => x.NgayNop <= NgayEnd)
                .Where(x => x.TrangThai == false)
                .ToList();

            return View();


        }



        [HttpPost]
        [AjaxOnly]
        public ActionResult LichSuDiemDanh (string NgayDiemDanh, string MaLop)
        {
            
            DateTime dt = DateTime.ParseExact(NgayDiemDanh, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            using (HocPhiEntities context = new HocPhiEntities())
            {
                var HsTheoLop = (from hs in context.HocSinhs
                                 join lop in context.Lops on hs.Lop.MaLop equals lop.MaLop
                                 where hs.Lop.MaLop == MaLop
                                 select hs);
                var DsDiemDanh = (from dd in context.DiemDanhs
                                  join hs in HsTheoLop on dd.HocSinh equals hs.MaHocSinh
                                  where dd.NgayDiemDanh.Year == dt.Year && dd.NgayDiemDanh.Month == dt.Month && dd.NgayDiemDanh.Day == dt.Day
                                  select dd
                                  ).ToList();
                var HSTheoTrangThaiDiemDanh = HsTheoLop.FullOuterJoin(DsDiemDanh, a => a.MaHocSinh, b => b.HocSinh, (a, b, MaHocSinh) => new
                {
                    MaHocSinh,
                    a.TenHocSinh,
                    a.GioiTinh,
                    a.NamSinh,
                    a.TenPhuHuynh,
                    a.DienThoai,
                    a.DiaChiLienHe,
                    b?.TrangThaiDiemDanh
                });
                List<DiemDanhViewModel> _hs = new List<DiemDanhViewModel>();
                foreach (var hs in HSTheoTrangThaiDiemDanh)
                {
                    _hs.Add(new DiemDanhViewModel()
                    {
                        MaHocSinh = hs.MaHocSinh,
                        TenHocSinh = hs.TenHocSinh,
                        GioiTinh = hs.GioiTinh,
                        NamSinh = hs.NamSinh,
                        TenPhuHuynh = hs.TenPhuHuynh,
                        DienThoai = hs.DienThoai,
                        DiaChiLienHe = hs.DiaChiLienHe,
                        TrangThaiDiemDanh = hs.TrangThaiDiemDanh
                    });
                }

                return PartialView("LichSuDiemDanh", _hs);
            } 
        }
    }
}