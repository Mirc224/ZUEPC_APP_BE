using AutoMapper;
using Users.Base.Application.Domain;
using Users.Base.Domain;
using ZUEPC.Application.Auth.Commands;
using ZUEPC.Application.Institutions.Commands.Institutions;
using ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Commands.PersonNames;
using ZUEPC.Application.Persons.Commands.Persons;
using ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Commands.PublicationIdentifiers;
using ZUEPC.Application.Publications.Commands.PublicationNames;
using ZUEPC.Application.Publications.Commands.Publications;
using ZUEPC.Auth.Domain;
using ZUEPC.DataAccess.Models.Common;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.DataAccess.Models.PublicationActivity;
using ZUEPC.DataAccess.Models.PublicationAuthor;
using ZUEPC.DataAccess.Models.RelatedPublication;
using ZUEPC.DataAccess.Models.Users;
using ZUEPC.EvidencePublication.Base.Commands;
using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;
using ZUEPC.EvidencePublication.Base.Domain.Persons;
using ZUEPC.EvidencePublication.Base.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.EvidencePublication.Base.Domain.RelatedPublications;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;
using ZUEPC.Import.Models;
using ZUEPC.Import.Models.Commond;
using static ZUEPC.Import.Models.ImportInstitution;
using static ZUEPC.Import.Models.ImportPerson;
using static ZUEPC.Import.Models.ImportPublication;

namespace ZUEPC.Application.Mapping;

public class DomainToResponseProfile : Profile
{
	public DomainToResponseProfile()
	{
		CreateBaseClassesMapping();
		CreateUserMapping();
		CreateImportToDomainMapping();

		CreateModelToDomainMapping();
		CreateImportToCommandMapping();

		CreateDomainToCommandMapping();
		CreateCommandToModelMapping();
	}

	private void CreateUserMapping()
	{
		CreateMap<UserModel, User>()
			.ReverseMap();
		CreateMap<RegisterUserCommand, UserModel>();
		CreateMap<RoleModel, Role>();
		CreateMap<AuthResult, LoginUserCommandResponse>();
		CreateMap<AuthResult, RefreshTokenCommandResponse>();
		CreateMap<RevokeResult, RevokeTokenCommandResponse>();
		CreateMap<RevokeResult, LogoutUserCommandResponse>();
	}

	private void CreateImportToDomainMapping()
	{
		// Publication
		CreateMap<ImportPublication, Publication>();
		CreateMap<ImportPublicationIdentifier, PublicationIdentifier>();
		CreateMap<ImportPublicationExternDatabaseId, PublicationExternDatabaseId>()
			.IncludeBase<EPCImportExternDatabaseIdBase, EPCExternDatabaseIdBase>();
		  CreateMap<ImportPublicationName, PublicationName>();

		// Person
		CreateMap<ImportPerson, Person>();
		CreateMap<ImportPersonExternDatabaseId, PersonExternDatabaseId>()
			.IncludeBase<EPCImportExternDatabaseIdBase, EPCExternDatabaseIdBase>();
		CreateMap<ImportPersonName, PersonName>();

		// Institution
		CreateMap<ImportInstitution, Institution>();
		CreateMap<ImportInstitutionExternDatabaseId, InstitutionExternDatabaseId>()
			.IncludeBase<EPCImportExternDatabaseIdBase, EPCExternDatabaseIdBase>();
		CreateMap<ImportInstitutionName, InstitutionName>();
	}

	private void CreateBaseClassesMapping()
	{
		CreateMap<EPCBaseModel, EPCBase>()
			.ReverseMap();

		CreateMap<EPCExternDatabaseIdBaseModel, EPCExternDatabaseIdBase>()
			.ReverseMap();

		CreateMap<EPCImportExternDatabaseIdBase, EPCExternDatabaseIdBase>()
			.ReverseMap();

		// Base command to model mapping
		CreateMap<EPCCreateBaseCommand, EPCBaseModel>()
			.ReverseMap();
		CreateMap<EPCUpdateBaseCommand, EPCBaseModel>()
			.ReverseMap();
	}

	private void CreateModelToDomainMapping()
	{
		// Publications
		CreateMap<PublicationModel, Publication>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		// PublicationIdentifiers
		CreateMap<PublicationIdentifierModel, PublicationIdentifier>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		// PublicationExternDatabaseIdentifiers
		CreateMap<PublicationExternDatabaseIdModel, PublicationExternDatabaseId>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		// PublicationName
		CreateMap<PublicationNameModel, PublicationName>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// Institutions
		CreateMap<InstitutionModel, Institution>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		// InstitutionExternDatabaseIdentifiers
		CreateMap<InstitutionExternDatabaseIdModel, InstitutionExternDatabaseId>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		// InstitutionNames
		CreateMap<InstitutionNameModel, InstitutionName>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// Persons
		CreateMap<PersonModel, Person>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		// PersonIdentifiers
		CreateMap<PersonExternDatabaseIdModel, PersonExternDatabaseId>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		// PersonNames
		CreateMap<PersonNameModel, PersonName>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// PublicationActivities
		CreateMap<PublicationActivityModel, PublicationActivity>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// PublicationAuthors
		CreateMap<PublicationAuthorModel, PublicationAuthor>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// RelatedPublications
		CreateMap<RelatedPublicationModel, RelatedPublication>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
	}

	private void CreateImportToCommandMapping()
	{
		// Base doamin to command mapping
		CreateMap<EPCBase, EPCCreateBaseCommand>();
		// Base doamin to command mapping
		CreateMap<EPCBase, EPCUpdateBaseCommand>();


		// ImportPublication
		CreateMap<ImportPublication, CreatePublicationCommand>();
		//CreateMap<ImportPublication, UpdatePublicationCommand>();

		//// ImportPublicationIdentifier
		//CreateMap<ImportPublicationIdentifier, CreatePublicationIdentifierCommand>();
		//CreateMap<ImportPublicationIdentifier, UpdatePublicationIdentifierCommand>();

		//// ImportPublicationExternDatabaseId
		//CreateMap<ImportPublicationExternDatabaseId, CreatePublicationExternDatabaseIdCommand>();
		//CreateMap<ImportPublicationExternDatabaseId, UpdatePublicationExternDatabaseIdCommand>();

		// ImportPerson
		CreateMap<ImportPerson, CreatePersonCommand>();
		//CreateMap<ImportPerson, UpdatePersonCommand>();

		//// ImportPersonExternDatabaseId
		//CreateMap<ImportPersonExternDatabaseId, CreatePersonExternDatabaseIdCommand>();
		//CreateMap<ImportPersonExternDatabaseId, UpdatePersonExternDatabaseIdCommand>();

		//// ImportPersonName
		//CreateMap<ImportPersonName, CreatePersonNameCommand>();
		//CreateMap<ImportPersonName, UpdatePersonNameCommand>();
		// ImportInstitution
		CreateMap<ImportInstitution, CreateInstitutionCommand>();
	}

	private void CreateDomainToCommandMapping()
	{
		// Publication
		//CreateMap<Publication, CreatePublicationCommand>()
		//	.IncludeBase<EPCBase, EPCCreateBaseCommand>();
		CreateMap<Publication, UpdatePublicationCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>();

		// PublicationIdentifier
		CreateMap<PublicationIdentifier, CreatePublicationIdentifierCommand>()
			.IncludeBase<EPCBase, EPCCreateBaseCommand>();
		CreateMap<PublicationIdentifier, UpdatePublicationIdentifierCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>();

		// PublicationExternDatabaseId
		CreateMap<PublicationExternDatabaseId, CreatePublicationExternDatabaseIdCommand>()
			.IncludeBase<EPCBase, EPCCreateBaseCommand>();
		CreateMap<PublicationExternDatabaseId, UpdatePublicationExternDatabaseIdCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>();

		// PublicationName
		CreateMap<PublicationName, CreatePublicationNameCommand>()
			.IncludeBase<EPCBase, EPCCreateBaseCommand>();
		CreateMap<PublicationName, UpdatePublicationNameCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>();


		// Person
		//CreateMap<Person, CreatePersonCommand>()
		//	.IncludeBase<EPCBase, EPCCreateBaseCommand>();
		CreateMap<Person, UpdatePersonCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>();

		// PersonExternDatabaseId
		CreateMap<PersonExternDatabaseId, CreatePersonExternDatabaseIdCommand>()
			.IncludeBase<EPCBase, EPCCreateBaseCommand>();
		CreateMap<PersonExternDatabaseId, UpdatePersonExternDatabaseIdCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>();

		// PersonName
		CreateMap<PersonName, CreatePersonNameCommand>()
			.IncludeBase<EPCBase, EPCCreateBaseCommand>();
		CreateMap<PersonName, UpdatePersonNameCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>();

		// Institution
		//CreateMap<Institution, CreateInstitutionCommand>()
		//	.IncludeBase<EPCBase, EPCCreateBaseCommand>();
		CreateMap<Institution, UpdateInstitutionCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>();
	}

	private void CreateCommandToModelMapping()
	{
		// Publication 
		CreateMap<CreatePublicationCommand, PublicationModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>();
		CreateMap<UpdatePublicationCommand, PublicationModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>();

		// PublicationIdentifier
		CreateMap<CreatePublicationIdentifierCommand, PublicationIdentifierModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>();
		CreateMap<UpdatePublicationIdentifierCommand, PublicationIdentifierModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>();

		// PublicationExternIdentifier
		CreateMap<CreatePublicationExternDatabaseIdCommand, PublicationExternDatabaseIdModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>();
		CreateMap<UpdatePublicationExternDatabaseIdCommand, PublicationExternDatabaseIdModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>();

		// PublicationName
		CreateMap<CreatePublicationNameCommand, PublicationNameModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>();
		CreateMap<UpdatePublicationNameCommand, PublicationNameModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>();

		// Person
		CreateMap<CreatePersonCommand, PersonModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>();
		CreateMap<UpdatePersonCommand, PersonModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>();

		// PersonExternDatabaseId
		CreateMap<CreatePersonExternDatabaseIdCommand, PersonExternDatabaseIdModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>();
		CreateMap<UpdatePersonExternDatabaseIdCommand, PersonExternDatabaseIdModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>();

		// PersonName
		CreateMap<CreatePersonNameCommand, PersonNameModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>();
		CreateMap<UpdatePersonNameCommand, PersonNameModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>();

		// Institution
		CreateMap<CreateInstitutionCommand, InstitutionModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>();
		CreateMap<UpdateInstitutionCommand, InstitutionModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>();
	}
}

