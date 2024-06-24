using System;
using System.Collections.Generic;

namespace Sistem_Pemberkasan.Models.EF;

public partial class MUser
{
    public int IdUser { get; set; }

    public int? IdRole { get; set; }

    public string? Nik { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? UInsertBy { get; set; }

    public DateTime? UInsertDate { get; set; }

    public int? StatusUser { get; set; }

    public virtual MRole? IdRoleNavigation { get; set; }

    public virtual MPegawai? NikNavigation { get; set; }
}
