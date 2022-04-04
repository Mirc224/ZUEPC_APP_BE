namespace ZUEPC.Base.Commands;

public abstract class UpdateCommandWithIdBase<TId>
{
	public TId Id { get; set; }
}
