using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Akavache;
using CastleGo.Shared;
using CastleGo.Shared.Common;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using VirtualRisks.WebApi.RestClient;
using VirtualRisks.Mobiles.Helpers;
using VirtualRisks.Mobiles.Models;
using CastleGo.Shared.Games.Events;
using CastleGo.Shared.Games;
using MvvmCross.Platform.Core;
using MvvmCross.Plugins.Messenger;
using VirtualRisks.Mobiles.Messages;

namespace VirtualRisks.Mobiles.ViewModels
{
    public class MainViewModel : MvxViewModelBase
    {
        private IVirtualRisksAPI _api;
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _dialogs;
        private readonly ICommonViewResource CommonVr;
        private readonly IGamePageViewResource Vr;
        private readonly IMvxMainThreadDispatcher _dispatcher;
        private readonly IMvxMessenger _messenger;

        public MvxObservableCollection<SoldierItemModel> Items { get; set; } = new MvxObservableCollection<SoldierItemModel>();

        private MvxInteraction<GameStateUpdate> _gameUpdate = new MvxInteraction<GameStateUpdate>();
        public IMvxInteraction<GameStateUpdate> GameUpdate => _gameUpdate;

        private MvxInteraction<GameStateUpdate> _gameInit = new MvxInteraction<GameStateUpdate>();
        public IMvxInteraction<GameStateUpdate> GameInit => _gameInit;

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
        private readonly List<ICondition<EventBaseModel>> _eventConditions;

        public static volatile int LatestEventRevision;
        public bool InGame { get; private set; }

        private int GetEventValue()
        {
            return LatestEventRevision;
        }
       
        public MainViewModel(IVirtualRisksAPI api, IMvxNavigationService navigationService, IUserDialogs dialogs,
            ICommonViewResource commonVr, IGamePageViewResource vr,
            IMvxMainThreadDispatcher dispatcher,
            IMvxMessenger messenger)
        {
            CommonVr = commonVr;
            Vr = vr;
            _api = api;
            _navigationService = navigationService;
            _dialogs = dialogs;
            _dispatcher = dispatcher;
            _messenger = messenger;

            _eventConditions = new List<ICondition<EventBaseModel>>()
            {
                new EventCondition<BattalionMovementEventModel>(BattalionMovementEventHandler),
                new EventCondition<CastleHasBeenOccupiedEventModel>(CastleHasBeenOccupiedEventHandle),
                //new EventCondition<DefendedCastleEventModel>(DefendedCastleEventHandler),
                //new EventCondition<FailedAttackCastleEventModel>(FailedAttackCastleEventHandler),
                //new EventCondition<OccupiedCastleEventModel>(OccupiedCastleEventHandler),

                //new EventCondition<SiegeHasBeenOccupiedEventModel>(SiegeHasBeenOccupiedEventHandler),
                //new EventCondition<DefendedSiegeEventModel>(DefendedSiegeEventHandler),
                //new EventCondition<FailedAttackSiegeEventModel>(FailedAttackSiegeEventHandler),
                //new EventCondition<OccupiedSiegeInCastleEventModel>(OccupiedSiegeInCastleEventHandler)
            };
        }
        private GameStateUpdate _previousState;

        private void BattalionMovementEventHandler(BattalionMovementEventModel obj)
        {
            var siegeAtCastle = _previousState.Castles.FirstOrDefault(e => new Guid(e.Id) == obj.DestinationCastleId);
            if (siegeAtCastle?.Position == null)
                return;
            var item = State.Castles.FirstOrDefault(e => e.Id == obj.DestinationCastleId.ToString());
            if (item == null)
                return;
            var siegeByCastle = _previousState.Castles.FirstOrDefault(e => new Guid(e.Id) == obj.CastleId);
            if (siegeByCastle == null)
                return;
            var siegeByArmy = siegeByCastle.Army;
            var ev = new EventModel(nameof(BattalionMovementEventModel), obj.ExecuteAt, string.Empty);
            string text;
            // remove tank
            UiRemoveTank(obj.Id.ToString());
            if (siegeAtCastle.Army != siegeByArmy)
            {
                string siegeByName = siegeByArmy == Army.Blue
                    ? _game.CreatedBy == Settings.UserId ? CommonVr.You : _game.User.Name
                    : GameHelper.GetOpponentName(_game, siegeAtCastle.Army);

                if (siegeAtCastle.Siege == null)
                {
                    text = string.Format(Vr.SiegeEventText, siegeByName, siegeAtCastle.Name);
                    siegeAtCastle.Siege = new SiegeStateModel
                    {
                        OwnerUserId = siegeByCastle.OwnerUserId
                    };
                }
                else
                {
                    if (siegeAtCastle.Siege.OwnerUserId != siegeByCastle.OwnerUserId)
                    {
                        text = string.Format(Vr.BattleVsSiegeEventText, siegeByName);
                    }
                    else
                    {
                        text = string.Format(Vr.AddMoreSoldiersToSiegeEventText, CommonVr.You, siegeAtCastle.Name);
                    }
                }
            }
            else
            {
                text = string.Format(Vr.AddMoreSoldiersToCastleEventText, CommonVr.You, siegeAtCastle.Name);
            }
            ev.Text = text;
            AddEvent(ev);
        }
        private void CastleHasBeenOccupiedEventHandle(CastleHasBeenOccupiedEventModel obj)
        {
            var castle = _previousState.Castles.FirstOrDefault(e => new Guid(e.Id) == obj.CastleId);
            if (castle?.Position == null)
                return;
            var item = State.Castles.FirstOrDefault(e => e.Id == castle.Id);
            if (item == null)
                return;
            BattleEventHandle(castle, item.Position, obj.ExecuteAt, () =>
            {
                var opponentArmy = GameHelper.GetOpponentArmy(_game);
                if (obj.BattleLog != null)
                    AddBattaleLogEvent(string.Format(Vr.CastleHasBeenOccupiedEventText, castle.Name), false, obj.ExecuteAt, obj.BattleLog, opponentArmy);
                // change castle info
                castle.Army = opponentArmy;
                switch (opponentArmy)
                {
                    case Army.Red:
                        castle.OwnerId = _game.OpponentHeroId;
                        castle.OwnerUserId = _game.OpponentId;
                        break;
                    case Army.Blue:
                        castle.OwnerId = _game.UserHeroId;
                        castle.OwnerUserId = _game.CreatedBy;
                        break;
                }
                castle.Siege = null;
                //MessagingCenter.Send(obj, "CastleHasBeenOccupiedEventModel");
                UpdateScoreStatusUi(_previousState);
            });
        }
        private void AddBattaleLogEvent(string text, bool isUserAttacking, DateTime battleAt, BattleLogModel battleLog, Army opponentArmy)
        {
            var ev = new EventModel(nameof(BattleEventModel), battleAt, text)
            {
                ActionParamater = battleLog,
                HasAction = true
            };
            //if (isUserAttacking)
            //    ev.Action = new MyCommand<object>("ShowBattleLogCommand", ShowBattleLogOfUserAttackCommandExecute);
            //else
            //    ev.Action = new MyCommand<object>("ShowBattleLogCommand", ShowBattleLogOfOpponentAttackCommandExecute);
            AddEvent(ev);
        }
        private void UpdateScoreStatusUi(GameStateUpdate state)
        {
            if (state?.Castles == null)
                return;
            var userArmy = GameHelper.GetUserArmy(_game);
            var opponentArmy = userArmy == Army.Blue ? Army.Red : Army.Blue;
            var score = state.Castles.Count(e => e.Army == userArmy).ToString();
            var opponentScore = state.Castles.Count(e => e.Army == opponentArmy).ToString();
            //if (UserStatus == null)
            //    UserStatus = new StatusModel()
            //    {
            //        Name = $"{GameHelper.GetArmyName(_game, userArmy)} ({CommonVr.You})",
            //        Color = GameHelper.GetColorByArmy(userArmy),
            //        Score = score,
            //        Coins = state.UserCoins.ToString()
            //    };
            //else
            //    UserStatus.Update(score, state.UserCoins.ToString());
            //if (OpponentStatus == null)
            //    OpponentStatus = new StatusModel()
            //    {
            //        Name = $"{GameHelper.GetArmyName(_game, opponentArmy)} ({GameHelper.GetOpponentName(_game, userArmy)})",
            //        Color = GameHelper.GetColorByArmy(opponentArmy),
            //        Score = opponentScore,
            //        Coins = state.OpponentCoins.ToString()
            //    };
            //else
            //    OpponentStatus.Update(opponentScore, state.OpponentCoins.ToString());
        }
        private void BattleEventHandle(CastleStateModel castleModel, PositionModel castleMapItem, DateTime battleTime, Action action)
        {
            if (castleModel?.Position == null)
                return;
            if (castleMapItem == null)
                return;
            var ev = new EventModel(nameof(BattleEventModel), battleTime, string.Format(Vr.BattleEventText, castleModel.Name));
            AddEvent(ev);
            action.Invoke();
        }
        public List<EventModel> Events { get; set; } = new List<EventModel>();
        private int _eventCount;

        public int EventCount
        {
            get { return _eventCount; }
            set { SetProperty(ref _eventCount, value); }
        }

        private void AddEvent(EventModel ev)
        {
            Events.Add(ev);
            EventCount++;
            _messenger.Publish(new AddEventMessage(this, ev));
        }

        private void UiRemoveTank(string v)
        {
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
                    _dispatcher.RequestMainThreadAction(() =>
                    {
                        var result = r.Result;
                        foreach (var soldier in result.Soldiers)
                        {
                            Items.Add(new SoldierItemModel()
                            {
                                Army = SelectedCastle.Army
                            });
                        }
                    });
                });
        }

        public override async Task Initialize()
        {
            _buildTask = new CancellationTokenSource();
            await base.Initialize();
        }

        public override void ViewDisappearing()
        {
            try
            {
                _buildTask.Cancel(false);
                _buildTask.Dispose();
                _buildTask = null;
            }
            catch (Exception e)
            {
            }
            base.ViewDisappearing();
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
        private GameModel _game;
        public async Task LoadGame(string id)
        {
            GameId = id;
            _loading.Raise(true);
            // game info
            _game = RestHelpers.ParseGame(await BlobCache.LocalMachine.GetOrFetchObject($"Info_{GameId}", () => _api.Game.InfoAsync(GameId),
                DateTimeOffset.Now.AddDays(1)).FirstOrDefaultAsync());
            State = new GameStateUpdate()
            {
                Routes = _game.Routes?.ToList() ?? new List<CastleRouteDto>(),
            };
            LatestEventRevision = (await _api.Game.GetStreamVersionAsync(id)).GetValueOrDefault(0);
            var gameResult = await _api.Game.BuildAsync(id, GetEventValue());
            _loading.Raise(false);
            if (gameResult.HasError.GetValueOrDefault(false))
            {
                Settings.CurrentGameId = string.Empty;
                _dialogs.Alert("Error when get game State");
                return;
            }
            InGame = true;
            ParseGameState(gameResult);
            // for first time
            if (State != null)
                _previousState = new GameStateUpdate()
                {
                    Castles = State.Castles,
                    UserSoldiers = State.UserSoldiers,
                    OpponentSoldiers = State.OpponentSoldiers,
                    Routes = State.Routes,
                    Events = State.Events,
                    IsBlue = State.IsBlue
                };
            ShowEventsLog();
            _gameInit.Raise(State);
            GetGameStateTask().ConfigureAwait(false);
        }

        private async Task GetGameStateTask()
        {
            await Task.Delay(TimeSpan.FromSeconds(20), _buildTask.Token);
            if (_buildTask.IsCancellationRequested)
                return;
            GetGameState();
            GetGameStateTask().ConfigureAwait(false);
        }
        List<EventBaseModel> _eventsFormated = new List<EventBaseModel>();
        private int _currentEvent;
        public GameAction CurrentAction { get; set; }
        private void ShowEventsLog()
        {
            _eventsFormated = GameHelper.GetEventFormatted(State?.Events ?? new List<WebApi.RestClient.Models.EventBaseModel>());
            if (_eventsFormated.Count == 0)
            {
                CurrentAction = GameAction.Nope;
                //StartShowLatestGameState();
                return;
            }
            CurrentAction = GameAction.PlayEventLog;
            _currentEvent = 0;
            PlayEvent();
        }
        private void PlayEvent()
        {
            if (_currentEvent >= _eventsFormated.Count)
            {
                CurrentAction = GameAction.Nope;
                //StartShowLatestGameState();
                return;
            }
            var @event = _eventsFormated[_currentEvent];
            foreach (var condition in _eventConditions)
            {
                if (condition.IsTrue(@event))
                {
                    condition.Handle(@event);
                }
            }
            _currentEvent++;
            PlayEvent();
        }
        private async Task GetGameState()
        {
            var gameResult = await _api.Game.BuildAsync(GameId, LatestEventRevision);
            ParseGameState(gameResult);
            ShowEventsLog();
            _gameUpdate.Raise(State);
        }

        private void ParseGameState(WebApi.RestClient.Models.GameStateModel gameResult)
        {
            if (gameResult == null)
                return;
            if (State != null)
                _previousState = new GameStateUpdate()
                {
                    Castles = State.Castles,
                    UserSoldiers = State.UserSoldiers,
                    OpponentSoldiers = State.OpponentSoldiers,
                    Routes = State.Routes,
                    Events = State.Events,
                    IsBlue = State.IsBlue
                };
            State.Castles = gameResult.Castles?.Select(e => RestHelpers.ParseCastleState(e)).ToList();
            State.UserSoldiers = gameResult.UserSoldiers?.Select(e => RestHelpers.ParseSoldier(e)).ToList();
            State.OpponentSoldiers = gameResult.OpponentSoldiers?.Select(e => RestHelpers.ParseSoldier(e)).ToList();
            State.Events = gameResult.Events?.ToList();
            State.IsBlue = gameResult.UserId == Settings.UserId;
            LatestEventRevision = gameResult.StreamRevision.GetValueOrDefault(0);
            _api.Game.UpdateStreamVersionAsync(GameId, gameResult.StreamRevision.GetValueOrDefault(0))
                .ContinueWith(r =>
                {

                })
                .ConfigureAwait(false);
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
            var runningTime = DateTime.UtcNow;
            var duration = route.Route.Duration;
            var executeTime = runningTime.Add(duration);
            var positions = route.FormattedRoute;
            if (isReverse)
                route.FormattedRoute.Reverse();
            _battalionAdded.Raise(
                new CastleGo.Shared.Games.Events.BattalionMovementEventModel
                {
                    CastleId = new Guid(fromCastle),
                    DestinationCastleId = new Guid(toCastle),
                    Soldiers = new List<string>(),
                    Route = route.Route,
                    Positions = positions,
                    Id = battalionId,
                    RunningAt = runningTime,
                    ExecuteAt = executeTime
                });
            _loading.Raise(true);
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(duration);
                GetGameState().ConfigureAwait(false);
            }).ConfigureAwait(false);
            _api.Game.BattalionAsync(GameId, RestHelpers.CreateBattalion(fromCastle, toCastle, battalionId)).ContinueWith(r =>
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
            Icon = castle.Army == Army.Blue ? "blue_castle" : "red_castle";
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

        public void MoveSoldiers(string castleId)
        {
            _loading.Raise(true);
            _api.Game.MoveSoldierAsync(GameId, new WebApi.RestClient.Models.MoveSoldierModel(castleId))
                .ContinueWith(r =>
                {
                    _loading.Raise(false);
                    if (!r.IsCompleted)
                    {
                        _dialogs.Alert("Move soldier error");
                        return;
                    }

                    GetGameState();
                });
        }

        public void ShowEvents()
        {
            if(EventCount == 0)
                return;
            _navigationService.Navigate<EventsViewModel, EventsViewRequest>(new EventsViewRequest(Events));
        }
    }
}