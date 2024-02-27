namespace ListingTracker.DbEntities
{
    public class ActivityLog
    {
        public int Id { get; set; }
        public string LogType { get; set; }
        public string LogDescription { get; set; }
        public string LogDetails { get; set; }
        public string LogBy { get; set; }
        public DateTime LogDate { get; set; }
    }
}
