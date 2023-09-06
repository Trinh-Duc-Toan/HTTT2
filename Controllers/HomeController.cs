using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HocPhi.Models;

namespace HocPhi.Controllers
{
    public class HomeController : Controller
         
    {
        HocPhiEntities db = new HocPhiEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult ForgotPass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login lg)
        {
            
            if(ModelState.IsValid)
            {
                using (HocPhiEntities db = new HocPhiEntities())
                {
                    var log = db.Accounts.Where(a => a.Email.Equals(lg.Email) && a.Password.Equals(lg.MatKhau)).FirstOrDefault();
                    if (log != null)
                    {
                        var getNameAccount = db.GiaoViens.Where(n => n.MaGiaoVien == log.MaGiaoVien).FirstOrDefault();

                        
                        if (getNameAccount != null)
                        {
                            var getClass = db.Lops.Where(n => n.MaGiaoVien == getNameAccount.MaGiaoVien).FirstOrDefault();
                            Session["getClass"] = getClass.MaLop.ToString();

                            Session["Admin"] = getNameAccount.TenGiaoVien;
                            Session["AdHinh"] = log.HinhAnh;
                            Session["dangnhap"] = log.Email;
                            Session["ID"] = log.ID;
                            Session["quyen"] = log.Quyen;
                            return RedirectToAction("Admin/Index", "Admin");
                        } else
                        {
                            Session["Admin"] = "Admin";
                            Session["AdHinh"] = log.HinhAnh;
                            Session["dangnhap"] = log.Email;
                            Session["ID"] = log.ID;
                            Session["quyen"] = log.Quyen;
                            
                            return RedirectToAction("Admin/Index", "Admin");

                        }
                       
                    }
                    else
                    {
                        ViewBag.SuccessMessage = "Sai Email hoặc password !";
                    }
                }
            }
            return View();


        }
    }
}