using OcppTestTool.Models.Entities.Http;
using OcppTestTool.Models.Entities.Protocol;
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
        public event EventHandler<OcppProtocol?>? CloseRequested;

        [ObservableProperty] private OcppProtocolDraft draft = new();
        [ObservableProperty] private string? errorMessage;
        [ObservableProperty] private bool isBusy;

        /// <summary>부모가 주입하는 저장 함수: Draft -> API 호출 -> Result</summary>
        public Func<OcppProtocolDraft, Task<ApiResult<OcppProtocol>>>? SaveFunc { get; set; }

        [RelayCommand]
        private void Cancel() => CloseRequested?.Invoke(this, null);

        [RelayCommand]
        private async Task Save()
        {
            ErrorMessage = null;

            if (string.IsNullOrWhiteSpace(Draft.Name) || string.IsNullOrWhiteSpace(Draft.Version))
            {
                ErrorMessage = "Name과 Version은 필수입니다.";
                return;
            }

            if (SaveFunc is null)
            {
                ErrorMessage = "저장 동작이 구성되지 않았습니다.";
                MessageBox.Show(ErrorMessage, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IsBusy = true;

            try
            {
                var result = await SaveFunc(Draft);
                if (!result.Success)
                {
                    ErrorMessage = result.Error ?? "저장에 실패했습니다.";
                    MessageBox.Show(ErrorMessage, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // 창 유지, 에러 표시
                }

                // 성공 → 부모가 목록 리로드할 수 있도록 Draft(또는 필요 시 result.Value) 반환
                CloseRequested?.Invoke(this, result.Data);
            }
            finally { IsBusy = false; }
        }
    }
}
