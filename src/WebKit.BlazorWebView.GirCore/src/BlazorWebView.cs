using System.Runtime.Versioning;

namespace WebKit.BlazorWebView.GirCore;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class BlazorWebView : WebView
{
	public BlazorWebView(IServiceProvider serviceProvider)
	{
		_ = new WebKitWebViewManager(this, serviceProvider);
	}
}
