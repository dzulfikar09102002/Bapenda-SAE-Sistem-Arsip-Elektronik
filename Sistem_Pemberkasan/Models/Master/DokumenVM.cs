using Sistem_Pemberkasan.Models.EF;

namespace Sistem_Pemberkasan.Models.Master
{
	public class DokumenVM
	{
		public class Index
		{
			private readonly ModelContext _context;
			public List<MDokuman> DokumenList { get; set; } = new List<MDokuman>();
            public MUser User { get; set; } = new MUser();
            public Index(ModelContext context, string session)
			{
				_context = context;
				DokumenList = context.MDokumen.ToList();
                if (DokumenList.Count > 0)
                {
                    DokumenList = DokumenList;
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
			public MDokuman NewRow { get; set; } = new();
            public Add()
            {
                
            }
        }

		public class Edit
		{
            private readonly ModelContext _context;
			public MDokuman DokumenRow { get; set; } = new();
            public Edit(ModelContext context, int id)
            {
                _context = context;
                DokumenRow = context.MDokumen.FirstOrDefault(x => x.IdDokumen == id);
            }
            public Edit()
            {

            }
        }
	}
}
