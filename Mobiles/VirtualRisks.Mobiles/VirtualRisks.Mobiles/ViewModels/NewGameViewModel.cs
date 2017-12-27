using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using VirtualRisks.Mobiles.Models;
using VirtualRisks.WebApi.RestClient;
using VirtualRisks.WebApi.RestClient.Models;

namespace VirtualRisks.Mobiles.ViewModels
{
    public class NewGameViewModel : MvxViewModel<NewGameRequest, NewGameResponse>
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IVirtualRisksAPI _api;
        private readonly IUserDialogs _dialogs;


        public MvxInteraction<bool> CreateGameInteraction { get; } = new MvxInteraction<bool>();

        public NewGameViewModel(IMvxNavigationService navigationService, IVirtualRisksAPI api, IUserDialogs dialogs)
        {
            _navigationService = navigationService;
            _api = api;
            _dialogs = dialogs;
        }

        public MvxObservableCollection<PlayerModel> Items { get; set; } = new MvxObservableCollection<PlayerModel>();
        public IMvxCommand NextCommand => new MvxAsyncCommand(Next);
        private async Task Next()
        {
            try
            {
                var result = await _api.Game.CreateAsync(new CreateGameModel()
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
                });
                CreateGameInteraction.Raise(true);
                await _navigationService.Close(this, new NewGameResponse()
                {
                    Id = result.Id
                });
            }
            catch (Exception e)
            {
                CreateGameInteraction.Raise(false);
                _dialogs.Alert("Error when creating game");
            }
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