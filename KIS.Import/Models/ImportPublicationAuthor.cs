namespace ZUEPC.Import.Models;

public class ImportPublicationAuthor
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public double? ContributionRatio { get; set; }
	public string? Role { get; set; }
	public ImportPerson Person { get; set; }
	public ImportInstitution ReportingInstitution { get; set; }

	public string? ContributionRatioString
	{
		set
		{
			ContributionRatio = default;
			string currentDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			var convertedVal = value?.Replace(",", currentDecimalSeparator).Replace(".", currentDecimalSeparator);
			if (double.TryParse(convertedVal, out var tmpNumber))
			{
				ContributionRatio = tmpNumber;
			}
		}
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
