using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HocPhi.Models
{
    [MetadataType(typeof(Login))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }
    public class Login
    {
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public string TenAdmin { get; set; }
        public string HinhAnh { get; set; }

    }
}