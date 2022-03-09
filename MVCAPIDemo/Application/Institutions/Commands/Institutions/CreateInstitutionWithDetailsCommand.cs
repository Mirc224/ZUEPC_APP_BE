﻿using MediatR;
using ZUEPC.Application.Institutions.Entities.Inputs.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Entities.Inputs.InstitutionNames;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class CreateInstitutionWithDetailsCommand : EPCCreateBaseCommand, IRequest<CreateInstitutionWithDetailsCommandResponse>
{
	public int? Level { get; set; }
	public string? InstitutionType { get; set; }
	public IEnumerable<InstitutionNameCreateDto>? Names { get; set; }
	public IEnumerable<InstitutionExternDatabaseIdCreateDto>? ExternDatabaseIds { get; set; }
}