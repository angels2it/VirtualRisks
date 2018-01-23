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
using VirtualRisks.Mobiles.Resources;
using MvvmCross.Platform.Core;

namespace VirtualRisks.Mobiles.ViewModels
{
    public enum GameAction
    {
        Nope,
        GroupSoldier,
        PlayEventLog
    }
    public interface ICommonViewResource
    {
        string ComfirmTitle { get; }
        string LoginSessionEnded { get; }
        string Initing { get; }
        string GetUserLocation { get; }
        string Creating { get; }
        string ErrorGetLocation { get; }
        string Loading { get; }
        string Computer { get; }
        string Close { get; }
        string You { get; }
        string Vs { get; }
        string Score { get; }
        string SelectAll { get; }
        string Removing { get; }
        string Remove { get; }
        string Revenue { get; }
    }

    public class CommonViewResource : ICommonViewResource
    {
        public string ComfirmTitle => AppResource.confirm_title;
        public string LoginSessionEnded => AppResource.alert_sessionended;
        public string Initing => AppResource.loading_init;
        public string GetUserLocation => AppResource.loading_getlocation;
        public string Creating => AppResource.loading_create;
        public string ErrorGetLocation => AppResource.error_getlocation;
        public string Loading => AppResource.loading;
        public string Computer => AppResource.computer;
        public string Close => AppResource.bt_close;
        public string You => AppResource.you;
        public string Vs => AppResource.vs;
        public string Score => AppResource.score;
        public string SelectAll => AppResource.selectall;
        public string Removing => AppResource.removing;
        public string Remove => AppResource.remove;
        public string Revenue => AppResource.revenue;
    }
    public interface IGamePageViewResource
    {
        /// <summary>
        /// {0} - source castle name
        /// {1} - destination castle name
        /// </summary>
        string ConfirmBattalionMessage { get; }

        string LoadingBattalion { get; }
        string StatusBuildGame { get; }
        string Move { get; }
        string Regret { get; }
        string MoveSoldier { get; }
        /// <summary>
        /// {0} - selected soldiers
        /// </summary>
        string MoveSoldiers { get; }

        string BattalionError { get; }
        string BattalionSuccess { get; }
        string GameStateError { get; }
        string GameEnded { get; }
        string MyHero { get; }
        string OpponentHero { get; }
        /// <summary>
        /// {0} time formatted
        /// </summary>
        string OpponentHeroWithTime { get; }

        string YourCastleHasInit { get; }
        string NeutralCastleHasInit { get; }
        string OpponentCastleHasInit { get; }
        /// <summary>
        /// {0} - user name
        /// {1} - castle name
        /// </summary>
        string SiegeEventText { get; }

        /// <summary>
        /// {0} - user name
        /// </summary>
        string BattleVsSiegeEventText { get; }
        /// <summary>
        /// {0} - user name
        /// {1} - castle name
        /// </summary>
        string AddMoreSoldiersToSiegeEventText { get; }
        /// <summary>
        /// {0} - user name
        /// {1} - castle name
        /// </summary>
        string AddMoreSoldiersToCastleEventText { get; }
        /// <summary>
        /// {0} - castle name
        /// </summary>
        string BattleEventText { get; }
        /// <summary>
        /// {0} - castle name
        /// </summary>
        string CastleHasBeenOccupiedEventText { get; }

        string FailedAttackCastleEventText { get; }
        string DefendedCastleEventText { get; }
        /// <summary>
        /// {0} - castle name
        /// </summary>
        string OccupiedCastleEventText { get; }

        /// <summary>
        /// {0} - castle name
        /// </summary>
        string SiegeHasBeenOccupiedEventText { get; }
        /// <summary>
        /// {0} - castle name
        /// </summary>
        string DefendedSiegeEventText { get; }
        /// <summary>
        /// {0} castle name
        /// </summary>
        string FailedAttackSiegeEventText { get; }
        /// <summary>
        /// {0} - castle name
        /// </summary>
        string OccupiedSiegeEventText { get; }

        /// <summary>
        /// {0} - distance text
        /// {1} - time text
        /// </summary>
        string RouteInfoText { get; }
    }

    public class GamePageViewResource : IGamePageViewResource
    {
        public string ConfirmBattalionMessage => AppResource.confirm_battalion_message;
        public string LoadingBattalion => AppResource.loading_battalion;
        public string StatusBuildGame => AppResource.status_buildgame;
        public string Move => AppResource.bt_move;
        public string Regret => AppResource.bt_regret;
        public string MoveSoldier => AppResource.lb_movesoldier;
        public string MoveSoldiers => AppResource.lb_movesoldiers;
        public string BattalionError => AppResource.error_battalion;
        public string BattalionSuccess => AppResource.success_battalion;
        public string GameStateError => AppResource.error_gamestate;
        public string GameEnded => AppResource.alert_gameended;
        public string MyHero => AppResource.myhero;
        public string OpponentHero => AppResource.opponent_hero;
        public string OpponentHeroWithTime => AppResource.opponent_hero_with_time;
        public string YourCastleHasInit => AppResource.lb_castleinit_user;
        public string NeutralCastleHasInit => AppResource.lb_castleinit_neutral;
        public string OpponentCastleHasInit => AppResource.lb_castleinit_opponent;
        public string SiegeEventText => AppResource.event_siegehassetup;
        public string BattleVsSiegeEventText => AppResource.event_battlevssiege;
        public string AddMoreSoldiersToSiegeEventText => AppResource.event_addmoresoldiertosiege;
        public string AddMoreSoldiersToCastleEventText => AppResource.event_addmoresoldiertocastle;
        public string BattleEventText => AppResource.event_battlebegun;
        public string CastleHasBeenOccupiedEventText => AppResource.event_castlehasbeenoccupied;
        public string FailedAttackCastleEventText => AppResource.event_failedattackcastle;
        public string DefendedCastleEventText => AppResource.event_defendedcastle;
        public string OccupiedCastleEventText => AppResource.event_occupiedcastle;
        public string SiegeHasBeenOccupiedEventText => AppResource.event_siegehasbeenoccupied;
        public string DefendedSiegeEventText => AppResource.event_defendedsiege;
        public string FailedAttackSiegeEventText => AppResource.event_failedattacksiege;
        public string OccupiedSiegeEventText => AppResource.event_occupiedsiege;
        public string RouteInfoText => AppResource.label_battalionrouteinfo;
    }
    public class EventModel
    {
        public string TimeText { get; set; }
        public string Text { get; set; }
        public bool HasAction { get; set; }
        public Action Action { get; set; }
        public object ActionParamater { get; set; }
        public InitActionModel InitAction { get; set; }

        public EventModel(DateTime time, string text)
        {
            time = time.ToLocalTime();
            if (time.Day != DateTime.Now.Day)
                TimeText = time.ToString("DD-MM hh:mm");
            else
                TimeText = time.ToString("hh:mm");
            Text = text;
        }
    }

    public class InitActionModel
    {
        public ActionType Type { get; set; }
        public TimeSpan Interval { get; set; }
        public Func<string> UpdateAction { get; set; }
        public CancellationTokenSource CancelToken { get; set; }
    }

    public enum ActionType
    {
        CountDown
    }
    public class MainViewModel : MvxViewModelBase
    {
        private IVirtualRisksAPI _api;
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _dialogs;
        private readonly ICommonViewResource CommonVr;
        private readonly IGamePageViewResource Vr;
        private readonly IMvxMainThreadDispatcher _dispatcher;

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
            IMvxMainThreadDispatcher dispatcher)
        {
            CommonVr = commonVr;
            Vr = vr;
            _api = api;
            _navigationService = navigationService;
            _dialogs = dialogs;
            _dispatcher = dispatcher;

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
            var ev = new EventModel(obj.ExecuteAt, string.Empty);
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
            var ev = new EventModel(battleAt, text)
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
            var ev = new EventModel(battleTime, string.Format(Vr.BattleEventText, castleModel.Name));
            AddEvent(ev);
            action.Invoke();
        }
        public MvxObservableCollection<EventModel> Events { get; set; } = new MvxObservableCollection<EventModel>();
        public int EventCount { get; set; }
        private void AddEvent(EventModel ev)
        {
            Events.Add(ev);
            EventCount++;
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
            var gameResult = await _api.Game.BuildAsync(GameId);
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
            _api.Game.UpdateStreamVersionAsync(GameId, gameResult.StreamRevision.GetValueOrDefault(0)).ConfigureAwait(false);
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
    }
}