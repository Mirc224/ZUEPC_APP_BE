namespace ZUEPC.Base.Commands;

public abstract class DeleteModelCommandBase<TId>
{
	public TId Id { get; set; }
}
