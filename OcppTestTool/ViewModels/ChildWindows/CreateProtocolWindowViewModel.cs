using OcppTestTool.Features.Protocol;
using OcppTestTool.Infrastructure.Validation;
using OcppTestTool.Models.Entities.Common;
using OcppTestTool.Models.Entities.Http;
using OcppTestTool.Models.Entities.Protocol;
using OcppTestTool.Services.UI;
using OcppTestTool.Services.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.ViewModels.ChildWindows
{
    public partial class CreateProtocolWindowViewModel : ObservableObject, IModalViewModel<OcppProtocol?>
    {
        private readonly IUiMessageService _uiMessage;
        private readonly Validator<OcppProtocolDraft> _validator;

        public event EventHandler<OcppProtocol?>? CloseRequested;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Title))]
        [NotifyPropertyChangedFor(nameof(SaveButtonText))]
        private DataEditMode mode;

        [ObservableProperty] private OcppProtocolDraft draft = new();
        [ObservableProperty] private string? errorMessage;
        [ObservableProperty] private bool isBusy;

        public string Title => Mode == DataEditMode.Create ? "프로토콜 신규 등록" : "프로토콜 수정";
        public string SaveButtonText => Mode == DataEditMode.Create ? "등록" : "수정";

        /// <summary>부모가 주입하는 저장 함수: Draft -> API 호출 -> ApiResult</summary>
        public Func<OcppProtocolDraft, Task<ApiResult<OcppProtocol>>>? SaveFunc { get; set; }

        public CreateProtocolWindowViewModel(IUiMessageService uiMessage)
        {
            _uiMessage = uiMessage;
            _validator = ProtocolDraftValidator.Instance;
        }

        [RelayCommand]
        private void Cancel() => CloseRequested?.Invoke(this, null);

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // 1) 유효성 검사 (Aggregate: 모든 오류를 한 번에)
                var vr = _validator.Validate(Draft, ValidationMode.FailFast);
                if (!vr.IsValid)
                {
                    var sb = new StringBuilder("입력값을 확인해주세요:\n");
                    for (int i = 0; i < vr.Errors.Count; i++)
                    {
                        var e = vr.Errors[i];
                        sb.Append("• ").Append(e.Property).Append(" - ").Append(e.Message);
                        if (i < vr.Errors.Count - 1) sb.Append('\n');
                    }
                    await _uiMessage.ShowAsync("유효성 검사 실패", sb.ToString());
                    return;
                }

                // 2) SaveFunc 실행
                if (SaveFunc is null)
                {
                    await _uiMessage.ShowAsync("저장 실패", "SaveFunc 가 설정되지 않았습니다.");
                    return;
                }

                var result = await SaveFunc(Draft);
                if (!result.Success || result.Data is null)
                {
                    await _uiMessage.ShowAsync("저장 실패", result.Error ?? "알 수 없는 오류가 발생했습니다.");
                    return;
                }

                // 3) 성공 → 부모에게 알림
                CloseRequested?.Invoke(this, result.Data);
            }
            finally
            {
                IsBusy = false;
            }
            
        }
    }
}
