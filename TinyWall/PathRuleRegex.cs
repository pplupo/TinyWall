using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace pylorak.TinyWall
{
    internal static class PathRuleRegex
    {
        private static readonly Regex ParserRegex = new(@"(\${.*?})", RegexOptions.Compiled);

        public static bool ContainsRegex(string? rule)
        {
            if (string.IsNullOrEmpty(rule))
                return false;

            return ParserRegex.IsMatch(rule);
        }

        public static Regex BuildRegexFromRule(string rule)
        {
            if (rule is null)
                throw new ArgumentNullException(nameof(rule));

            var finalPattern = new StringBuilder();
            finalPattern.Append('^');

            string normalizedRule = NormalizeSeparators(rule);
            var parts = ParserRegex.Split(normalizedRule);

            foreach (string part in parts)
            {
                if (string.IsNullOrEmpty(part))
                    continue;

                if (part.StartsWith("${", StringComparison.Ordinal) && part.EndsWith("}", StringComparison.Ordinal))
                {
                    finalPattern.Append(part.Substring(2, part.Length - 3));
                }
                else
                {
                    finalPattern.Append(Regex.Escape(part));
                }
            }

            finalPattern.Append('$');
            return new Regex(finalPattern.ToString(), RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public static IEnumerable<string> ResolveMatches(string rule)
        {
            if (string.IsNullOrEmpty(rule))
                yield break;

            string normalizedRule = NormalizeSeparators(rule);
            if (!ContainsRegex(normalizedRule))
            {
                if (File.Exists(normalizedRule))
                    yield return normalizedRule;
                yield break;
            }

            Regex matcher = BuildRegexFromRule(normalizedRule);

            string? searchRoot = GetSearchRoot(normalizedRule);
            if (string.IsNullOrEmpty(searchRoot) || !Directory.Exists(searchRoot))
                yield break;

            string fileHint = GetFileNameHint(normalizedRule);
            bool hasLiteralFileHint = !string.IsNullOrEmpty(fileHint) && !ContainsRegex(fileHint);

            IEnumerable<string> candidates = hasLiteralFileHint
                ? SafeEnumerateFiles(searchRoot, fileHint)
                : SafeEnumerateFiles(searchRoot);

            foreach (var candidate in candidates)
            {
                string normalizedCandidate = NormalizeSeparators(candidate);
                if (matcher.IsMatch(normalizedCandidate))
                    yield return normalizedCandidate;
            }
        }

        private static string NormalizeSeparators(string value)
        {
            return value.Replace("/", "\\");
        }

        private static string? GetSearchRoot(string normalizedRule)
        {
            int regexIndex = normalizedRule.IndexOf("${", StringComparison.Ordinal);
            string candidate = regexIndex >= 0 ? normalizedRule.Substring(0, regexIndex) : normalizedRule;

            int lastSeparator = candidate.LastIndexOf('\\');
            if (lastSeparator >= 0)
            {
                string prefix = candidate.Substring(0, lastSeparator);
                if (!string.IsNullOrEmpty(prefix))
                    return prefix;
            }

            string? root = Path.GetPathRoot(normalizedRule);
            return string.IsNullOrEmpty(root) ? null : root;
        }

        private static string GetFileNameHint(string normalizedRule)
        {
            int lastSeparator = normalizedRule.LastIndexOf('\\');
            return lastSeparator >= 0 ? normalizedRule.Substring(lastSeparator + 1) : normalizedRule;
        }

        private static IEnumerable<string> SafeEnumerateFiles(string root, string searchPattern = "*")
        {
            var stack = new Stack<string>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                var currentDir = stack.Pop();

                IEnumerable<string> files;
                try
                {
                    files = Directory.EnumerateFiles(currentDir, searchPattern, SearchOption.TopDirectoryOnly);
                }
                catch
                {
                    files = Array.Empty<string>();
                }

                foreach (var file in files)
                    yield return file;

                IEnumerable<string> subdirs;
                try
                {
                    subdirs = Directory.EnumerateDirectories(currentDir);
                }
                catch
                {
                    continue;
                }

                foreach (var dir in subdirs)
                    stack.Push(dir);
            }
        }
    }
}
