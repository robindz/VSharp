using System;
using System.Threading;
using VSharp.Constants;
using VSharp.Models.Monitoring;

namespace VSharp.Models
{
    internal class SubtitleMonitorTask
    {
        public event EventHandler<SubtitlesAvailableEventArgs> SubtitleAvailable;
        public event EventHandler<Exception> ErrorOccured;

        public SubtitleMonitorTaskId Id { get; private set; }
        public int VideoSeq { get; private set; }
        public Language Language { get; private set; }
        public SubtitleType Type { get; private set; }

        private readonly Timer _timer;
        private readonly VSharpService _service;
        private readonly TimeSpan _period;
        private bool checking;

        public SubtitleMonitorTask(int videoSeq, Language language, SubtitleType type, TimeSpan period, VSharpService service)
        {
            VideoSeq = videoSeq;
            Language = language;
            Type = type;
            Id = new SubtitleMonitorTaskId(videoSeq, language, type);
            _service = service;
            _period = period;

            _timer = new Timer(async (s) =>
            {
                if (checking)
                    return;

                // Set flag  to prevent overlapping checks
                checking = true;

                try
                {
                    VODInfo vodInfo = await _service.GetVODInfoAsync(VideoSeq);

                    // Only continue if the VOD has caption data
                    if (vodInfo.CaptionInfo == null)
                        return;

                    foreach (var details in vodInfo.CaptionInfo.Details)
                    {
                        if (details.Language == Language.Value && details.Type.ToLower() == Type.Value)
                        {
                            SubtitleAvailable.Invoke(null, CreateSubtitleEventArgs(VideoSeq, details));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorOccured.Invoke(null, e);
                }

                checking = false;
            }, null, Timeout.Infinite, Timeout.Infinite);
        }

        public void StartTask()
            => _timer.Change(TimeSpan.Zero, _period);

        public void StopTask()
            => _timer.Change(Timeout.Infinite, Timeout.Infinite);

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            SubtitleMonitorTask t = (SubtitleMonitorTask)obj;
            return t.Id == Id;
        }

        public override int GetHashCode()
            => Id.GetHashCode();

        private SubtitlesAvailableEventArgs CreateSubtitleEventArgs(int videoSeq, VODInfo.Caption.CaptionDetails details)
            => new SubtitlesAvailableEventArgs(videoSeq, details.Language, details.Country, details.Locale, 
                                               details.Label, details.Source, details.Type, details.FanName);
    }
}
