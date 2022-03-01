using AutoMapper;
using Users.Base.Application.Domain;
using Users.Base.Domain;
using ZUEPC.Application.Auth.Commands;
using ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Commands.InstitutionNames;
using ZUEPC.Application.Institutions.Commands.Institutions;
using ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Commands.PersonNames;
using ZUEPC.Application.Persons.Commands.Persons;
using ZUEPC.Application.PublicationActivities.Commands;
using ZUEPC.Application.PublicationAuthors.Commands;
using ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Commands.PublicationIdentifiers;
using ZUEPC.Application.Publications.Commands.PublicationNames;
using ZUEPC.Application.Publications.Commands.Publications;
using ZUEPC.Application.RelatedPublications.Commands;
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

public class MappingProfile : Profile
{
	public MappingProfile()
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
		CreateMap<ImportPublication, Publication>()
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType))
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear))
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear));
		CreateMap<ImportPublicationIdentifier, PublicationIdentifier>()
			.ForMember(dest => dest.IdentifierValue, opts => opts.MapFrom(src => src.IdentifierValue))
			.ForMember(dest => dest.IdentifierName, opts => opts.MapFrom(src => src.IdentifierName))
			.ForMember(dest => dest.ISForm, opts => opts.MapFrom(src => src.ISForm));
		CreateMap<ImportPublicationExternDatabaseId, PublicationExternDatabaseId>()
			.IncludeBase<EPCImportExternDatabaseIdBase, EPCExternDatabaseIdBase>();
		CreateMap<ImportPublicationName, PublicationName>();

		// Person
		CreateMap<ImportPerson, Person>()
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear));
		CreateMap<ImportPersonExternDatabaseId, PersonExternDatabaseId>()
			.IncludeBase<EPCImportExternDatabaseIdBase, EPCExternDatabaseIdBase>();
		CreateMap<ImportPersonName, PersonName>()
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName));

		// Institution
		CreateMap<ImportInstitution, Institution>()
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstitutionType, opts => opts.MapFrom(src => src.InstititutionType));
		CreateMap<ImportInstitutionExternDatabaseId, InstitutionExternDatabaseId>()
			.IncludeBase<EPCImportExternDatabaseIdBase, EPCExternDatabaseIdBase>();
		CreateMap<ImportInstitutionName, InstitutionName>()
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));

		// PublicationAuthor
		CreateMap<ImportPublicationAuthor, PublicationAuthor>()
			.ForMember(dest => dest.ContributionRatio, opts => opts.MapFrom(src => src.ContributionRatio))
			.ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role));

		// RelatedPublication
		CreateMap<ImportRelatedPublication, RelatedPublication>()
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType))
			.ForMember(dest => dest.CitationCategory, opts => opts.MapFrom(src => src.CitationCategory));

		// PublicationActivity
		CreateMap<ImportPublicationActivity, PublicationActivity>()
			.ForMember(dest => dest.ActivityYear, opts => opts.MapFrom(src => src.ActivityYear))
			.ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
			.ForMember(dest => dest.GovernmentGrant, opts => opts.MapFrom(src => src.GovernmentGrant));
	}

	private void CreateBaseClassesMapping()
	{
		CreateMap<EPCBaseModel, EPCBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.VersionDate, opts => opts.MapFrom(src => src.VersionDate))
			.ForMember(dest => dest.OriginSourceType, opts => opts.MapFrom(src => src.OriginSourceType))
			.ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => src.CreatedAt))
			.ReverseMap();

		CreateMap<EPCExternDatabaseIdBaseModel, EPCExternDatabaseIdBase>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue))
			.ReverseMap();

		CreateMap<EPCImportExternDatabaseIdBase, EPCExternDatabaseIdBase>()
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue))
			.ReverseMap();

		// Base command to model mapping
		CreateMap<EPCCreateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.VersionDate, opts => opts.MapFrom(src => src.VersionDate))
			.ForMember(dest => dest.OriginSourceType, opts => opts.MapFrom(src => src.OriginSourceType))
			.ReverseMap();
		CreateMap<EPCUpdateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.OriginSourceType, opts => opts.MapFrom(src => src.OriginSourceType))
			.ForMember(dest => dest.VersionDate, opts => opts.MapFrom(src => src.VersionDate))
			.ReverseMap();
	}

	private void CreateModelToDomainMapping()
	{
		// Publications
		CreateMap<PublicationModel, Publication>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear))
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType))
			.ReverseMap();
		// PublicationIdentifiers
		CreateMap<PublicationIdentifierModel, PublicationIdentifier>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.IdentifierValue, opts => opts.MapFrom(src => src.IdentifierValue))
			.ForMember(dest => dest.IdentifierName, opts => opts.MapFrom(src => src.IdentifierName))
			.ForMember(dest => dest.ISForm, opts => opts.MapFrom(src => src.ISForm))
			.ReverseMap();
		// PublicationExternDatabaseIdentifiers
		CreateMap<PublicationExternDatabaseIdModel, PublicationExternDatabaseId>()
			 .IncludeBase<EPCExternDatabaseIdBaseModel, EPCExternDatabaseIdBase>()
			 .ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			 .ReverseMap();
		// PublicationName
		CreateMap<PublicationNameModel, PublicationName>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ReverseMap();

		// Institutions
		CreateMap<InstitutionModel, Institution>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstitutionType, opts => opts.MapFrom(src => src.InstitutionType))
			.ReverseMap();
		// InstitutionExternDatabaseIdentifiers
		CreateMap<InstitutionExternDatabaseIdModel, InstitutionExternDatabaseId>()
			.IncludeBase<EPCExternDatabaseIdBaseModel, EPCExternDatabaseIdBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ReverseMap();
		// InstitutionNames
		CreateMap<InstitutionNameModel, InstitutionName>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ReverseMap();

		// Persons
		CreateMap<PersonModel, Person>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear))
			.ReverseMap();
		// PersonIdentifiers
		CreateMap<PersonExternDatabaseIdModel, PersonExternDatabaseId>()
			.IncludeBase<EPCExternDatabaseIdBaseModel, EPCExternDatabaseIdBase>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ReverseMap();
		// PersonNames
		CreateMap<PersonNameModel, PersonName>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ReverseMap();

		// PublicationActivities
		CreateMap<PublicationActivityModel, PublicationActivity>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
			.ForMember(dest => dest.GovernmentGrant, opts => opts.MapFrom(src => src.GovernmentGrant))
			.ForMember(dest => dest.ActivityYear, opts => opts.MapFrom(src => src.ActivityYear))
			.ReverseMap();

		// PublicationAuthors
		CreateMap<PublicationAuthorModel, PublicationAuthor>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ContributionRatio, opts => opts.MapFrom(src => src.ContributionRatio))
			.ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role))
			.ReverseMap();

		// RelatedPublications
		CreateMap<RelatedPublicationModel, RelatedPublication>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.RelatedPublicationId, opts => opts.MapFrom(src => src.RelatedPublicationId))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType))
			.ForMember(dest => dest.CitationCategory, opts => opts.MapFrom(src => src.CitationCategory))
			.ReverseMap();
	}

	private void CreateImportToCommandMapping()
	{
		// Base doamin to command mapping
		CreateMap<EPCBase, EPCCreateBaseCommand>()
			.ForMember(dest => dest.VersionDate, opts => opts.MapFrom(src => src.VersionDate))
			.ForMember(dest => dest.OriginSourceType, opts => opts.MapFrom(src => src.OriginSourceType));
		// Base doamin to command mapping
		CreateMap<EPCBase, EPCUpdateBaseCommand>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.VersionDate, opts => opts.MapFrom(src => src.VersionDate))
			.ForMember(dest => dest.OriginSourceType, opts => opts.MapFrom(src => src.OriginSourceType));

		

		// ImportPublication
		CreateMap<ImportPublication, CreatePublicationCommand>()
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType))
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear));

		// ImportPerson
		CreateMap<ImportPerson, CreatePersonCommand>()
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear));

		// ImportInstitution
		CreateMap<ImportInstitution, CreateInstitutionCommand>()
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstititutionType, opts => opts.MapFrom(src => src.InstititutionType));
	}

	private void CreateDomainToCommandMapping()
	{
		// Publication
		CreateMap<Publication, UpdatePublicationCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>()
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType))
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear));

		// PublicationIdentifier
		CreateMap<PublicationIdentifier, CreatePublicationIdentifierCommand>()
			.IncludeBase<EPCBase, EPCCreateBaseCommand>()
			.ForMember(dest => dest.IdentifierName, opts => opts.MapFrom(src => src.IdentifierName))
			.ForMember(dest => dest.IdentifierValue, opts => opts.MapFrom(src => src.IdentifierValue))
			.ForMember(dest => dest.ISForm, opts => opts.MapFrom(src => src.ISForm));
		CreateMap<PublicationIdentifier, UpdatePublicationIdentifierCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>()
			.ForMember(dest => dest.IdentifierName, opts => opts.MapFrom(src => src.IdentifierName))
			.ForMember(dest => dest.IdentifierValue, opts => opts.MapFrom(src => src.IdentifierValue))
			.ForMember(dest => dest.ISForm, opts => opts.MapFrom(src => src.ISForm));

		// PublicationExternDatabaseId
		CreateMap<PublicationExternDatabaseId, CreatePublicationExternDatabaseIdCommand>()
			.IncludeBase<EPCBase, EPCCreateBaseCommand>()
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId));
		CreateMap<PublicationExternDatabaseId, UpdatePublicationExternDatabaseIdCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>()
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId));

		// PublicationName
		CreateMap<PublicationName, CreatePublicationNameCommand>()
			.IncludeBase<EPCBase, EPCCreateBaseCommand>()
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId));
		CreateMap<PublicationName, UpdatePublicationNameCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>()
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId));

		// Person
		CreateMap<Person, UpdatePersonCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>()
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear));
			
		// PersonExternDatabaseId
		CreateMap<PersonExternDatabaseId, CreatePersonExternDatabaseIdCommand>()
			.IncludeBase<EPCBase, EPCCreateBaseCommand>()
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId));
		CreateMap<PersonExternDatabaseId, UpdatePersonExternDatabaseIdCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>()
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId));

		// PersonName
		CreateMap<PersonName, CreatePersonNameCommand>()
			.IncludeBase<EPCBase, EPCCreateBaseCommand>()
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName));
		CreateMap<PersonName, UpdatePersonNameCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>()
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName));

		// Institution
		CreateMap<Institution, UpdateInstitutionCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>()
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstititutionType, opts => opts.MapFrom(src => src.InstitutionType));

		//  InstitutionExternDatabaseId
		CreateMap<InstitutionExternDatabaseId, CreateInstitutionExternDatabaseIdCommand>()
			.IncludeBase<EPCBase, EPCCreateBaseCommand>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));
		CreateMap<InstitutionExternDatabaseId, UpdateInstitutionExternDatabaseIdCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));

		// InstitutionName
		CreateMap<InstitutionName, CreateInstitutionNameCommand>()
			.IncludeBase<EPCBase, EPCCreateBaseCommand>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));
		CreateMap<InstitutionName, UpdateInstitutionNameCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));

		// PublicationAuthor
		CreateMap<PublicationAuthor, CreatePublicationAuthorCommand>()
			.IncludeBase<EPCBase, EPCCreateBaseCommand>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ContributionRatio, opts => opts.MapFrom(src => src.ContributionRatio))
			.ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role));
		CreateMap<PublicationAuthor, UpdatePublicationAuthorCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ContributionRatio, opts => opts.MapFrom(src => src.ContributionRatio))
			.ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role));

		// RelatedPublication
		CreateMap<RelatedPublication, CreateRelatedPublicationCommand>()
			.IncludeBase<EPCBase, EPCCreateBaseCommand>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.RelatedPublicationId, opts => opts.MapFrom(src => src.RelatedPublicationId))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType))
			.ForMember(dest => dest.CitationCategory, opts => opts.MapFrom(src => src.CitationCategory))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType));
		CreateMap<RelatedPublication, UpdateRelatedPublicationCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.RelatedPublicationId, opts => opts.MapFrom(src => src.RelatedPublicationId))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType))
			.ForMember(dest => dest.CitationCategory, opts => opts.MapFrom(src => src.CitationCategory))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType));

		// PublicationActivity
		CreateMap<PublicationActivity, CreatePublicationActivityCommand>()
			.IncludeBase<EPCBase, EPCCreateBaseCommand>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
			.ForMember(dest => dest.ActivityYear, opts => opts.MapFrom(src => src.ActivityYear))
			.ForMember(dest => dest.GovernmentGrant, opts => opts.MapFrom(src => src.GovernmentGrant));
		CreateMap<PublicationActivity, UpdatePublicationActivityCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
			.ForMember(dest => dest.ActivityYear, opts => opts.MapFrom(src => src.ActivityYear))
			.ForMember(dest => dest.GovernmentGrant, opts => opts.MapFrom(src => src.GovernmentGrant));
	}

	private void CreateCommandToModelMapping()
	{
		// Publication 
		CreateMap<CreatePublicationCommand, PublicationModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear))
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType));
		CreateMap<UpdatePublicationCommand, PublicationModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear))
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType));
		// PublicationIdentifier
		CreateMap<CreatePublicationIdentifierCommand, PublicationIdentifierModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.IdentifierValue, opts => opts.MapFrom(src => src.IdentifierValue))
			.ForMember(dest => dest.ISForm, opts => opts.MapFrom(src => src.ISForm))
			.ForMember(dest => dest.IdentifierName, opts => opts.MapFrom(src => src.IdentifierName));
		CreateMap<UpdatePublicationIdentifierCommand, PublicationIdentifierModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.IdentifierValue, opts => opts.MapFrom(src => src.IdentifierValue))
			.ForMember(dest => dest.ISForm, opts => opts.MapFrom(src => src.ISForm))
			.ForMember(dest => dest.IdentifierName, opts => opts.MapFrom(src => src.IdentifierName));

		// PublicationExternIdentifier
		CreateMap<CreatePublicationExternDatabaseIdCommand, PublicationExternDatabaseIdModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));
		CreateMap<UpdatePublicationExternDatabaseIdCommand, PublicationExternDatabaseIdModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));

		// PublicationName
		CreateMap<CreatePublicationNameCommand, PublicationNameModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));
		CreateMap<UpdatePublicationNameCommand, PublicationNameModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));

		// Person
		CreateMap<CreatePersonCommand, PersonModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear));
		CreateMap<UpdatePersonCommand, PersonModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear));

		// PersonExternDatabaseId
		CreateMap<CreatePersonExternDatabaseIdCommand, PersonExternDatabaseIdModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));
		CreateMap<UpdatePersonExternDatabaseIdCommand, PersonExternDatabaseIdModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));

		// PersonName
		CreateMap<CreatePersonNameCommand, PersonNameModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));
		CreateMap<UpdatePersonNameCommand, PersonNameModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));

		// Institution
		CreateMap<CreateInstitutionCommand, InstitutionModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstitutionType, opts => opts.MapFrom(src => src.InstititutionType));
		CreateMap<UpdateInstitutionCommand, InstitutionModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstitutionType, opts => opts.MapFrom(src => src.InstititutionType));

		// InstitutionExternDatabaseId
		CreateMap<CreateInstitutionExternDatabaseIdCommand, InstitutionExternDatabaseIdModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));
		CreateMap<UpdateInstitutionExternDatabaseIdCommand, InstitutionExternDatabaseIdModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));

		// InstitutionName
		CreateMap<CreateInstitutionNameCommand, InstitutionNameModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));
		CreateMap<UpdateInstitutionNameCommand, InstitutionNameModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));

		// PublicationAuthor
		CreateMap<CreatePublicationAuthorCommand, PublicationAuthorModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.ContributionRatio, opts => opts.MapFrom(src => src.ContributionRatio))
			.ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role));
		CreateMap<UpdatePublicationAuthorCommand, PublicationAuthorModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.ContributionRatio, opts => opts.MapFrom(src => src.ContributionRatio))
			.ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role));

		// RelatedPublication
		CreateMap<CreateRelatedPublicationCommand, RelatedPublicationModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.RelatedPublicationId, opts => opts.MapFrom(src => src.RelatedPublicationId))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType))
			.ForMember(dest => dest.CitationCategory, opts => opts.MapFrom(src => src.CitationCategory));
		CreateMap<UpdateRelatedPublicationCommand, RelatedPublicationModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.RelatedPublicationId, opts => opts.MapFrom(src => src.RelatedPublicationId))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType))
			.ForMember(dest => dest.CitationCategory, opts => opts.MapFrom(src => src.CitationCategory));

		// PublicationActivity
		CreateMap<CreatePublicationActivityCommand, PublicationActivityModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
			.ForMember(dest => dest.ActivityYear, opts => opts.MapFrom(src => src.ActivityYear))
			.ForMember(dest => dest.GovernmentGrant, opts => opts.MapFrom(src => src.GovernmentGrant));
		CreateMap<UpdatePublicationActivityCommand, PublicationActivityModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
			.ForMember(dest => dest.ActivityYear, opts => opts.MapFrom(src => src.ActivityYear))
			.ForMember(dest => dest.GovernmentGrant, opts => opts.MapFrom(src => src.GovernmentGrant));
	}
}

