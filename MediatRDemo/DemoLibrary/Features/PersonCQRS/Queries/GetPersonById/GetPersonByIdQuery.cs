using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Features.PersonCQRS.Queries.GetPersonById
{
    public record GetPersonByIdQuery(int id) : IRequest<PersonModel>;

    //Classes version.
    //public class GetPersonByIdClassQuery : IRequest<PersonModel>
    //{
    //    public int Id { get; set; }

    //    public GetPersonByIdClassQuery(int id)
    //    {
    //        Id = id;
    //    }
    //}
}
