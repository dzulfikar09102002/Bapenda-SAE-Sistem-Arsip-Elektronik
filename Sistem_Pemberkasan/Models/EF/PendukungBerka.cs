using System;
using System.Collections.Generic;

namespace Sistem_Pemberkasan.Models.EF;

public partial class PendukungBerka
{
    public int IdPendukungBerkas { get; set; }

    public int? IdKategoriPendukungBerkas { get; set; }

    public int? IdBerkas { get; set; }

    public string? KeteranganPendukungBerkas { get; set; }

    public string? NamaDokumenPendukungBerkas { get; set; }

    public byte[] FileBloob { get; set; } = null!;

    public int? StatusPendukungBerkas { get; set; }

    public int? PbInsertBy { get; set; }

    public DateTime? PbInsertDate { get; set; }

    public virtual Berka? IdBerkasNavigation { get; set; }

    public virtual MKategoriPendukungBerka? IdKategoriPendukungBerkasNavigation { get; set; }
}
