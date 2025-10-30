using OcppTestTool.Models.Entities.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
}
