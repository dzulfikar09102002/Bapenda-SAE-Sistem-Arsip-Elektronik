using System;
using System.Collections.Generic;

namespace Sistem_Pemberkasan.Models.EF;

public partial class MFormDokuman
{
    public int IdFormDokumen { get; set; }

    public int? IdDokumen { get; set; }

    public int? IdKategoriBerkas { get; set; }

    public bool? StatusFormDokumen { get; set; }

    public int? FdInsertBy { get; set; }

    public DateTime? FdInsertDate { get; set; }

    public virtual MDokuman? IdDokumenNavigation { get; set; }

    public virtual MKategoriBerka? IdKategoriBerkasNavigation { get; set; }
}
