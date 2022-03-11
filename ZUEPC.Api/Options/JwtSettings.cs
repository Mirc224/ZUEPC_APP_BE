namespace ZUEPC.Options;

public class JwtSettings
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public string Secret { get; set; }
	public TimeSpan TokenLifetime { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}
