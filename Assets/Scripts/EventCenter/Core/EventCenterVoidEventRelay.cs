using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using System;
using System.IO;
using System.Text.RegularExpressions;
#endif

namespace FlyRabbit.EventCenter
{
    [DisallowMultipleComponent]
    public sealed class EventCenterVoidEventRelay : MonoBehaviour
    {
        [SerializeField] private EventName m_EventName;
        [SerializeField] private bool m_ListenOnEnable = true;
        [SerializeField] private UnityEvent m_OnEvent = new UnityEvent();

        [SerializeField, HideInInspector] private int m_CachedParamCount;

        private bool m_IsSubscribed;

        public EventName EventName => m_EventName;

        public UnityEvent OnEvent => m_OnEvent;

        private void OnEnable()
        {
            if (!m_ListenOnEnable)
            {
                return;
            }
            if (m_CachedParamCount != 0)
            {
                return;
            }
            if (m_IsSubscribed)
            {
                return;
}
            EventCenter.AddListener(m_EventName, OnTriggered);
            m_IsSubscribed = true;
        }

        private void OnDisable()
        {
            if (!m_IsSubscribed)
            {
                return;
            }

            EventCenter.RemoveListener(m_EventName, OnTriggered);
            m_IsSubscribed = false;
        }

        private void OnTriggered()
        {
            m_OnEvent?.Invoke();
        }

#if UNITY_EDITOR
        private const string EventNameSourcePath = "Assets/Scripts/EventCenter/Core/EventName.cs";
        private static readonly Regex EnumMemberLineRegex = new Regex(
            @"^\s*(?<Name>[A-Za-z_][A-Za-z0-9_]*)\s*(?:=\s*[^,]+)?\s*,?\s*$",
            RegexOptions.Compiled);

        private static readonly Regex XmlListItemRegex = new Regex(
            @"<item><description>.*</description></item>",
            RegexOptions.Compiled);

        private void OnValidate()
        {
            m_CachedParamCount = GetDocParamCount(m_EventName);
            if (m_CachedParamCount < 0)
            {
                m_CachedParamCount = 0;
            }
            if (m_CachedParamCount > 5)
            {
                m_CachedParamCount = 5;
            }
        }

        private static int GetDocParamCount(EventName eventName)
        {
            try
            {
                var fullPath = Path.GetFullPath(EventNameSourcePath);
                if (!File.Exists(fullPath))
                {
                    return 0;
                }

                var lines = File.ReadAllLines(fullPath);
                var memberLineIndex = FindMemberLineIndex(lines, eventName.ToString());
                if (memberLineIndex < 0)
                {
                    return 0;
                }

                var count = 0;
                for (var i = memberLineIndex - 1; i >= 0; i--)
                {
                    var trimmed = lines[i].TrimStart();
                    if (!trimmed.StartsWith("///", StringComparison.Ordinal))
                    {
                        break;
                    }

                    if (XmlListItemRegex.IsMatch(trimmed))
                    {
                        count++;
                    }
                }

                return count;
            }
            catch
            {
                return 0;
            }
        }

        private static int FindMemberLineIndex(string[] lines, string memberName)
        {
            for (var i = 0; i < lines.Length; i++)
            {
                var match = EnumMemberLineRegex.Match(lines[i]);
                if (!match.Success)
                {
                    continue;
                }
                if (string.Equals(match.Groups["Name"].Value, memberName, StringComparison.Ordinal))
                {
                    return i;
                }
            }
            return -1;
        }
#endif
    }
}
