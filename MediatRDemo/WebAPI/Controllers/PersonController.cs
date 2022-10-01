using DemoLibrary.Features.PersonCQRS.Commands.InsertPerson;
using DemoLibrary.Features.PersonCQRS.Queries.GetAllPeople;
using DemoLibrary.Features.PersonCQRS.Queries.GetPersonById;
using DemoLibrary.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PersonController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/<PersonController>
        [HttpGet]
        public async Task<List<PersonModel>> Get()
        {
            List<PersonModel> people;

            people = await _mediator.Send(new GetPersonListQuery());

            return people;
        }

        // GET api/<PersonController>/5
        [HttpGet("{id}")]
        public async Task<PersonModel> Get(int id)
        {
            return await _mediator.Send(new GetPersonByIdQuery(id));
        }

        // POST api/<PersonController>
        [HttpPost]
        public async Task<PersonModel> Post([FromBody] PersonModel person)
        {
            var model = new InsertPersonCommand(person.FirstName, person.LastName);
            return await _mediator.Send(model);
        }

    }
}
