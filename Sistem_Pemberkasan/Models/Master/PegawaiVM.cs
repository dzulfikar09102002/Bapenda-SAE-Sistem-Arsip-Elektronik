using Microsoft.EntityFrameworkCore;
using Sistem_Pemberkasan.Models.EF;

using Sistem_Pemberkasan.Models.Lib;


namespace Sistem_Pemberkasan.Models.Master
{
    public class PegawaiVM
    {

        public class Index
        {
            public List<MUser> ListPegawai { get; set; }

            public Index(ModelContext context)
            {
                ListPegawai = context.MUsers
                    .Include(u => u.IdRoleNavigation)
                    .Include(p => p.NikNavigation)
                    .ThenInclude(p => p.IdBidangNavigation)
                    .ToList();
            }
        }
        public class Add
        {  
			public MPegawai NewPegawaiRow { get; set; } = new();
            public MUser NewUserRow { get; set; } = new();
			public List<MBidang> ListBidang { get; set; }
            public List<MRole> ListRole { get; set; }
			public Add(ModelContext context)
			{
				ListBidang = context.MBidangs.Where(x => x.StatusBidang == 1).ToList() ?? new List<MBidang>();

                ListRole = context.MRoles.Where(x => x.StatusRole == 1).ToList() ?? new List<MRole>();
			}

            public Add()
            {
                
            }
        }

        public class Edit
        {
			public MPegawai EditPegawaiRow { get; set; } = new();

            public MUser EditUserRow { get; set; } = new();

            public List<MBidang> ListBidang { get; set; }
            public List<MRole> ListRole { get; set; }

            public string OldPassword { get; set; }
            public string NewPassword { get; set; }

			public Edit(ModelContext context , string Nik)
            {
                EditPegawaiRow = context.MPegawais.Where(x => x.Nik == Nik).FirstOrDefault() ?? new MPegawai();
                EditUserRow = context.MUsers.Where(x => x.Nik == Nik).FirstOrDefault() ?? new MUser();
				var bidangList = context.MBidangs.ToList();
				if (bidangList != null)
				{
					ListBidang = bidangList;
				}

				var roleList = context.MRoles.ToList();
				if (roleList != null)
				{
					ListRole = roleList;
				}
			}
            public Edit()
            {
                
            }
        }

        public class ProfileUser 
        {
            public List<MUser> UserNow { get; set; }
            public ProfileUser(ModelContext context,string email)
            {
                var userNow = context.MUsers.Include(x => x.IdRoleNavigation).Include(x => x.NikNavigation).ThenInclude(x=>x.IdBidangNavigation).Where(x => x.Email == email).ToList();
                if (userNow != null)
                {
                    UserNow = userNow;
                }
            }
        }

    }
}
