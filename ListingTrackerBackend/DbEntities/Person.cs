namespace ListingTracker.DbEntities
{
    public class Person
    {
        // id
        public Guid Id { get; set; }
        // first name
        public string FirstName { get; set; }
        // last name
        public string LastName { get; set; }
        // email
        public string Email { get; set; }
        // phone number
        public string PhoneNumber { get; set; }
        // address
        public string Address { get; set; }
        // city
        public string City { get; set; }
        // state
        public string State { get; set; }
        // zip code
        public string ZipCode { get; set; }
        // country
        public string Country { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<PersonWithFile> PersonWithFileUploads { get; set; }
        public ICollection<AcceptedPersonWithMatchingRecord> AcceptedPersonWithMatchingRecords { get; set; }
    }
}
