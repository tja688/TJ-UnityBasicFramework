using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace FlyRabbit.EventCenter.EditorSupport
{
    internal static class EventNameDocCache
    {
        private const string EventNameSourcePath = "Assets/Scripts/EventCenter/Core/EventName.cs";

        private static readonly Regex EnumMemberLineRegex = new Regex(
            @"^\s*(?<Name>[A-Za-z_][A-Za-z0-9_]*)\s*(?:=\s*[^,]+)?\s*,?\s*$",
            RegexOptions.Compiled);

        private static readonly Regex XmlListItemRegex = new Regex(
            @"<item><description>.*</description></item>",
            RegexOptions.Compiled);

        private static DateTime m_LastWriteTimeUtc;
        private static readonly Dictionary<string, int> m_ParamCountByName = new Dictionary<string, int>(StringComparer.Ordinal);

        internal static int GetParamCount(EventName eventName)
        {
            EnsureLoaded();
            return m_ParamCountByName.TryGetValue(eventName.ToString(), out var count) ? count : 0;
        }

        internal static void Invalidate()
        {
            m_LastWriteTimeUtc = default;
            m_ParamCountByName.Clear();
        }

        private static void EnsureLoaded()
        {
            try
            {
                var fullPath = Path.GetFullPath(EventNameSourcePath);
                if (!File.Exists(fullPath))
                {
                    m_ParamCountByName.Clear();
                    m_LastWriteTimeUtc = default;
                    return;
                }

                var writeTime = File.GetLastWriteTimeUtc(fullPath);
                if (writeTime == m_LastWriteTimeUtc && m_ParamCountByName.Count > 0)
                {
                    return;
                }

                m_LastWriteTimeUtc = writeTime;
                m_ParamCountByName.Clear();

                var lines = File.ReadAllLines(fullPath);
                for (var i = 0; i < lines.Length; i++)
                {
                    var match = EnumMemberLineRegex.Match(lines[i]);
                    if (!match.Success)
                    {
                        continue;
                    }

                    var name = match.Groups["Name"].Value;
                    var count = 0;
                    for (var j = i - 1; j >= 0; j--)
                    {
                        var trimmed = lines[j].TrimStart();
                        if (!trimmed.StartsWith("///", StringComparison.Ordinal))
                        {
                            break;
                        }
                        if (XmlListItemRegex.IsMatch(trimmed))
                        {
                            count++;
                        }
                    }

                    m_ParamCountByName[name] = count;
                }
            }
            catch
            {
                m_ParamCountByName.Clear();
                m_LastWriteTimeUtc = default;
            }
        }
    }
}

