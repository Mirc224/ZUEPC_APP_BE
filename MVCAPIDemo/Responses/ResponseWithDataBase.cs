namespace ZUEPC.Responses;

public class ResponseWithDataBase<T> : ResponseBase
{
	public T? Data { get; set; }
}
