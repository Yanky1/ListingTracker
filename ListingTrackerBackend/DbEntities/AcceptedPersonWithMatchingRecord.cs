
namespace ListingTracker.DbEntities
{
    public class AcceptedPersonWithMatchingRecord
    {
        public Guid AcceptedPersonId { get; set; }
        public AcceptedPerson AcceptedPerson { get; set; }
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
        public bool IsDeletedAsMatch { get; set; }

    }
}
