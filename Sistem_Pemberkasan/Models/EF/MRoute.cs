using System;
using System.Collections.Generic;

namespace Sistem_Pemberkasan.Models.EF;

public partial class MRoute
{
    public int Id { get; set; }

    public string NameRoute { get; set; } = null!;

    public string DescRoute { get; set; } = null!;

    public virtual ICollection<MRouteRole> MRouteRoles { get; set; } = new List<MRouteRole>();
}
