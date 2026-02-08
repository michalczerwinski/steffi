using System.Text.Json.Serialization;

namespace Steffi.Cli.Helpers;

[JsonSerializable(typeof(List<string>))]
internal partial class PreviewServerJsonContext : JsonSerializerContext;
