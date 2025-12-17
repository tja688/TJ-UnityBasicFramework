#if UNITY_EDITOR
using System;

namespace FlyRabbit.EventCenter
{
    internal static class EventDiagnostics
    {
        internal static bool Enabled;
        internal static event Action<EventName, string> OnTrigger;

        internal static void NotifyTrigger(EventName eventName, string signature)
        {
            if (!Enabled)
            {
                return;
            }
            var handler = OnTrigger;
            if (handler != null)
            {
                handler(eventName, signature ?? string.Empty);
            }
        }
    }

    internal static class EventDiagnosticsSignature
    {
        internal static string TypeName(Type type) => type.FullName ?? type.Name;
    }

    internal static class EventDiagnosticsSignatureCache
    {
        internal static readonly string Empty = string.Empty;
    }

    internal static class EventDiagnosticsSignatureCache<T1>
    {
        internal static readonly string Value = EventDiagnosticsSignature.TypeName(typeof(T1));
    }

    internal static class EventDiagnosticsSignatureCache<T1, T2>
    {
        internal static readonly string Value = string.Concat(
            EventDiagnosticsSignature.TypeName(typeof(T1)),
            ",",
            EventDiagnosticsSignature.TypeName(typeof(T2)));
    }

    internal static class EventDiagnosticsSignatureCache<T1, T2, T3>
    {
        internal static readonly string Value = string.Concat(
            EventDiagnosticsSignature.TypeName(typeof(T1)),
            ",",
            EventDiagnosticsSignature.TypeName(typeof(T2)),
            ",",
            EventDiagnosticsSignature.TypeName(typeof(T3)));
    }

    internal static class EventDiagnosticsSignatureCache<T1, T2, T3, T4>
    {
        internal static readonly string Value = string.Concat(
            EventDiagnosticsSignature.TypeName(typeof(T1)),
            ",",
            EventDiagnosticsSignature.TypeName(typeof(T2)),
            ",",
            EventDiagnosticsSignature.TypeName(typeof(T3)),
            ",",
            EventDiagnosticsSignature.TypeName(typeof(T4)));
    }

    internal static class EventDiagnosticsSignatureCache<T1, T2, T3, T4, T5>
    {
        internal static readonly string Value = string.Concat(
            EventDiagnosticsSignature.TypeName(typeof(T1)),
            ",",
            EventDiagnosticsSignature.TypeName(typeof(T2)),
            ",",
            EventDiagnosticsSignature.TypeName(typeof(T3)),
            ",",
            EventDiagnosticsSignature.TypeName(typeof(T4)),
            ",",
            EventDiagnosticsSignature.TypeName(typeof(T5)));
    }
}
#endif

