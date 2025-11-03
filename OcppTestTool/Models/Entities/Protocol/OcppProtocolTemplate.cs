using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace OcppTestTool.Models.Entities.Protocol
{
    public sealed class OcppProtocolTemplate
    {
        public int Id { get; init; }
        public int ProtocolId { get; init; }
        public string SchemaName { get; init; } = "";
        public string TemplateName { get; init; } = "";
        public string Direction { get; init; } = "";
        public string MessageType { get; init; } = "";
        public string Version { get; init; } = "";
        public JsonNode? Payload { get; init; }
        public string DefaultVariables { get; init; } = "";
        public string Description { get; init; } = "";
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
    }
    
}
