using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Text;

[Generator]
public class GirCoreNotifyGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var fieldDeclarations = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (s, _) => IsFieldWithAttribute(s),
				transform: static (ctx, _) => GetFieldInfo(ctx))
			.Where(static m => m is not null);

		context.RegisterSourceOutput(fieldDeclarations.Collect(), Execute);
	}

	private static bool IsFieldWithAttribute(SyntaxNode node)
	{
		if (node is not FieldDeclarationSyntax field)
			return false;

		return field.AttributeLists
			.SelectMany(list => list.Attributes)
			.Any(attr => attr.Name.ToString().Contains("GirCoreNotify"));
	}

	private static FieldInfo? GetFieldInfo(GeneratorSyntaxContext context)
	{
		if (context.Node is not FieldDeclarationSyntax field)
			return null;

		if (!field.Modifiers.Any(SyntaxKind.PrivateKeyword))
			return null;

		var classDeclaration = field.Ancestors().OfType<ClassDeclarationSyntax>().FirstOrDefault();
		if (classDeclaration == null)
			return null;

		var classModifiers = string.Join(" ", classDeclaration.Modifiers
			.Where(m => m.ValueText != "partial")
			.Select(m => m.ValueText));

		var namespaceName = GetNamespace(classDeclaration);
		var className = classDeclaration.Identifier.ValueText;

		var variable = field.Declaration.Variables.FirstOrDefault();
		if (variable == null)
			return null;

		var fieldName = variable.Identifier.ValueText;
		var fieldType = field.Declaration.Type?.ToString() ?? "object";

		var propertyName = GeneratePropertyName(fieldName);

		var girCoreNotifyAttr = field.AttributeLists
			.SelectMany(list => list.Attributes)
			.FirstOrDefault(attr => attr.Name.ToString().Contains("GirCoreNotify"));

		var validateValue = true;
		var customMethodName = (string?)null;

		if (girCoreNotifyAttr?.ArgumentList?.Arguments != null)
		{
			foreach (var arg in girCoreNotifyAttr.ArgumentList.Arguments)
			{
				if (arg.NameEquals?.Name.Identifier.ValueText == "ValidateValue")
				{
					if (arg.Expression is LiteralExpressionSyntax literal)
						validateValue = literal.Token.ValueText == "true";
				}
				else if (arg.NameEquals?.Name.Identifier.ValueText == "CustomMethodName")
				{
					if (arg.Expression is LiteralExpressionSyntax literal)
						customMethodName = literal.Token.ValueText?.Trim('"');
				}
			}
		}

		return new FieldInfo(namespaceName, classModifiers, className, fieldName, propertyName, fieldType, validateValue, customMethodName);
	}

	private static string GeneratePropertyName(string fieldName)
	{
		if (fieldName.StartsWith("_") && fieldName.Length > 1)
		{
			return char.ToUpper(fieldName[1]) + fieldName[2..];
		}

		return char.ToUpper(fieldName[0]) + fieldName[1..];
	}

	private static string GetNamespace(SyntaxNode node)
	{
		var namespaceDeclaration = node.Ancestors().OfType<BaseNamespaceDeclarationSyntax>().FirstOrDefault();
		return namespaceDeclaration?.Name.ToString() ?? "Global";
	}

	private static void Execute(SourceProductionContext context, ImmutableArray<FieldInfo?> fields)
	{
		if (fields.IsDefaultOrEmpty)
			return;

		var nonNullFields = fields.Where(f => f is not null)!;

		foreach (var classGroup in nonNullFields.GroupBy(f => new { f!.Namespace, f.ClassName, f.ClassModifiers }))
		{
			var source = GeneratePartialClass(classGroup.Key.Namespace, classGroup.Key.ClassModifiers, classGroup.Key.ClassName, classGroup!);
			context.AddSource($"{classGroup.Key.ClassName}.GirCoreNotify.g.cs", source);
		}
	}

	private static string GeneratePartialClass(string namespaceName, string classModifiers, string className, IGrouping<object, FieldInfo> fields)
	{
		var sb = new StringBuilder();

		// Using statements
		sb.AppendLine("using System;");
		sb.AppendLine("using System.Collections.Generic;");
		sb.AppendLine("using System.ComponentModel;");
		sb.AppendLine("using System.Runtime.CompilerServices;");
		sb.AppendLine();

		// Namespace
		if (namespaceName != "Global")
		{
			sb.AppendLine($"namespace {namespaceName};");
			sb.AppendLine();
		}

		// Partial class
		sb.AppendLine($"{classModifiers} partial class {className}");
		sb.AppendLine("{");

		// Property implementációk generálása
		foreach (var field in fields)
		{
			GenerateProperty(sb, field);
			sb.AppendLine();
		}

		sb.AppendLine("}");

		return sb.ToString();
	}

	private static void GenerateProperty(StringBuilder sb, FieldInfo field)
	{
		var methodName = field.CustomMethodName ?? "OnPropertyChanged";

		// Property
		sb.AppendLine($"    public {field.FieldType} {field.PropertyName}");
		sb.AppendLine("    {");
		sb.AppendLine($"        get => {field.FieldName};");
		sb.AppendLine("        set");
		sb.AppendLine("        {");

		if (field.ValidateValue)
		{
			sb.AppendLine($"            if (EqualityComparer<{field.FieldType}>.Default.Equals({field.FieldName}, value))");
			sb.AppendLine("                return;");
		}

		sb.AppendLine($"            {field.FieldName} = value;");
		sb.AppendLine($"            {methodName}();");
		sb.AppendLine("        }");
		sb.AppendLine("    }");
	}

	private record FieldInfo(
		string Namespace,
		string ClassModifiers,
		string ClassName,
		string FieldName,
		string PropertyName,
		string FieldType,
		bool ValidateValue,
		string? CustomMethodName);
}