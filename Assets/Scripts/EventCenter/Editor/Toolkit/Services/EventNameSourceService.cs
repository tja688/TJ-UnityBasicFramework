using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;

namespace FlyRabbit.EventCenter.EditorToolkit
{
    public sealed class EventNameSourceService
    {
        public const int MaxParamCount = 5;
        public const string DefaultAssetPath = "Assets/Scripts/EventCenter/Core/EventName.cs";

        private static readonly Regex MemberLineRegex = new Regex(
            @"^\s*(?<Name>[A-Za-z_][A-Za-z0-9_]*)\s*(?:=\s*[^,]+)?\s*,?\s*$",
            RegexOptions.Compiled);

        private static readonly Regex ObsoleteRegex = new Regex(
            @"\bObsolete\s*\(\s*""(?<Message>[^""]*)""\s*,\s*(?<IsError>true|false)\s*\)",
            RegexOptions.Compiled);

        private static readonly Regex XmlListItemRegex = new Regex(
            @"<item><description>(?<Text>.*)</description></item>",
            RegexOptions.Compiled);

        private readonly string m_AssetPath;

        public EventNameSourceService(string assetPath = DefaultAssetPath)
        {
            m_AssetPath = assetPath;
        }

        public string AssetPath => m_AssetPath;

        public IReadOnlyList<EventDefinition> ReadAll(out string error)
        {
            error = null;
            if (!File.Exists(Path.GetFullPath(m_AssetPath)))
            {
                error = $"EventName source not found: {m_AssetPath}";
                return Array.Empty<EventDefinition>();
            }

            try
            {
                var lines = ReadAllLinesUtf8(m_AssetPath);
                if (!TryFindEventNameEnumRange(lines, out var bodyStart, out var bodyEnd, out error))
                {
                    return Array.Empty<EventDefinition>();
                }

                var result = ParseMembers(lines, bodyStart + 1, bodyEnd - 1);
                return result.OrderBy(d => d.Name, StringComparer.Ordinal).ToArray();
            }
            catch (Exception ex)
            {
                error = $"Failed to read EventName.cs: {ex.Message}";
                return Array.Empty<EventDefinition>();
            }
        }

        public bool TryAppendEvent(string eventName, string summary, IReadOnlyList<EventParameterDoc> parameters, out string error)
        {
            error = null;
            if (!IsValidIdentifier(eventName))
            {
                error = "Invalid identifier. Use letters, digits, underscore; cannot start with a digit.";
                return false;
            }

            var lines = ReadAllLinesUtf8(m_AssetPath);
            if (!TryFindEventNameEnumRange(lines, out var bodyStart, out var bodyEnd, out error))
            {
                return false;
            }

            var existing = ParseMembers(lines, bodyStart + 1, bodyEnd - 1);
            if (existing.Any(d => string.Equals(d.Name, eventName, StringComparison.Ordinal)))
            {
                error = $"Event already exists: {eventName}";
                return false;
            }

            var insertIndex = bodyEnd;
            var indent = DetectMemberIndent(lines, bodyStart + 1, bodyEnd - 1) ?? "        ";

            var docLines = BuildDocLines(indent, summary, parameters);
            var memberLine = $"{indent}{eventName},";

            var toInsert = new List<string>();
            if (insertIndex > 0 && !string.IsNullOrWhiteSpace(lines[insertIndex - 1]))
            {
                toInsert.Add(string.Empty);
            }
            toInsert.AddRange(docLines);
            toInsert.Add(memberLine);

            lines.InsertRange(insertIndex, toInsert);
            WriteAllLinesUtf8(m_AssetPath, lines);
            AssetDatabase.ImportAsset(m_AssetPath);
            return true;
        }

        public bool TryUpdateMemberDoc(string memberName, string summary, IReadOnlyList<EventParameterDoc> parameters, out string error)
        {
            error = null;
            var lines = ReadAllLinesUtf8(m_AssetPath);
            if (!TryFindEventNameEnumRange(lines, out var bodyStart, out var bodyEnd, out error))
            {
                return false;
            }

            if (!TryFindMemberLineIndex(lines, bodyStart + 1, bodyEnd - 1, memberName, out var memberLineIndex))
            {
                error = $"Member not found: {memberName}";
                return false;
            }

            ReplaceDocBlock(lines, memberLineIndex, BuildDocLines(GetIndent(lines[memberLineIndex]), summary, parameters));
            WriteAllLinesUtf8(m_AssetPath, lines);
            AssetDatabase.ImportAsset(m_AssetPath);
            return true;
        }

        public bool TryDeprecate(string memberName, string reason, out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(reason))
            {
                error = "Deprecate reason is required.";
                return false;
            }

            var lines = ReadAllLinesUtf8(m_AssetPath);
            if (!TryFindEventNameEnumRange(lines, out var bodyStart, out var bodyEnd, out error))
            {
                return false;
            }

            if (!TryFindMemberLineIndex(lines, bodyStart + 1, bodyEnd - 1, memberName, out var memberLineIndex))
            {
                error = $"Member not found: {memberName}";
                return false;
            }

            EnsureUsingSystem(lines);

            var memberIndent = GetIndent(lines[memberLineIndex]);
            var attrLine = $"{memberIndent}[Obsolete(\"Deprecated: {EscapeForStringLiteral(reason)}\", false)]";

            var attrStart = memberLineIndex;
            while (attrStart > bodyStart && IsAttributeLine(lines[attrStart - 1]))
            {
                attrStart--;
            }

            var hasObsolete = false;
            for (var i = attrStart; i < memberLineIndex; i++)
            {
                if (lines[i].IndexOf("Obsolete", StringComparison.Ordinal) < 0)
                {
                    continue;
                }
                hasObsolete = true;
                lines[i] = attrLine;
                break;
            }

            if (!hasObsolete)
            {
                lines.Insert(memberLineIndex, attrLine);
                memberLineIndex++;
            }

            var definitions = ParseMembers(lines, bodyStart + 1, bodyEnd - 1);
            var current = definitions.FirstOrDefault(d => string.Equals(d.Name, memberName, StringComparison.Ordinal));
            var updatedSummary = UpsertDeprecatedNote(current?.Summary, reason);
            var updatedParams = current?.Parameters ?? new List<EventParameterDoc>();

            ReplaceDocBlock(lines, memberLineIndex, BuildDocLines(memberIndent, updatedSummary, updatedParams));
            WriteAllLinesUtf8(m_AssetPath, lines);
            AssetDatabase.ImportAsset(m_AssetPath);
            return true;
        }

        private static List<string> ReadAllLinesUtf8(string assetPath)
        {
            var fullPath = Path.GetFullPath(assetPath);
            return File.ReadAllLines(fullPath, new UTF8Encoding(false, true)).ToList();
        }

        private static void WriteAllLinesUtf8(string assetPath, List<string> lines)
        {
            var fullPath = Path.GetFullPath(assetPath);
            File.WriteAllLines(fullPath, lines, new UTF8Encoding(false));
        }

        private static bool TryFindEventNameEnumRange(List<string> lines, out int bodyStartLine, out int bodyEndLine, out string error)
        {
            error = null;
            bodyStartLine = -1;
            bodyEndLine = -1;

            var enumLine = -1;
            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].IndexOf("enum EventName", StringComparison.Ordinal) >= 0)
                {
                    enumLine = i;
                    break;
                }
            }
            if (enumLine < 0)
            {
                error = "Could not locate `enum EventName`.";
                return false;
            }

            var depth = 0;
            var foundOpen = false;
            for (var i = enumLine; i < lines.Count; i++)
            {
                foreach (var c in lines[i])
                {
                    if (c == '{')
                    {
                        depth++;
                        if (!foundOpen)
                        {
                            foundOpen = true;
                            bodyStartLine = i;
                        }
                    }
                    else if (c == '}')
                    {
                        depth--;
                        if (foundOpen && depth == 0)
                        {
                            bodyEndLine = i;
                            return true;
                        }
                    }
                }
            }

            error = "Could not find matching enum closing brace for EventName.";
            return false;
        }

        private static List<EventDefinition> ParseMembers(List<string> lines, int startLine, int endLine)
        {
            var pendingDoc = new List<string>();
            var pendingAttrs = new List<string>();
            var definitions = new List<EventDefinition>();

            for (var i = startLine; i <= endLine && i >= 0 && i < lines.Count; i++)
            {
                var line = lines[i];
                var trimmed = line.TrimStart();
                if (trimmed.StartsWith("///", StringComparison.Ordinal))
                {
                    pendingDoc.Add(line);
                    continue;
                }

                if (trimmed.StartsWith("[", StringComparison.Ordinal))
                {
                    pendingAttrs.Add(line);
                    continue;
                }

                var match = MemberLineRegex.Match(line);
                if (!match.Success)
                {
                    pendingDoc.Clear();
                    pendingAttrs.Clear();
                    continue;
                }

                var name = match.Groups["Name"].Value;
                var def = new EventDefinition { Name = name };

                ApplyDoc(def, pendingDoc);
                ApplyAttributes(def, pendingAttrs);

                definitions.Add(def);
                pendingDoc.Clear();
                pendingAttrs.Clear();
            }

            return definitions;
        }

        private static void ApplyAttributes(EventDefinition def, List<string> attributeLines)
        {
            foreach (var line in attributeLines)
            {
                var match = ObsoleteRegex.Match(line);
                if (!match.Success)
                {
                    continue;
                }
                def.IsDeprecated = true;
                def.DeprecatedMessage = match.Groups["Message"].Value;
                return;
            }
        }

        private static void ApplyDoc(EventDefinition def, List<string> docLines)
        {
            def.Summary = ExtractSummary(docLines);

            var paramDocs = ExtractParamDocs(docLines);
            for (var i = 0; i < paramDocs.Count && i < MaxParamCount; i++)
            {
                paramDocs[i].Index = i + 1;
            }
            def.Parameters.AddRange(paramDocs.Take(MaxParamCount));
        }

        private static string ExtractSummary(List<string> docLines)
        {
            var inSummary = false;
            var sb = new StringBuilder();

            foreach (var raw in docLines)
            {
                var line = StripDocPrefix(raw).Trim();
                if (!inSummary)
                {
                    if (line.StartsWith("<summary>", StringComparison.Ordinal))
                    {
                        inSummary = true;
                        var after = line.Substring("<summary>".Length).Trim();
                        if (after.Length > 0 && !after.StartsWith("</summary>", StringComparison.Ordinal))
                        {
                            sb.AppendLine(after);
                        }
                    }
                    continue;
                }

                if (line.IndexOf("</summary>", StringComparison.Ordinal) >= 0)
                {
                    var before = line.Replace("</summary>", string.Empty).Trim();
                    if (before.Length > 0)
                    {
                        sb.AppendLine(before);
                    }
                    break;
                }

                if (line.Length > 0)
                {
                    sb.AppendLine(line);
                }
            }

            return sb.ToString().Trim();
        }

        private static List<EventParameterDoc> ExtractParamDocs(List<string> docLines)
        {
            var result = new List<EventParameterDoc>();
            foreach (var raw in docLines)
            {
                var line = StripDocPrefix(raw).Trim();
                var match = XmlListItemRegex.Match(line);
                if (!match.Success)
                {
                    continue;
                }
                var text = match.Groups["Text"].Value.Trim();
                ParseParamText(text, out var typeName, out var desc);
                result.Add(new EventParameterDoc { TypeName = typeName, Description = desc });
            }
            return result;
        }

        private static void ParseParamText(string text, out string typeName, out string desc)
        {
            typeName = string.Empty;
            desc = string.Empty;

            var afterIndex = text;
            var indexSplit = text.Split(new[] { '：' }, 2);
            if (indexSplit.Length == 2)
            {
                afterIndex = indexSplit[1];
            }
            var arrowSplit = afterIndex.Split(new[] { '→' }, 2);
            typeName = arrowSplit[0].Trim();
            if (arrowSplit.Length == 2)
            {
                desc = arrowSplit[1].Trim();
            }
        }

        private static string StripDocPrefix(string line)
        {
            var idx = line.IndexOf("///", StringComparison.Ordinal);
            if (idx < 0)
            {
                return line;
            }
            var after = line.Substring(idx + 3);
            if (after.StartsWith(" ", StringComparison.Ordinal))
            {
                after = after.Substring(1);
            }
            return after;
        }

        private static bool IsValidIdentifier(string name) =>
            !string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[A-Za-z_][A-Za-z0-9_]*$");

        private static string DetectMemberIndent(List<string> lines, int startLine, int endLine)
        {
            for (var i = startLine; i <= endLine && i < lines.Count; i++)
            {
                var match = MemberLineRegex.Match(lines[i]);
                if (!match.Success)
                {
                    continue;
                }
                return GetIndent(lines[i]);
            }
            return null;
        }

        private static string GetIndent(string line)
        {
            var count = 0;
            while (count < line.Length && char.IsWhiteSpace(line[count]))
            {
                count++;
            }
            return line.Substring(0, count);
        }

        private static List<string> BuildDocLines(string indent, string summary, IReadOnlyList<EventParameterDoc> parameters)
        {
            var lines = new List<string>();
            lines.Add($"{indent}/// <summary>");

            var normalizedSummary = (summary ?? string.Empty).Trim();
            if (normalizedSummary.Length == 0)
            {
                normalizedSummary = "(no summary)";
            }

            foreach (var sLine in normalizedSummary.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None))
            {
                lines.Add($"{indent}/// {sLine.TrimEnd()}");
            }
            lines.Add($"{indent}/// </summary>");

            var paramCount = ClampInt(parameters?.Count ?? 0, 0, MaxParamCount);
            if (paramCount > 0)
            {
                lines.Add($"{indent}/// <remarks>");
                lines.Add($"{indent}/// <list type=\"bullet\">");
                for (var i = 0; i < paramCount; i++)
                {
                    var typeName = parameters[i]?.TypeName?.Trim() ?? string.Empty;
                    var desc = parameters[i]?.Description?.Trim() ?? string.Empty;

                    var text = $"参数{i + 1}：{typeName}";
                    if (!string.IsNullOrWhiteSpace(desc))
                    {
                        text += $"→{desc}";
                    }
                    lines.Add($"{indent}/// <item><description>{EscapeXml(text)}</description></item>");
                }
                lines.Add($"{indent}/// </list>");
                lines.Add($"{indent}/// </remarks>");
            }

            return lines;
        }

        private static int ClampInt(int value, int min, int max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        private static void ReplaceDocBlock(List<string> lines, int memberLineIndex, List<string> newDocLines)
        {
            var attrStart = memberLineIndex;
            while (attrStart > 0 && IsAttributeLine(lines[attrStart - 1]))
            {
                attrStart--;
            }

            var docEnd = attrStart - 1;
            var docStart = docEnd + 1;
            while (docStart - 1 >= 0 && IsDocLine(lines[docStart - 1]))
            {
                docStart--;
            }

            if (docStart <= docEnd)
            {
                lines.RemoveRange(docStart, docEnd - docStart + 1);
                lines.InsertRange(docStart, newDocLines);
            }
            else
            {
                lines.InsertRange(attrStart, newDocLines);
            }
        }

        private static bool IsDocLine(string line) => line.TrimStart().StartsWith("///", StringComparison.Ordinal);

        private static bool IsAttributeLine(string line) => line.TrimStart().StartsWith("[", StringComparison.Ordinal);

        private static bool TryFindMemberLineIndex(List<string> lines, int startLine, int endLine, string memberName, out int index)
        {
            index = -1;
            for (var i = startLine; i <= endLine && i < lines.Count; i++)
            {
                var match = MemberLineRegex.Match(lines[i]);
                if (!match.Success)
                {
                    continue;
                }

                if (string.Equals(match.Groups["Name"].Value, memberName, StringComparison.Ordinal))
                {
                    index = i;
                    return true;
                }
            }
            return false;
        }

        private static void EnsureUsingSystem(List<string> lines)
        {
            if (lines.Any(l => l.Trim() == "using System;"))
            {
                return;
            }

            for (var i = 0; i < lines.Count; i++)
            {
                if (!Regex.IsMatch(lines[i], @"^\s*namespace\s+\w"))
                {
                    continue;
                }

                lines.Insert(i, "using System;");
                lines.Insert(i + 1, string.Empty);
                return;
            }
        }

        private static string UpsertDeprecatedNote(string summary, string reason)
        {
            var lines = (summary ?? string.Empty)
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Select(l => l.TrimEnd())
                .Where(l => !l.TrimStart().StartsWith("Deprecated:", StringComparison.OrdinalIgnoreCase))
                .Where(l => !l.TrimStart().StartsWith("已废弃：", StringComparison.OrdinalIgnoreCase))
                .ToList();

            lines.Add($"Deprecated: {reason.Trim()}");
            return string.Join("\n", lines.Where(l => !string.IsNullOrWhiteSpace(l)));
        }

        private static string EscapeXml(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            return value
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;");
        }

        private static string EscapeForStringLiteral(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
