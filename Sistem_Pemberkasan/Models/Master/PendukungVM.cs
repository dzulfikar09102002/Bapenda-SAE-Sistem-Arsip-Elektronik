

using Sistem_Pemberkasan.Models.EF;
using System.Security.Cryptography.X509Certificates;

namespace Sistem_Pemberkasan.Models.Master
{
    public class PendukungVM
    {
        public class Index
        {
            private readonly ModelContext _context;
            public List<MKategoriPendukungBerka> KategoriPendukungBerkasList { get; set; } = new List<MKategoriPendukungBerka>();

            public Index(ModelContext context)
            {
                _context = context;
                KategoriPendukungBerkasList = context.MKategoriPendukungBerkas.ToList();
                if (KategoriPendukungBerkasList.Count > 0)
                {
                    KategoriPendukungBerkasList = KategoriPendukungBerkasList;
                }
            }
        }
        public class Add
        {
            public MKategoriPendukungBerka NewRowBerka { get; set; } = new();
            public Add()
            {

            }
        }
        public class Edit
        {
            private readonly ModelContext _context;
            public MKategoriPendukungBerka KategoriPendukungRow {  get; set; } = new();
            public Edit(ModelContext context, int id)
            {
                _context = context;
                KategoriPendukungRow = context.MKategoriPendukungBerkas.FirstOrDefault(x => x.IdKategoriPendukungBerkas == id);
            }
            public Edit()
            {

            }
        }

    }
}
