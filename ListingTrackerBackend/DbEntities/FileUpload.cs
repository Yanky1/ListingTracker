namespace ListingTracker.DbEntities
{
    public class FileUpload
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

        // join wiht categories table
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        // icollection of person
        public ICollection<PersonWithFile> FileUploadWithPeople { get; set; }
    }
}
