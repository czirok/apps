namespace YamlLocalizationTests;

public class Config
{
	public static IEnumerable<object[]> HelloWorld()
	{
		yield return new object[] { "en", "Hello World!", "Hello World!" };
		yield return new object[] { "de-DE", "Hello World!", "Hallo Welt!" };
		yield return new object[] { "de", "Hello World!", "Hallo Welt!" };
		yield return new object[] { "hu", "Hello World!", "Helló Világ!" };
	}

	public static IEnumerable<object[]> Answer()
	{
		yield return new object[] { "en", "The Answer", "The Answer to the Great Question ... Of Life, the Universe and Everything ... 42", 42 };
		yield return new object[] { "de-DE", "The Answer", "Die Antwort auf die Große Frage ... nach dem Leben, dem Universum und allem ... 42", 42 };
		yield return new object[] { "de", "The Answer", "Die Antwort auf die Große Frage ... nach dem Leben, dem Universum und allem ... 42", 42 };
		yield return new object[] { "hu", "The Answer", "A Válasz a Nagy Kérdésre ... Az Élet, a Mindenség Meg Minden ... 42", 42 };
	}
}