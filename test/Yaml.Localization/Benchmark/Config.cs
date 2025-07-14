namespace Benchmark;

public class Config
{
	public static IEnumerable<object[]> HelloWorld()
	{
		yield return new object[] { "en", "Hello World!", "Hello World!" };
		yield return new object[] { "de-DE", "Hello World!", "Hallo Welt!" };
		yield return new object[] { "hu", "Hello World!", "Helló Világ!" };
	}
}