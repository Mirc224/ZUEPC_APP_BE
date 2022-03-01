using ZUEPC.Application.RelatedPublications.Commands;
using ZUEPC.Application.RelatedPublications.Queries;
using ZUEPC.Base.Enums.Common;
using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.EvidencePublication.Base.Domain.RelatedPublications;
using ZUEPC.Import.Models;

namespace ZUEPC.Application.Import.Services;

public partial class ImportService
{
	private async Task ProcessRelatedPublicationsAsync(
		Publication updatedPublication,
		IEnumerable<ImportRelatedPublication> importRelatedPublications,
		DateTime versionDate,
		OriginSourceType source)
	{
		IEnumerable<RelatedPublication> currRelatedPublications = (await _mediator.Send(
				new GetPublicationRelatedPublicationsCommand()
				{
					SourcePublicationId = updatedPublication.Id
				})).RelatedPublications;
		List<Tuple<ImportRelatedPublication, RelatedPublication>> filledRelatedPublicationTuples = new();
		foreach (ImportRelatedPublication relatedPublication in importRelatedPublications)
		{
			Tuple<ImportRelatedPublication, RelatedPublication>? filledRelatedPublicationTuple = await ProcessRelatedPublicationImportPublicationAsync(
																	updatedPublication,
																	relatedPublication,
																	versionDate,
																	source);
			if (filledRelatedPublicationTuple is null)
			{
				continue;
			}
			filledRelatedPublicationTuples.Add(filledRelatedPublicationTuple);
		}

		IEnumerable<Tuple<ImportRelatedPublication, RelatedPublication>> relatedPublicationsToInsert =
										  from importRelatedPublication in importRelatedPublications
										  join filledRelatedPublicationTuple in filledRelatedPublicationTuples
										  on importRelatedPublication equals filledRelatedPublicationTuple.Item1
										  where !(from currRelatedPub in currRelatedPublications
												  where currRelatedPub.PublicationId == filledRelatedPublicationTuple.Item2.PublicationId &&
														currRelatedPub.RelatedPublicationId == filledRelatedPublicationTuple.Item2.RelatedPublicationId &&
														currRelatedPub.RelationType == filledRelatedPublicationTuple.Item2.RelationType &&
														currRelatedPub.CitationCategory == filledRelatedPublicationTuple.Item2.CitationCategory
												  select 1).Any()
										  select new Tuple<ImportRelatedPublication, RelatedPublication>(filledRelatedPublicationTuple.Item1,
																										 filledRelatedPublicationTuple.Item2);

		foreach (Tuple<ImportRelatedPublication, RelatedPublication> recordToInsert in relatedPublicationsToInsert)
		{
			RelatedPublication domainRelatedPublication = await CreateRelatedPublicationAsync(recordToInsert.Item2, versionDate, source);
		}

		IEnumerable<Tuple<ImportRelatedPublication, RelatedPublication>> relatedPublicationsToUpdate =
										  from importRelatedPublication in importRelatedPublications
										  join filledRelatedPublicationTuple in filledRelatedPublicationTuples
										  on importRelatedPublication equals filledRelatedPublicationTuple.Item1
										  where (from currRelatedPub in currRelatedPublications
												 where currRelatedPub.PublicationId == filledRelatedPublicationTuple.Item2.PublicationId &&
													   currRelatedPub.RelatedPublicationId == filledRelatedPublicationTuple.Item2.RelatedPublicationId &&
													   currRelatedPub.RelationType == filledRelatedPublicationTuple.Item2.RelationType &&
													   currRelatedPub.CitationCategory == filledRelatedPublicationTuple.Item2.CitationCategory &&
													   currRelatedPub.VersionDate < versionDate
												 select 1).Any()
										  select new Tuple<ImportRelatedPublication, RelatedPublication>(filledRelatedPublicationTuple.Item1,
																										 filledRelatedPublicationTuple.Item2);

		foreach (Tuple<ImportRelatedPublication, RelatedPublication> recordToUpdate in relatedPublicationsToUpdate)
		{
			await UpdateRelatedPublicationAsync(recordToUpdate.Item1, recordToUpdate.Item2, versionDate, source);
		}
	}

	private async Task UpdateRelatedPublicationAsync(
		ImportRelatedPublication importRelatedPublication,
		RelatedPublication currRelatedPublication,
		DateTime versionDate,
		OriginSourceType source)
	{
		RelatedPublication recordForUpdate = _mapper.Map<RelatedPublication>(importRelatedPublication);
		recordForUpdate.PublicationId = currRelatedPublication.PublicationId;
		recordForUpdate.RelatedPublicationId = currRelatedPublication.RelatedPublicationId;
		await UpdateRecordAsync<RelatedPublication, UpdateRelatedPublicationCommand>(recordForUpdate, versionDate, source);
	}

	private async Task<RelatedPublication> CreateRelatedPublicationAsync(
		RelatedPublication relatedPublication,
		DateTime versionDate,
		OriginSourceType source)
	{
		CreateRelatedPublicationCommand createCommand = _mapper.Map<CreateRelatedPublicationCommand>(relatedPublication);
		createCommand.VersionDate = versionDate;
		createCommand.OriginSourceType = source;
		return (await _mediator.Send(createCommand)).RelatedPublication;
	}

	private async Task<Tuple<ImportRelatedPublication, RelatedPublication>?> ProcessRelatedPublicationImportPublicationAsync(
		Publication sourcePublication,
		ImportRelatedPublication importRelatedPublication,
		DateTime versionDate,
		OriginSourceType source)
	{
		if (importRelatedPublication.RelatedPublication is null)
		{
			return null;
		}
		ImportPublication importPublication = importRelatedPublication.RelatedPublication;
		Publication destPublication = await ProcessImportedPublication(importPublication, versionDate, source);

		RelatedPublication relatedPublication = _mapper.Map<RelatedPublication>(importRelatedPublication);
		relatedPublication.PublicationId = sourcePublication.Id;
		relatedPublication.RelatedPublicationId = destPublication.Id;

		return new Tuple<ImportRelatedPublication, RelatedPublication>(importRelatedPublication, relatedPublication);
	}
}
