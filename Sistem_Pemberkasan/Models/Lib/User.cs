using Microsoft.EntityFrameworkCore;
using Sistem_Pemberkasan.Models.EF;
using System.Security.Cryptography;
using System.Text;

namespace Sistem_Pemberkasan.Models.Lib
{
    public class User
    {
        public static List<EF.MUser> GetUsers()
        {
            var result = new List<EF.MUser>();
            var context = new ModelContext();
            var getUser = context.MUsers.ToList();
            if (getUser != null && getUser.Count > 0)
            {
                result = getUser;
            }
            return result;
        }
        public static int GetUserId(string email)
        {
            var context = new ModelContext();
            var getUser = context.MUsers.FirstOrDefault(x => x.Email == email);
            int IdUser = getUser != null ? getUser.IdUser : 0;


            return IdUser;
        }

        public static int GetUserBidang(int id)
        {
            var context = new ModelContext();
            var getUser = context.MUsers.Include(x=>x.NikNavigation).ThenInclude(x=>x.IdBidangNavigation).FirstOrDefault(x => x.IdUser == id);
            int Idbidang = getUser != null ? getUser.NikNavigation.IdBidangNavigation.IdBidang : 0;


            return Idbidang;
        }


        public static void SaveData()
        {
            var context = new ModelContext();
            //context.MUsers.Add();
            //context.SaveChanges();

        }
        public static string Encrypt(string clearText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }
    }
}
