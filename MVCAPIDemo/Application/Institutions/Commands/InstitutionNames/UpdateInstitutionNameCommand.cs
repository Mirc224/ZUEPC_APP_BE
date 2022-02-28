﻿using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.InstitutionNames;

public class UpdateInstitutionNameCommand : EPCUpdateBaseCommand, IRequest<UpdateInstitutionNameCommandResponse>
{
	public string? NameType { get; set; }
	public string? Name { get; set; }
}
