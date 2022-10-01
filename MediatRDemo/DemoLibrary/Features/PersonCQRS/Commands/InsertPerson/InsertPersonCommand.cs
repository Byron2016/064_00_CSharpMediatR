using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Features.PersonCQRS.Commands.InsertPerson
{
    public record InsertPersonCommand(string FirstName, string LaststName) : IRequest<PersonModel>;

    //public class InsertPersonCommand : IRequest<PersonModel>
    //{
    //    public string FirstName { get; set; }
    //    public string LaststName { get; set; }

    //    public InsertPersonCommand(string firstName, string laststName)
    //    {
    //        FirstName = firstName;
    //        LaststName = laststName;
    //    }
    //}
}
