namespace ZUEPC.Import.Models;

public class ImportRecord
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public DateTime RecordVersionDate { get; set; }
	public ImportPublication Publication { get; set; }


	public string? RecordVersionDateString
	{
		set
		{
			if (value is null)
			{
				return;
			}
			if (!DateTime.TryParse(value, out var resultDate))
			{
				return;
			}
			RecordVersionDate = resultDate;
		}
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}