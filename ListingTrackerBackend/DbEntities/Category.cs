namespace ListingTracker.DbEntities
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int CategoryLevel { get; set; }
        public bool IsDeleted { get; set; }
    }
}
