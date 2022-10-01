using DemoLibrary.Models;

namespace DemoLibrary.Interfaces
{
    public interface IDataAccess
    {
        List<PersonModel> GetPeople();
        PersonModel InsertPerson(string firstName, string lastName);
    }
}