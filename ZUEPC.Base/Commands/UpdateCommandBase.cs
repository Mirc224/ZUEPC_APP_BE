namespace ZUEPC.Base.Commands;

public abstract class UpdateCommandBase<TId>
{
	public TId Id { get; set; }
}
