using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace EasyUIBinding.GirCore;

public static class NotifyPropertyModelExtensions
{
	[UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Property access is safe as we control the types involved.")]
	public static object? GetPropertyValue(this INotifyPropertyChanged obj, string propertyName)
	{
		var property = obj.GetType().GetProperty(propertyName)
				?? throw new ArgumentException($"Property '{propertyName}' in {obj.GetType().Name} not found.", nameof(propertyName));
		return property.GetValue(obj);
	}

	[UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Property access is safe as we control the types involved.")]
	public static void SetPropertyValue(this INotifyPropertyChanged obj, string propertyName, object? value)
	{
		var property = obj.GetType().GetProperty(propertyName)
				?? throw new ArgumentException($"Property '{propertyName}' in {obj.GetType().Name} not found.", nameof(propertyName));
		property.SetValue(obj, value);
	}
}