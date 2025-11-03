using OcppTestTool.Models.Entities.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OcppTestTool.Models.Dtos.Protocol
{
    public static class OcppProtocolMapper
    {
        public static OcppProtocol ToEntity(this OcppProtocolDto dto)
            => new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Version = dto.Version,
                Transport = dto.Transport,
                Codec = dto.Codec,
                Meta = new ProtocolMeta { Description = dto.Meta?.Description },
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };

        public static IReadOnlyList<OcppProtocol> ToEntities(this IEnumerable<OcppProtocolDto> src)
            => src.Select(ToEntity).ToList();
    }

    public sealed class OcppProtocolDto
    {
        [JsonPropertyName("id")] public int Id { get; init; }
        [JsonPropertyName("name")] public string Name { get; init; } = "";
        [JsonPropertyName("version")] public string Version { get; init; } = "";
        [JsonPropertyName("transport")] public string Transport { get; init; } = "";
        [JsonPropertyName("codec")] public string Codec { get; init; } = "";
        [JsonPropertyName("meta")] public OcppProtocolMetaDto? Meta { get; init; }
        [JsonPropertyName("created_at")] public DateTimeOffset CreatedAt { get; init; }
        [JsonPropertyName("updated_at")] public DateTimeOffset UpdatedAt { get; init; }
    }

    public sealed class OcppProtocolMetaDto
    {
        [JsonPropertyName("description")] public string? Description { get; init; }
    }

    public sealed class OcppProtocolCreateDto
    {
        [JsonPropertyName("name")] public string Name { get; init; } = "";
        [JsonPropertyName("version")] public string Version { get; init; } = "";
        [JsonPropertyName("transport")] public string Transport { get; init; } = "";
        [JsonPropertyName("codec")] public string Codec { get; init; } = "";
        [JsonPropertyName("meta")] public OcppProtocolMetaDto? Meta { get; init; }
    }

    public sealed class OcppProtocolEditDto
    {
        [JsonPropertyName("id")] public int Id { get; init; }
        [JsonPropertyName("name")] public string Name { get; init; } = "";
        [JsonPropertyName("version")] public string Version { get; init; } = "";
        [JsonPropertyName("transport")] public string Transport { get; init; } = "";
        [JsonPropertyName("codec")] public string Codec { get; init; } = "";
        [JsonPropertyName("meta")] public OcppProtocolMetaDto? Meta { get; init; }
    }
}
