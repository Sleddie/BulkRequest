using BulkRequest.App.Base;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BulkRequest.App.Wpf
{
    public static class ColourfulTextDocument
    {
        public static void HighlightWordWithColour(
            this TextPointer runEnd,
            string word,
            Brush highlightColour)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return;
            }

            while (runEnd is not null)
            {
                if (runEnd.GetPointerContext(LogicalDirection.Backward) is not TextPointerContext.Text)
                {
                    runEnd = runEnd.GetNextContextPosition(LogicalDirection.Backward);
                    continue;
                }

                string runText = runEnd.GetTextInRun(LogicalDirection.Backward);
                int highlightIndex = runText.LastIndexOf(word);

                if (highlightIndex < 0)
                {
                    runEnd = runEnd.GetNextContextPosition(LogicalDirection.Backward);
                    continue;
                }

                runEnd = runEnd.GetPositionAtOffset(
                    -(runText.Length - highlightIndex),
                    LogicalDirection.Forward);
                TextPointer wordEnd = runEnd.GetPositionAtOffset(
                    word.Length,
                    LogicalDirection.Forward);
                TextRange rangeToHighlight = new(
                    runEnd,
                    wordEnd);
                rangeToHighlight.ApplyPropertyValue(
                    TextBlock.BackgroundProperty,
                    highlightColour);
            }
        }

        public static void HighlightWordsWithColour(
            this FlowDocument document,
            params ColourfulWord[] words)
        {
            if (words.Length == 0)
            {
                return;
            }

            for (int i = 0; i < words.Length; i++)
            {
                ColourfulWord markToHighlight = words[i];
                TextPointer runEnd = document.ContentEnd
                    .GetInsertionPosition(LogicalDirection.Backward);

                Color highlightColour = Color.FromArgb(
                    markToHighlight.Colour.A,
                    markToHighlight.Colour.R,
                    markToHighlight.Colour.G,
                    markToHighlight.Colour.B);

                runEnd.HighlightWordWithColour(
                    markToHighlight.Word,
                    new SolidColorBrush(highlightColour));
            }
        }
    }
}