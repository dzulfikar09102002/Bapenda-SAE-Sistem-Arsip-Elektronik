using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Sistem_Pemberkasan.Models.EF;
using System.IO.Compression;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Sistem_Pemberkasan.Models.Lib
{
    public class UploadFile
    {
        public static int SaveDraft(Models.Transaksi.DraftVM.SaveDraft model, int StatusBerkas, int IdUser)
        {
            var context = new ModelContext();

            int id = 0;
            int? idBerkas = context.Berkas.DefaultIfEmpty().Max(x => (int?)x.IdBerkas);
            if (idBerkas.HasValue)
            {
                id = idBerkas.Value + 1;
            }
            else
            {
                id = 1;
            }
            context.Berkas.Add(new Berka
            {
                IdBerkas = id,
                IdKategoriBerkas = model.NewRow.IdKategoriBerkas,
                Nop = model.NewRow.Nop,
                NoReferensi = model.NewRow.NoReferensi,
                TanggalUnggah = DateTime.Now,
                NoSk = model.NewRow.NoSk,
                NamaPemohon = model.NewRow.NamaPemohon,
                AlamatPemohon = model.NewRow.AlamatPemohon,
                NoTelpPemohon = model.NewRow.NoTelpPemohon,
                KeteranganPst = model.NewRow.KeteranganPst,
                StatusBerkas = StatusBerkas,
                BeInsertBy = IdUser,
                BeInsertDate = DateTime.Now,
            });
            context.SaveChanges();

            return id;
        }
        public static async Task<string> SaveFileAsync(IFormFile file, int daftarid, int idBerkas, int IdUser)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            string[] allowedExtensions = { ".pdf", ".docx" };

            string JenisFile = daftarid.ToString();
            int idDokumen = Convert.ToInt32(JenisFile);
            string idBerkasString = idBerkas.ToString();
            string NameFileStandart = IdUser.ToString();
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd");

            var fileName = idBerkasString + "-" + NameFileStandart + "-" + JenisFile + "-" + dateTime + ".pdf";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets/berkas/", fileName);

            //Menyimpan File
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;

        }
        public static async Task<Tuple<string, byte[]>> SaveFileAsyncBool(IFormFile file, int daftarid, int idBerkas, int IdUser)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            string[] allowedExtensions = { ".pdf", ".docx" };

            string JenisFile = daftarid.ToString();
            int idDokumen = Convert.ToInt32(JenisFile);
            string idBerkasString = idBerkas.ToString();
            string NameFileStandart = IdUser.ToString();
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd");

            var fileName = idBerkasString + "-" + NameFileStandart + "-" + JenisFile + "-" + dateTime + ".pdf";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets/berkas/", fileName);

            // Array untuk menyimpan data file
            byte[] filearr;
            using (var ms = new MemoryStream())
            {
                // Salin file ke MemoryStream
                file.CopyTo(ms);

                // Konversi stream ke array byte
                filearr = ms.ToArray();

                // Dapatkan jenis file dari array byte
                var jnsfile = GetJenisFile(filearr);
            }
            return new Tuple<string, byte[]>(fileName, filearr);


        }

        public static async Task<string> SaveCompressedFileAsync(IFormFile file, int daftarid, int idBerkas, int IdUser)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            string[] allowedExtensions = { ".pdf", ".docx" };

            // Memastikan ekstensi file yang diizinkan
            if (!allowedExtensions.Contains(extension))
            {
                throw new ArgumentException("Ekstensi file tidak valid.");
            }

            string JenisFile = daftarid.ToString();
            int idDokumen = Convert.ToInt32(JenisFile);
            string idBerkasString = idBerkas.ToString();
            string NameFileStandart = IdUser.ToString();
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd");

            var fileName = idBerkasString + "-" + NameFileStandart + "-" + JenisFile + "-" + dateTime + ".pdf";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets/berkas/", fileName);
            var compressedFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets/berkas/compress/", fileName);

            //Menyimpan File dengan kompresi
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            using (var compressedFileStream = new FileStream(compressedFilePath, FileMode.Create))
            using (var gzipStream = new GZipStream(compressedFileStream, CompressionLevel.Fastest))
            {
                await file.CopyToAsync(fileStream); // Menyalin file ke fileStream
                fileStream.Position = 0; // Mengatur posisi stream ke awal
                await fileStream.CopyToAsync(gzipStream); // Kompresi fileStream ke gzipStream
            }

            return "compressed_" + fileName;
        }
        public static void DecompressFile(string compressedFilePath, string decompressedFilePath)
        {
            using (var compressedFileStream = new FileStream(compressedFilePath, FileMode.Open))
            using (var decompressedFileStream = new FileStream(decompressedFilePath, FileMode.Create))
            using (var gzipStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
            {
                gzipStream.CopyTo(decompressedFileStream);
            }
        }
        public static void SaveDetailDokumen(int idBerkas, int? idDokumen, string namaDetailDokumen, byte[] fileBloob)
        {
            var context = new ModelContext();

            int id = 0;
            int? idDetailDokumen = context.DetailDokumen.DefaultIfEmpty().Max(x => (int?)x.IdDetailDokumen);
            if (idDetailDokumen.HasValue)
            {
                id = idDetailDokumen.Value + 1;
            }
            else
            {
                id = 1;
            }
            context.DetailDokumen.Add(new DetailDokuman
            {
                IdDetailDokumen = id,
                IdBerkas = idBerkas,
                IdDokumen = idDokumen,
                NamaDetailDokumen = namaDetailDokumen,
                FileBloob = fileBloob,
            });
            context.SaveChanges();
        }

        public static int EditDraft(Models.Transaksi.DraftVM.EditDraft model, int IdUser)
        {
            var context = new ModelContext();

            var dataBerkasOld = context.Berkas.FirstOrDefault(x => x.IdBerkas == model.NewRow.IdBerkas);

            dataBerkasOld.IdKategoriBerkas = model.NewRow.IdKategoriBerkas;
            dataBerkasOld.Nop = model.NewRow.Nop;
            dataBerkasOld.NoReferensi = model.NewRow.NoReferensi;
            dataBerkasOld.TanggalUnggah = DateTime.Now;
            dataBerkasOld.NoSk = model.NewRow.NoSk;
            dataBerkasOld.NamaPemohon = model.NewRow.NamaPemohon;
            dataBerkasOld.AlamatPemohon = model.NewRow.AlamatPemohon;
            dataBerkasOld.NoTelpPemohon = model.NewRow.NoTelpPemohon;
            dataBerkasOld.KeteranganPst = model.NewRow.KeteranganPst;
            dataBerkasOld.StatusBerkas = 4;
            dataBerkasOld.BeInsertBy = IdUser;
            dataBerkasOld.BeInsertDate = DateTime.Now;

            context.SaveChanges();

            return dataBerkasOld.IdBerkas;
        }
        public static void EditDetailDokumen(int idBerkas, int? idDokumen, string namaDetailDokumen, byte[] fileBloob)
        {
            var context = new ModelContext();

            var dokumenOld = context.DetailDokumen.FirstOrDefault(x => x.IdBerkas == idBerkas);

            dokumenOld.NamaDetailDokumen = namaDetailDokumen;
            dokumenOld.FileBloob = fileBloob;

            context.SaveChanges();
        }
        public static int GetBerkasId(int id)
        {
            var context = new ModelContext();
            var getBerkas = context.Berkas.FirstOrDefault(x => x.IdBerkas == id);
            int idBerkas = getBerkas != null ? getBerkas.IdBerkas : 0;

            return idBerkas;
        }
        public static void UpdateStatus(int idBerkas, int Status)
        {
            var context = new ModelContext();

            var OldDataStatus = context.Berkas.FirstOrDefault(e => e.IdBerkas == idBerkas);
            OldDataStatus.StatusBerkas = Status;
            OldDataStatus.TanggalUnggah = DateTime.Now;
            context.SaveChanges();
        }
        public static JenisFile GetJenisFile( byte[] bytes)
        {
            JenisFile jenis = new JenisFile();
            if (bytes == null)
            {
                jenis.Jenis = ETipeFile.Unknown;
                jenis.ContentType = "text/html";
                jenis.FileExtension = ".txt";
            }
            else
            {
                if (bytes.Length >= 8 &&
                    bytes[0] == 137 && bytes[1] == 80 && bytes[2] == 78 && bytes[3] == 71 &&
                    bytes[4] == 13 && bytes[5] == 10 && bytes[6] == 26 && bytes[7] == 10)
                {
                    // Set the content type and file ext    ension for a PNG file
                    jenis.Jenis = ETipeFile.Image;
                    jenis.ContentType = "image/png";
                    jenis.FileExtension = ".png";
                }
                else if (bytes.Length > 4 && Encoding.ASCII.GetString(bytes.Take(5).ToArray()) == "%PDF-")
                {
                    // Set the content type and file extension for a PDF file
                    jenis.Jenis = ETipeFile.PDF;
                    jenis.ContentType = "application/pdf";
                    jenis.FileExtension = ".pdf";
                }
                else if (bytes.Length > 2 && bytes[0] == 0xFF && bytes[1] == 0xD8)
                {
                    // Set the content type and file extension for a JPEG image
                    jenis.Jenis = ETipeFile.Image;
                    jenis.ContentType = "image/jpeg";
                    jenis.FileExtension = ".jpg";
                }
                else
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(bytes))
                        jenis.Jenis = ETipeFile.Image;
                        jenis.ContentType = "image/jpeg";
                        jenis.FileExtension = ".jpg";
                    }
                    catch (ArgumentException)
                    {
                        jenis.Jenis = ETipeFile.Unknown;
                        jenis.ContentType = "text/html";
                        jenis.FileExtension = ".txt";
                    }

                }
            }

            return jenis;
        }
        //public static DetailDokuman Decode()
        //{

        //    var jenisFile = SurabayaTaxLib.Lib.General.Utility.GetJenisFile(Dok.DokumenFile);

        //    var cd = new System.Net.Mime.ContentDisposition
        //    {
        //        FileName = "FILENAME" + jenisFile.FileExtension, // Set your desired filename
        //        Inline = false,  // Change to true if you want the browser to try to open the file
        //    };
        //    Response.Headers.Add("Content-Disposition", cd.ToString());
        //    return File(Dok.DokumenFile, jenisFile.ContentType);
        //}

        public static bool CheckNamaDokumen(string namaDokumenInputan)
        {
            var context = new ModelContext();

            var CheckDokumen = context.MDokumen.ToList();

            string lowercaseInputan = namaDokumenInputan.ToLower().Replace(" ", "");

            for (int i = 0; i < CheckDokumen.Count; i++)
            {
                string NamaDokumenDatabase = CheckDokumen[i].NamaDokumen.ToLower().Replace(" ", "");
                if (lowercaseInputan == NamaDokumenDatabase)
                {
                    // Jika dokumen ditemukan, return false segera
                    return false;
                }
            }

            // Jika tidak ada dokumen yang ditemukan, return true
            return true;
        }

    }
}
