using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace FlyRabbit.EventCenter.EditorToolkit
{
    public sealed class EventUsageScanner
    {
        public const string DefaultIgnoredScriptFileName = "EventCenterVoidEventRelay.cs";

        private static readonly Regex AddRegex = new Regex(
            @"(?<!""[^\s]*)EventCenter\s*\.\s*AddListener\s*(?:<\s*(?<Types>[^>]+)\s*>)?\s*\(\s*EventName\s*\.\s*(?<Name>\w+)",
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex RemoveRegex = new Regex(
            @"(?<!""[^\s]*)EventCenter\s*\.\s*RemoveListener\s*(?:<\s*(?<Types>[^>]+)\s*>)?\s*\(\s*EventName\s*\.\s*(?<Name>\w+)",
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex TriggerRegex = new Regex(
            @"(?<!""[^\s]*)EventCenter\s*\.\s*TriggerEvent\s*(?:<\s*(?<Types>[^>]+)\s*>)?\s*\(\s*EventName\s*\.\s*(?<Name>\w+)",
            RegexOptions.Compiled | RegexOptions.Singleline);

        public IReadOnlyList<EventUsageRecord> ScanScripts(IEnumerable<string> scriptAssetPaths)
        {
            var results = new List<EventUsageRecord>();
            foreach (var assetPath in scriptAssetPaths)
            {
                if (string.IsNullOrWhiteSpace(assetPath))
                {
                    continue;
                }
                if (IsIgnoredScript(assetPath))
                {
                    continue;
                }

                var fullPath = Path.GetFullPath(assetPath);
                if (!File.Exists(fullPath))
                {
                    continue;
                }

                var code = File.ReadAllText(fullPath);
                code = RemoveComments(code);

                CollectMatches(results, code, assetPath, EventUsageKind.TriggerEvent, TriggerRegex);
                CollectMatches(results, code, assetPath, EventUsageKind.AddListener, AddRegex);
                CollectMatches(results, code, assetPath, EventUsageKind.RemoveListener, RemoveRegex);
            }

            return results;
        }

        private static bool IsIgnoredScript(string assetPath)
        {
            var fileName = Path.GetFileName(assetPath);
            return string.Equals(fileName, DefaultIgnoredScriptFileName, StringComparison.OrdinalIgnoreCase);
        }

        public IReadOnlyList<EventUsageGroup> GroupAndMarkMismatches(IReadOnlyList<EventUsageRecord> records)
        {
            var dict = new Dictionary<string, EventUsageGroup>(StringComparer.Ordinal);
            foreach (var record in records)
            {
                if (string.IsNullOrWhiteSpace(record.EventName))
                {
                    continue;
                }

                if (!dict.TryGetValue(record.EventName, out var group))
                {
                    group = new EventUsageGroup { EventName = record.EventName };
                    dict.Add(record.EventName, group);
                }

                if (group.ObservedSignature == null)
                {
                    group.ObservedSignature = record.Signature ?? string.Empty;
                }

                switch (record.Kind)
                {
                    case EventUsageKind.AddListener:
                        group.Adds.Add(record);
                        break;
                    case EventUsageKind.RemoveListener:
                        group.Removes.Add(record);
                        break;
                    case EventUsageKind.TriggerEvent:
                        group.Triggers.Add(record);
                        break;
                    default:
                        break;
                }
            }

            foreach (var group in dict.Values)
            {
                MarkGroupMismatches(group);
            }

            var list = new List<EventUsageGroup>(dict.Values);
            list.Sort((a, b) => string.CompareOrdinal(a.EventName, b.EventName));
            return list;
        }

        private static void MarkGroupMismatches(EventUsageGroup group)
        {
            var expected = group.ObservedSignature ?? string.Empty;
            foreach (var usage in group.Adds)
            {
                if (!string.Equals(usage.Signature ?? string.Empty, expected, StringComparison.Ordinal))
                {
                    usage.IsMismatch = true;
                    group.HasObservedMismatch = true;
                }
            }
            foreach (var usage in group.Removes)
            {
                if (!string.Equals(usage.Signature ?? string.Empty, expected, StringComparison.Ordinal))
                {
                    usage.IsMismatch = true;
                    group.HasObservedMismatch = true;
                }
            }
            foreach (var usage in group.Triggers)
            {
                if (!string.Equals(usage.Signature ?? string.Empty, expected, StringComparison.Ordinal))
                {
                    usage.IsMismatch = true;
                    group.HasObservedMismatch = true;
                }
            }
        }

        private static void CollectMatches(
            List<EventUsageRecord> results,
            string code,
            string assetPath,
            EventUsageKind kind,
            Regex regex)
        {
            var matches = regex.Matches(code);
            foreach (Match match in matches)
            {
                var eventName = match.Groups["Name"].Value;
                var signature = RemoveAllWhitespace(match.Groups["Types"].Value);
                var line = GetLineNumber(code, match.Index);

                results.Add(new EventUsageRecord
                {
                    Kind = kind,
                    EventName = eventName,
                    Signature = signature,
                    AssetPath = assetPath,
                    Line = line,
                });
            }
        }

        private static string RemoveComments(string code)
        {
            code = Regex.Replace(code, @"//.*", string.Empty);
            code = Regex.Replace(code, @"/\*[\s\S]*?\*/", string.Empty);
            return code;
        }

        private static string RemoveAllWhitespace(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            return Regex.Replace(text, @"\s+", string.Empty);
        }

        private static int GetLineNumber(string text, int index)
        {
            var line = 1;
            var max = Math.Min(index, text.Length);
            for (var i = 0; i < max; i++)
            {
                if (text[i] == '\n')
                {
                    line++;
                }
            }
            return line;
        }
    }
}
