using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace EasyUIBinding.GirCore.Binding;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
public abstract class NotifyPropertyModel : INotifyPropertyChanged
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

	public object this[string propertyName]
	{
		get
		{
			var property = GetType().GetProperty(propertyName)
				?? throw new ArgumentException($"Property '{propertyName}' not found.", nameof(propertyName));
			return property.GetValue(this, null)!;
		}
		set
		{
			var property = GetType().GetProperty(propertyName)
				?? throw new ArgumentException($"Property '{propertyName}' not found.", nameof(propertyName));
			property.SetValue(this, value, null);
		}
	}
}

