using System;
using System.Collections.Generic;

namespace Sistem_Pemberkasan.Models.EF;

public partial class MDokuman
{
    public int IdDokumen { get; set; }

    public string? NamaDokumen { get; set; }

    public int? DInsertBy { get; set; }

    public DateTime? DInsertDate { get; set; }

    public int? StatusDokumen { get; set; }

    public virtual ICollection<DetailDokuman> DetailDokumen { get; set; } = new List<DetailDokuman>();

    public virtual ICollection<MFormDokuman> MFormDokumen { get; set; } = new List<MFormDokuman>();
}
