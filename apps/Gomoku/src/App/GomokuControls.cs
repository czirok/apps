using EasyUIBinding.GirCore;
using EasyUIBinding.GirCore.Binding;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Runtime.Versioning;
using Yaml.Localization;

namespace Gomoku;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class GomokuControls : Gtk.Box, IDisposable
{
	internal readonly List<Input> Inputs;
	internal Adw.Dialog SettingsDialog = Adw.Dialog.New();
	private readonly Gtk.Box _dialogBox = UI.Box(Gtk.Orientation.Vertical, 0).Design();
	private readonly Gtk.Box _spacer = UI.Box(Gtk.Orientation.Vertical, 0);
	private Gtk.Button _closeButton = Gtk.Button.NewFromIconName("window-close-symbolic");
	private Adw.PreferencesGroup _settingsGroup = Adw.PreferencesGroup.New();

	internal readonly Gtk.Button UndoButton = Gtk.Button.NewFromIconName("edit-undo-symbolic");
	internal readonly Gtk.Button RedoButton = Gtk.Button.NewFromIconName("edit-redo-symbolic");
	internal readonly Gtk.Button RestartButton = Gtk.Button.NewFromIconName("view-refresh-symbolic");

	private readonly Board _board;
	private readonly BoardModel _boardModel;
	private readonly CultureSettings _cultures;
	private readonly IStringLocalizer<I18N> L;
	private readonly ILogger<GomokuControls> _logger;

	public event Action? LanguageChanged;

	public GomokuControls(
		Board board,
		BoardModel boardModel,
		CultureSettings cultureSettings,
		IStringLocalizer<I18N> localizer,
		ILogger<GomokuControls> logger)
	{
		SetOrientation(Gtk.Orientation.Vertical);

		ArgumentNullException.ThrowIfNull(board);
		ArgumentNullException.ThrowIfNull(boardModel);
		ArgumentNullException.ThrowIfNull(cultureSettings);
		ArgumentNullException.ThrowIfNull(localizer);
		ArgumentNullException.ThrowIfNull(logger);

		_board = board;
		_boardModel = boardModel;
		_cultures = cultureSettings;
		L = localizer;
		_logger = logger;


		// Black background
		var provider = Gtk.CssProvider.New();
		var css = ".settings-dialog-bg { background-color: transparent; }";
		provider.LoadFromData(css, css.Length);
		Gtk.StyleContext.AddProviderForDisplay(Gdk.Display.GetDefault()!, provider, Gtk.Constants.STYLE_PROVIDER_PRIORITY_APPLICATION);

		SettingsDialog.SetCssClasses(["settings-dialog-bg"]);

		SettingsDialog.SetContentWidth(500);
		SettingsDialog.SetContentHeight(600);

		_spacer.SetVexpand(true);

		_closeButton = Gtk.Button.NewFromIconName("window-close-symbolic");
		_closeButton.OnClicked += CloseDialog;
		_closeButton.SetHalign(Gtk.Align.Center);

		_dialogBox.Append(_settingsGroup);
		_dialogBox.Append(_spacer);
		_dialogBox.Append(_closeButton);

		SettingsDialog.SetChild(UI.Scroll(_dialogBox));

		Inputs = [];

		CreateInputs();
		CreateUI();
		NewGameOnMainThread();

		UndoButton.OnClicked += (sender, args) => OnUndo();
		RedoButton.OnClicked += (sender, args) => OnRedo();
		RestartButton.OnClicked += (sender, args) => OnRestart();
	}

	public void NewGameOnMainThread()
	{
		GLib.Functions.IdleAdd(GLib.Constants.PRIORITY_LOW, new GLib.SourceFunc(() =>
		{
			_boardModel.GameHistory = new GameHistory();
			_board.GameEngine = new GameEngine2003(_boardModel);
			if (_boardModel.GameMode == GameMode.HumanVsEngine)
				_board.BeginGame(StoneColor.White, StoneColor.Black);
			else
				_board.BeginGame(StoneColor.Black, StoneColor.White);
			return GLib.Constants.SOURCE_REMOVE;
		}));
	}

	public void NewGameOnMainThread(List<(int x, int y, int player)> moves)
	{
		GLib.Functions.IdleAdd(GLib.Constants.PRIORITY_LOW, new GLib.SourceFunc(() =>
		{
			_board.GameEngine = new GameEngine2003(_boardModel);
			if (_boardModel.GameMode == GameMode.HumanVsEngine)
				_board.BeginGame(StoneColor.White, StoneColor.Black);
			else
				_board.BeginGame(StoneColor.Black, StoneColor.White);
			return GLib.Constants.SOURCE_REMOVE;
		}));
	}

	private void CreateInputs()
	{
		Inputs.AddRange([

			new SpinInteger("thinking-time", L["Thinking time (sec)"], 5, new IntRange(1, 60))
				.BindTo(_boardModel, nameof(BoardModel.ThinkingTime)),

			new SpinInteger("board-size", L["Board size"], 20, new IntRange(10, 60))
				.BindTo(_boardModel, nameof(BoardModel.BoardSize))
				.OnChanged(() => {
					NewGameOnMainThread();
				}),

			new UndoRedo("undo-redo", L["Undo/Redo"]).OnUndoClick(OnUndo).OnRedoClick(OnRedo),

			new Toggle<GameMode>("game-mode", L["Game mode"], new Dictionary<GameMode, string>
			{
				{ GameMode.HumanVsEngine, L["You start"] },
				{ GameMode.EngineVsHuman, L["AI starts"] }
			}, _boardModel.GameMode, _boardModel.GameMode)
				.BindTo(_boardModel, nameof(BoardModel.GameMode))
				.OnChanged(() => {
					if (_boardModel.GameMode == GameMode.HumanVsEngine)
					{
						// Player starts: even numbers (0,2,4,6...)
						_boardModel.GameHistory.CurrentMoveIndex = 0;
					}
					else
					{
						// AI starts: odd numbers (1,3,5,7...)
						_boardModel.GameHistory.CurrentMoveIndex = 1;
					}
					NewGameOnMainThread();
					SettingsDialog.Close();
				}),

			new Button("reset", new ButtonLabel(L["New game"])).OnClick(() => {
				OnRestart();
				SettingsDialog.Close();
			}),

			new Combo<BoardTheme>("board-theme", L["Board theme"], new Dictionary<BoardTheme, string>
			{
				{ BoardTheme.WoodWithStones, L["Wood with stones"] },
				{ BoardTheme.PaperAndPencil, L["Paper and pencil"] }
			}, _boardModel.BoardTheme, _boardModel.BoardTheme)
				.BindTo(_boardModel, nameof(BoardModel.BoardTheme))
				.OnChanged(() => {
					_board.InvalidateOnMainThread();
					SettingsDialog.Close();
				}),

			new ClipboardButton("copy", L["Copy Gomoku AI protocol"])
				.BindTo(_boardModel, nameof(BoardModel.GomokuAIProtocol)),

			new Combo<string>(
				"languages",
				L["Languages"],
				_cultures.SpecificActiveSelector(),
				_cultures.SpecificDefaultCultureInfo().Name,
				CultureInfo.CurrentUICulture.Name)
				.OnChanged((sender, args) => {
					if (args is not InputDictionaryChangedEventArgs<string> inputArgs)
						return;

					if (inputArgs.Title == null || !_cultures.SpecificActiveCultures().Contains(inputArgs.Key))
						return;

					var culture = inputArgs.Key.ToSpecificCulture() ?? _cultures.SpecificDefaultCultureInfo();

					if (CultureInfo.CurrentUICulture.Name == culture.Name)
						return;

					CultureInfo.CurrentUICulture = culture;
					CultureInfo.CurrentCulture = culture;

					RefreshUI();

					LanguageChanged?.Invoke();
				})
		]);

		if (Inputs.FirstOrDefault(x => x.Name == "board-size") is SpinInteger spin && spin.Row is Adw.SpinRow boardSizeRow)
		{
			boardSizeRow.Subtitle = L["WARNING! The game is restarting!"];
		}
		if (Inputs.FirstOrDefault(x => x.Name == "game-mode") is Toggle<GameMode> gameMode && gameMode.Row is Adw.ActionRow gameModeRow)
		{
			gameModeRow.Subtitle = L["WARNING! The game is restarting!"];
		}
	}

	private void OnRestart()
	{
		NewGameOnMainThread();
	}

	private void OnRedo()
	{
		if (_boardModel.GameHistory.CanRedo)
		{
			_boardModel.GameHistory.Redo(_boardModel.GameMode);
			_board.RebuildBoardFromHistory();
			_board.GameEngine.SetMoves(_boardModel.GameHistory.Moves.Take(_boardModel.GameHistory.CurrentMoveIndex).ToList());
		}
	}

	private void OnUndo()
	{
		{
			if (_boardModel.GameHistory.CanUndo)
			{
				_boardModel.GameHistory.Undo(_boardModel.GameMode);
				_board.RebuildBoardFromHistory();
				_board.GameEngine.SetMoves(_boardModel.GameHistory.Moves.Take(_boardModel.GameHistory.CurrentMoveIndex).ToList());
			}
		}
	}

	private void CreateUI()
	{
		Append(_boardModel.StatusLabel);
		Append(_board);

		foreach (var input in Inputs)
		{
			_settingsGroup.Add(input.Row);
		}

		_board.ReplaceStatusOnMainThread();
	}

	private void CloseDialog(Gtk.Button sender, EventArgs args)
	{
		SettingsDialog.Close();
	}

	private void ClearUI()
	{
		foreach (var input in Inputs)
		{
			_settingsGroup.Remove(input.Row);
			input.Dispose();
		}
		Inputs.Clear();

		Remove(_board);
		Remove(_boardModel.StatusLabel);
	}

	public void RefreshUI()
	{
		ClearUI();
		CreateInputs();
		CreateUI();
	}

	public override void Dispose()
	{
		_closeButton.OnClicked -= CloseDialog;
		ClearUI();
		base.Dispose();
	}
}
