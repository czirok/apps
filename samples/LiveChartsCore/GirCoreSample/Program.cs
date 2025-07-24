using System.Runtime.Versioning;
using EasyUIBinding.GirCore;
using GirCoreSample;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
internal partial class Program
{
	internal const string ApplicationId = "LiveCharts.Samples.GirCore";
	private static void Main(string[] _)
	{
		Adw.Module.Initialize();
		GdkPixbuf.Module.Initialize();
		Cairo.Module.Initialize();

		SkiaSharp.Views.GirCore.Module.Initialize();
		EasyUIBinding.GirCore.Style.Initialize();

		var themeDetector = new ThemeDetector();
		if (themeDetector.IsDarkTheme)
			LiveCharts.Configure(config => config.AddDarkTheme());
		else
			LiveCharts.Configure(config => config.AddLightTheme());

		var application = Adw.Application.New(ApplicationId, Gio.ApplicationFlags.FlagsNone);

		application.OnActivate += (sender, args) =>
		{
			var window = Adw.ApplicationWindow.New((Adw.Application)sender);
			window.SetDefaultSize(1000, 600);

			var content = new Adw.Bin
			{
				Vexpand = true,
				Hexpand = true,
			};

			var splitView = Adw.NavigationSplitView.New();

			var breakpoint = Adw.Breakpoint.New(Adw.BreakpointCondition.Parse("max-width: 400sp"));
			breakpoint.AddSetter(splitView, "collapsed", new GObject.Value(true));
			breakpoint.AddSetter(splitView, "show-content", new GObject.Value(true));
			window.AddBreakpoint(breakpoint);

			splitView.SetCollapsed(false);
			splitView.SetShowContent(true);

			var sidebarPage = CreateSidebarPage();
			splitView.SetSidebar(sidebarPage);

			var contentPage = CreateContentPage();
			splitView.SetContent(contentPage);

			window.Content = splitView;

			window.Show();

			Adw.NavigationPage CreateSidebarPage()
			{
				var headerBar = Adw.HeaderBar.New();
				var page = new Adw.NavigationPage();
				var preferencesPage = Adw.PreferencesPage.New();
				var preferencesGroup = Adw.PreferencesGroup.New();

				var categories = new SortedDictionary<string, List<string>>();

				foreach (var (key, factory) in AOT.ViewFactories)
				{
					var parts = key.Split('.');
					var category = parts[0];
					if (!categories.TryGetValue(category, out List<string>? demo))
					{
						demo = [];
						categories[category] = demo;
					}

					demo.Add(parts[1]);
				}

				foreach (var key in categories.Keys.ToList())
				{
					categories[key].Sort(StringComparer.Ordinal);
				}

				Gtk.Label? selectedLabel = null;
				Button? defaultButton = null;
				Gtk.Label? defaultLabel = null;
				string? viewKey = null;

				themeDetector.ThemeChanged += (isDark) =>
				{
					if (viewKey is null) return;

					var disposable = content.Child as IDisposable;
					disposable?.Dispose();

					if (isDark)
						LiveCharts.Configure(config => config.AddDarkTheme());
					else
						LiveCharts.Configure(config => config.AddLightTheme());

					if (AOT.ViewFactories.TryGetValue(viewKey, out var factory))
					{
						var widget = factory();
						content.Child = widget;
					}
					else
					{
						Console.WriteLine($"View not found: {viewKey}");
					}
				};

				foreach (var (category, demos) in categories)
				{
					var expanderRow = Adw.ExpanderRow.New();
					expanderRow.SetTitle(FormatPascalCase(category));

					foreach (var demo in demos)
					{
						var label = Gtk.Label.New(FormatPascalCase(demo));
						label.SetHalign(Gtk.Align.Start);
						label.SetHexpand(true);
						label.SetMarginTop(12);
						label.SetMarginStart(12);
						label.SetMarginBottom(12);
						label.SetMarginEnd(12);
						var button = new Button($"{category}.{demo}", label)
							.OnClick((sender, args) =>
							{
								if (sender is not Button chartButton || chartButton.Row.Child is not Gtk.Label clickedLabel)
								{
									return;
								}
								selectedLabel?.RemoveCssClass("accent");

								clickedLabel.AddCssClass("accent");
								selectedLabel = clickedLabel;

								var disposable = content.Child as IDisposable;
								disposable?.Dispose();
								page.SetTitle(FormatTitle(chartButton.Name));

								try
								{
									viewKey = chartButton.Name;
									if (AOT.ViewFactories.TryGetValue(viewKey, out var factory))
									{
										var widget = factory();
										content.Child = widget;
									}
									else
									{
										Console.WriteLine($"View not found: {viewKey}");
									}
								}
								catch (Exception ex)
								{
									Console.WriteLine($"Failed to create view: {ex.Message}");
								}

								if (splitView.GetCollapsed())
								{
									splitView.SetShowContent(true);
								}
							});

						expanderRow.AddRow(button.Row);

						if (category == "General" && demo == "Scrollable")
						{
							expanderRow.SetExpanded(true);
							defaultButton = button;
							defaultLabel = label;
						}
					}

					preferencesGroup.Add(expanderRow);
				}

				if (defaultButton is not null && defaultLabel is not null)
				{
					defaultLabel.AddCssClass("accent");
					selectedLabel = defaultLabel;
					page.SetTitle(FormatTitle(defaultButton.Name));

					try
					{
						viewKey = defaultButton.Name;
						if (AOT.ViewFactories.TryGetValue(viewKey, out var factory))
						{
							var widget = factory();
							content.Child = widget;
						}
						else
						{
							Console.WriteLine($"View not found: {viewKey}");
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Failed to create view: {ex.Message}");
					}

				}


				preferencesPage.Add(preferencesGroup);

				var scroll = Gtk.ScrolledWindow.New();
				scroll.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
				scroll.Child = preferencesPage;
				scroll.Vexpand = true;
				scroll.Hexpand = true;

				var toolbarView = Adw.ToolbarView.New();
				toolbarView.AddTopBar(headerBar);
				toolbarView.Content = scroll;

				page.SetTag("sidebar");
				page.SetChild(toolbarView);

				return page;
			}

			Adw.NavigationPage CreateContentPage()
			{
				var headerBar = Adw.HeaderBar.New();
				var toolbarView = Adw.ToolbarView.New();
				toolbarView.AddTopBar(headerBar);
				toolbarView.Content = content;

				var page = new Adw.NavigationPage();
				page.SetTitle("Live Charts with GirCore and SkiaSharp");
				page.SetTag("content");
				page.SetChild(toolbarView);

				return page;
			}
		};

		application.OnShutdown += (sender, args) =>
		{
		};

		application.Run(0, null);
	}
	static string FormatPascalCase(string text)
	{
		return PascalCaseRegex().Replace(text, " $1");
	}

	static string FormatTitle(string name)
	{
		var parts = name.Split('.');
		if (parts.Length != 2)
			return name;

		var category = FormatPascalCase(parts[0]);
		var demo = FormatPascalCase(parts[1]);
		return $"{category} / {demo}";
	}

	[System.Text.RegularExpressions.GeneratedRegex("(?<!^)([A-Z])")]
	private static partial System.Text.RegularExpressions.Regex PascalCaseRegex();
}