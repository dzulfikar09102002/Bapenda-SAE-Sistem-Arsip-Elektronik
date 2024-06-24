using Microsoft.EntityFrameworkCore;
using Sistem_Pemberkasan.Models.EF;
using Sistem_Pemberkasan.Models.Lib;

namespace Sistem_Pemberkasan.Models.Master
{
	public class BerkasVM
	{
		public class Index
		{
			private readonly ModelContext _context;
			public List<MKategoriBerka> BerkasList { get; set; } = new List<MKategoriBerka>();
			public MUser User { get; set; } = new MUser();
			public Index(ModelContext context, string session)
			{
				_context = context;
				BerkasList = context.MKategoriBerkas.Include(x => x.IdBidangNavigation)
					.Include(x => x.MFormDokumen.Where(p => p.StatusFormDokumen == true))
						.ThenInclude(x => x.IdDokumenNavigation)
					.ToList();
				if (BerkasList.Count > 0)
				{
					BerkasList = BerkasList;
				}
				var userNow = context.MUsers.Where(x => x.Email == session).FirstOrDefault();
				if (userNow != null)
				{
					User = userNow;
				}
			}

		}
		public class Add
		{
			public MBidang NewRow { get; set; } = new();
			public MKategoriBerka newRow { get; set; } = new();

			public List<MBidang> ListBidang { get; set; }
			public List<MDokuman> ListDokumen { get; set; }
			public int?[] daftarIDSelect { get; set; }

			public Add(ModelContext context)
			{
				var bidangList = context.MBidangs.Where(x => x.StatusBidang == 1).ToList();
				if (bidangList != null)
				{
					ListBidang = bidangList;
				}

				var daftarId = new List<int?>();
				var dokumenList = context.MDokumen.ToList();
				if (dokumenList != null)
				{
					ListDokumen = dokumenList; ListDokumen = dokumenList;

				}

			}
			public Add()
			{

			}
		}

		public class Edit
		{
			public MKategoriBerka BerkasRow { get; set; } = new();
			public List<MBidang> ListBidang { get; set; }
			public List<MDokuman> ListDokumen { get; set; }
			public List<MFormDokuman> ListFormDokumen { get; set; }
			public int?[] daftarIDSelect { get; set; }

			public Edit(ModelContext context, int id)
			{
				var bidangList = context.MBidangs.Where(x => x.StatusBidang == 1).ToList();

				if (bidangList != null)
				{
					ListBidang = bidangList;
				}

				var listFormDokumen = context.MFormDokumen
					.Include(x => x.IdDokumenNavigation)
					.Where(x => x.IdKategoriBerkas == id)
					.ToList();

				if (listFormDokumen != null)
				{
					ListFormDokumen = listFormDokumen;
				}

				var dokumenIds = listFormDokumen.Select(formDokumen => formDokumen.IdDokumen);
				var dokumenList = context.MDokumen
					.Where(dokumen => !dokumenIds.Contains(dokumen.IdDokumen))
					.ToList();

				if (dokumenList != null)
				{
					ListDokumen = dokumenList;
				}

				BerkasRow = context.MKategoriBerkas.FirstOrDefault(x => x.IdKategoriBerkas == id);
			}


			public Edit()
			{

			}
		}


		public class Detail
		{
			public List<MFormDokuman> BerkasList { get; set; } = new List<MFormDokuman>();
			public Detail(ModelContext context, int idKategori)
			{
				var DetailData = context.MFormDokumen.Include(x => x.IdKategoriBerkasNavigation).Include(x => x.IdDokumenNavigation).Where(x => x.IdKategoriBerkas == idKategori).ToList();
				if (DetailData != null)
				{
					BerkasList = DetailData;
				}

			}

		}
	}
}
