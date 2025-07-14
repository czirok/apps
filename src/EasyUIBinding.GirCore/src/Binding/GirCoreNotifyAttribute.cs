namespace EasyUIBinding.GirCore.Binding;

[AttributeUsage(AttributeTargets.Field)]
public class GirCoreNotifyAttribute : Attribute
{
	public bool ValidateValue { get; set; } = true;
	public string? CustomMethodName { get; set; }
}