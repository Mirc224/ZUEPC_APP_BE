using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class CreateInstitutionCommandResponse : ResponseBase
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public Institution Institution { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}