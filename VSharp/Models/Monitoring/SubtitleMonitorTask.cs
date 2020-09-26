using System;
using System.Threading;
using VSharp.Constants;
using VSharp.Models.Monitoring;
using VSharp.Models.Events;

namespace VSharp.Models
{
    internal class SubtitleMonitorTask : AVideoMonitorTask
    {
        public event EventHandler<SubtitlesAvailableEventArgs> SubtitleAvailable;
        public event EventHandler<Exception> ExceptionThrown;

        public SubtitleMonitorTaskId Id { get; private set; }
        public Language Language { get; private set; }
        public SubtitleType Type { get; private set; }

        public SubtitleMonitorTask(int videoSeq, Language language, SubtitleType type, TimeSpan period, VSharpService service) : base(videoSeq, period, service)
        {
            Language = language;
            Type = type;
            Id = new SubtitleMonitorTaskId(videoSeq, language, type);

            Timer = new Timer(async (s) =>
            {
                if (Checking)
                    return;

                // Set flag to prevent overlapping checks
                Checking = true;

                try
                {
                    VODInfo vodInfo = await Service.GetVODInfoAsync(VideoSeq);

                    // Only continue if the VOD has caption data
                    if (vodInfo.CaptionInfo == null)
                        return;

                    foreach (var details in vodInfo.CaptionInfo.Details)
                    {
                        if (details.Locale == Language.Value && details.Type.ToLower() == Type.Value)
                        {
                            SubtitleAvailable.Invoke(null, CreateSubtitleEventArgs(VideoSeq, details));
                        }
                    }
                }
                catch (Exception e)
                {
                    ExceptionThrown.Invoke(null, e);
                }
                finally
                {
                    Checking = false;
                }
            }, null, Timeout.Infinite, Timeout.Infinite);
        }

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
