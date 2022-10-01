using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Features.PersonCQRS.Queries.GetAllPeople
{
    public record GetPersonListQuery() : IRequest<List<PersonModel>>;

    //Classes version.
    //public class GetPersonListClassQuery: IRequest<List<PersonModel>>
    //{
    //}
}
