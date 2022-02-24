using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Text;
using ZUEPC.Application.Import.Commands;
using ZUEPC.Application.Import.Validators;
using ZUEPC.Localization;

namespace ZUEPC.Application.Import.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ImportController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IStringLocalizer<DataAnnotations> _localizer;

		public ImportController(IMediator mediator, IStringLocalizer<DataAnnotations> localizer)
		{
			_mediator = mediator;
			_localizer = localizer;
		}

		[HttpPost("crepc")]
		public async Task<IActionResult> ImportCREPC()
		{
			var request = new ImportCREPCXmlCommand();
			using(var reader = new StreamReader(Request.Body, Encoding.UTF8))
			{
				request.XMLBody = await reader.ReadToEndAsync();
			}
			var validationResult = ValidateImportXmlCommand(request);
			if (validationResult != null)
			{
				return validationResult;
			}

			var response = await _mediator.Send(request);
			if (!response.Success)
				return StatusCode(500);
			return Ok();
		}

		[HttpPost("dawinci")]
		public async Task<IActionResult> ImportDaWinci()
		{
			var request = new ImportDaWinciXmlCommand();
			using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
			{
				request.XMLBody = await reader.ReadToEndAsync();
			}
			var validationResult = ValidateImportXmlCommand(request);
			if (validationResult != null)
			{
				return validationResult;
			}

			var response = await _mediator.Send(request);
			if (!response.Success)
				return StatusCode(500);
			return Ok();
		}

		private IActionResult? ValidateImportXmlCommand(ImportXmlBaseCommand command)
		{
			var validationResult = new ImportXmlBaseCommandValidator(_localizer).Validate(command);
			if (!validationResult.IsValid)
			{
				foreach (var error in validationResult.Errors)
				{
					ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
				}
				return UnprocessableEntity(ModelState);
			}
			return null;
		}
	}
}