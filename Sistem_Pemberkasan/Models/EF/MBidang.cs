using System;
using System.Collections.Generic;

namespace Sistem_Pemberkasan.Models.EF;

public partial class MBidang
{
    public int IdBidang { get; set; }

    public string? NamaBidang { get; set; }

    public string? Singkatan { get; set; }

    public int? StatusBidang { get; set; }

    public int? BInsertBy { get; set; }

    public DateTime? BInsertDate { get; set; }

    public virtual ICollection<MKategoriBerka> MKategoriBerkas { get; set; } = new List<MKategoriBerka>();

    public virtual ICollection<MPegawai> MPegawais { get; set; } = new List<MPegawai>();
}
