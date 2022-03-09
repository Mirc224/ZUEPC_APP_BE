using MediatR;
using ZUEPC.Application.PublicationActivities.Entities.Inputs.PublicationActivities;
using ZUEPC.Application.PublicationAuthors.Entities.Inputs.PublicationAuthor;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationIdentifiers;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationNames;
using ZUEPC.Application.RelatedPublications.Entities.Inputs.RelatedPublications;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class CreatePublicationWithDetailsCommand : 
	EPCCreateBaseCommand, 
	IRequest<CreatePublicationWithDetailsCommandResponse>
{
	public string? DocumentType { get; set; }
	public int? PublishYear { get; set; }
	public IEnumerable<PublicationNameCreateDto>? Names { get; set; }
	public IEnumerable<PublicationIdentifierCreateDto>? Identifiers { get; set; }
	public IEnumerable<PublicationExternDatabaseIdCreateDto>? ExternDatabaseIds { get; set; }
	public IEnumerable<PublicationAuthorCreateDto>? Authors { get; set; }
	public IEnumerable<RelatedPublicationCreateDto>? RelatedPublications { get; set; }
	public IEnumerable<PublicationActivityCreateDto>? PublicationActivities { get; set; }
}
