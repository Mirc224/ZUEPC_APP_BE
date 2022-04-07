using ZUEPC.Application.RelatedPublications.Commands;
using ZUEPC.Application.RelatedPublications.Queries;
using ZUEPC.Base.Enums.Common;
using ZUEPC.Base.Extensions;
using ZUEPC.EvidencePublication.Domain.Publications;
using ZUEPC.EvidencePublication.Domain.RelatedPublications;
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
				new GetPublicationRelatedPublicationsQuery()
				{
					SourcePublicationId = updatedPublication.Id
				})).Data;
		List<Tuple<ImportRelatedPublication, Publication, Publication>> relatedPublicationTuples = new();
		foreach (ImportRelatedPublication relatedPublication in importRelatedPublications)
		{
			Tuple<ImportRelatedPublication, Publication, Publication>? filledRelatedPublicationTuple = await ProcessRelatedPublicationImportPublicationAsync(
														updatedPublication,
														relatedPublication,
														versionDate,
														source);
			if (filledRelatedPublicationTuple is null)
			{
				continue;
			}
			relatedPublicationTuples.Add(filledRelatedPublicationTuple);
		}

		IEnumerable<Tuple<ImportRelatedPublication, Publication, Publication>> relatedPublicationsToInsert =
										  from importRelatedPublication in importRelatedPublications
										  join relatedPublicationTuple in relatedPublicationTuples
										  on importRelatedPublication equals relatedPublicationTuple.Item1
										  where !(from currRelatedPub in currRelatedPublications
												  where currRelatedPub.PublicationId == relatedPublicationTuple.Item2.Id &&
														currRelatedPub.RelatedPublicationId == relatedPublicationTuple.Item3.Id &&
														currRelatedPub.RelationType == relatedPublicationTuple.Item1.RelationType &&
														currRelatedPub.CitationCategory == relatedPublicationTuple.Item1.CitationCategory
												  select 1).Any()
										  select new Tuple<ImportRelatedPublication, Publication, Publication>(
											  relatedPublicationTuple.Item1,
											  relatedPublicationTuple.Item2,
											  relatedPublicationTuple.Item3);

		await InsertRelatedPublicationCollectionAsync(relatedPublicationsToInsert, versionDate, source);

		IEnumerable<Tuple<ImportRelatedPublication, RelatedPublication>> relatedPublicationsToUpdate =
										  from importRelatedPublication in importRelatedPublications
										  join filledRelatedPublicationTuple in relatedPublicationTuples
										  on importRelatedPublication equals filledRelatedPublicationTuple.Item1
										  select new
										  {
											  ImportRelatedPublication = filledRelatedPublicationTuple.Item1,
											  SourcePublication = filledRelatedPublicationTuple.Item2,
											  DestPublication = filledRelatedPublicationTuple.Item3,
										  } into intermediate
										  join currRelatedPub in currRelatedPublications
										  on
										  new
										  {
											  SourcePublicationId = intermediate.SourcePublication.Id,
											  DestPublicationId = intermediate.DestPublication.Id,
											  RelationType = intermediate.ImportRelatedPublication.RelationType,
											  CitationCategory = intermediate.ImportRelatedPublication.CitationCategory
										  }
										  equals
										  new
										  {
											  SourcePublicationId = currRelatedPub.PublicationId,
											  DestPublicationId = currRelatedPub.RelatedPublicationId,
											  RelationType = currRelatedPub.RelationType,
											  CitationCategory = currRelatedPub.CitationCategory
										  }
										  select new Tuple<ImportRelatedPublication, RelatedPublication>(
											  intermediate.ImportRelatedPublication,
											  currRelatedPub);


		foreach (Tuple<ImportRelatedPublication, RelatedPublication> recordToUpdate in relatedPublicationsToUpdate)
		{
			if(recordToUpdate is null)
			{
				continue;
			}
			await UpdateRelatedPublicationAsync(
				recordToUpdate.Item1, 
				recordToUpdate.Item2, 
				versionDate, 
				source);
		}
	}

	private async Task UpdateRelatedPublicationAsync(
		ImportRelatedPublication importRelatedPublication,
		RelatedPublication currRelatedPublication,
		DateTime versionDate,
		OriginSourceType source)
	{
		RelatedPublication recordForUpdate = _mapper.Map<RelatedPublication>(importRelatedPublication);
		recordForUpdate.Id = currRelatedPublication.Id;
		recordForUpdate.PublicationId = currRelatedPublication.PublicationId;
		recordForUpdate.RelatedPublicationId = currRelatedPublication.RelatedPublicationId;
		await UpdateRecordAsync<RelatedPublication, UpdateRelatedPublicationCommand>(recordForUpdate, versionDate, source);
	}

	private async Task InsertRelatedPublicationCollectionAsync(
		IEnumerable<Tuple<ImportRelatedPublication, Publication, Publication>> importRelatedPublications,
		DateTime versionDate,
		OriginSourceType source)
	{
		foreach (Tuple<ImportRelatedPublication, Publication, Publication> recordToInsert in importRelatedPublications.OrEmptyIfNull())
		{
			await InsertRelatedPublicationAsync(recordToInsert.Item2, recordToInsert.Item3, recordToInsert.Item1, versionDate, source);
		}
	}

	private async Task<RelatedPublication> InsertRelatedPublicationAsync(
		Publication sourcePublication,
		Publication destinationPublication,
		ImportRelatedPublication importRelatedPublication,
		DateTime versionDate,
		OriginSourceType source)
	{
		RelatedPublication relatedPublication = _mapper.Map<RelatedPublication>(importRelatedPublication);
		relatedPublication.PublicationId = sourcePublication.Id;
		relatedPublication.RelatedPublicationId = destinationPublication.Id;

		CreateRelatedPublicationCommand createCommand = _mapper.Map<CreateRelatedPublicationCommand>(relatedPublication);
		createCommand.VersionDate = versionDate;
		createCommand.OriginSourceType = source;
		return (await _mediator.Send(createCommand)).Data;
	}

	private async Task<Tuple<ImportRelatedPublication, Publication, Publication>?> ProcessRelatedPublicationImportPublicationAsync(
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

		return new Tuple<ImportRelatedPublication, Publication, Publication>(
			importRelatedPublication, 
			sourcePublication,
			destPublication);
	}
}
