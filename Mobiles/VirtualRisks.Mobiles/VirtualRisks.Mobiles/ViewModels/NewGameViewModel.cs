using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using VirtualRisks.WebApi.RestClient;
using VirtualRisks.WebApi.RestClient.Models;

namespace VirtualRisks.Mobiles.ViewModels
{
    public class PlayerModel
    {
        public string Name { get; set; }
        public bool IsAddButton { get; set; }
        public string Type { get; set; }
    }

    public class NewGameRequest
    {

    }
    public class NewGameResponse
    {
        public string Id { get; set; }
    }
    public class NewGameViewModel : MvxViewModel<NewGameRequest, NewGameResponse>
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IVirtualRisksAPI _api;
        private readonly IUserDialogs _dialogs;

        public NewGameViewModel(IMvxNavigationService navigationService, IVirtualRisksAPI api, IUserDialogs dialogs)
        {
            _navigationService = navigationService;
            _api = api;
            _dialogs = dialogs;
        }

        public MvxObservableCollection<PlayerModel> Items { get; set; } = new MvxObservableCollection<PlayerModel>();
        public IMvxCommand NextCommand => new MvxCommand(Next);
        private void Next()
        {
            _dialogs.ShowLoading("Creating game...");
            _api.Game.CreateAsync(new CreateGameModel()
            {
                Difficulty = "Normal",
                SelfPlaying = true,
                Lat = 10.78761,
                Lng = 106.6987,
                UserArmySetting = new GameArmySettingModel()
                {
                    Id = "c795ddfe-41d8-44b2-8fcb-212f8601c554",
                    Castles = new List<GameCastleSettingModel>()
                    {
                        new GameCastleSettingModel()
                        {
                            Id = "string",
                            Name = "string"
                        }
                    }
                }
            }).ContinueWith(r =>
            {
                _dialogs.HideLoading();
                if (r.IsFaulted || r.IsCanceled)
                {
                    _dialogs.Alert("Error when creating game");
                    return;
                }
                _navigationService.Close(this, new NewGameResponse()
                {
                    Id = r.Result.Id
                });
            });
        }
        public override Task Initialize()
        {
            Items.Add(new PlayerModel()
            {
                Name = "Viking",
                Type = "Human"
            });
            Items.Add(new PlayerModel()
            {
                Name = "BOT 1",
                Type = "Bot"
            });
            Items.Add(new PlayerModel()
            {
                IsAddButton = true
            });
            return base.Initialize();
        }

        public override void Prepare(NewGameRequest parameter)
        {

        }
    }
}