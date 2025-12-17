using System.Collections.Generic;

namespace FlyRabbit.EventCenter.EditorToolkit
{
    public sealed class EventUsageGroup
    {
        public string EventName;
        public string ObservedSignature;
        public string DefinitionSignature;
        public bool HasObservedMismatch;
        public bool HasDefinitionMismatch;
        public bool MissingDefinition;
        public bool IsDeprecated;
        public string DeprecatedMessage;

        public readonly List<EventUsageRecord> Triggers = new List<EventUsageRecord>();
        public readonly List<EventUsageRecord> Adds = new List<EventUsageRecord>();
        public readonly List<EventUsageRecord> Removes = new List<EventUsageRecord>();
    }
}

