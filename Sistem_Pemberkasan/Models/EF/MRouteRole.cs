using System;
using System.Collections.Generic;

namespace Sistem_Pemberkasan.Models.EF;

public partial class MRouteRole
{
    public int Id { get; set; }

    public int IdRole { get; set; }

    public int IdRoute { get; set; }

    public virtual MRole IdRoleNavigation { get; set; } = null!;

    public virtual MRoute IdRouteNavigation { get; set; } = null!;
}
