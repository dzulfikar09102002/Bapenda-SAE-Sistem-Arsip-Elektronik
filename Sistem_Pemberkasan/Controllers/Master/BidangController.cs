using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sistem_Pemberkasan.Models;
using Sistem_Pemberkasan.Models.EF;
using Sistem_Pemberkasan.Models.Lib;

namespace Sistem_Pemberkasan.Controllers.Master
{
    [Authorize(Roles = "Pengelola")]

    public class BidangController : Controller
	{
		private readonly ModelContext _context;
		private readonly CookieData _cookieData;
		string strViewPath = string.Empty;
        
        public BidangController(ModelContext context , IHttpContextAccessor httpContextAccessor)
        {
            _cookieData = new CookieData(context , httpContextAccessor);
			_context = context;
			strViewPath = string.Concat("../Master/", GetType().Name.Replace("Controller", ""), "/");
		}
        public IActionResult Index()
		{
            var model = new Models.Master.BidangVM.Index(_context);
			return View(strViewPath + "Index" , model);
		}
       
        public IActionResult Add()
		{
			return PartialView(strViewPath + "_Add");
		}
   
        [HttpPost]
        public IActionResult StoreAdd(Models.Master.BidangVM.Add model)
        {
            int insertBy = _cookieData.GetIdUser();
            var session = HttpContext.Session.GetString("Role");
            ResponseBase response = new() { Status = StatusEnum.Success };
            try
            {
                if (string.IsNullOrEmpty(model.NewRow.NamaBidang))
                {
                    response.Status = StatusEnum.Error;
                    response.Message = "Kolom Harus Di isi";
                    return Json(response);
                }

                int id = 0;
                int? maxId = _context.MBidangs.Max(x => (int?)x.IdBidang);

                if (maxId.HasValue)
                {
                    id = maxId.Value + 1;
                }
                else
                {
                    id = 1;
                }

                _context.MBidangs.Add(new MBidang
                {
                    IdBidang = id,
                    NamaBidang = model.NewRow.NamaBidang,
                    Singkatan = model.NewRow.Singkatan,
                    StatusBidang = 1,
                    BInsertBy = insertBy,
                    BInsertDate = DateTime.Now
                });

                _context.SaveChanges();
                response.Message = "Data Berhasil Ditambahkan";
            }
            catch (Exception ex)
            {
                response.Status = StatusEnum.Error;
                response.Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            }

            return Json(response);
        }

        public IActionResult Edit(int idBidang)
		{
            try
            {
				Models.Master.BidangVM.Edit model;
				TempData["TEMP_EDIT"] = idBidang;
				model = new Models.Master.BidangVM.Edit(_context, idBidang);
				return PartialView(strViewPath + "_Edit" , model);
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
        public IActionResult StoreEdit(Models.Master.BidangVM.Edit model)
        {
            int insertBy = _cookieData.GetIdUser();

			ResponseBase response = new ResponseBase { Status = StatusEnum.Success };
            try
            {
                var idKey = Convert.ToInt32(TempData["TEMP_EDIT"]);
                var checkData = _context.MBidangs.FirstOrDefault(x => x.IdBidang == idKey);
                if (checkData != null)
                {
                    checkData.NamaBidang = model.BidangRow.NamaBidang;
                    checkData.Singkatan = model.BidangRow.Singkatan;
                    checkData.StatusBidang = model.BidangRow.StatusBidang;
                    checkData.BInsertBy = insertBy;


					_context.SaveChanges();
                    response.Message = "Data berhasil diubah";
                }
                else
                {
                    response.Status = StatusEnum.Error;
                    response.Message = "Data tidak ditemukan";
                }
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
