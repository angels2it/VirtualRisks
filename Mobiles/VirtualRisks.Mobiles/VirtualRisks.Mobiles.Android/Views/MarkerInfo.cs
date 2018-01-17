namespace VirtualRisks.Mobiles.Droid.Views
{
    public class MarkerInfo
    {

        public MarkerInfo(MarkerType type, string snippet, object @object = null)
        {
            Type = type;
            Key = snippet;
            Object = @object;
        }

        public string Key { get; set; }
        public MarkerType Type { get; set; }
        public object Object { get; internal set; }
    }
}