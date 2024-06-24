using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sistem_Pemberkasan.Models;
using Sistem_Pemberkasan.Models.EF;
﻿using Microsoft.AspNetCore.Authorization;
using Sistem_Pemberkasan.Models.Lib;

namespace Sistem_Pemberkasan.Controllers.Master
{

    [Authorize(Roles = "Pengelola")]

    public class PegawaiController : Controller
    {
		private readonly ModelContext _context;
		private readonly CookieData _cookieData;
		string strViewPath = string.Empty;
		public PegawaiController(ModelContext context , IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
			_cookieData = new CookieData(context , httpContextAccessor);
            strViewPath = string.Concat("../Master/",GetType().Name.Replace("Controller",""),"/");
        }
        public IActionResult Index()
        {
            var model = new Models.Master.PegawaiVM.Index(_context);
            return View(strViewPath + "Index" , model);
        }
        public IActionResult Add()
        {
			var model = new Models.Master.PegawaiVM.Add(_context);
			return PartialView(strViewPath + "_Add" , model);
        }


        [HttpPost]
        public IActionResult StoreAdd(Models.Master.PegawaiVM.Add model)

        {
			int insertBy = _cookieData.GetIdUser();
			ResponseBase response = new() { Status = StatusEnum.Success };
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					if (string.IsNullOrEmpty(model.NewPegawaiRow.NamaPegawai) ||
						string.IsNullOrEmpty(model.NewPegawaiRow.Nik) ||
						model.NewPegawaiRow.IdBidang == 0 ||
						string.IsNullOrEmpty(model.NewUserRow.Email) ||
						string.IsNullOrEmpty(model.NewUserRow.Password))
					{
						response.Status = StatusEnum.Error;
						response.Message = "Kolom Harus Di isi";
						return Json(response);
					}
					// Lakukan operasi penyimpanan data di dalam transaksi
					_context.MPegawais.Add(new MPegawai
					{
						Nik = model.NewPegawaiRow.Nik,
						IdBidang = model.NewPegawaiRow.IdBidang,
						NamaPegawai = model.NewPegawaiRow.NamaPegawai,
						JenisKelamin = model.NewPegawaiRow.JenisKelamin,
						NoTelepon = model.NewPegawaiRow.NoTelepon,
						StatusPegawai = 1,
						PInsertBy = insertBy,
						PInsertDate = DateTime.Now
					});

					// Menyimpan MPegawai baru
					_context.SaveChanges();

					int idUser = 0;
					int? maxIdUser = _context.MUsers.Max(x => (int?)x.IdUser);
					if (maxIdUser.HasValue)
					{
						idUser = maxIdUser.Value + 1;
					}
					else
					{
						idUser = 1;
					}
                    string hashedPassword = new PasswordHasher<MUser>().HashPassword(null, model.NewUserRow.Password);
                    _context.MUsers.Add(new MUser
					{
						IdUser = idUser,
						IdRole = model.NewUserRow.IdRole,
						Nik = model.NewPegawaiRow.Nik,
						Email = model.NewUserRow.Email,
						Password = hashedPassword,
						StatusUser = 1,
						UInsertBy = insertBy,
						UInsertDate = DateTime.Now
					});

					// Menyimpan MUser baru
					_context.SaveChanges();

					// Jika semua operasi berhasil, commit transaksi
					transaction.Commit();
					response.Message = "Data Berhasil Ditambahkan";
				}
				catch (Exception ex)
				{
					// Jika terjadi exception, rollback transaksi
					transaction.Rollback();
					response.Status = StatusEnum.Error;
					response.Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
				}
			}
			return Json(response);
		}

        public IActionResult Edit(String Nik)
        {
			try
			{
				Models.Master.PegawaiVM.Edit model;
				model = new Models.Master.PegawaiVM.Edit(_context, Nik);
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
		public IActionResult StoreEdit(Models.Master.PegawaiVM.Edit model)
		{
			int insertBy = _cookieData.GetIdUser();
			ResponseBase response = new ResponseBase { Status = StatusEnum.Success };

			try
			{
				var idkey = model.EditPegawaiRow.Nik;
				var checkData = _context.MPegawais.FirstOrDefault(x => x.Nik == idkey);
				var checkData2 = _context.MUsers.FirstOrDefault(x => x.Nik == idkey);

				if (checkData != null)
				{
					checkData.NamaPegawai = model.EditPegawaiRow.NamaPegawai;
					checkData.IdBidang = model.EditPegawaiRow.IdBidang;
					checkData.JenisKelamin = model.EditPegawaiRow.JenisKelamin;
					checkData.NoTelepon = model.EditPegawaiRow.NoTelepon;
					checkData.StatusPegawai = model.EditPegawaiRow.StatusPegawai;
					checkData.PInsertBy = insertBy;
					checkData.PInsertDate = DateTime.Now;
				}

				if (checkData2 != null)
				{
					checkData2.IdRole = model.EditUserRow.IdRole;
					checkData2.Email = model.EditUserRow.Email;
					checkData2.StatusUser = model.EditPegawaiRow.StatusPegawai;
					checkData2.UInsertBy = insertBy;
					checkData2.UInsertDate = DateTime.Now;
					
					if(model.OldPassword != null && model.NewPassword != null)
					{
						var passwordHasher = new PasswordHasher<MUser>();
						var passwordVerification = passwordHasher.VerifyHashedPassword(checkData2, checkData2.Password, model.OldPassword);
						if (passwordVerification == PasswordVerificationResult.Success)
						{
							checkData2.Password = passwordHasher.HashPassword(checkData2, model.NewPassword);
							_context.SaveChanges(); 
						}
						else
						{
							response.Status = StatusEnum.Error;
							response.Message = "Password yang dimasukkan tidak sesuai";
							return Json(response);
						}
					}
					
				}

				if (checkData != null && checkData2 != null)
				{
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
			}

			return Json(response);
		}


	}
}
