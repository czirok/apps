namespace EasyUIBinding.GirCore;

public class WrapPreferencesGroup : Adw.WrapBox, IDisposable
{
	private readonly List<Input>? disposables;

	public WrapPreferencesGroup(List<Input>? inputs = null)
	{
		disposables = inputs;
		SetChildSpacing(6);
		SetLineSpacing(6);
		SetMarginStart(12);
		SetMarginEnd(12);
		AddCssClass("wrap-preferences-group");

		if (disposables is null || disposables.Count == 0) return;

		foreach (var input in disposables)
		{
			Append(UI.Group(input.Row));
		}
	}

	public void Add(Input child)
	{
		disposables?.Add(child);
		Append(UI.Group(child.Row));
	}

	public override void Dispose()
	{
		if (disposables is not null)
		{
			foreach (var disposable in disposables)
			{
				disposable.Dispose();
			}
		}
		base.Dispose();
	}
}
