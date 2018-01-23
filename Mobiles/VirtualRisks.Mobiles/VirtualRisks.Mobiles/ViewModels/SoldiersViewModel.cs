using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CastleGo.Shared.Common;
using CastleGo.Shared.Games;
using MvvmCross.Core.ViewModels;

namespace VirtualRisks.Mobiles.ViewModels
{
    public class SoldiersViewRequest
    {
        public List<SoldierModel> Items { get; set; }
        public Army Army { get; set; }
    }
    public class SoldiersViewModel : MvxViewModel<SoldiersViewRequest>
    {
        private MvxInteraction<bool> _loading = new MvxInteraction<bool>();

        public IMvxInteraction<bool> Loading => _loading;

        public MvxObservableCollection<SoldierItemModel> Items { get; set; } = new MvxObservableCollection<SoldierItemModel>();

        public override void Prepare(SoldiersViewRequest parameter)
        {
            if(parameter.Items == null)
                return;
            foreach (var item in parameter.Items)
            {
                Items.Add(new SoldierItemModel()
                {
                    Army = parameter.Army
                });
            }
        }
    }
}
