namespace Resultix
{
	public class ResultConfig
	{
		/// <summary>
		/// Setting this to true will cause an exception to be thrown if null is passed in for one of the lambdas in the match functions.
		/// </summary>
		public static bool EnforceMatches { get; set; }
	}
}