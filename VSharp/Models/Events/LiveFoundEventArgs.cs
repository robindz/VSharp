using System;

namespace VSharp.Models.Events
{
    public class NewLiveEventArgs : EventArgs
    {
        public int VideoSeq { get; private set; }
        public int PickSortOrder { get; private set; }
        public int PlayTimeInSeconds { get; private set; }
        public long PlayCount { get; private set; }
        public long LikeCount { get; private set; }
        public long CommentCount { get; private set; }
        public string VideoType { get; private set; }
        public string Title { get; private set; }
        public string ThumbnailUrl { get; private set; }
        public string ScreenOrientation { get; private set; }
        public string UpcomingYn { get; private set; }
        public string SpecialLiveYn { get; private set; }
        public string LiveThumbYn { get; private set; }
        public string ProductId { get; private set; }
        public string PackageProductId { get; private set; }
        public string ProductType { get; private set; }
        public string ChannelPlusPublicYn { get; private set; }
        public string ExposeStatus { get; private set; }
        public string RepresentChannelName { get; private set; }
        public string RepresentChannelProfileImageUrl { get; private set; }
        public string Type { get; private set; }
        public DateTime WillStartAt { get; private set; }
        public DateTime WillEndAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime OnAirStartAt { get; private set; }
        public ChannelInfo ChannelInfo { get; private set; }

        public NewLiveEventArgs(int videoSeq, int pickSortOrder, int playTimeInSeconds, long playCount, long likeCount, long commentCount, string videoType, 
                                  string title, string thumbnailUrl, string screenOrientation, string upcomingYn, string specialLiveYn, string liveThumbYn, 
                                  string productId, string packageProductId, string productType, string channelPlusPublicYn, string exposeStatus, 
                                  string representChannelName, string representChannelProfileImageUrl, string type, DateTime willStartAt, DateTime willEndAt, 
                                  DateTime createdAt, DateTime onAirStartAt, ChannelInfo channelInfo)
        {
            VideoSeq = videoSeq;
            PickSortOrder = pickSortOrder;
            PlayTimeInSeconds = playTimeInSeconds;
            PlayCount = playCount;
            LikeCount = likeCount;
            CommentCount = commentCount;
            VideoType = videoType;
            Title = title;
            ThumbnailUrl = thumbnailUrl;
            ScreenOrientation = screenOrientation;
            UpcomingYn = upcomingYn;
            SpecialLiveYn = specialLiveYn;
            LiveThumbYn = liveThumbYn;
            ProductId = productId;
            PackageProductId = packageProductId;
            ProductType = productType;
            ChannelPlusPublicYn = channelPlusPublicYn;
            ExposeStatus = exposeStatus;
            RepresentChannelName = representChannelName;
            RepresentChannelProfileImageUrl = representChannelProfileImageUrl;
            Type = type;
            WillStartAt = willStartAt;
            WillEndAt = willEndAt;
            CreatedAt = createdAt;
            OnAirStartAt = onAirStartAt;
            ChannelInfo = channelInfo;
        }
    }
}
