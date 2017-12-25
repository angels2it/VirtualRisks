using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using VirtualRisks.WebApi.RestClient;
using VirtualRisks.WebApi.RestClient.Models;

namespace VirtualRisks.Mobiles.ViewModels
{
    public class LocationModel
    {
        public double Lat { get; set; }
        public double Lng { get; set; }

        public LocationModel(double lat, double lng)
        {
            Lat = lat;
            Lng = lng;
        }
    }

    public class CastleModel
    {
        public LocationModel Position { get; set; }
        public int Index { get; internal set; }
    }
    public class MainViewModel : MvxViewModel
    {
        public List<CastleModel> Castles { get; private set; }
        private readonly IVirtualRisksAPI _api;
        private readonly IMvxNavigationService _navigationService;
        public MainViewModel(IVirtualRisksAPI api, IMvxNavigationService navigationService)
        {
            _api = api;
            Castles = new List<CastleModel>();
            _navigationService = navigationService;
        }
        
        public override Task Initialize()
        {
            return base.Initialize();
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            _navigationService.Navigate<NewGameViewModel>();
        }

        public IMvxCommand ResetTextCommand => new MvxCommand(ResetText);
        private void ResetText()
        {
            Text = "Hello MvvmCross";
        }

        private string _text = "Hello MvvmCross";
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        public List<object> Routes { get; private set; }

        public async Task GetCastlesAsync()
        {
            //var response = await _api.GetCastlesAsync();
            //foreach (var castle in response.Castles)
            //{
            //    Castles.Add(new CastleModel()
            //    {
            //        Position = new LocationModel(castle.Position.Lat.Value, castle.Position.Lng.Value),
            //        Index = castle.Index.Value
            //    });
            //}
            //Routes = response.Routes.ToList();
        }
    }
}