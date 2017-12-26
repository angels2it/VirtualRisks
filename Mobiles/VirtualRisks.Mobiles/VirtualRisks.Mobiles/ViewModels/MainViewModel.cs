using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using VirtualRisks.WebApi.RestClient;
using VirtualRisks.WebApi.RestClient.Models;
using VirtualRisks.Mobiles.Helpers;

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

    public class GameStateUpdate
    {
        public List<CastleRouteStateModel> Routes { get; set; }
        public List<CastleStateModel> Castles { get; set; }
    }
    public class MainViewModel : MvxViewModel
    {
        private readonly IVirtualRisksAPI _api;
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _dialogs;


        private MvxInteraction<GameStateUpdate> _gameUpdate = new MvxInteraction<GameStateUpdate>();

        // need to expose it as a public property for binding (only IMvxInteraction is needed in the view)
        public IMvxInteraction<GameStateUpdate> GameUpdate => _gameUpdate;

        public MainViewModel(IVirtualRisksAPI api, IMvxNavigationService navigationService, IUserDialogs dialogs)
        {
            _api = api;
            _navigationService = navigationService;
            _dialogs = dialogs;
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            if (!string.IsNullOrEmpty(Settings.CurrentGameId))
            {
                LoadGame(Settings.CurrentGameId);
                return;
            }
            _dialogs.ShowLoading();
            await Task.Delay(2000);
            _dialogs.HideLoading();
            _navigationService.Navigate<NewGameViewModel, NewGameResponse>().ContinueWith(CreateGameSuccess).ConfigureAwait(false);
        }

        private async Task CreateGameSuccess(Task<NewGameResponse> task)
        {
            var result = task.Result;
            Settings.CurrentGameId = result.Id;
            await LoadGame(result.Id);
        }

        public async Task LoadGame(string id)
        {
            _dialogs.ShowLoading("Get game info...");
            var gameResult = await _api.Game.BuildAsync(id);
            _dialogs.HideLoading();
            if (gameResult.HasError.GetValueOrDefault(false))
            {
                _dialogs.Alert("Error when get game state");
                return;
            }
            var request = new GameStateUpdate()
            {
                Routes = gameResult.Routes?.ToList() ?? new List<CastleRouteStateModel>(),
                Castles = gameResult.Castles?.ToList() ?? new List<CastleStateModel>()
            };
            _gameUpdate.Raise(request);
        }
    }
}