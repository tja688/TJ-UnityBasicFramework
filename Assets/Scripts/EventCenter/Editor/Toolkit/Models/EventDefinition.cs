using System.Collections.Generic;

namespace FlyRabbit.EventCenter.EditorToolkit
{
    public sealed class EventDefinition
    {
        public string Name;
        public string Summary;
        public bool IsDeprecated;
        public string DeprecatedMessage;
        public readonly List<EventParameterDoc> Parameters = new List<EventParameterDoc>();

        public string Signature
        {
            get
            {
                if (Parameters.Count == 0)
                {
                    return string.Empty;
                }
                var parts = new string[Parameters.Count];
                for (var i = 0; i < Parameters.Count; i++)
                {
                    parts[i] = Parameters[i].TypeName ?? string.Empty;
                }
                return string.Join(",", parts);
            }
        }
    }
}

