using Sistem_Pemberkasan.Models.EF;

namespace Sistem_Pemberkasan.Models.Lib
{
    public class TambahKategori
    {
        public static int SaveKategoriBerkas(Models.Master.BerkasVM.Add model, int IdBidang, int IdUser)
        {
            var context = new ModelContext();

            int id = 0;
            int? idBerkas = context.MKategoriBerkas.DefaultIfEmpty().Max(x => (int?)x.IdKategoriBerkas);
            if (idBerkas.HasValue)
            {
                id = idBerkas.Value + 1;
            }
            else
            {
                id = 1;
            }
            context.MKategoriBerkas.Add(new MKategoriBerka
            {
                IdKategoriBerkas = id,
                IdBidang = model.NewRow.IdBidang,
                JenisKategoriBerkas = model.newRow.JenisKategoriBerkas,
                StatusKategoriBerkas = 1,
                KbInsertBy = IdUser,
                KbInsertDate = DateTime.Now,
            });
            context.SaveChanges();

            return id;
        }
        public static void SaveFormDokumen(int IdKategoriBerkas, int IdDokumen, int IdUser)
        {
            var context = new ModelContext();

            int id = 0;
            int? idFormDokumen = context.MFormDokumen.DefaultIfEmpty().Max(x => (int?)x.IdFormDokumen);
            if (idFormDokumen.HasValue)
            {
                id = idFormDokumen.Value + 1;
            }
            else
            {
                id = 1;
            }
            context.MFormDokumen.Add(new MFormDokuman
            {
                IdFormDokumen = id,
                IdDokumen = IdDokumen,
                IdKategoriBerkas = IdKategoriBerkas,
                StatusFormDokumen = true,
                FdInsertBy = IdUser,
                FdInsertDate = DateTime.Now,
            });
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
            context.SaveChanges();

        }
    }
}
