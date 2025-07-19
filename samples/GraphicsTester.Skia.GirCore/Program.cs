using GraphicsTester.Scenarios;
using Gtk.MauiGraphicsSkia.GirCore;
using System.Runtime.Versioning;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
internal class Program
{
	internal const string ApplicationId = "gir.core.mauigraphics";
	private static void Main(string[] _)
	{
		Adw.Module.Initialize();
		Gtk.Module.Initialize();
		GdkPixbuf.Module.Initialize();
		Cairo.Module.Initialize();
		Graphene.Module.Initialize();

		SkiaSharp.Views.GirCore.Module.Initialize();

		var items = new Dictionary<string, AbstractScenario>();

		var application = Adw.Application.New(ApplicationId, Gio.ApplicationFlags.FlagsNone);
		application.OnActivate += (sender, args) =>
		{
			var skiaGraphicsView = new GirCoreSkiaGraphicsView
			{
				Vexpand = true,
				Hexpand = true,
			};

			var window = Adw.ApplicationWindow.New((Adw.Application)sender);
			window.SetDefaultSize(900, 900);

			var splitView = Adw.NavigationSplitView.New();

			// Breakpoint beállítása a sidebar automatikus elrejtéséhez
			var breakpoint = Adw.Breakpoint.New(Adw.BreakpointCondition.Parse("max-width: 400sp"));
			breakpoint.AddSetter(splitView, "collapsed", new GObject.Value(true));
			breakpoint.AddSetter(splitView, "show-content", new GObject.Value(true));
			window.AddBreakpoint(breakpoint);

			// Kezdeti állapot beállítása
			splitView.SetCollapsed(false);
			splitView.SetShowContent(true);

			var sidebarPage = CreateSidebarPage();
			splitView.SetSidebar(sidebarPage);

			var contentPage = CreateContentPage();
			splitView.SetContent(contentPage);

			window.Content = splitView;

			window.OnNotify += (sender, args) =>
			{
				if (args.Pspec.GetName() == "default-width")
				{
					var allocation = window.GetAllocatedWidth();
					if (allocation < 400)
					{
						splitView.SetCollapsed(true);
						splitView.SetShowContent(true);
					}
					else
					{
						splitView.SetCollapsed(false);
						splitView.SetShowContent(true);
					}
				}
			};

			window.Show();

			Adw.NavigationPage CreateSidebarPage()
			{
				var headerBar = Adw.HeaderBar.New();

				var listBox = Gtk.ListBox.New();
				listBox.SetSelectionMode(Gtk.SelectionMode.Single);

				foreach (var scenario in ScenarioList.Scenarios)
				{
					var row = Adw.ActionRow.New();
					row.SetTitle(scenario.ToString());
					listBox.Append(row);
				}
				listBox.SelectRow(listBox.GetRowAtIndex(0));

				listBox.OnRowSelected += (sender, args) =>
				{
					if (args.Row != null)
					{
						var index = args.Row.GetIndex();
						skiaGraphicsView.Drawable = ScenarioList.Scenarios[index];

						if (splitView.GetCollapsed())
						{
							splitView.SetShowContent(true);
						}
					}
				};

				var scroll = Gtk.ScrolledWindow.New();
				scroll.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
				scroll.Child = listBox;
				scroll.Vexpand = true;
				scroll.Hexpand = true;

				var toolbarView = Adw.ToolbarView.New();
				toolbarView.AddTopBar(headerBar);
				toolbarView.Content = scroll;

				var page = new Adw.NavigationPage();
				page.SetTitle("Samples");
				page.SetTag("sidebar");
				page.SetChild(toolbarView);

				return page;
			}

			Adw.NavigationPage CreateContentPage()
			{
				// White background
				var provider = Gtk.CssProvider.New();
				var css = ".white-skia-demo { background-color: white; }";
				provider.LoadFromData(css, css.Length);
				Gtk.StyleContext.AddProviderForDisplay(Gdk.Display.GetDefault()!, provider, Gtk.Constants.STYLE_PROVIDER_PRIORITY_APPLICATION);

				var scroll = Gtk.ScrolledWindow.New();
				scroll.SetCssClasses(["white-skia-demo"]);

				scroll.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
				scroll.Child = skiaGraphicsView;
				scroll.Vexpand = true;
				scroll.Hexpand = true;

				var headerBar = Adw.HeaderBar.New();
				var toolbarView = Adw.ToolbarView.New();
				toolbarView.AddTopBar(headerBar);
				toolbarView.Content = scroll;

				var page = new Adw.NavigationPage();
				page.SetTitle("GirCore Maui Graphics SkiaSharp");
				page.SetTag("content");
				page.SetChild(toolbarView);

				return page;
			}
		};
		application.Run(0, null);
	}
}