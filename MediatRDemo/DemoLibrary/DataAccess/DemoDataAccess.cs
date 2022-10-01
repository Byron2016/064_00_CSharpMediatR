using DemoLibrary.Interfaces;
using DemoLibrary.Models;

namespace DemoLibrary.DataAccess
{
    public class DemoDataAccess : IDataAccess
    {
        private List<PersonModel> _people = new();
        public DemoDataAccess()
        {
            _people = Enumerable.Range(0, 20).Select(n =>
            {
                var p = new PersonModel()
                {
                    Id = n,
                    FirstName = $"FirstName_{n}",
                    LastName = $"LastName_{n}",
                };
                return p;
            }).ToList();
        }

        public List<PersonModel> GetPeople()
        {
            return _people;
        }

        public PersonModel InsertPerson(string firstName, string lastName)
        {
            PersonModel p = new() { FirstName = firstName, LastName = lastName };
            p.Id = _people.Max(x => x.Id) + 1;
            return p;
        }
    }
}
