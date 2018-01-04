using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using VirtualRisks.WebApi.RestClient;
using VirtualRisks.WebApi.RestClient.Models;

namespace VirtualRisks.Mobiles.ViewModels
{
    public class CastleViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IVirtualRisksAPI _api;
        public MvxObservableCollection<SoldierModel> Items { get; set; } = new MvxObservableCollection<SoldierModel>();
        private string _icon;
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public CastleViewModel(IMvxNavigationService navigationService, IVirtualRisksAPI api)
        {
            _navigationService = navigationService;
            _api = api;
        }

        public void Close()
        {
            _navigationService.Close(this);
        }

        public void InitData(string id)
        {
            
        }
    }
}