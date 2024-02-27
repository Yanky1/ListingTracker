using ListingTracker.DbEntities;

namespace ListingTracker.LogicClasses
{
    public class PersonMatcher
    {
        public List<Person> GenerateMatchingList(Person person, List<Person> allPeople)
        {
            // generate a list of people that match the given person
            var matchingPeople = new List<Person>();
            foreach (var p in allPeople)
            {
                if (p.Id != person.Id)
                {
                    if (p.FirstName == person.FirstName && p.LastName == person.LastName)
                    {
                        matchingPeople.Add(p);
                    }
                }
            }

            return matchingPeople;

        }
    }
}
