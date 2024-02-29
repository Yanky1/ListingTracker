using Org.BouncyCastle.Utilities.Encoders;
using System.Buffers.Text;

namespace ListingTracker.DbEntities
{
    public class FileUploadViewModel
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
        public string FileSize { get; set; }
        public string FileContentType { get; set; }
        public string FileDescription { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateUploaded { get; set; }
        public int CategoryId { get; set; }
        public string file { get; set; }
    }
}
