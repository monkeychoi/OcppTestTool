using OcppTestTool.Infrastructure.Validation;
using OcppTestTool.Models.Entities.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OcppTestTool.Features.Protocol
{
    public static partial class ProtocolDraftValidator
    {
        private static readonly Lazy<Validator<OcppProtocolDraft>> _instance = new(Create, isThreadSafe: true);
        public static Validator<OcppProtocolDraft> Instance => _instance.Value;

        public static Validator<OcppProtocolDraft> Create() =>
        new Validator<OcppProtocolDraft>()
            .Rule(nameof(OcppProtocolDraft.Name),
                    d => !string.IsNullOrWhiteSpace(d.Name),
                    "프로토콜 이름은 필수입니다.")
            .Rule(nameof(OcppProtocolDraft.Name),
                    d => d.Name is null || d.Name.Length >= 2,
                    "이름은 2자 이상이어야 합니다.")
            .Rule(nameof(OcppProtocolDraft.Version),
                    d => !string.IsNullOrWhiteSpace(d.Version),
                    "버전은 필수입니다.")
            .Rule(nameof(OcppProtocolDraft.Transport),
                    d => !string.IsNullOrWhiteSpace(d.Transport),
                    "Transport는 필수입니다.")
            .Rule(nameof(OcppProtocolDraft.Codec),
                    d => !string.IsNullOrWhiteSpace(d.Codec),
                    "Codec는 필수입니다.");
    }
}
