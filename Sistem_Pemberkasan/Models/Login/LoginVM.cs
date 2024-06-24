using Microsoft.EntityFrameworkCore;
using Sistem_Pemberkasan.Models.EF;

namespace PenagihanPADWeb.Models.Login
{
    public class LoginVM
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string PesanError { get; set; } = "";
        public string? ReturnUrl { get; set; }
        public String? RememberMe { get; set; }
        public LoginVM()
        {

        }
        public class Register 
        {
            public Register()
            {
                
            }
        }
        //public LoginVM(string email, string password, string pesanError, string role)
        //{
        //    this.Email = email;
        //    this.Password = password;
        //    this.PesanError = pesanError;
           

        //}
        //public getRole(ModelContext context,string idrole) 
        //{
        //    var hasilJoin = from MPegawai in context.MPegawais // context adalah instance dari DbContext
        //                    join MRole in context.MRoles
        //                    on MPegawai.IdRole equals MRole.IdRole
        //                    select new
        //                    {
        //                        RoleName = MRole.JenisRole,
        //                        // Anda dapat menambahkan kolom lain yang Anda perlukan
        //                    };
        //    if (hasilJoin != null) 
        //    {
        //        Role = hasilJoin;
        //    }
        //}

    }
}
