using Sistem_Pemberkasan.Models.EF;
using System.Security.Claims;

namespace Sistem_Pemberkasan.Models.Lib
{
    public class CookieData
    {
        private readonly ModelContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieData()
        {
        }

        public CookieData(ModelContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public int GetIdUser() 
        {
			var context = new ModelContext();

			int result = 0;
			if (_httpContextAccessor.HttpContext != null)
			{
				var getClaimUser = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                var getId = context.MUsers.FirstOrDefault(x => x.Email == getClaimUser.Value);
				if (getId == null)
				{
					return result;
				}

				result = getId.IdUser;
			}

			return result;

		}
      
        public string GetUser()
        {
            string result = "unidentified";
            if (_httpContextAccessor.HttpContext != null)
            { 
                var getClaimUser = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                if (getClaimUser == null)
                {
                    return result;
                }

                result = getClaimUser.Value;
            }

            return result;
        }
    }
}
