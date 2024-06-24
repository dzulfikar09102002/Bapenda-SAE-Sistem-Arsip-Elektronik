using System;
using System.Collections.Generic;

namespace Sistem_Pemberkasan.Models.EF;

public partial class MPegawai
{
    public string Nik { get; set; } = null!;

    public int? IdBidang { get; set; }

    public string? NamaPegawai { get; set; }

    public bool? JenisKelamin { get; set; }

    public string? NoTelepon { get; set; }

    public int? StatusPegawai { get; set; }

    public DateTime? PInsertDate { get; set; }

    public int? PInsertBy { get; set; }

    public virtual MBidang? IdBidangNavigation { get; set; }

    public virtual ICollection<MUser> MUsers { get; set; } = new List<MUser>();
}
