namespace FlyRabbit.EventCenter.EditorToolkit
{
    public sealed class EventUsageRecord
    {
        public EventUsageKind Kind;
        public string EventName;
        public string Signature;
        public string AssetPath;
        public int Line;
        public bool IsMismatch;
    }
}

