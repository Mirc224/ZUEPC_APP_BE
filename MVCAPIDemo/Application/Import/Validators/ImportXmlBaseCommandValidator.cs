using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Xml.Linq;
using ZUEPC.Application.Import.Commands;
using ZUEPC.Localization;

namespace ZUEPC.Application.Import.Validators;

public class ImportXmlBaseCommandValidator : AbstractValidator<ImportXmlBaseCommand>
{
	public ImportXmlBaseCommandValidator(IStringLocalizer<DataAnnotations> localizer)
	{
		//RuleFor(x => x.XMLBody)
		//	.Must(x => IsValidXMLDocument(x))
		//	.WithMessage(localizer["XMLDocumentNotValid"]);
	}

	private bool IsValidXMLDocument(string? docString)
	{
		if (docString is null)
		{
			return false;
		}
		try
		{
			XDocument.Parse(docString);
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}
}
