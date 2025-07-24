using System.Runtime.Versioning;

namespace GirCoreSample.VisualTest.Tabs;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Adw.Bin, IDisposable
{
	private readonly Lines.Basic.View linesBasicView;
	private readonly Bars.Basic.View barsBasicView;

	public View()
	{
		var viewSwitcher = Adw.ViewSwitcher.New();

		viewSwitcher.Stack = Adw.ViewStack.New();
		viewSwitcher.Stack.Vexpand = true;
		viewSwitcher.Stack.Hexpand = true;
		viewSwitcher.Policy = Adw.ViewSwitcherPolicy.Wide;

		var headerBar = Adw.HeaderBar.New();
		headerBar.SetDecorationLayout(string.Empty);
		headerBar.SetTitleWidget(viewSwitcher);

		var toolbarView = Adw.ToolbarView.New();
		toolbarView.AddTopBar(headerBar);
		toolbarView.Content = viewSwitcher.Stack;

		viewSwitcher.Stack.AddTitledWithIcon(
			linesBasicView = new Lines.Basic.View(),
			Guid.NewGuid().ToString(),
			"Tab 1",
			"office-chart-line-stacked-symbolic"
		);

		viewSwitcher.Stack.AddTitledWithIcon(
			barsBasicView = new Bars.Basic.View(),
			Guid.NewGuid().ToString(),
			"Tab 2",
			"office-chart-bar-stacked-symbolic"
		);

		Child = toolbarView;
	}

	public override void Dispose()
	{
		linesBasicView.Dispose();
		barsBasicView.Dispose();
		base.Dispose();
	}
}
