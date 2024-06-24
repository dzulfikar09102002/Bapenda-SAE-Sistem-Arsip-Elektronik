using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Sistem_Pemberkasan.Models.EF;
using Sistem_Pemberkasan.Models.Lib;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Sistem_Pemberkasan.Models.Transaksi.MonitoringVM;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sistem_Pemberkasan.Models.Transaksi
{
    public class DraftVM
    {
        //Index VM

        public class Index
        {
            public List<Berka> BerkasList { get; set; } = new List<Berka>();
            public List<Berka> BerkasListNofilter { get; set; } = new List<Berka>();
            public MUser User { get; set; } = new MUser();
            public List<MKategoriBerka> FilterKategori { get; set; } = new List<MKategoriBerka>();
            public List<Berka> FilterBerkas { get; set; } = new List<Berka>();
            public List<MBidang> FilterBidang { get; set; } = new List<MBidang>();
            public DateTime? tanggalMulai { get; set; }
            public DateTime? tanggalAkhir { get; set; }
            public int IdBidang { get; set; }
            public string SearchBox { get; set; }
            public int IdKategori { get; set; }
            public int StatusBerkas { get; set; }
            public Index()
            {

            }
            public Index(ModelContext context, int idUser)
            {
                //var berkasList = context.Berkas
                //    .Include(x => x.IdKategoriBerkasNavigation)
                //    .ThenInclude(x => x.IdBidangNavigation)
                //    .ToList();
                var userBidang = context.MUsers.Include(x => x.NikNavigation).Where(x => x.IdUser == idUser).FirstOrDefault();
                var berkasList = context.Berkas.Include(x => x.IdKategoriBerkasNavigation).ThenInclude(x => x.IdBidangNavigation).OrderByDescending(x => x.TanggalUnggah).ToList();
                if (berkasList != null)
                {
                    BerkasListNofilter = berkasList;
                }
                var filterKategori = context.MKategoriBerkas.ToList();
                if (FilterKategori != null)
                {
                    FilterKategori = filterKategori;
                }
                var filterStatus = context.Berkas.ToList();
                if (FilterKategori != null)
                {
                    FilterBerkas = filterStatus;
                }
                var filterBidang = context.MBidangs.ToList();
                if (filterBidang != null)
                {
                    FilterBidang = filterBidang;
                }
            }
            public Index(ModelContext context, int idUser, int idKategori, int idBidang, DateTime? tanggalMulai, DateTime? tanggalAkhir, int status)
            {

                var berkasList = context.Berkas.Include(x => x.IdKategoriBerkasNavigation).ThenInclude(x => x.IdBidangNavigation).OrderByDescending(c => c.TanggalUnggah).ToList();

                if (idBidang != 0)
                {
                    berkasList = berkasList.Where(d => d.IdKategoriBerkasNavigation.IdBidang == idBidang).OrderBy(c => c.TanggalUnggah).ToList();
                }
                if (idKategori != 0)
                {
                    berkasList = berkasList.Where(d => d.IdKategoriBerkas == idKategori).OrderBy(c => c.TanggalUnggah).ToList();
                }
                if (status != 5)
                {
                    berkasList = berkasList.Where(d => d.StatusBerkas == status).OrderBy(c => c.TanggalUnggah).ToList();
                }
                if (tanggalMulai != null && tanggalAkhir != null)
                {
                    berkasList = berkasList.Where(d => d.TanggalUnggah >= tanggalMulai && d.TanggalUnggah <= tanggalAkhir).OrderBy(c => c.TanggalUnggah).ToList();
                }
              
                BerkasList = berkasList;
            }
            public Index(ModelContext context, string searchBox)
            {

                var berkasList = context.Berkas.Include(x => x.IdKategoriBerkasNavigation).ThenInclude(x => x.IdBidangNavigation).OrderByDescending(c => c.TanggalUnggah).ToList();

                if (searchBox != null)
                {
                    berkasList = berkasList.Where(item =>
                        (
                            (item.NamaPemohon ?? "").ToLower().Contains(searchBox.ToLower()) ||
                            (item.Nop ?? "").ToString().Contains(searchBox) ||
                            (item.NoReferensi ?? "").ToString().Contains(searchBox))
                        )
                    .ToList();
                    BerkasList = berkasList;
                }
                else
                {
                    BerkasList = berkasList;
                }
            }
        }

        //Detail VM
        public class Detail
        {
            public List<Berka> BerkasOne { get; set; } = new List<Berka>();
            public List<MFormDokuman> NamaDokumenList { get; set; }
            public List<DetailDokuman> ListDokumen { get; set; }
            public MUser UserBidang { get; set; } = new MUser();
            public List<IFormFile>? files { get; set; }
            [Required]
            public Berka NewRow { get; set; } = new ();
            public Detail(ModelContext context, int id, int idKategori , int idUser)
            {
                var DetailData = context.Berkas.Include(x => x.IdKategoriBerkasNavigation).ThenInclude(x => x.IdBidangNavigation).Where(x => x.IdBerkas == id).OrderByDescending(c => c.TanggalUnggah).ToList();
             
                if (DetailData != null)
                {
                    BerkasOne = DetailData;
                    NewRow = context.Berkas.Include(x => x.IdKategoriBerkasNavigation).Where(x => x.IdBerkas == id).OrderByDescending(c => c.TanggalUnggah).FirstOrDefault();
                }
                var dokumen = context.DetailDokumen.Include(x => x.IdBerkasNavigation).Include(x => x.IdDokumenNavigation).Where(x => x.IdBerkas == id).ToList();
                if (dokumen != null)
                {
                    ListDokumen = dokumen;
                }
                var FormFile = context.MFormDokumen.Include(x => x.IdDokumenNavigation).Where(x => x.IdKategoriBerkas == idKategori).ToList();
                if (FormFile != null)
                {
                    NamaDokumenList = FormFile;
                }
                UserBidang = context.MUsers.Include(x => x.NikNavigation).ThenInclude(x => x.IdBidangNavigation).FirstOrDefault(x => x.IdUser == idUser);
            }
            public Detail()
            {

            }

        }
        //Form Upload VM
        public class ModalBox
        {
            public Berka NewRow { get; set; } = new();
            public List<MKategoriBerka> ListKategori { get; set; }
            public List<MDokuman> DokumenListCoba { get; set; }
            public List<MFormDokuman> DokumenList { get; set; }
            public int?[] DaftarId { get; set; }

            public ModalBox(ModelContext context, string idBidengUserNow)
            {

                int bidangUserNow = Convert.ToInt32(idBidengUserNow);
                var kategoriList = context.MKategoriBerkas.Where(x => x.StatusKategoriBerkas == 1).Where(x => x.IdBidang == bidangUserNow).ToList();
                if (kategoriList != null)
                {
                    ListKategori = kategoriList;
                }
            }
            public ModalBox(ModelContext context, int id)
            {
                var FormFile = context.MFormDokumen
                    .Include(x => x.IdDokumenNavigation)
                    .Where(x => x.IdKategoriBerkas == Convert.ToInt32(id)).Where(x => x.StatusFormDokumen == true)
                    .ToList();


                var daftarId = new List<int?>();
                if (FormFile != null)
                {
                    daftarId.Add(4);
                    DokumenList = FormFile;
                    foreach (var item in FormFile)
                    {
                        if (item.IdDokumen != 4)
                        {
                            daftarId.Add(item.IdDokumen);
                        }
                    }
                }

                DaftarId = daftarId.ToArray();

            }
        }
        //File Form
        public class SaveDraft
        {
            public MFormDokuman IdFormDokumenList { get; set; } = new MFormDokuman();
            public List<int> DaftarId { get; set; } = new List<int>();
            public List<IFormFile> files { get; set; }
            [Required]
            public Berka NewRow { get; set; } = new Berka();
            public SaveDraft()
            {

            }
        }
        public class EditDraft
        {
            public MFormDokuman IdFormDokumenList { get; set; } = new MFormDokuman();
            public List<IFormFile> files { get; set; }
            public Berka? NewRow { get; set; } = new Berka();
            public EditDraft()
            {

            }

        }



    }
}
