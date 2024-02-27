using ListingTracker.DbEntities;

namespace ListingTracker.Classes
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public bool IsOverwrite { get; set; }
        public bool IsDeleted { get; set; }
        // join with categories table
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // join with file uploads table
        public int FileUploadId { get; set; }
        public FileUpload FileUpload { get; set; }

    }
}
