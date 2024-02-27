namespace ListingTracker.DbEntities
{
    public class AcceptedSourceTracking
    {
        public int Id { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public Guid AcceptedPersonId { get; set; }
        public AcceptedPerson AcceptedPerson { get; set; }
        public Guid PersonId { get; set; }
        public Person Person { get; set; }

    }
}
