using Microsoft.AspNetCore.Mvc;
using Sistem_Pemberkasan.Models;
using Microsoft.AspNetCore.Authorization;
using Sistem_Pemberkasan.Models.EF;
using Sistem_Pemberkasan.Models.Lib;

namespace Sistem_Pemberkasan.Controllers.Master
{
    [Authorize(Roles = "Pengelola")]

    public class KelolaBerkasController : Controller
    {
        private readonly ModelContext _context;
        string strViewPath = string.Empty;
		private readonly CookieData _cookieData;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public KelolaBerkasController(ModelContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            strViewPath = string.Concat("../Master/", GetType().Name.Replace("Controller", ""), "/");
			_cookieData = new CookieData(context, httpContextAccessor);

		}
        public IActionResult Index()
        {

			string EmailUser = _cookieData.GetUser();
			var model = new Models.Master.BerkasVM.Index(_context, EmailUser);

            return View(strViewPath + "Index", model);
        }
        public IActionResult Add()
        {
            //var model = new Models.Transaksi.BerkasVM.ModalBox(_context);
            var model = new Models.Master.BerkasVM.Add(_context);
            return PartialView(strViewPath + "_Add", model);
        }
        //Detail Dokumen
        public IActionResult DetailDokumen(string IdKategoriBerkas)
        {
            var model = new Models.Master.BerkasVM.Detail(_context, Convert.ToInt32(IdKategoriBerkas));
            return PartialView(strViewPath + "_DetailDokumen", model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveKategoriBerkas(Models.Master.BerkasVM.Add model)
        {

            ResponseBase responseBase = new ResponseBase();
            
            int IdUser = _cookieData.GetIdUser();

			//status berkas 1=draft , 2=Tersimpan ,0=Terhapus 
			int IdBidang = Models.Lib.User.GetUserBidang(IdUser);
            try
            {
                if (model.daftarIDSelect != null)
                {
                    int idKategoriBerkas = Models.Lib.TambahKategori.SaveKategoriBerkas(model, IdBidang, IdUser);

                    foreach (var item in model.daftarIDSelect)
                    {
                        //Models.Lib.TambahKategori.SaveFormDokumen(idKategoriBerkas, IdDokumen, IdUser);
                        Models.Lib.TambahKategori.SaveFormDokumen(idKategoriBerkas, item.Value, IdUser);
                    }
                    responseBase.Message = "Data Sukses Ditambahkan";
                    responseBase.Status = StatusEnum.Success;
                }
                else
                {
                    throw new Exception("Data Gagal Ditambahkan");
                }
            }
            catch (Exception ex)
            {
                responseBase.Message = ex.Message;
                responseBase.Status = StatusEnum.Error;
            }
            return Json(responseBase);
        }
		public IActionResult Edit(int id)
		{
			try
			{
				//var idkey = Convert.ToInt32(id.DecryptString<SurabayaTaxLib.Lib.DataPegawai.Bidang>());
				Models.Master.BerkasVM.Edit model;

				TempData["TEMP_EDIT"] = id;
				model = new Models.Master.BerkasVM.Edit(_context, id);
				return PartialView(strViewPath + "_Edit", model);
			}
			catch (Exception ex)
			{
				ResponseBase response = new();
				response.Status = StatusEnum.Error;
				response.Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
				return Json(response);
			}

		}
		[HttpPost]
		public IActionResult Update(Models.Master.BerkasVM.Edit model)
		{
			ResponseBase response = new() { Status = StatusEnum.Success };
			int IdUser = _cookieData.GetIdUser();
			try
			{
                int IdBidang = model.BerkasRow.IdBidang.Value;
                int idKategoriBerkas = model.BerkasRow.IdKategoriBerkas;

				for (int i = 0; i < model.ListFormDokumen.Count; i++)
				{
					var formDokumen = model.ListFormDokumen[i];
					var checkData = _context.MFormDokumen.FirstOrDefault(x => x.IdFormDokumen == formDokumen.IdFormDokumen);
					if (checkData != null)
					{
                        var Status = formDokumen.StatusFormDokumen;
                        if (formDokumen.StatusFormDokumen == null)
                        {
                             Status = true;
                        }
						checkData.StatusFormDokumen = Status;
					}
				}
                var checkKategoriBerkas = _context.MKategoriBerkas.FirstOrDefault(x => x.IdKategoriBerkas == idKategoriBerkas);
                if (checkKategoriBerkas != null) 
                {
                    checkKategoriBerkas.IdBidang = IdBidang;
                    checkKategoriBerkas.JenisKategoriBerkas = model.BerkasRow.JenisKategoriBerkas;
                    checkKategoriBerkas.StatusKategoriBerkas = model.BerkasRow.StatusKategoriBerkas;
                }
				_context.SaveChanges();

                if (model.daftarIDSelect != null) 
                {
                    foreach (var item in model.daftarIDSelect)
                    {
                        //Models.Lib.TambahKategori.SaveFormDokumen(idKategoriBerkas, IdDokumen, IdUser);
                        Models.Lib.TambahKategori.SaveFormDokumen(idKategoriBerkas, item.Value, IdUser);
                    }

                }
                response.Message = "Data Sukses Di Edit";
                response.Status = StatusEnum.Success;
            }
			catch (Exception ex)
			{
				response.Status = StatusEnum.Error;
				response.Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
				TempData.Keep("TEMP_EDIT");
			}

			return Json(response);
		}





	}
}
