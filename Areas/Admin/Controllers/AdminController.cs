using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using HocPhi.Models;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using HocPhi.Common;

using System.Data.Entity;
using System.IO;

namespace HocPhi.Areas.Admin.Controllers
{
    
    public class AdminController : Controller
    {
        HocPhiEntities db = new HocPhiEntities();
        private object item;

        // GET: Admin/Admin
        public ActionResult Index()
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            var tsHeHoc = db.HeHocs.OrderByDescending(m => m.MaHeHoc);
            ViewBag.tsHeHoc = tsHeHoc.Count();

            var tsGV = db.GiaoViens.OrderByDescending(m => m.MaGiaoVien);
            ViewBag.tsGV = tsGV.Count();

            var tsLH = db.Lops.OrderByDescending(m => m.MaLop);
            ViewBag.tsLH = tsLH.Count();

            var tsHS = db.HocSinhs.OrderByDescending(m => m.MaHocSinh);
            ViewBag.tsHS = tsHS.Count();

            var tsBL = db.BienLais.OrderByDescending(m => m.MaBienLai);
            ViewBag.tsBL = tsBL.Count();


            return View();
        }
        public ActionResult HeHoc()
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            var hehoc = db.HeHocs.ToList();      
            return View(hehoc);
        }
        [HttpGet]
        public ActionResult ThemHeHoc()
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }

            return View();
        }

        [HttpPost]  
        public ActionResult ThemHeHoc(HeHoc hehoc)
        {

            if (ModelState.IsValid)
            {
                db.HeHocs.Add(hehoc);
                db.SaveChanges();
                return RedirectToAction("HeHoc");
            }
            return View();
        }
        [HttpGet]
        public ActionResult SuaHeHoc(string mahehoc)
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            HeHoc hehoc = db.HeHocs.SingleOrDefault(n => n.MaHeHoc == mahehoc);
            return View(hehoc);
        }
        [HttpPost]
        public ActionResult SuaHeHoc(HeHoc hehoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hehoc).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("HeHoc");
            }
            return View();
        }
        public ActionResult XoaHeHoc(string mahehoc)
        {
            HeHoc hehoc = db.HeHocs.SingleOrDefault(n => n.MaHeHoc == mahehoc);
            db.HeHocs.Remove(hehoc);
            db.SaveChanges();
            return RedirectToAction("HeHoc");
        }
        public ActionResult HocSinh(HocSinh hs)
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
              //  ViewBag.hs = db.HocSinhs.ToList();
                var hocsinh = db.HocSinhs.ToList();
            ViewBag.hs = hocsinh;
              //  foreach(hocsinh in )
                
                return View(hocsinh);
           
        }
        [HttpGet]
        public ActionResult ThemHocSinh()
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            ViewBag.HeHoc_MHH = new SelectList(db.HeHocs.ToList().OrderBy(n => n.MaHeHoc), "MaHeHoc", "MaHeHoc");
            ViewBag.Lop_MaL = new SelectList(db.Lops.ToList().OrderBy(n => n.MaLop), "MaLop", "MaLop");

            return View();
        }
        [HttpPost]
        public ActionResult ThemHocSinh(HocSinh hocsinh)
        {
            ViewBag.HeHoc_MHH = new SelectList(db.HeHocs.ToList().OrderBy(n => n.MaHeHoc), "MaHeHoc", "MaHeHoc");
            ViewBag.Lop_MaL = new SelectList(db.Lops.ToList().OrderBy(n => n.MaLop), "MaLop", "MaLop");
            if (ModelState.IsValid)
            {
                hocsinh.TrangThai = 1;
                db.HocSinhs.Add(hocsinh);
                db.SaveChanges();
                return RedirectToAction("HocSinh");
            }
            return View();
        }
        [HttpGet]
        public ActionResult SuaHocSinh(string mahocsinh)
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            HocSinh hocsinh = db.HocSinhs.SingleOrDefault(n => n.MaHocSinh == mahocsinh);
            return View(hocsinh);
        }

        [HttpPost]
        public ActionResult SuaHocSinh(HocSinh hocsinh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hocsinh).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("HocSinh");
            }
            return View();
                
        }
        public ActionResult XoaHocSinh(string mahocsinh)
        {
            HocSinh hocsinh = db.HocSinhs.SingleOrDefault(n => n.MaHocSinh == mahocsinh);
            hocsinh.TrangThai = 0;
           // db.HocSinhs.Remove(hocsinh);
            db.SaveChanges();
            return RedirectToAction("HocSinh");
        }

        public ActionResult Lop()
        {
          
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            var lop = db.Lops.ToList();
            
            ViewBag.Lop_MaL = new SelectList(db.Lops.ToList().OrderBy(n => n.MaLop), "MaLop", "MaLop");
            return View(lop);
            

        }
        [HttpGet]
        public ActionResult XemHocSinh(string malop)
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            var products = db.HocSinhs.Where(pr => pr.MaLop == malop).ToList();

            return View(products);
        }

        public JsonResult GetStateListLop_hocsinh(string malop)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<HocSinh> ds = db.HocSinhs.Where(x => x.MaLop == malop).ToList();
            return Json(ds, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStateListhehoc_lop(string malop)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Lop> ds = db.Lops.Where(x => x.MaHeHoc == malop).ToList();
            return Json(ds, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStateListhehoc_hocsinh(string mahocsinh)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<HocSinh> ds = db.HocSinhs.Where(x => x.MaHeHoc == mahocsinh).ToList();
            return Json(ds, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetStateListGV(string magv)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<GiaoVien> ds = db.GiaoViens.Where(x => x.MaGiaoVien == magv).ToList();
            return Json(ds, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult ThemLop()
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            var sql = @"SELECT MaGiaoVien FROM GiaoVien GV0 WHERE GV0.MaGiaoVien NOT IN (SELECT GV.MaGiaoVien FROM GiaoVien GV, Lop L WHERE GV.MaGiaoVien = L.MaGiaoVien)";
            ViewBag.HeHoc_MHH = new SelectList(db.HeHocs.ToList().OrderBy(n => n.MaHeHoc), "MaHeHoc", "MaHeHoc");

            ViewBag.Ma_GV = new SelectList(db.Database.SqlQuery<QueryResults>(sql).ToList(), "MaGiaoVien", "MaGiaoVien");
            ViewBag.Ma_GV1 = new SelectList(db.GiaoViens.ToList().OrderBy(n => n.MaGiaoVien), "MaGiaoVien1", "MaGiaoVien1");

            return View();
        }
      
        [HttpPost]
        public ActionResult ThemLop(Lop lop)
        {
            ViewBag.Ma_GV = new SelectList(db.GiaoViens.ToList().OrderBy(n => n.MaGiaoVien), "MaGiaoVien", "MaGiaoVien");
           // ViewBag.Ma_GV1 = new SelectList(db.GiaoViens.ToList().OrderBy(n => n.MaGiaoVien), "MaGiaoVien1", "MaGiaoVien1");

            ViewBag.HeHoc_MHH = new SelectList(db.HeHocs.ToList().OrderBy(n => n.MaHeHoc), "MaHeHoc", "MaHeHoc");
            if (ModelState.IsValid)
            {
                db.Lops.Add(lop);
                db.SaveChanges();
                return RedirectToAction("Lop");
            }
            return View();
        }

        [HttpGet]
        public ActionResult SuaLop(string malop)
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            ViewBag.Ma_GV = new SelectList(db.GiaoViens.ToList().OrderBy(n => n.MaGiaoVien), "MaGiaoVien", "MaGiaoVien");

            Lop lop = db.Lops.SingleOrDefault(n => n.MaLop == malop);
            return View(lop);
        }

        [HttpPost]
        public ActionResult SuaLop(Lop lop)
        {
            ViewBag.Ma_GV = new SelectList(db.GiaoViens.ToList().OrderBy(n => n.MaGiaoVien), "MaGiaoVien", "MaGiaoVien");
            if (ModelState.IsValid)
            {
                db.Entry(lop).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Lop");
            }
            return View();
                
        }
        public ActionResult XoaLop(string malop)
        {
            Lop lop = db.Lops.SingleOrDefault(n => n.MaLop == malop);
            db.Lops.Remove(lop);
            db.SaveChanges();
            return RedirectToAction("Lop");
        }
        public ActionResult DiemDanh()
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            return View();
        }
        public ActionResult exit()
        {
            Session.Clear();
            return Redirect("/Home/Login");
        }
        public ActionResult GiaoVien()
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            var gv = db.GiaoViens.ToList();
            return View(gv);
        }
        [HttpGet]
        public ActionResult ThemGiaoVien()
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ThemGiaoVien(GiaoVien gv)
        {
            if (ModelState.IsValid)
            {
                db.GiaoViens.Add(gv);
                db.SaveChanges();
                return RedirectToAction("GiaoVien");
            }
            return View();
        }
        [HttpGet]
        public ActionResult SuaGiaoVien(string magiaovien)
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            GiaoVien gv = db.GiaoViens.SingleOrDefault(n => n.MaGiaoVien == magiaovien);
            return View(gv);
        }

        [HttpPost]
        public ActionResult SuaGiaoVien(GiaoVien gv)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gv).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GiaoVien");
            }
            return View();
               
        }
        public ActionResult XoaGiaoVien(string magiaovien)
        {
            GiaoVien gv = db.GiaoViens.SingleOrDefault(n => n.MaGiaoVien == magiaovien);
            db.GiaoViens.Remove(gv);
            db.SaveChanges();
            return RedirectToAction("GiaoVien");
        }
        [HttpGet]
        public ActionResult ChonLopDiemDanh()
        {
            if (Session["quyen"] == null)
            {
                var malop = Session["getClass"].ToString();

                var dsLop = db.Lops.Where(n => n.MaLop == malop).ToList();

                return View(dsLop);
            }
            else
            {
                using (HocPhiEntities context = new HocPhiEntities())
                {
                    var dsLop = (from lop in context.Lops
                                 select lop).ToList();
                    //var dsLop = db.Lops.Where(n=>n.MaLop == malop).ToList();

                    return View(dsLop);
                }
            }
            

                
        }
        [HttpPost]
        public ActionResult LayDanhSachDiemDanh(FormCollection form)
        {
            string MaLop = form[0].ToString();
            ViewBag.Operation = "create";
            using (HocPhiEntities context = new HocPhiEntities())
            {
                var HsTheoLop = (from hs in context.HocSinhs
                                 join lop in context.Lops on hs.Lop.MaLop equals lop.MaLop
                                 where hs.Lop.MaLop == MaLop
                                 select hs);
                var DsDiemDanh = (from dd in context.DiemDanhs
                                  join hs in HsTheoLop on dd.HocSinh equals hs.MaHocSinh
                                  where dd.NgayDiemDanh.Year == DateTime.Now.Year && dd.NgayDiemDanh.Month == DateTime.Now.Month && dd.NgayDiemDanh.Day == DateTime.Now.Day
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
                foreach(var hs in HSTheoTrangThaiDiemDanh)
                {
                    if (hs.TrangThaiDiemDanh != null)
                    {
                        ViewBag.Operation = "edit";
                    }
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
                Session["TongSoHocSinh"] = _hs.Count();
                return View("LayDanhSachDiemDanh", _hs);

            }
                
        }
        
        [HttpPost]
        public ActionResult DiemDanhUpdate(FormCollection form)
        {
            string[] status = form["statuslist"].ToString().Split(new Char[] { ',' });
            List<bool> statusbool = status.Select(x =>{
                    if (x == "true") return true;
                    else return false;
            }).ToList();
            string[] dsHS  = form[1].ToString().Split(new Char[] { ',' });
            ViewBag.TongSoHocSinh = (int)Session["TongSoHocSinh"];
            using (HocPhiEntities context = new HocPhiEntities())
            {
                using (IDbConnection db = new SqlConnection(context.Database.Connection.ConnectionString))
                {
                    if (form["operation"].ToString() == "create")
                    {
                        int i = 0;
                        dsHS.ToList().ForEach(
                            x =>
                            {
                                var p = new DynamicParameters();
                                p.Add("@MahocSinh", x);
                                p.Add("@TrangThai", statusbool[i]);
                                db.Execute("sp_DiemDanhHS", p, commandType: CommandType.StoredProcedure);
                                i++;
                            });
                        ViewBag.DaDiemDanh = i;
                    }
                    else
                    {
                        int j = 0;
                        dsHS.ToList().ForEach(
                            x =>
                            {
                                var q = new DynamicParameters();
                                q.Add("@MaHs", x);
                                q.Add("@TrangThaiDiemDanh", statusbool[j]);
                                db.Execute("sp_CapNhatTrangThaiDiemDanh", q, commandType: CommandType.StoredProcedure);
                                j++;
                            });
                        ViewBag.DaDiemDanh = j;
                    }    
                }
            }
            return View();

        }
        public ActionResult BienLai()
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            var blai = db.BienLais.ToList();
            ViewBag.bienlai = blai;
          
            return View();
        }
        [HttpGet]
        public ActionResult ThemBienLai()
        {
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            ViewBag.HeHoc_MHH = new SelectList(db.HeHocs.ToList().OrderBy(n => n.MaHeHoc), "MaHeHoc", "MaHeHoc");
            ViewBag.Ma_HS = new SelectList(db.HocSinhs.ToList().OrderBy(n => n.MaHocSinh), "MaHocSinh", "MaHocSinh");

            return View();
        }
        [HttpPost]
        public ActionResult ThemBienLai(BienLai BienLai, FormCollection form)
        {
            ViewBag.HeHoc_MHH = new SelectList(db.HeHocs.ToList().OrderBy(n => n.MaHeHoc), "MaHeHoc", "MaHeHoc");
            ViewBag.Ma_HS = new SelectList(db.HocSinhs.ToList().OrderBy(n => n.MaHocSinh), "MaHocSinh", "MaHocSinh");

            
            BienLai.MaHocSinh = form["txt_NewMaHocSinh"].ToString();
        
            BienLai.TienAn1ngay = Convert.ToInt32(form["txt_NewTienAn1ngay"]);
            BienLai.NgayNop = Convert.ToDateTime(form["txt_NewNgayNop"]);
            BienLai.TienAn1thang = Convert.ToDouble(BienLai.TienAn1ngay * 26);
            BienLai.NguoiNop =form["txt_NewNguoiNop"];
            BienLai.TrangThai = Convert.ToBoolean(form["txtTrangThai"]);
           //  BienLai.TongCong = Convert.ToDouble(form["tong"]);
            //BienLai.TongCong = 
            db.BienLais.Add(BienLai);
            db.SaveChanges();
            return RedirectToAction("BienLai");

        }
        [HttpGet]
        public ActionResult SuaBienLai(int mabienlai)
        {


            BienLai BienLai = db.BienLais.SingleOrDefault(n => n.MaBienLai == mabienlai);

            return View(BienLai);

        }
        [HttpPost]
        public ActionResult SuaBienLai(BienLai BienLai, FormCollection form)
        {
            ViewBag.HeHoc_MHH = new SelectList(db.HeHocs.ToList().OrderBy(n => n.MaHeHoc), "MaHeHoc", "MaHeHoc");
            ViewBag.Ma_HS = new SelectList(db.HocSinhs.ToList().OrderBy(n => n.MaHocSinh), "MaHocSinh", "MaHocSinh");


            BienLai.TienAn1ngay = Convert.ToInt32(form["txt_NewTienAn1ngay"]);
            BienLai.NgayNop = Convert.ToDateTime(form["txt_NewNgayNop"]);
            BienLai.TienAn1thang = Convert.ToDouble(BienLai.TienAn1ngay * 26);
            BienLai.NguoiNop = form["txt_NewNguoiNop"];
            BienLai.TrangThai = Convert.ToBoolean(form["txtTrangThai"]);
            db.Entry(BienLai).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("BienLai");
        }
        [HttpGet]
        public ActionResult PhieuThu(int mabienlai)
        {
            var blai = db.BienLais.Where(pr => pr.MaBienLai == mabienlai).ToList();
            ViewBag.bienlai = blai;
            return View();
        }
        public ActionResult QuanLyTK()
        {
            
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            var gv = db.GiaoViens.ToList();
            
            return View(gv);
            
        }
        [HttpGet]
        public ActionResult ThemTK(string magiaovien)
        {
           
            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }

            Account gv = db.Accounts.SingleOrDefault(n => n.MaGiaoVien == magiaovien);
            return View(gv);
        }
        [HttpPost]
        public ActionResult ThemTK(Account gv)
        {

            if (Session["ID"] == null || Session["ID"].ToString() == " ")
            {
                return Redirect("/Home/Login");
            }
            string filename = Path.GetFileNameWithoutExtension(gv.abc.FileName);
            string extension = Path.GetExtension(gv.abc.FileName);
            filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
            gv.HinhAnh = "~/img/" + filename;
            filename = Path.Combine(Server.MapPath("~/img/"), filename);
            gv.abc.SaveAs(filename);
            ViewBag.Ma_GV = new SelectList(db.GiaoViens.ToList().OrderBy(n => n.MaGiaoVien), "MaGiaoVien", "MaGiaoVien");
            ViewBag.Ma_GVv = new SelectList(db.GiaoViens.ToList().OrderBy(n => n.TenGiaoVien), "TenGiaoVien", "TenGiaoVien");

            if (ModelState.IsValid)
            {
                db.Accounts.Add(gv);
                db.SaveChanges();
                return RedirectToAction("QuanLyTK");
            }
            return View();
        }

        private void @foreach(object var, object item, dynamic bienlai)
        {
            throw new NotImplementedException();
        }
    }
    
}