# VSharp
[![NuGet](https://img.shields.io/nuget/vpre/VSharp.svg?maxAge=2592000?style=plastic)](https://www.nuget.org/packages/VSharp)

The VSharp library offers two objects you can interact with to use the VLive API: `VSharpService` and `VSharpMonitor`. Both offer different functionalities which will be discussed further.

## VSharpService

To create a `VSharpService` object, you'll first need to get an `appId` to authorize your requests. This `appId` can be found by visiting any VLive profile, opening your web browser's developer tools and searching for `app_id` in the requests sent when loading the VLive profile. Copy the `app_id` url parameter from one of the requests that uses it.

Next up you'll have to decide on a `locale` to use, VSharp will add this `locale` to the requests sent to the VLive API. The data the API will respond with might change depending on the value of the `locale` specified. For example, titles of certain VLives might change depending on the `locale` used. Currently, VSharp supports two values for this parameter: `Locale.EN` and `Locale.KO`, which represent English and Korean respectively.

Lastly, VSharp allows you to pass your own User-Agent that it'll use when sending requests to the VLive API. The default value of the User-Agent is `"VSharp VLive API Wrapper"`

Now you're ready to create a `VSharpService` instance!

```cs
VSharpService service = new VSharpService("my_app_id", Locale.EN);
VSharpService service = new VSharpService("my_app_id", "my_user_agent", Locale.EN);
```

`VSharpService` offers the following methods for retrieving data:

Method | Description | Exceptions | Note
--- | --- | --- | ---
`DecodeChannelCodeAsync` | Retrieves the correspending `channelSeq` for the provided `channelCode` | `ArgumentException`, `InternalServerErrorException`, `TaskCanceledException`, `UnauthorizedException`, `UndecodableChannelCodeException`, `UnkownErrorException`, `UnmappableResponseException` |
`GetChannelAsync` | Retrieves data about the channel related to the provided `channelCode` or `channelSeq` | `ArgumentException`, `InternalServerErrorException`, `NoSuchChannelException`, `TaskCanceledException`, `UnauthorizedException`, `UndecodableChannelCodeException`, `UnkownErrorException`, `UnmappableResponseException` |
`GetChannelVideoListAsync` | Retrieves a list of videos defined by the provided `channelCode` or `channelSeq`, `count` and `page` | `ArgumentException`, `InternalServerErrorException`, `TaskCanceledException`, `UnauthorizedException`, `UndecodableChannelCodeException`, `UnkownErrorException`, `UnmappableResponseException` | 
`GetUpcomingVideoListAsync` | Retrieves a list of upcoming videos defined by the provided `channelCode` or `channelSeq`, `count` and `page` | `ArgumentException`, `InternalServerErrorException`, `TaskCanceledException`, `UnauthorizedException`, `UndecodableChannelCodeException`, `UnkownErrorException`, `UnmappableResponseException` | 
`GetPostListAsync` | Retrieves a list of posts from the provided `board` | `ArgumentException`, `InternalServerErrorException`, `TaskCanceledException`, `UnauthorizedException`, `UnkownErrorException`, `UnmappableResponseException` | `board` values for a channel can be found in the `CelebBoards` and `FanBoards` properties of a `Channel`
`GetAboutInfoAsync` | Retrieves the about information of the channel defined by the provided `channelCode` or `channelSeq` | `ArgumentException`, `InternalServerErrorException`, `NoSuchChannelException`, `TaskCanceledException`, `UnauthorizedException`, `UnkownErrorException`, `UnmappableResponseException` |
`GetVideoStatusAsync` | Retrieves the status of the video defined by the provided `videoSeq` | `ArgumentException`, `InternalServerErrorException`, `TaskCanceledException`, `UnkownErrorException`, `UnmappableResponseException` |
`GetVODInfoAsync` | Retrieves all information for the VLive defined by the provided `videoSeq` | `ArgumentException`, `InternalServerErrorException`, `ResourceUnavailableException`, `TaskCanceledException`, `UnkownErrorException`, `UnmappableResponseException` |

### Iterators

You can create three types of iterators using the `VSharpService`.

#### PostListIterator

The `PostListIterator` allows you to easily retrieve all posts from a specific `board`. Each fetch it'll retrieve `count` new posts.

```cs
PostListIterator iterator = service.CreatePostListIterator(board, count);
List<Post> posts = new List<Post>();

while (iterator.HasNext())
{
    PostListResponse response = await iterator.FetchNextAsync();
    posts.AddRange(response.Posts);
}
```

#### ChannelVideoListIterator

The `ChannelVideoListIterator` allows you to easily retrieve all videos from a specific `channelSeq`. Each fetch it'll retrieve `count` new videos.

```cs
ChannelVideoListIterator iterator = service.CreateChannelVideoListIterator(channelSeq, count);
List<Video> videos = new List<Video>();

while (iterator.HasNext())
{
    ChannelVideoListResponse response = await iterator.FetchNextAsync();
    videos.AddRange(response.Videos);
}
```

#### UpcomingVideoListIterator

The `UpcomingVideoListIterator` allows you to easily retrieve all upcoming videos from a specific `channelSeq`. Each fetch it'll retrieve `count` new videos.

```cs
UpcomingVideoListIterator iterator = service.CreateUpcomingVideoListIterator(channelSeq, count);
List<Video> videos = new List<Video>();

while (iterator.HasNext())
{
    UpcomingVideoListResponse response = await iterator.FetchNextAsync();
    videos.AddRange(response.Videos);
}
```
