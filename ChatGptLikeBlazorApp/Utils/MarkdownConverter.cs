using Markdig;

namespace ChatGptLikeBlazorApp.Utils;

public static class MarkdownConverter
{
    private static MarkdownPipeline _pipeline;
    static MarkdownConverter()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();
    }

    public static string ToHtml(string markdown) => Markdown.ToHtml(markdown, _pipeline);
}
