using OcppTestTool.Models;
using OcppTestTool.Models.Dtos.Protocol;
using OcppTestTool.Models.Entities.Protocol;
using OcppTestTool.Models.Entities.User;
using OcppTestTool.Services.Protocol;
using OcppTestTool.Services.Windows;
using OcppTestTool.ViewModels.ChildWindows;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Abstractions.Controls;

namespace OcppTestTool.ViewModels.Pages
{
    public partial class ProtocolManagementViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private readonly IProtocolManagementService _service;
        private readonly IWindowService _windows;

        [ObservableProperty] private ObservableCollection<OcppProtocol> protocols = new();
        [ObservableProperty] private OcppProtocol? selectedProtocol;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string errorMessage = "";

        public ProtocolManagementViewModel(IWindowService windows, IProtocolManagementService service)
        {
            _windows = windows;
            _service = service;
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
            await LoadAsync();
        }

        [RelayCommand]
        private async Task LoadAsync()
        {
            ErrorMessage = "";
            IsBusy = true;

            try
            {
                var list = await _service.GetProtocolsAsync();
                Protocols = new ObservableCollection<OcppProtocol>(list);
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
        private Task RefreshAsync() => LoadAsync();

        [RelayCommand]
        private async Task OpenCreateAsync()
        {
            var created = await _windows.ShowDialogAsync<CreateProtocolWindowViewModel, OcppProtocol?>(vm =>
            {
                // 초기값 세팅이 필요하면 여기서
                vm.Draft = new OcppProtocolDraft { Transport = "WebSocket", Codec = "JSON" };

                vm.SaveFunc = async draft =>
                {
                    var dto = new OcppProtocolCreateDto
                    {
                        Name = draft.Name,
                        Version = draft.Version,
                        Transport = draft.Transport,
                        Codec = draft.Codec,
                        Meta = string.IsNullOrWhiteSpace(draft.Description)
                            ? null
                            : new OcppProtocolMetaDto { Description = draft.Description }
                    };
                    return await _service.CreateProtocolAsync(dto);
                };

            });

            if (created is null) return; // 취소

            await LoadAsync(); // 성공으로 닫혔으면 목록 갱신
        }
    }
}
