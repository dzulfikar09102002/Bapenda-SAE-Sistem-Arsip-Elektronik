using System;
using System.Collections.Generic;

namespace Sistem_Pemberkasan.Models.EF;

public partial class MKategoriBerka
{
    public int IdKategoriBerkas { get; set; }

    public int? IdBidang { get; set; }

    public string? JenisKategoriBerkas { get; set; }

    public int? StatusKategoriBerkas { get; set; }

    public int? KbInsertBy { get; set; }

    public DateTime? KbInsertDate { get; set; }

    public virtual ICollection<Berka> Berkas { get; set; } = new List<Berka>();

    public virtual MBidang? IdBidangNavigation { get; set; }

    public virtual ICollection<MFormDokuman> MFormDokumen { get; set; } = new List<MFormDokuman>();
}
