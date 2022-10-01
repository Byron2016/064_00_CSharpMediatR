using DemoLibrary.Features.PersonCQRS.Queries.GetAllPeople;
using DemoLibrary.Features.PersonCQRS.Queries.GetPersonById;
using DemoLibrary.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLibrary.Features.PersonCQRS.Handlers
{
    public class GetPersonByIdHandler : IRequestHandler<GetPersonByIdQuery, PersonModel>
    {
        private readonly IMediator _mediator;

        public GetPersonByIdHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<PersonModel> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPersonListQuery());

            var output = result.FirstOrDefault(x => x.Id == request.id);

            return output;
        }
    }
}
