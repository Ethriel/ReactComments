using FluentValidation;
using HtmlAgilityPack;

namespace ReactComments.Services.Validation.Extensions
{
    public static class HtmlValidationExtensions
    {
        private static readonly string[] allowedTags = { "a", "code", "i", "strong", "p" };

        private static readonly Dictionary<string, string[]> allowedAttributes = new() { { "a", new[] { "href", "title", "rel", "target" } } };

        public static IRuleBuilderOptions<T, string?> MustBeAllowedHtml<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder.Must(text =>
            {
                var doc = new HtmlDocument();
                doc.OptionCheckSyntax = false;
                doc.LoadHtml(text);

                var nodes = doc.DocumentNode
                    .Descendants()
                    .Where(n => n.NodeType == HtmlNodeType.Element);

                var tag = string.Empty;
                var tagAttrs = default(string[]);

                foreach (var node in nodes)
                {
                    tag = node.Name.ToLower();
                    if (!allowedTags.Contains(tag)) return false;

                    if (allowedAttributes.TryGetValue(tag, out tagAttrs))
                    {
                        if (node.Attributes.Any(a => !tagAttrs.Contains(a.Name.ToLower()))) return false;
                    }
                    else if (node.Attributes.Count != 0) return false;
                }

                return true;
            })
            .WithMessage("Comment contains invalid or disallowed HTML!");
        }
    }
}
