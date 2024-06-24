using Microsoft.AspNetCore.Mvc;
using Sistem_Pemberkasan.Models.EF;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistem_Pemberkasan.Models.Master
{
	public class BidangVM
	{
		public class Index
		{
            //private readonly ModelContext _context;

            public List<MBidang> BidangList { get; set; } = new List<MBidang>();

            public Index(ModelContext context)
            {
                BidangList = context.MBidangs.ToList() ?? new List<MBidang>(); 
            }

		}

		public class Add
		{
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int id_bidang { get; set; }
            public MBidang NewRow { get; set; } = new();
            public Add()
            {
                
            }
        }

		public class Edit
		{
			private readonly ModelContext _context;
			public MBidang BidangRow { get; set; } = new();
            public Edit(ModelContext context , int idBidang)
            {
				BidangRow = context.MBidangs.Where(x => x.IdBidang == idBidang).FirstOrDefault() ?? new MBidang();
			}

            public Edit()
            {
                
            }
        }
	}
}
