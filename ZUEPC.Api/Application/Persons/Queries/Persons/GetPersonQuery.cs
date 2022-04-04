using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Persons.Queries.Persons;

public class GetPersonQuery : 
	EPCSimpleQueryBase<long>,
	IRequest<GetPersonQueryResponse>
{
}
