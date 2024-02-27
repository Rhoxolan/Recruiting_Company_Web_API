namespace Recruiting_Company_Web_API.Types.Exceptions
{
	public class RecruitingCompanyAuthenticationException : Exception
	{
		public RecruitingCompanyAuthenticationException()
		{
		}

		public RecruitingCompanyAuthenticationException(string message)
			: base(message)
		{
		}

		public RecruitingCompanyAuthenticationException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
