using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace VirtualRisks.Mobiles.ViewModels
{
    public class PlayerModel
    {
        public string Name { get; set; }
        public bool IsAddButton { get; set; }
        public string Type { get; set; }
    }
    public class NewGameViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public NewGameViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public MvxObservableCollection<PlayerModel> Items { get; set; } = new MvxObservableCollection<PlayerModel>();
        public IMvxCommand NextCommand => new MvxCommand(Next);
        private void Next()
        {
            _navigationService.Close(this);
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
    }
}