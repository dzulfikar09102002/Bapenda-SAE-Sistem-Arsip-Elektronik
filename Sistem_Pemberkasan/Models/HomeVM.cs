using Sistem_Pemberkasan.Models.EF;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Sistem_Pemberkasan.Models
{
    public class HomeVM
    {
        public class Index
        {
            public List<MBidang> BidangList { get; set; } = new List<MBidang>();
            public int SelectedYear { get; set; }
            public int TotalDraft { get; set; }
            public int TotalTersimpan { get; set; }
            public int TotalSudahDirevisi { get; set; }
            public int TotalBelumDirevisi { get; set; }
            public int Total { get; set; }
            public MUser User { get; set; } = new MUser();
            public int Tahun { get; set; }

            public Index(ModelContext context , int? selectedYear)
            {
                SelectedYear = selectedYear ?? DateTime.Now.Year;

                var totalDraft = context.Berkas.Where(x => x.StatusBerkas == 1 && x.TanggalUnggah.Value.Year == SelectedYear).ToList();
                    TotalDraft = totalDraft.Count;

                var totalTersimpan = context.Berkas.Where(x => x.StatusBerkas == 2 && x.TanggalUnggah.Value.Year == SelectedYear).ToList();
                    TotalTersimpan = totalTersimpan.Count;    

                var totalBelumDirevisi = context.Berkas.Where(x => x.StatusBerkas == 3 && x.TanggalUnggah.Value.Year == SelectedYear).ToList();
                    TotalBelumDirevisi = totalBelumDirevisi.Count;

                var totalSudahDirevisi = context.Berkas.Where(x => x.StatusBerkas == 4 && x.TanggalUnggah.Value.Year == SelectedYear).ToList();
                    TotalSudahDirevisi = totalSudahDirevisi.Count;

                var total = TotalDraft + TotalTersimpan + TotalSudahDirevisi + TotalBelumDirevisi;
                Total = total;

               var bidangList = context.MBidangs
                    .Include(x => x.MKategoriBerkas)    
                    .ThenInclude(x => x.Berkas.Where(x => x.TanggalUnggah.Value.Year == SelectedYear))
                    .ToList();
                BidangList = bidangList;

            }
        }
    }
}

