# VSharp
[![NuGet](https://img.shields.io/nuget/vpre/VSharp.svg?maxAge=2592000?style=plastic)](https://www.nuget.org/packages/VSharp)

The VSharp library offers two objects you can interact with to use the VLive API: `VSharpService` and `VSharpMonitor`. Both offer different functionalities which will be discussed further.

## VSharpService

To create a `VSharpService` object, you'll first need to get an `appId` to authorize your requests. This `appId` can be found by visiting any VLive profile, opening your web browser's developer tools and searching for `app_id` in the requests sent when loading the VLive profile. Copy the `app_id` url parameter from one of the requests that uses it.

Next up you'll have to decide on a `locale` to use, VSharp will add this `locale` to the requests sent to the VLive API. The data the API will respond with might change depending on the value of the `locale` specified. For example, titles of certain VLives might change depending on the `locale` used. Currently, VSharp supports two values for this parameter: `Locale.EN` and `Locale.KO`, which represent English and Korean respectively.

Lastly, VSharp allows you to pass your own User-Agent that it'll use when sending requests to the VLive API. The default value of the User-Agent is `"VSharp VLive API Wrapper <Current Version>"`

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
`GetNoticesAsync` | Retrieves a list of notices from the channel defined by the provided `channelCode` or `channelSeq` | `ArgumentException`, `InternalServerErrorException`, `TaskCanceledException`, `UnauthorizedException`, `UndecodableChannelCodeException`, `UnkownErrorException`, `UnmappableResponseException` |
`GetPostListAsync` | Retrieves a list of posts from the provided `board` | `ArgumentException`, `InternalServerErrorException`, `TaskCanceledException`, `UnauthorizedException`, `UnkownErrorException`, `UnmappableResponseException` | `board` values for a channel can be found in the `CelebBoards` and `FanBoards` properties of a `Channel`
`GetAboutInfoAsync` | Retrieves the about information of the channel defined by the provided `channelCode` or `channelSeq` | `ArgumentException`, `InternalServerErrorException`, `NoSuchChannelException`, `TaskCanceledException`, `UnauthorizedException`, `UnkownErrorException`, `UnmappableResponseException` |
`GetVideoStatusAsync` | Retrieves the status of the video defined by the provided `videoSeq` | `ArgumentException`, `InternalServerErrorException`, `TaskCanceledException`, `UnkownErrorException`, `UnmappableResponseException` |
`GetVODInfoAsync` | Retrieves all information for the VLive defined by the provided `videoSeq` | `ArgumentException`, `InternalServerErrorException`, `ResourceUnavailableException`, `TaskCanceledException`, `UnkownErrorException`, `UnmappableResponseException` |

`count` is limited to a maximum value of 50.

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

## VSharpMonitor

The `VSharpMonitor` allows you to register tasks with three types of monitors. On the completion of a task, `VSharpMonitor` will fire an event with the completed task data that you can subscribe to.

To create a `VSharpMonitor` you will need an instance of `VSharpService`.

```cs
VSharpMonitor monitor = new VSharpMonitor(service);
```

Method | Event | Description | Note
--- | --- | --- | ---
`RegisterSubtitleMonitor` | `SubtitleAvailable` | Monitor a VLive defined by the provided `videoSeq` for when a specific kind of subtitles are available | Subtitle monitor tasks will unregister automatically upon completion
`UnegisterSubtitleMonitor` | | Will stop the monitoring task defined by the provided `videoSeq`, `language` and `type` if the task exists |
`RegisterLiveMonitor` | `NewLive` | Monitor a channel defined by the provided `channelSeq` for when the channel starts a new VLive | Has to be explicitly unregistered from using `UnregisterLiveMonitor`, `count` is used to define how many of the most recently available videos for that channel are being checked
`UnregisterLiveMonitor` | | Will stop the monitoring task defined by the provided `channelSeq` if the task exists |
`RegisterUploadMonitor` | `NewUpload` | Monitor a channel defined by the provided `channelSeq` for when the channel uploads a new VLive | Has to be explicitly unregistered from using `UnregisterLiveMonitor`, `count` is used to define how many of the most recently available videos for that channel are being checked
`UnregisterUploadMonitor` | | Will stop the monitoring task defined by the provided `channelSeq` if the task exists | 

Every time you register a monitor task, you can provide a `TimeSpan` that will act as the period that the task will check for updates.

Exceptions thrown inside the monitoring tasks are captured and redirect to the `ExceptionThrown` event.

### Examples

#### Registering a subtitle monitor task for https://www.vlive.tv/video/214915/ that checks for official English subtitles every 5 minutes

```cs
public static async Task Main(string[] args)
{
    VSharpService service = new VSharpService("my_app_id", Locale.EN);
    VSharpMonitor monitor = new VSharpMonitor(service);
    
    monitor.RegisterSubtitleMonitor(214915, Language.English, SubtitleType.Official, TimeSpan.FromMinutes(5));
    monitor.SubtitleAvailable += SubtitleAvailableHandler;

    await Task.Delay(-1);
}

private static void SubtitleAvailableHandler(object sender, Models.Events.SubtitlesAvailableEventArgs e)
{
    /* do something with e...*/
}
```

#### Registering a live monitor task for https://channels.vlive.tv/EDBF that checks for new VLives every 30 seconds

```cs
public static async Task Main(string[] args)
{
    VSharpService service = new VSharpService("my_app_id", Locale.EN);
    VSharpMonitor monitor = new VSharpMonitor(service);

    DecodeChannelCodeResponse response = await service.DecodeChannelCodeAsync("EDBF");
    monitor.RegisterLiveMonitor(response.ChannelSeq, 5, TimeSpan.FromSeconds(30));
    monitor.NewLive += NewLive;

    await Task.Delay(-1);
}

private static void NewLive(object sender, Models.Events.NewLiveEventArgs e)
{
    /* do something with e...*/
}
```

#### Registering an upload monitor task for https://channels.vlive.tv/EDBF that checks for new VLives every 2 minutes
```cs
public static async Task Main(string[] args)
{
    VSharpService service = new VSharpService("my_app_id", Locale.EN);
    VSharpMonitor monitor = new VSharpMonitor(service);

    DecodeChannelCodeResponse response = await service.DecodeChannelCodeAsync("EDBF");
    monitor.RegisterUploadMonitor(response.ChannelSeq, 5, TimeSpan.FromMinutes(2));
    monitor.NewUpload += NewUpload;

    await Task.Delay(-1);
}

private static void NewUpload(object sender, Models.Events.NewUploadEventArgs e)
{
    /* do something with e...*/
}
```
