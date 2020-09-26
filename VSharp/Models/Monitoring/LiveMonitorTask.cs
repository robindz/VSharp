using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VSharp.Constants;
using VSharp.Models.Events;

namespace VSharp.Models.Monitoring
{
    internal class LiveMonitorTask : AChannelMonitorTask
    {
        public event EventHandler<LiveFoundEventArgs> LiveFound;
        public event EventHandler<Exception> ExceptionThrown;

        private readonly int _count;

        private DateTime lastLive = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(-2), TimeZones.KST);

        public LiveMonitorTask(int channelSeq, int count, TimeSpan period, VSharpService service) : base(channelSeq, period, service)
        {
            _count = count;
            Timer = new Timer(async (s) =>
            {
                if (Checking)
                    return;

                // Set flag to prevent overlapping checks
                Checking = true;

                try
                {
                    ChannelVideoListResponse channelVideoListResponse = await Service.GetChannelVideoListAsync(ChannelSeq, _count, 1);
                    List<Video> videos = channelVideoListResponse.Videos;

                    // TODO:
                    // CHANGE VOD TO LIVE AND CHECK IF ONAIRSTARTAT IS THE BEST OPTION
                    videos = videos.Where(x => x.VideoType == "VOD" && x.OnAirStartAt > lastLive).ToList();
                    for (int i = videos.Count - 1; i >= 0; i--)
                    {
                        LiveFound.Invoke(null, CreateLiveFoundEventArgs(videos[i]));
                    }

                    if (videos.Any())
                    {
                        lastLive = videos.Max(x => x.OnAirStartAt);
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
            LiveMonitorTask t = (LiveMonitorTask)obj;
            return t.ChannelSeq == ChannelSeq;
        }

        public override int GetHashCode()
            => ChannelSeq.GetHashCode();

        private LiveFoundEventArgs CreateLiveFoundEventArgs(Video v)
            => new LiveFoundEventArgs(v.VideoSeq, v.PickSortOrder, v.PlayTimeInSeconds, v.PlayCount, v.LikeCount, v.CommentCount, v.VideoType, v.Title, 
                                      v.ThumbnailUrl, v.ScreenOrientation, v.UpcomingYn, v.SpecialLiveYn, v.LiveThumbYn, v.ProductId, v.PackageProductId, 
                                      v.ProductType, v.ChannelPlusPublicYn, v.ExposeStatus, v.RepresentChannelName, v.RepresentChannelProfileImageUrl, v.Type, 
                                      v.WillStartAt, v.WillEndAt, v.CreatedAt, v.OnAirStartAt);
    }
}
