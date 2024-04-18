namespace Recruiting_Company_Web_API.Services
{
	public class ServiceResult<T> : ServiceResultBase
	{
		public T? Value { get; private set; }

		public static ServiceResult<T> Failure(ServiceErrorType type, string? message = null)
		{
			return new ServiceResult<T>
			{
				Error = new ServiceError
				{
					ErrorType = type,
					Message = message
				}
			};
		}

		public static ServiceResult<T> Success(T value)
		{
			return new ServiceResult<T>
			{
				Succeded = true,
				Value = value
			};
		}
	}
}
