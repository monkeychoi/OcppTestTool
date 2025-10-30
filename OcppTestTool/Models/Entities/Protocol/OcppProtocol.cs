using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Models.Entities.Protocol
{
    public sealed class OcppProtocol
    {
        public int Id { get; init; }
        public string Name { get; init; } = "";
        public string Version { get; init; } = "";
        public string Transport { get; init; } = "";
        public string Codec { get; init; } = "";
        public ProtocolMeta Meta { get; init; } = new();
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
    }

    public sealed class ProtocolMeta
    {
        public string? Description { get; init; }
    }
}
