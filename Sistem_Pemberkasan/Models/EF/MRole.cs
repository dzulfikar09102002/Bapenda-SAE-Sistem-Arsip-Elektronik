using System;
using System.Collections.Generic;

namespace Sistem_Pemberkasan.Models.EF;

public partial class MRole
{
    public int IdRole { get; set; }

    public string? JenisRole { get; set; }

    public DateTime? RInsertDate { get; set; }

    public int? RInsertBy { get; set; }

    public int? StatusRole { get; set; }

    public virtual ICollection<MUser> MUsers { get; set; } = new List<MUser>();
}
