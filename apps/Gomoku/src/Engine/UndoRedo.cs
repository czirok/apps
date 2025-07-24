using EasyUIBinding.GirCore;

namespace Gomoku;

public class UndoRedo : View, IDisposable
{
	private readonly Gtk.Button _undoButton;
	private readonly Gtk.Button _redoButton;

	protected Action? UndoCallback;
	public void SetUndoCallback(Action callback) => UndoCallback = callback;

	protected Action? RedoCallback;
	public void SetRedoCallback(Action callback) => RedoCallback = callback;

	public UndoRedo(string name, string description)
		: base(name, description, string.Empty)
	{
		_undoButton = Gtk.Button.NewFromIconName("edit-undo-symbolic");
		_undoButton.Valign = Gtk.Align.Center;
		_redoButton = Gtk.Button.NewFromIconName("edit-redo-symbolic");
		_redoButton.Valign = Gtk.Align.Center;
		Row.AddSuffix(_undoButton);
		Row.AddSuffix(_redoButton);
		_undoButton.OnClicked += OnUndoButtonClicked;
		_redoButton.OnClicked += OnRedoButtonClicked;
	}

	private void OnUndoButtonClicked(object sender, EventArgs e)
	{
		UndoCallback?.Invoke();
	}

	private void OnRedoButtonClicked(object sender, EventArgs e)
	{
		RedoCallback?.Invoke();
	}

	public override void Dispose()
	{
		_undoButton.OnClicked -= OnUndoButtonClicked;
		_redoButton.OnClicked -= OnRedoButtonClicked;
		base.Dispose();
	}
}

public static partial class Extensions
{
	public static UndoRedo OnUndoClick(this UndoRedo input, Action action)
	{
		input.SetUndoCallback(action);
		return input;
	}

	public static UndoRedo OnRedoClick(this UndoRedo input, Action action)
	{
		input.SetRedoCallback(action);
		return input;
	}
}
