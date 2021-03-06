using ZUEPC.Application.PublicationActivities.Commands;
using ZUEPC.Application.PublicationActivities.Queries;
using ZUEPC.Base.Enums.Common;
using ZUEPC.Base.Commands;
using ZUEPC.Base.Domain;
using ZUEPC.EvidencePublication.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Domain.Publications;
using ZUEPC.Import.Models;

namespace ZUEPC.Application.Import.Services;

public partial class ImportService
{
	private async Task ProcessPublicationActivitiesAsync(
		Publication currPublication,
		IEnumerable<ImportPublicationActivity> publicationActivities,
		DateTime versionDate,
		OriginSourceType source)
	{
		IEnumerable<PublicationActivity> currPublicationActivities = (await _mediator.Send(
				new GetPublicationPublicationActivitiesQuery()
				{
					PublicationId = currPublication.Id
				})).Data;

		IEnumerable<ImportPublicationActivity> activitiesToInsert =
										from importPubActivity in publicationActivities
										where !(from currPubActivity in currPublicationActivities
												where importPubActivity.Category == currPubActivity.Category &&
													  importPubActivity.GovernmentGrant == currPubActivity.GovernmentGrant &&
													  importPubActivity.ActivityYear == currPubActivity.ActivityYear
												select 1).Any()
										select importPubActivity;

		List<PublicationActivity> mappedActivitiesToInsert = _mapper.Map<List<PublicationActivity>>(activitiesToInsert);
		foreach (PublicationActivity mappedActivity in mappedActivitiesToInsert)
		{
			mappedActivity.PublicationId = currPublication.Id;
		}
		await InsertRecordsAsync<PublicationActivity, CreatePublicationActivityCommand>(
			mappedActivitiesToInsert,
			versionDate,
			source);

		IEnumerable<Tuple<ImportPublicationActivity, PublicationActivity>> activityTuplesToUpdate =
										from importPubActivity in publicationActivities
										join currPubActivity in currPublicationActivities
										on
										new
										{
											Category = importPubActivity.Category,
											GovernmentGrant = importPubActivity.GovernmentGrant,
											ActivityYear = importPubActivity.ActivityYear,
										}
										equals
										new
										{
											Category = currPubActivity.Category,
											GovernmentGrant = currPubActivity.GovernmentGrant,
											ActivityYear = currPubActivity.ActivityYear,
										}
										select new Tuple<ImportPublicationActivity, PublicationActivity>(
											importPubActivity,
											currPubActivity);

		foreach (Tuple<ImportPublicationActivity, PublicationActivity> tuple in activityTuplesToUpdate)
		{
			await UpdatePublicationActivityAsync(tuple.Item2, tuple.Item1, versionDate, source);
		}
	}

	private async Task UpdatePublicationActivityAsync(
		PublicationActivity currRecord,
		ImportPublicationActivity importRecord,
		DateTime versionDate,
		OriginSourceType source)
	{
		PublicationActivity recordForUpdate = _mapper.Map<PublicationActivity>(importRecord);
		recordForUpdate.Id = currRecord.Id;
		recordForUpdate.PublicationId = currRecord.PublicationId;
		await UpdateRecordAsync<PublicationActivity, UpdatePublicationActivityCommand>(
				recordForUpdate,
				versionDate,
				source);
	}
}
