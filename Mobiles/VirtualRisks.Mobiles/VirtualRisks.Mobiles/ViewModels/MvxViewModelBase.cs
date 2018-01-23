using MvvmCross.Core.ViewModels;

namespace VirtualRisks.Mobiles.ViewModels
{
    public abstract class MvxViewModelBase : MvxViewModel
    {
        protected MvxInteraction<bool> _loading = new MvxInteraction<bool>();
        public IMvxInteraction<bool> Loading => _loading;
    }
}