using System;
using System.Collections.Generic;

namespace Sistem_Pemberkasan.Models.EF;

public partial class Berka
{
    public int IdBerkas { get; set; }

    public int? IdKategoriBerkas { get; set; }

    public string? Nop { get; set; }

    public string? NoReferensi { get; set; }

    public DateTime? TanggalUnggah { get; set; }

    public string? NoSk { get; set; }

    public string? NamaPemohon { get; set; }

    public string? AlamatPemohon { get; set; }

    public string? NoTelpPemohon { get; set; }

    public string? KeteranganPst { get; set; }

    public int? StatusBerkas { get; set; }

    public int? BeInsertBy { get; set; }

    public DateTime? BeInsertDate { get; set; }

    public virtual ICollection<DetailDokuman> DetailDokumen { get; set; } = new List<DetailDokuman>();

    public virtual MKategoriBerka? IdKategoriBerkasNavigation { get; set; }

    public virtual ICollection<PendukungBerka> PendukungBerkas { get; set; } = new List<PendukungBerka>();
}
