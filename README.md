<div>
	<div>
		<img src="https://raw.githubusercontent.com/Byron2016/00_forImages/main/images/Logo_01_00.png" align="left" alt="MyLogo" width="200">
	</div>
	&nbsp;
	<div>
		<h1>064_00_CSharpMediatR</h1>
	</div>
</div>

&nbsp;

## Project Description

**MediatRDemo** is a practice using MediatR package. following IAmTimCorey tutorial [Intro to MediatR - Implementing CQRS and Mediator Patterns](https://www.youtube.com/watch?v=yozD5Tnd8nw).
It show use of MediatR into two UI: Blazor Server and WebCoreAPI.

## Stepts

This stepts are for ASP.NET 6.0

1. Create a new **Blazor Server App** project with these caracteristics:
	- Project Name: BlazorUI
	- Solution Name: MediatRDemo
	- Framework: .NET 6.0 (Long-term support)
	- Authentication type: None
	- Configure for HTTPS
	- Do not use top-level statements

2. Create a new **Class Library** project with these caracteristics:
	- Project Name: DemoLibrary
	- Framework: .NET 6.0 (Long-term support)

3. Add Data Access layer and call from UI:
    1. Inside **class library DemoLibrary**
		1. Inside folder Models add PersonModel model
		```c#
			namespace DemoLibrary.Models
			{
				public class PersonModel
				{
					public int Id { get; set; }
					public string? FirstName { get; set; }
					public string? LastName { get; set; }
				}
			}
		```

		2. Inside folder Interfaces add IDataAccess interface
		```c#
		namespace DemoLibrary.Interfaces
		{
			public interface IDataAccess
			{
				List<PersonModel> GetPeople();
				PersonModel InsertPerson(string firstName, string lastName);
			}
		}
		```

		3. Inside folder DataAccess add DemoDataAccess Class
		```c#
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
		```

    2. Inside project **project BlazorUI**
		1. Add a reference to class library DemoLibrary
		2. Add usings to _Imports.razor
		```c#
		....
		@using DemoLibrary.DataAccess
		@using DemoLibrary.Models
		``` 

		3. Modify page Index.razor
		```c#
		@page "/"

		<PageTitle>Index</PageTitle>

		<h1>We The People</h1>

		<ul>
			@foreach(var p in people)
			{
				<li>@p.Id @p.FirstName @p.LastName</li>
			}
		</ul>

		@code {
			List<PersonModel> people;

			protected override Task OnInitializedAsync()
			{
				//get list of people
				var demoDataAccess = new DemoDataAccess();
				people = demoDataAccess.GetPeople();
				return base.OnInitializedAsync();
			}
		}
		``` 
4. Implement MediatR into our Solution:
	1. Inside **class library DemoLibrary**
    	1. Add nuget package [MediatR](https://www.nuget.org/packages/MediatR/11.0.0?_src=template) 

		2. Build MediatR folder structure
			1. Add CSQR folders structure
				1. Features
					1. PersonCSQR
						1. Commands
							1. InsertPerson
						2. Queries
							1. GetAllPeople
							2. GetPersonById
						3. Handlers 

		3. Add classes to MediatR folder structure
			1. To folder Features/PersonCSQR/Queries/GetAllPeople 
			```c#
			namespace DemoLibrary.Features.PersonCQRS.Queries.GetAllPeople
			{
				public record GetPersonListQuery() : IRequest<List<PersonModel>>;

				//Classes version.
				//public class GetPersonListClassQuery: IRequest<List<PersonModel>>
				//{
				//}
			}
			```

			2. To folder Features/PersonCSQR/Queries/GetPersonById 
			```c#

			```

			3. To folder Features/PersonCSQR/Commands/InsertPerson
			```c#

			```

			4. To folder Features/PersonCSQR/Handlers  
			```c#
			namespace DemoLibrary.Features.PersonCQRS.Handlers
			{
				public class GetPersonListHandler : IRequestHandler<GetPersonListQuery, List<PersonModel>>
				{
					private readonly IDataAccess _data;

					public GetPersonListHandler(IDataAccess data)
					{
						_data = data;
					}
					public Task<List<PersonModel>> Handle(GetPersonListQuery request, CancellationToken cancellationToken)
					{
						return Task.FromResult(_data.GetPeople());
					}
				}
			}
			```

		4. Add class **DemoLibraryMediatREntryPoint**
		```c#
		namespace DemoLibrary
		{
			public class DemoLibraryMediatREntryPoint
			{
			}
		}
		```
	
	2. Inside **project BlazorUI**
    	1. Add nuget package [MediatR.Extensions.Microsoft.DependencyInjection](https://www.nuget.org/packages/MediatR.Extensions.Microsoft.DependencyInjection/11.0.0?_src=template) 

    	2. Add DataAccess dependent injection 
		```c#
		namespace BlazorUI
		{
			public class Program
			{
				public static void Main(string[] args)
				{
					....
					builder.Services.AddTransient<IDataAccess, DemoDataAccess>();
					//builder.Services.AddMediatR(typeof(DemoDataAccess).Assembly);
					builder.Services.AddMediatR(typeof(DemoLibraryMediatREntryPoint).Assembly);
					....
				}
			}
		}
		```

    	3. Add usings to _Imports.razor
		```c#
		....
		@using DemoLibrary.Features.PersonCQRS.Handlers
		@using DemoLibrary.Features.PersonCQRS.Commands.InsertPerson
		@using DemoLibrary.Features.PersonCQRS.Queries.GetAllPeople
		``` 

		4. Modify page Index.razor
		```c#
		@page "/"
		@inject MediatR.IMediator _mediator

		....

		@code {
			List<PersonModel> people;

			protected override Task OnInitializedAsync()
			{
				//get list of people
				/*
				var demoDataAccess = new DemoDataAccess();
				people = demoDataAccess.GetPeople();
				*/

				people = await _mediator.Send(new GetPersonListQuery());

				//return base.OnInitializedAsync();
			}
		}
		``` 

5. Change UI to API Project:
	1. Create a new **ASP.NET Core Web API** project with these caracteristics:
		- Project Name: WebAPI
		- Framework: .NET 6.0 (Long-term support)
		- Authentication type: None
		- Configure for HTTPS
		- Use Controllers
		- enable OpenAPI support
		- Do not use top-level statements

	2. Add a reference to **class library DemoLibrary**


    3. Add nuget package [MediatR.Extensions.Microsoft.DependencyInjection](https://www.nuget.org/packages/MediatR.Extensions.Microsoft.DependencyInjection/11.0.0?_src=template) 

	4. Add DataAccess dependent injection 
	```c#
	namespace BlazorUI
	{
		public class Program
		{
			public static void Main(string[] args)
			{
				....
				builder.Services.AddTransient<IDataAccess, DemoDataAccess>();
				//builder.Services.AddMediatR(typeof(DemoDataAccess).Assembly);
				builder.Services.AddMediatR(typeof(DemoLibraryMediatREntryPoint).Assembly);
				....
			}
		}
	}
	```

	5. Add API Controller class 
	```c#
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
		}
	}
	```

6. Implement get person by id and insert a person method
	1. Get person by id 
		1. Inside **class library DemoLibrary**
			1. Add classes to MediatR folder structure
				1. To folder Features/PersonCSQR/Queries/GetPersonById 
				```c#
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
				```

				2. To folder Features/PersonCSQR/Handlers  
				```c#
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
				```

		2. Inside **project BlazorUI**
			1. Add API Controller class 
			```c#
			namespace WebAPI.Controllers
			{
				[Route("api/[controller]")]
				[ApiController]
				public class PersonController : ControllerBase
				{
					....

					// GET api/<PersonController>/5
					[HttpGet("{id}")]
					public async Task<PersonModel> Get(int id)
					{
						return await _mediator.Send(new GetPersonByIdQuery(id));
					}
				}
			}
		```

	2. Insert person 
		1. Inside **class library DemoLibrary**
			1. Add classes to MediatR folder structure
				1. To folder Features/PersonCSQR/Commands/InsertPerson
				```c#
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
				```

				2. To folder Features/PersonCSQR/Handlers  
				```c#
				namespace DemoLibrary.Features.PersonCQRS.Handlers
				{
					public class InsertPersonHandler : IRequestHandler<InsertPersonCommand, PersonModel>
					{
						private readonly IDataAccess _data;

						public InsertPersonHandler(IDataAccess data)
						{
							_data = data;
						}
						public Task<PersonModel> Handle(InsertPersonCommand request, CancellationToken cancellationToken)
						{
							return Task.FromResult(_data.InsertPerson(request.FirstName, request.LaststName));
						}
					}
				}
				```

		2. Inside **project BlazorUI**
			1. Add API Controller class 
			```c#
			namespace WebAPI.Controllers
			{
				[Route("api/[controller]")]
				[ApiController]
				public class PersonController : ControllerBase
				{
					....

					// POST api/<PersonController>
					[HttpPost]
					public async Task<PersonModel> Post([FromBody] PersonModel person)
					{
						var model = new InsertPersonCommand(person.FirstName, person.LastName);
						return await _mediator.Send(model);
					}
				}
			}
		```