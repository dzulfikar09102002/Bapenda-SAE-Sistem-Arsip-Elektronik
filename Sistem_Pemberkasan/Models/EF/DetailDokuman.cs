using System;
using System.Collections.Generic;

namespace Sistem_Pemberkasan.Models.EF;

public partial class DetailDokuman
{
    public int IdDetailDokumen { get; set; }

    public int? IdBerkas { get; set; }

    public int? IdDokumen { get; set; }

    public string? NamaDetailDokumen { get; set; }

    public byte[] FileBloob { get; set; } = null!;

    public virtual Berka? IdBerkasNavigation { get; set; }

    public virtual MDokuman? IdDokumenNavigation { get; set; }
}
