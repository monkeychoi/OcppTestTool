using OcppTestTool.Models.Entities.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OcppTestTool.Models.Dtos.Protocol
{
    public static class OcppProtocolTemplateMapper
    {
        public static OcppProtocolTemplate ToEntity(this OcppProtocolTemplateDto dto)
            => new()
            {
                Id = dto.Id,
                ProtocolId = dto.ProtocolId,
                SchemaName = dto.SchemaName,
                TemplateName = dto.TemplateName,
                Direction = dto.Direction,
                MessageType = dto.MessageType,
                Version = dto.Version,
                Payload = dto.Payload.ValueKind == JsonValueKind.Undefined
                            ? null
                            : JsonNode.Parse(dto.Payload.GetRawText()),
                DefaultVariables = dto.DefaultVariables,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        public static IReadOnlyList<OcppProtocolTemplate> ToEntities(this IEnumerable<OcppProtocolTemplateDto> src)
            => src.Select(ToEntity).ToList();
    }

    public sealed class OcppProtocolTemplateDto
    {
        [JsonPropertyName("id")] public int Id { get; init; }
        [JsonPropertyName("protocol_id")] public int ProtocolId { get; init; }
        [JsonPropertyName("schema_name")] public string SchemaName { get; init; } = "";
        [JsonPropertyName("template_name")] public string TemplateName { get; init; } = "";
        [JsonPropertyName("direction")] public string Direction { get; init; } = "";
        [JsonPropertyName("message_type")] public string MessageType { get; init; } = "";
        [JsonPropertyName("version")] public string Version { get; init; } = "";
        [JsonPropertyName("payload")] public JsonElement Payload { get; init; }
        [JsonPropertyName("default_variables")] public string DefaultVariables { get; init; } = "";
        [JsonPropertyName("description")] public string Description { get; init; } = "";
        [JsonPropertyName("created_at")] public DateTimeOffset CreatedAt { get; init; }
        [JsonPropertyName("updated_at")] public DateTimeOffset UpdatedAt { get; init; }
    }

    //public sealed class OcppProtocolMetaDto
    //{
    //    [JsonPropertyName("description")] public string? Description { get; init; }
    //}

    //public sealed class OcppProtocolCreateDto
    //{
    //    [JsonPropertyName("name")] public string Name { get; init; } = "";
    //    [JsonPropertyName("version")] public string Version { get; init; } = "";
    //    [JsonPropertyName("transport")] public string Transport { get; init; } = "";
    //    [JsonPropertyName("codec")] public string Codec { get; init; } = "";
    //    [JsonPropertyName("meta")] public OcppProtocolMetaDto? Meta { get; init; }
    //}

    //public sealed class OcppProtocolEditDto
    //{
    //    [JsonPropertyName("id")] public int Id { get; init; }
    //    [JsonPropertyName("name")] public string Name { get; init; } = "";
    //    [JsonPropertyName("version")] public string Version { get; init; } = "";
    //    [JsonPropertyName("transport")] public string Transport { get; init; } = "";
    //    [JsonPropertyName("codec")] public string Codec { get; init; } = "";
    //    [JsonPropertyName("meta")] public OcppProtocolMetaDto? Meta { get; init; }
    //}
}
