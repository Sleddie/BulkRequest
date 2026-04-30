using DefaultSolutions;
using System.Diagnostics;
using System.Drawing;

namespace BulkRequest.App.Base
{
    public abstract class QueryTextEditing : IQueryTextEditing
    {
        private const int UIDelay = 400;

        public abstract string Text { get; set; }
        public abstract bool IsDefaultSettings { get; set; }
        public virtual string DefaultLocaleMark { get; set; } = "[l]";
        public abstract string LocaleMark { get; set; }
        public virtual string DefaultKernelMark { get; set; } = "[k]";
        public abstract string KernelMark { get; set; }
        public virtual string DefaultMonthMark { get; set; } = "[m]";
        public abstract string MonthMark { get; set; }
        public virtual string DefaultYearMark { get; set; } = "[y]";
        public abstract string YearMark { get; set; }
        protected long StopwatchStart { get; set; }
        protected bool IsHandlersEnabled { get; set; }
        protected virtual string? CurrentTextFileName { get; set; }
        protected virtual ColourfulWord[] Marks { get
            {
                return [
                    new(LocaleMark ?? DefaultLocaleMark, Color.Orange),
                    new(KernelMark ?? DefaultKernelMark, Color.Gold)/*,
                    new(MonthMark ?? DefaultMonthMark, Color.LightGreen),
                    new(YearMark ?? DefaultYearMark, Color.LightBlue)*/
                    ];
            } }

        protected virtual void OpenAndReadTextFile() =>
            Text = FileExplore.OpenAndReadText(CurrentTextFileName);

        protected virtual void CheckDefaultSettings()
        {
            if (IsDefaultSettings)
            {
                LocaleMark = DefaultLocaleMark;
                KernelMark = DefaultKernelMark;
                MonthMark = DefaultMonthMark;
                YearMark = DefaultYearMark;
            }
        }

        protected virtual void CheckAllMarks()
        {
            LocaleMark = LocaleMark.Trim();
            KernelMark = KernelMark.Trim();
            MonthMark = MonthMark.Trim();
            YearMark = YearMark.Trim();
            StopwatchStart = Stopwatch.GetTimestamp();
            HightlightAllMarksAsync();
        }

        protected virtual async Task HightlightAllMarksAsync()
        {
            await Task.Delay(UIDelay + 100);
            double realDelay = Stopwatch.GetElapsedTime(StopwatchStart)
                .TotalMilliseconds;

            if (!IsHandlersEnabled
                || realDelay < UIDelay)
            {
                return;
            }

            IsHandlersEnabled = false;
            HighlightMarksInTextControl();
            IsHandlersEnabled = true;
        }

        protected abstract void HighlightMarksInTextControl();
    }

    public interface IQueryTextEditing
    {
        string Text { get; set; }
        bool IsDefaultSettings { get; set; }
        string DefaultKernelMark { get; set; }
        string DefaultLocaleMark { get; set; }
        string DefaultMonthMark { get; set; }
        string DefaultYearMark { get; set; }
        string KernelMark { get; set; }
        string LocaleMark { get; set; }
        string MonthMark { get; set; }
        string YearMark { get; set; }
    }
}