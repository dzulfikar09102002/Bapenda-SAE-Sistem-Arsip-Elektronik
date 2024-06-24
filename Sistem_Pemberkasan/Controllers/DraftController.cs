using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting.Internal;
using Sistem_Pemberkasan.Models;
using Sistem_Pemberkasan.Models.EF;
using Sistem_Pemberkasan.Models.Lib;
using System.Linq;


namespace Sistem_Pemberkasan.Controllers
{
    [Authorize(Roles = "Pengelola,Perekam")]

    public class DraftController : Controller
    {
        private readonly ModelContext _context;
        string strViewPath = string.Empty;
        private readonly CookieData _cookieData;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public List<MKategoriBerka> ListKategori { get; private set; }
        public DraftController(ModelContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            strViewPath = string.Concat("../Transaksi/", GetType().Name.Replace("Controller", ""), "/");
            _cookieData = new CookieData(context, httpContextAccessor);
            _hostingEnvironment = hostingEnvironment;

        }
        //Halaman Utama Index
        public IActionResult Index()
        {
            string EmailUser = _cookieData.GetUser();
            int idUser = _cookieData.GetIdUser();
            var model = new Models.Transaksi.DraftVM.Index(_context, idUser);
            return View(strViewPath + "Index", model);
        }
        //Filter Berkas
        [HttpPost]
        public IActionResult Filter(Models.Transaksi.DraftVM.Index input)
        {
            int idUser = _cookieData.GetIdUser();
            var model = new Models.Transaksi.DraftVM.Index(_context, idUser, input.IdKategori, input.IdBidang, input.tanggalMulai, input.tanggalAkhir, input.StatusBerkas);

            return PartialView(strViewPath + "_TableFilter", model);
        }
        [HttpPost]
        [HttpGet]
        public IActionResult Search(Models.Transaksi.DraftVM.Index input)
        {
            int idUser = _cookieData.GetIdUser();

            var model = new Models.Transaksi.DraftVM.Index(_context, input.SearchBox);

            return PartialView(strViewPath + "_TableFilter", model);
        }

        //Detail Dokumen
        public IActionResult DetailDokumen(string IdBerkas, string IdKategori)
        {
            int idUser = _cookieData.GetIdUser();
            var model = new Models.Transaksi.DraftVM.Detail(_context, Convert.ToInt32(IdBerkas), Convert.ToInt32(IdKategori), idUser);
            return PartialView(strViewPath + "_DetailDokumen", model);
        }
        //controller modal box input

        public IActionResult ModalBoxForm()
        {
            int idUser = _cookieData.GetIdUser();
            var userBidang = _context.MUsers.Include(x => x.NikNavigation).FirstOrDefault(x => x.IdUser == idUser);
            string idBidangUserNow = Convert.ToString(userBidang.NikNavigation.IdBidang);

            var model = new Models.Transaksi.DraftVM.ModalBox(_context, idBidangUserNow);
            return PartialView(strViewPath + "_FormModal", model);
        }
        //Form Upload File
        public IActionResult FileForm(string id)
        {
            var model = new Models.Transaksi.DraftVM.ModalBox(_context, Convert.ToInt32(id));

            return PartialView(strViewPath + "_FileFormInput", model);

        }
        //Simpan Data / Save Data

        [HttpPost]
        public async Task<IActionResult> SaveDraft(Models.Transaksi.DraftVM.SaveDraft model, string dataPilihan)
        {
            ResponseBase responseBase = new ResponseBase();
            int IdUser = _cookieData.GetIdUser();


            //status berkas 1=draft , 2=Tersimpan ,0=Terhapus 
            int statusBerkas = 1;
            try
            {
            var getUser = _context.MUsers.First(x => x.IdUser == Convert.ToInt32("a"));
                if (model.files != null)
                {
                    int i = 1;
                    // Memeriksa ukuran file
                    const long fileSizeLimit = 2 * 1024 * 1024;
                    for (int z = 0; z < model.files.Count; z++) 
                    {
                        if (model.files[z].Length > fileSizeLimit)
                        {
                            throw new Exception("Ukuran file tidak boleh melebihi 2 MB.");
                            
                        }
                    }
                    //simpan draft dahulu
                    int idBerkas = Models.Lib.UploadFile.SaveDraft(model, statusBerkas, IdUser);
                    //looping file
                    foreach (var file in model.files)
                    {
                        
                        if (Convert.ToInt32(dataPilihan) == 1)
                        {
                            var result = await Models.Lib.UploadFile.SaveFileAsyncBool(file, 4, idBerkas, IdUser);


                            int GetIdBerkas = Models.Lib.UploadFile.GetBerkasId(idBerkas);

                            Models.Lib.UploadFile.SaveDetailDokumen(GetIdBerkas, 4, result.Item1, result.Item2);

                            responseBase.Message = "File Jadi Satu Sukses Diunggah";
                            responseBase.Status = StatusEnum.Success;
                            break;
                        }
                        else
                        {
                            var result = await Models.Lib.UploadFile.SaveFileAsyncBool(file, model.DaftarId[i], idBerkas, IdUser);


                            int GetIdBerkas = Models.Lib.UploadFile.GetBerkasId(idBerkas);

                            Models.Lib.UploadFile.SaveDetailDokumen(GetIdBerkas, model.DaftarId[i], result.Item1, result.Item2);

                            //Models.Lib.UploadFile.SaveDetailDokumen(GetIdBerkas, model.DaftarId[i], fileName,);

                            responseBase.Message = "File Terpisah Sukses Diunggah";
                            responseBase.Status = StatusEnum.Success;
                            i++;
                        }

                    }
                }
                else
                {
                    throw new Exception("File Upload Tidak Boleh Kosong!");
                }
            }
            catch (Exception ex)
            {
                responseBase.Message = ex.Message;
                responseBase.Status = StatusEnum.Error;

                string innerEx = ex.InnerException?.ToString() ?? "";

                if (!string.IsNullOrEmpty(innerEx));
                    Models.Lib.errorAPI.LogErrorAsync("https://localhost:7232/api/ErrorLog", "SistemPemberkasan", innerEx, ex.StackTrace);
            }
            return Json(responseBase);
        }
        public IActionResult StatusChange(int IdBerkas, int Aksi)
        {
            ResponseBase responseBase = new ResponseBase();

            int IdBerkasInt = Convert.ToInt32(IdBerkas);
            //int ActionInt = Convert.ToInt32(Action);
            var checkBerkas = _context.Berkas.Where(x => x.IdBerkas == IdBerkasInt).FirstOrDefault();
            int StatusInt = Convert.ToInt32(checkBerkas.StatusBerkas);

            try
            {
                if (checkBerkas != null)
                {
                    //status berkas 1=draft , 2=Tersimpan ,0=Terhapus 
                    if (Aksi == 0)
                    {
                        StatusInt = 0;
                        Models.Lib.UploadFile.UpdateStatus(IdBerkasInt, StatusInt);
                        responseBase.Message = "File Sukses Dihapus";
                        responseBase.Status = StatusEnum.Success;
                    }
                    else if (Aksi == 1)
                    {
                        StatusInt = 1;
                        Models.Lib.UploadFile.UpdateStatus(IdBerkasInt, StatusInt);
                        responseBase.Message = "File Sukses Dikembalikan";

                        responseBase.Status = StatusEnum.Success;
                    }
                    else if (Aksi == 2)
                    {
                        StatusInt = 2;
                        Models.Lib.UploadFile.UpdateStatus(IdBerkasInt, StatusInt);
                        responseBase.Message = "File Sukses Tersimpan";
                        responseBase.Status = StatusEnum.Success;
                    }
                    else if (Aksi == 3)
                    {
                        StatusInt = 3;
                        Models.Lib.UploadFile.UpdateStatus(IdBerkasInt, StatusInt);
                        responseBase.Message = "File Sukses Tersimpan";
                        responseBase.Status = StatusEnum.Success;
                    }
                    else
                    {
                        throw new Exception("Operasi Gagal");
                    }
                }
                else
                {
                    throw new Exception("Data Tidak Ditemukan");
                }
            }
            catch (Exception ex)
            {
                responseBase.Message = ex.Message;
                responseBase.Status = StatusEnum.Error;
            }
            return Json(responseBase);
        }
        [HttpPost]
        public async Task<IActionResult> EditDraft(Models.Transaksi.DraftVM.EditDraft model)
        {
            ResponseBase responseBase = new ResponseBase();
            int IdUser = _cookieData.GetIdUser();

            //status berkas 1=draft , 2=Tersimpan 3= revisi 4=sudah revisi ,0=Terhapus 
            var CheckFiles = _context.DetailDokumen.Where(x => x.IdBerkas == model.NewRow.IdBerkas).ToList();

            List<string> ListNamaFile = new List<string>();
            List<int?> ListJenisFile = new List<int?>();
            foreach (var item in CheckFiles)
            {
                ListNamaFile.Add(item.NamaDetailDokumen);
                ListJenisFile.Add(item.IdDokumen);
            }
            //edit berkas dulu
            int idBerkas = Models.Lib.UploadFile.EditDraft(model, IdUser);
            try
            {
                if (model.files != null)
                {
                    int i = 0;
                    // Memeriksa ukuran file
                    const long fileSizeLimit = 2 * 1024 * 1024;
                    for (int z = 0; i < model.files.Count; z++)
                    {
                        if (model.files[z].Length > fileSizeLimit)
                        {
                            throw new Exception("File gagal di ubah Ukuran file tidak boleh melebihi 2 MB.");
                        }
                    }
                    //looping file
                    foreach (var file in model.files)
                    {
                        //simpan file ke database
                        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                        var fileName = ListNamaFile[0];

                        byte[] fileBloob;
                        using (var ms = new MemoryStream())
                        {
                            // Salin file ke MemoryStream
                            file.CopyTo(ms);

                            // Konversi stream ke array byte
                            fileBloob = ms.ToArray();
                        }

                        //saveFileDokumen
                        Models.Lib.UploadFile.EditDetailDokumen(model.NewRow.IdBerkas, ListJenisFile[i], fileName, fileBloob);

                        responseBase.Message = "File Sukses Di Edit";
                        responseBase.Status = StatusEnum.Success;
                        i++;
                    }
                }
                else
                {
                    Models.Lib.UploadFile.UpdateStatus(model.NewRow.IdBerkas, 4);
                    responseBase.Message = "File Sukses Di Edit tanpa merubah dokumen";
                    responseBase.Status = StatusEnum.Success;
                }
            }
            catch (Exception ex)
            {
                responseBase.Message = ex.Message;
                responseBase.Status = StatusEnum.Error;
            }
            return Json(responseBase);

        }
        [HttpGet]
        public IActionResult DownloadCompressedFile(string namaDetailDokumen)
        {
            var fileInfo = _context.DetailDokumen.FirstOrDefault(f => f.NamaDetailDokumen == namaDetailDokumen);

            var mimeType = Models.Lib.UploadFile.GetJenisFile(fileInfo.FileBloob);
            if (fileInfo == null)
            {
                return NotFound();
            }

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileInfo.NamaDetailDokumen,
                Inline = true,
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());

            return File(fileInfo.FileBloob, mimeType.ContentType);
        }


    }
}
