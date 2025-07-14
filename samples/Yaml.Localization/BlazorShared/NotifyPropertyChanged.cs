using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BlazorShared;

public class NotifyPropertyChanged : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = default!)
	{
		var handler = PropertyChanged;
		handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	protected void OtherPropertyChanged(string propertyName)
	{
		var handler = PropertyChanged;
		handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
