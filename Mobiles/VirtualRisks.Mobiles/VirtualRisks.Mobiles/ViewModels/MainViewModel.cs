using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MvvmCross.Binding.Bindings;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Core;
using VirtualRisks.WebApi.RestClient;
using VirtualRisks.WebApi.RestClient.Models;
using VirtualRisks.Mobiles.Helpers;
using VirtualRisks.Mobiles.Models;

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
    public class GameStateUpdate
    {
        public List<CastleRouteStateModel> Routes { get; set; }
        public List<CastleStateModel> Castles { get; set; }
    }

    public class SoldierItemModel
    {
        public string Army { get; internal set; }
    }
    public class MainViewModel : MvxViewModel
    {
        private readonly IVirtualRisksAPI _api;
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _dialogs;

        public MvxObservableCollection<SoldierItemModel> Items { get; set; } = new MvxObservableCollection<SoldierItemModel>();

        private MvxInteraction<GameStateUpdate> _gameUpdate = new MvxInteraction<GameStateUpdate>();
        private MvxInteraction<bool> _loading = new MvxInteraction<bool>();

        public IMvxInteraction<GameStateUpdate> GameUpdate => _gameUpdate;
        public IMvxInteraction<bool> Loading => _loading;

        private string GameId;
        public CastleStateModel SelectedCastle { get; set; }

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

        public MainViewModel(IVirtualRisksAPI api, IMvxNavigationService navigationService, IUserDialogs dialogs)
        {
            _api = api;
            _navigationService = navigationService;
            _dialogs = dialogs;
        }

        public void InitCastleDetail(string id)
        {
            _loading.Raise(true);
            Items.Clear();
            _api.Game.CastleAsync(Settings.CurrentGameId, id, 0)
                .ContinueWith(r =>
                {
                    _loading.Raise(false);
                    var result = r.Result;
                    foreach (var soldier in result.Soldiers)
                    {
                        Items.Add(new SoldierItemModel()
                        {
                            Army = SelectedCastle.Army
                        });
                    }
                });
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            if (!Settings.IsAuth)
            {
                await _navigationService.Navigate<LoginViewModel, LoginResponse>().ContinueWith(r =>
                {
                    CreateGamePopup();
                });
                return;
            }
            if (!string.IsNullOrEmpty(Settings.CurrentGameId))
            {
                await LoadGame(Settings.CurrentGameId);
                return;
            }
            CreateGamePopup();
        }

        private void CreateGamePopup()
        {
            _navigationService.Navigate<NewGameViewModel, NewGameResponse>().ContinueWith(CreateGameSuccess).ConfigureAwait(false);
        }
        private async Task CreateGameSuccess(Task<NewGameResponse> task)
        {
            var result = task.Result;
            Settings.CurrentGameId = result.Id;
            await LoadGame(result.Id);
        }

        private GameStateUpdate state;
        public async Task LoadGame(string id)
        {
            GameId = id;
            _loading.Raise(true);
            var gameResult = await _api.Game.BuildAsync(id);
            _loading.Raise(false);
            if (gameResult.HasError.GetValueOrDefault(false))
            {
                Settings.CurrentGameId = string.Empty;
                _dialogs.Alert("Error when get game state");
                return;
            }
            state = new GameStateUpdate()
            {
                Routes = gameResult.Routes?.ToList() ?? new List<CastleRouteStateModel>(),
                Castles = gameResult.Castles?.ToList() ?? new List<CastleStateModel>()
            };
            _gameUpdate.Raise(state);
        }

        public void DragCastleLargeDistance()
        {
            _dialogs.Alert("No any castle can handle your action");
        }

        public void DragToCastle(string fromCastle, string toCastle)
        {
            _loading.Raise(true);
            _api.Game.BattalionAsync(GameId, new BattalionModel()
            {
                CastleId = fromCastle,
                DestinationCastleId = toCastle,
                MoveByPercent = true,
                PercentOfSelectedSoldiers = 100,
                Soldiers = new List<string>()
            }).ContinueWith(r =>
            {
                _loading.Raise(false);
                _dialogs.Toast("Battalion successful", TimeSpan.FromSeconds(3));
            });
        }

        public void CastleClicked(string snippet)
        {
            var castle = state?.Castles?.FirstOrDefault(e => e.Id == snippet);
            if (castle == null)
                return;
            SelectedCastle = castle;
            Icon = castle.Army == "Blue" ? "blue_castle" : "red_castle";
            Name = castle.Name;
        }
    }

    public class CastleViewRequest
    {
        public CastleStateModel Castle { get; internal set; }
    }

    public class CastleViewResponse
    {

    }
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