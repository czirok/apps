namespace WebKit.BlazorWebView.GirCore;

public record BlazorWebViewOptions
{
	public required Type RootComponent { get; init; }
	public string HostPath { get; init; } = Path.Combine("wwwroot", "index.html");
	public string ContentRoot => Path.GetDirectoryName(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, HostPath)))!;
	public string RelativeHostPath => Path.GetFileName(HostPath);
}