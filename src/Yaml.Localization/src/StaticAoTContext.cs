using YamlDotNet.Serialization;
namespace Yaml.Localization;

/// <summary>
/// Static context for AoT compilation.
/// </summary>
[YamlStaticContext]
[YamlSerializable(typeof(CultureSettings))]
[YamlSerializable(typeof(CultureItem))]
public partial class StaticAoTContext : StaticContext { }