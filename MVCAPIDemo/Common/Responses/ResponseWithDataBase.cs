namespace ZUEPC.Common.Responses;

public class ResponseWithDataBase<T> : ResponseBase
{
	public T? Data { get; set; }
}
