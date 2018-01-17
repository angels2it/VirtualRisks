using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Akavache;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
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
        public List<CastleRouteDto> Routes { get; set; }
        public List<CastleStateModel> Castles { get; set; }
        public List<SoldierModel> UserSoldiers { get; set; }
        public List<SoldierModel> OpponentSoldiers { get; set; }
        public bool IsBlue { get; set; }
        public int GetSoldiersAmount()
        {
            if (IsBlue)
                return UserSoldiers?.Count ?? 0;
            return OpponentSoldiers?.Count ?? 0;
        }

        public List<SoldierModel> GetMySoldiers()
        {
            if (IsBlue)
                return UserSoldiers;
            return OpponentSoldiers;
        }

        internal string GetMyArmy()
        {
            if (IsBlue)
                return "Blue";
            return "Red";
        }
    }

    public class SoldierItemModel
    {
        public string Army { get; internal set; }
    }
    public class MainViewModel : MvxViewModel
    {
        private IVirtualRisksAPI _api;
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _dialogs;

        public MvxObservableCollection<SoldierItemModel> Items { get; set; } = new MvxObservableCollection<SoldierItemModel>();

        private MvxInteraction<GameStateUpdate> _gameUpdate = new MvxInteraction<GameStateUpdate>();
        public IMvxInteraction<GameStateUpdate> GameUpdate => _gameUpdate;

        private MvxInteraction<GameStateUpdate> _gameInit = new MvxInteraction<GameStateUpdate>();
        public IMvxInteraction<GameStateUpdate> GameInit => _gameInit;
        private MvxInteraction<bool> _loading = new MvxInteraction<bool>();

       
        public IMvxInteraction<bool> Loading => _loading;
        private CancellationTokenSource _buildTask = new CancellationTokenSource();


        private MvxInteraction<BattalionMovementEventModel> _battalionAdded = new MvxInteraction<BattalionMovementEventModel>();

        public IMvxInteraction<BattalionMovementEventModel> BattalionAdded => _battalionAdded;

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
            // game state
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
        }

        private void UpdateRestApi()
        {
            _api = Mvx.Resolve<IVirtualRisksAPI>();
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

        public GameStateUpdate State;
        public async Task LoadGame(string id)
        {
            GameId = id;
            _loading.Raise(true);
            // game info
            var info = await BlobCache.LocalMachine.GetOrFetchObject($"Info_{GameId}", () => _api.Game.InfoAsync(GameId),
                DateTimeOffset.Now.AddDays(1)).FirstOrDefaultAsync();
            State = new GameStateUpdate()
            {
                Routes = info.Routes?.ToList() ?? new List<CastleRouteDto>(),
            };
            var gameResult = await _api.Game.BuildAsync(id);
            _loading.Raise(false);
            if (gameResult.HasError.GetValueOrDefault(false))
            {
                Settings.CurrentGameId = string.Empty;
                _dialogs.Alert("Error when get game State");
                return;
            }

            State.Castles = gameResult.Castles?.ToList() ?? new List<CastleStateModel>();
            State.IsBlue = gameResult.UserId == Settings.UserId;
            State.UserSoldiers = gameResult.UserSoldiers?.ToList() ?? new List<SoldierModel>();
            State.OpponentSoldiers = gameResult.OpponentSoldiers?.ToList() ?? new List<SoldierModel>();
            _gameInit.Raise(State);
            GetGameState().ConfigureAwait(false);
        }

        private async Task GetGameState()
        {
            await Task.Delay(TimeSpan.FromMinutes(1), _buildTask.Token);
            if(_buildTask.IsCancellationRequested)
                return;
            var gameResult = await _api.Game.BuildAsync(GameId);
            State.Castles = gameResult.Castles?.ToList() ?? new List<CastleStateModel>();
            State.UserSoldiers = gameResult.UserSoldiers?.ToList() ?? new List<SoldierModel>();
            State.OpponentSoldiers = gameResult.OpponentSoldiers?.ToList() ?? new List<SoldierModel>();
            _gameUpdate.Raise(State);
            GetGameState().ConfigureAwait(false);
        }
        public void DragCastleLargeDistance()
        {
            _dialogs.Toast("No any castle can handle your action");
        }

        public void DragToCastle(string fromCastle, string toCastle)
        {
            var battalionId = Guid.NewGuid();
            var route = State.Routes.FirstOrDefault(e =>
                e.FromCastle == fromCastle && e.ToCastle == toCastle ||
                e.FromCastle == toCastle && e.ToCastle == fromCastle);
            if (route == null)
            {
                _dialogs.Alert("Can not find route for this battalion");
                return;
            }

            bool isReverse = route.FromCastle == toCastle;
            _battalionAdded.Raise(
                new BattalionMovementEventModel(
                    new Guid(fromCastle),
                    new Guid(toCastle),
                    new List<string>(),
                    new RouteModel()
                    {
                        Distance = route.Route.Distance,
                        Duration = route.Route.Duration,
                        Steps = route.Route.Steps
                    }, isReverse ? route.FormattedRoute.Reverse().ToList() : route.FormattedRoute,
                    battalionId,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(1)));
            _loading.Raise(true);
            _api.Game.BattalionAsync(GameId, new BattalionModel()
            {
                CastleId = fromCastle,
                DestinationCastleId = toCastle,
                MoveByPercent = true,
                PercentOfSelectedSoldiers = 100,
                Id = battalionId,
                Soldiers = new List<string>()
            }).ContinueWith(r =>
            {
                _loading.Raise(false);
                _dialogs.Toast("Battalion successful", TimeSpan.FromSeconds(3));
            });
        }

        public void CastleClicked(string snippet)
        {
            var castle = State?.Castles?.FirstOrDefault(e => e.Id == snippet);
            if (castle == null)
                return;
            SelectedCastle = castle;
            Icon = castle.Army == "Blue" ? "blue_castle" : "red_castle";
            Name = castle.Name;
        }

        public async Task InitGame()
        {
            if (!Settings.IsAuth)
            {
                await _navigationService.Navigate<LoginViewModel, LoginResponse>().ContinueWith(r =>
                {
                    UpdateRestApi();
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

        public void ShowMySoldiers()
        {
            _navigationService.Navigate<SoldiersViewModel, SoldiersViewRequest>(new SoldiersViewRequest()
            {
                Items = State.GetMySoldiers(),
                Army = State.GetMyArmy()
            });
        }
    }
}