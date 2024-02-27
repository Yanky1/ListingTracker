namespace ListingTracker.DbEntities
{
    public class PersonWithFile
    {
        // file upload
        public Guid FileUploadId { get; set; }
        public FileUpload FileUpload { get; set; }
        public Guid PersonId { get; set; }
        public Person Person { get; set; }

    }
}
