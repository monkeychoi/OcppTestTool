using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Infrastructure.Validation
{
    public enum ValidationMode { FailFast, Aggregate }

    public sealed record ValidationError(string Property, string Message)
    {
        public override string ToString() => $"[{Property}] {Message}";
    }

    public sealed class ValidationResultEx
    {
        public bool IsValid => Errors.Count == 0;
        public List<ValidationError> Errors { get; } = new();
    }

    public sealed class Validator<T>
    {
        // 규칙: 모델을 받아 유효하면 null, 아니면 ValidationError 반환
        private readonly List<Func<T, ValidationError?>> _rules = new();

        public Validator<T> Rule(string property, Func<T, bool> predicate, string message)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            _rules.Add(model => predicate(model) ? null : new ValidationError(property, message));
            return this;
        }

        public ValidationResultEx Validate(T model, ValidationMode mode = ValidationMode.Aggregate)
        {
            var result = new ValidationResultEx();
            foreach (var rule in _rules)
            {
                var err = rule(model);
                if (err is null) continue;

                result.Errors.Add(err);
                if (mode is ValidationMode.FailFast) break;
            }
            return result;
        }
    }
}
