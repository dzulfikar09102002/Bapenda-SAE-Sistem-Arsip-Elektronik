using System;
using System.Collections.Generic;

namespace Sistem_Pemberkasan.Models.EF;

public partial class MKategoriPendukungBerka
{
    public int IdKategoriPendukungBerkas { get; set; }

    public string? NamaKategoriPendukungBerkas { get; set; }

    public int? StatusKategoriPendukung { get; set; }

    public int? KpInsertBy { get; set; }

    public DateTime? KpInsertDate { get; set; }

    public virtual ICollection<PendukungBerka> PendukungBerkas { get; set; } = new List<PendukungBerka>();
}
