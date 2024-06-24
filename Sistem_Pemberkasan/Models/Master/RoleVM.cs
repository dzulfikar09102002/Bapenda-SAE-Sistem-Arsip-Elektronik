using Sistem_Pemberkasan.Models.EF;

namespace Sistem_Pemberkasan.Models.Master
{
	public class RoleVM
	{
		public class Index
		{

		}
		
		public class Add
		{
			public MRole NewRow { get; set; } = new();
            public Add()
            {
                
            }
        }

		public class Edit
		{
			public MRole NewRole { get; set;} = new();
            public Edit()
            {
                
            }
        }
	}
}
