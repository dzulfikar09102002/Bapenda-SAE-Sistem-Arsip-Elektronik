using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using PenagihanPADWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Sistem_Pemberkasan.Models.EF;
using Sistem_Pemberkasan.Models.Lib;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sistem_Pemberkasan.Controllers
{
    [Authorize(Roles = "Pengelola,Perekam,User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        private readonly CookieData _cookieData;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public HomeController(ILogger<HomeController> logger, ModelContext context, IHttpContextAccessor httpContextAccessor)
        {

            _logger = logger;
            _context = context;
            _cookieData = new CookieData(context, httpContextAccessor);
           
        }
     
        [Authorize]
        public IActionResult Index()
        {

			string EmailUser = _cookieData.GetUser();

            int selectedYear = HttpContext.Session.GetInt32("SelectedYear") ?? DateTime.Now.Year;
            var model = new Models.HomeVM.Index(_context , selectedYear);
		
			return View( model);
        }
        [HttpPost]
        public IActionResult UpdateSelectedYear(int selectedYear)
        {
            string EmailUser = _cookieData.GetUser();
            HttpContext.Session.SetInt32("SelectedYear", selectedYear);
            var model = new Models.HomeVM.Index(_context, selectedYear)
            {
                SelectedYear = selectedYear
            };
            return View("Index", model);
        }
        [HttpGet]
        public IActionResult GetChartData()
        {
            int selectedYear = HttpContext.Session.GetInt32("SelectedYear") ?? DateTime.Now.Year;
            var data = new List<int>();
			// Loop untuk mengambil data untuk setiap bulan dalam setahun
			for (int bulan = 1; bulan <= 12; bulan++)
			{
				// Filter data dokumen berdasarkan bulan dan tahun saat ini
				var dokumenBulan = _context.Berkas.Where(d => d.TanggalUnggah.Value.Month == bulan && d.TanggalUnggah.Value.Year == selectedYear && d.StatusBerkas !=0 ).ToList();

				// Hitung jumlah dokumen untuk bulan ini
				int jumlahDokumen = dokumenBulan.Count();

				// Tambahkan jumlah dokumen ke list chartData
				data.Add(jumlahDokumen);
			}
			var categories = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var chartData = new
            {
                categories = categories,
                series = new[] { new { name = "Dokumen Terupload", data = data } }
            };
            return Content(JsonConvert.SerializeObject(chartData), "application/json");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}