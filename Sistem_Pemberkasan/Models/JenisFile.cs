namespace Sistem_Pemberkasan.Models
{

    public class JenisFile
    {
        public ETipeFile Jenis { get; set; }
        public string ContentType { get; set; } = "";
        public string FileExtension { get; set; } = "";
    }

    public enum ETipeFile
    {
        PDF = 0,//application/pdf
        Image = 1,//image/png
        Excel = 2,//application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
        App = 3,//application/x-msdownload  - dll / exe
        Zip = 4,//application/x-zip-compressed  - 
        Word = 5,//application/vnd.openxmlformats-officedocument.wordprocessingml.document
        Unknown = 6
    }

}
