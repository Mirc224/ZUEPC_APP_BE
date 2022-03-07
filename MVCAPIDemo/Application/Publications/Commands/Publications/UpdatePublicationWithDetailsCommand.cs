using MediatR;
using ZUEPC.Application.PublicationAuthors.Entities.Inputs.PublicationAuthor;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationIdentifiers;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationNames;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class UpdatePublicationWithDetailsCommand :
	EPCUpdateBaseCommand, 
	IRequest<UpdatePublicationWithDetailsCommandResponse>
{
	public string? DocumentType { get; set; }
	public int? PublishYear { get; set; }
	public IEnumerable<PublicationNameCreateDto>? NamesToInsert { get; set; }
	public IEnumerable<PublicationNameUpdateDto>? NamesToUpdate { get; set; }
	public IEnumerable<long>? NamesToDelete { get; set; }
	public IEnumerable<PublicationIdentifierCreateDto>? IdentifiersToInsert { get; set; }
	public IEnumerable<PublicationIdentifierUpdateDto>? IdentifiersToUpdate { get; set; }
	public IEnumerable<long>? IdentifiersToDelete { get; set; }
	public IEnumerable<PublicationExternDatabaseIdCreateDto>? ExternDatabaseIdsToInsert { get; set; }
	public IEnumerable<PublicationExternDatabaseIdUpdateDto>? ExternDatabaseIdsToUpdate { get; set; }
	public IEnumerable<long>? ExternDatabaseIdsToDelete { get; set; }
	public IEnumerable<PublicationAuthorCreateDto>? AuthorsToInsert { get; set; }
	public IEnumerable<PublicationAuthorUpdateDto>? AuthorsToUpdate { get; set; }
	public IEnumerable<long>? AuthorsToDelete { get; set; }
}
