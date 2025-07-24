using System.Windows.Input;

namespace EasyUIBinding.GirCore;

public class Command(Action execute) : ICommand
{
	public bool CanExecute(object? parameter) => true;
	public void Execute(object? parameter) => execute();
	public event EventHandler? CanExecuteChanged;
}

public class Command<T>(Action<T> execute) : ICommand
{
	public bool CanExecute(object? parameter) => true;

	public void Execute(object? parameter)
	{
		if (parameter is T value)
		{
			execute(value);
		}
		else
		{
			throw new ArgumentException($"Invalid command parameter type. Expected {typeof(T)}, got {parameter?.GetType().Name ?? "null"}");
		}
	}

	public event EventHandler? CanExecuteChanged;
}