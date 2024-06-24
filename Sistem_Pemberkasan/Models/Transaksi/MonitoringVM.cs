using Microsoft.EntityFrameworkCore;
using Sistem_Pemberkasan.Models.EF;
using Sistem_Pemberkasan.Models.Lib;
using Sistem_Pemberkasan.Models.Master;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Sistem_Pemberkasan.Models.Transaksi
{
    public class MonitoringVM
    {
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

            public class BerkasPendukungViewModel
            {
                public Berka? Berkas { get; set; }
                public PendukungBerka? PendukungBerkas { get; set; }
            }

            public List<BerkasPendukungViewModel>? BerkasListCoba { get; set; } = new List<BerkasPendukungViewModel>();

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
                var berkasList = context.Berkas.Include(x => x.IdKategoriBerkasNavigation).ThenInclude(x => x.IdBidangNavigation).OrderByDescending(x => x.TanggalUnggah).Where(x => x.StatusBerkas == 2).OrderByDescending(c => c.TanggalUnggah).ToList();

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
            public Index(ModelContext context, int idUser, int idKategori, int idBidang, DateTime? tanggalMulai, DateTime? tanggalAkhir)
            {

                var berkasList = context.Berkas.Include(x => x.PendukungBerkas).ThenInclude(x => x.IdKategoriPendukungBerkasNavigation).Include(x => x.IdKategoriBerkasNavigation).ThenInclude(x => x.IdBidangNavigation).OrderByDescending(x => x.TanggalUnggah).Where(x => x.StatusBerkas == 2).OrderByDescending(c => c.TanggalUnggah).ToList();

                if (idBidang != 0)
                {
                    berkasList = berkasList.Where(d => d.IdKategoriBerkasNavigation.IdBidang == idBidang).ToList();
                }
                if (idKategori != 0)
                {
                    berkasList = berkasList.Where(d => d.IdKategoriBerkas == idKategori).ToList();
                }
                if (tanggalMulai != null && tanggalAkhir != null)
                {
                    berkasList = berkasList.Where(d => d.TanggalUnggah >= tanggalMulai && d.TanggalUnggah <= tanggalAkhir).ToList();
                }
                User = context.MUsers.Include(x => x.NikNavigation).ThenInclude(x => x.IdBidangNavigation).FirstOrDefault(x => x.IdUser == idUser);
                BerkasList = berkasList;
            }
            public Index(ModelContext context, string searchBox)
            {

                var berkasList = context.Berkas.Include(x => x.IdKategoriBerkasNavigation).ThenInclude(x => x.IdBidangNavigation).Where(x => x.StatusBerkas == 2).OrderByDescending(c => c.TanggalUnggah).ToList();

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

        public class Detail
        {
            public List<Berka> BerkasOne { get; set; } = new List<Berka>();
            public List<MFormDokuman> NamaDokumenList { get; set; }
            public List<DetailDokuman> ListDokumen { get; set; }
            public List<PendukungBerka> ListPendukung { get; set; }
            public MUser UserBidang { get; set; } = new MUser();

            public Detail(ModelContext context, int id, int idKategori,int idUser)
            {
                var DetailData = context.Berkas.Include(x => x.IdKategoriBerkasNavigation).ThenInclude(x => x.IdBidangNavigation).Where(x => x.IdBerkas == id).ToList();
                if (DetailData != null)
                {
                    BerkasOne = DetailData;
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
                var keterangan = context.PendukungBerkas.Include(x => x.IdKategoriPendukungBerkasNavigation).Where(x => x.IdBerkas == id).Where(x=>x.StatusPendukungBerkas == 1).ToList(); 
                if(keterangan != null)
                {
                    ListPendukung = keterangan;
                }
                UserBidang = context.MUsers.Include(x => x.NikNavigation).ThenInclude(x => x.IdBidangNavigation).FirstOrDefault(x => x.IdUser == idUser);
            }

        }

        public class addPendukung
        {
            public List<Berka> BerkasList { get; set; } = new List<Berka>();
            public Berka berkasId { get; set; } = new();
            public MKategoriPendukungBerka NewPendukung { get; set; } = new();
            public List<MKategoriPendukungBerka> KategoriList { get; set; }
            public addPendukung(ModelContext context, int id)
            {
                var berkas = context.Berkas.Include(x => x.IdKategoriBerkasNavigation).Where(x => x.IdBerkas == id).ToList();
                var berkasID = context.Berkas.Include(x => x.IdKategoriBerkasNavigation).Where(x => x.IdBerkas == id).FirstOrDefault();
                if (berkas != null)
                {
                    BerkasList = berkas;
                    berkasId = berkasID;

                }

                var pendukung = context.MKategoriPendukungBerkas.Where(x => x.StatusKategoriPendukung == 1 ).ToList();  
                if (pendukung != null)
                {
                    KategoriList = pendukung;  
                }

            }

            public IFormFile file { get; set; }
            [Required]
            public PendukungBerka NewRowPendukung { get; set; } = new PendukungBerka();
            public addPendukung()
            {

            }
        }

        public class editPendukung
        {
            public List<PendukungBerka> ListPendukung { get; set; } = new List<PendukungBerka>();
            public editPendukung(ModelContext context, int id)
            {
                var Pendukung = context.PendukungBerkas.Include(x => x.IdKategoriPendukungBerkasNavigation).Where(x=>x.IdBerkas==id).ToList();
                if(Pendukung != null)
                {
                    ListPendukung = Pendukung;
                }
            }
            public IFormFile file { get; set; }
            public editPendukung()
            {

            }
        }

        public class SaveEditPendukung
        {
            public PendukungBerka? NewRow { get; set; } = new PendukungBerka();
            public IFormFile file { get; set; }
            public SaveEditPendukung()
            {

            }
        }

        public class filter
        {
            public filter()
            {

            }
        }
    }
}
