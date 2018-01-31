using VirtualRisks.Mobiles.Resources;

namespace VirtualRisks.Mobiles.ViewModels
{
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
}