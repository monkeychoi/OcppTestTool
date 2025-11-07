using OcppTestTool.Helpers;
using OcppTestTool.Models;
using OcppTestTool.Models.Dtos.Protocol;
using OcppTestTool.Models.Entities.Common;
using OcppTestTool.Models.Entities.Protocol;
using OcppTestTool.Models.Entities.User;
using OcppTestTool.Services.Protocol;
using OcppTestTool.Services.UI;
using OcppTestTool.Services.Windows;
using OcppTestTool.ViewModels.ChildWindows;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Threading;
using Wpf.Ui;
using Wpf.Ui.Abstractions.Controls;

namespace OcppTestTool.ViewModels.Pages
{
    public partial class ProtocolTemplateManagementViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private readonly IProtocolManagementService _service;
        private readonly IWindowService _windows;
        private readonly IUiMessageService _ui;

        [ObservableProperty] private ObservableCollection<OcppProtocol> protocols = new();
        [ObservableProperty] private OcppProtocol? selectedProtocol = null;

        [ObservableProperty] private ObservableCollection<OcppProtocolTemplate> protocolTemplates = new();
        [ObservableProperty] private OcppProtocolTemplate? selectedProtocolTemplate;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string errorMessage = "";

        public ProtocolTemplateManagementViewModel(IWindowService windows, IProtocolManagementService service, IUiMessageService ui)
        {
            _windows = windows;
            _service = service;
            _ui = ui;
        }

        public async Task OnNavigatedToAsync()
        {
            if (!_isInitialized)
                await InitializeViewModel();
           
        }

        public Task OnNavigatedFromAsync() => Task.CompletedTask;

        private async Task InitializeViewModel()
        {
            _isInitialized = true;
            await LoadProtocolsAsync();
        }

        async partial void OnSelectedProtocolChanged(OcppProtocol? value)
        {
            if (value == null)
            {
                ProtocolTemplates.Clear(); // 초기 상태
                return;
            }

            await LoadProtocolTemplatesAsync(value.Id);
        }

        /// <summary>
        /// 프로토콜 목록 로드
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task LoadProtocolsAsync()
        {
            ErrorMessage = "";
            IsBusy = true;

            try
            {
                var list = await _service.GetProtocolsAsync();

                Protocols.Clear();

                foreach (var item in list)
                    Protocols.Add(item);

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }


        /// <summary>
        /// 프로토콜 템플릿 목록 로드
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task LoadProtocolTemplatesAsync(int protocolId)
        {
            ErrorMessage = "";
            IsBusy = true;

            await Dispatcher.Yield(DispatcherPriority.Render);

            try
            {
                var list = await _service.GetProtocolTemplatesAsync(protocolId);
                //ProtocolTemplates = new ObservableCollection<OcppProtocolTemplate>(list);

                ProtocolTemplates.Clear();

                const int batch = 1; // 상황에 맞게 조절
                for (int i = 0; i < list.Count; i += batch)
                {
                    var slice = list.Skip(i).Take(batch);
                    foreach (var item in slice)
                        ProtocolTemplates.Add(item);

                    // UI에 양보해서 오버레이·스크롤·히트테스트가 살아있게
                    await Task.Yield();
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadProtocolTemplatesAsync();
        }
            

        /// <summary>
        /// 신규 등록
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OpenCreateAsync()
        {
            //var created = await _windows.ShowDialogAsync<CreateProtocolWindowViewModel, OcppProtocol?>(vm =>
            //{
            //    // 모드 설정
            //    vm.Mode = DataEditMode.Create;

            //    // 초기값 세팅이 필요하면 여기서
            //    vm.Draft = new OcppProtocolDraft { Transport = "WebSocket", Codec = "JSON" };

            //    vm.SaveFunc = async draft =>
            //    {
            //        var dto = new OcppProtocolCreateDto
            //        {
            //            Name = draft.Name,
            //            Version = draft.Version,
            //            Transport = draft.Transport,
            //            Codec = draft.Codec,
            //            Meta = string.IsNullOrWhiteSpace(draft.Description)
            //                ? null
            //                : new OcppProtocolMetaDto { Description = draft.Description }
            //        };
            //        return await _service.CreateProtocolAsync(dto);
            //    };

            //});

            //if (created is null) return; // 취소

            //await LoadAsync(); // 성공으로 닫혔으면 목록 갱신
        }

        /// <summary>
        /// 항목 수정
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OpenEditAsync()
        {
            //if (SelectedProtocol is null)
            //{
            //    MessageBox.Show("수정할 프로토콜을 선택하세요.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}

            //var edited = await _windows.ShowDialogAsync<CreateProtocolWindowViewModel, OcppProtocol?>(vm =>
            //{
            //    // 모드 설정
            //    vm.Mode = DataEditMode.Edit;

            //    // 기존 값으로 세팅
            //    vm.Draft = new OcppProtocolDraft 
            //    { 
            //        Id = SelectedProtocol.Id,
            //        Name = SelectedProtocol.Name,
            //        Version = SelectedProtocol.Version,
            //        Transport = SelectedProtocol.Transport,
            //        Codec = SelectedProtocol.Codec,
            //        Description = SelectedProtocol.Meta?.Description
            //    };

            //    vm.SaveFunc = async draft =>
            //    {
            //        var dto = new OcppProtocolEditDto
            //        {
            //            Id = draft.Id,
            //            Name = draft.Name,
            //            Version = draft.Version,
            //            Transport = draft.Transport,
            //            Codec = draft.Codec,
            //            Meta = string.IsNullOrWhiteSpace(draft.Description)
            //                ? null
            //                : new OcppProtocolMetaDto { Description = draft.Description }
            //        };
            //        return await _service.EditProtocolAsync(dto);
            //    };

            //});

            ////if (edited is null) return; // 취소
            ////await LoadAsync(); // 성공으로 닫혔으면 목록 갱신
            //// 단건갱신 TODO: 이 방식은 추후 만약 Paging이도입되었을때 고려해서 처리해야함
            //if (edited is not null)
            //{
            //    var idx = Protocols.IndexOf(Protocols.First(p => p.Id == edited.Id));
            //    if (idx >= 0) Protocols[idx] = edited;
            //}
        }

        /// <summary>
        /// 항목 삭제
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OpenDeleteAsync()
        {
            //if (SelectedProtocol is null)
            //{
            //    MessageBox.Show("삭제할 프로토콜을 선택하세요.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}

            //var name = SelectedProtocol.Name ?? $"ID {SelectedProtocol.Id}";
            //var confirm = await _ui.ConfirmAsync("삭제 확인", $"선택한 프로토콜을 삭제하시겠습니까?\n• {name}");
            //if (!confirm) return;

            //if (IsBusy) return;
            //IsBusy = true;
            //try
            //{
            //    var id = SelectedProtocol.Id;
            //    var result = await _service.DeleteProtocolAsync(id);
            //    if (!result.Success)
            //    {
            //        await _ui.ShowAsync("삭제 실패", result.Error ?? "알 수 없는 오류가 발생했습니다.");

            //        // 404라면 로컬 목록에서도 제거(선택 사항)
            //        if (result.StatusCode == 404)
            //        {
            //            RemoveSelectedProtocol();
            //        }

            //        return;
            //    }

            //    // 단건 제거(가벼움). 정렬 유지 등 고려가 필요하면 Refresh로 대체 가능.
            //    RemoveSelectedProtocol();
            //}
            //finally
            //{
            //    IsBusy = false;
            //}

        }

        /// <summary>
        /// 컬렉션에서 선택된 프로토콜 제거
        /// </summary>
        private void RemoveSelectedProtocol()
        {
            //if (SelectedProtocol is null) return;
            //var idx = Protocols.IndexOf(SelectedProtocol);
            //if (idx >= 0) Protocols.RemoveAt(idx);
            //SelectedProtocol = null;
        }
    }
}
