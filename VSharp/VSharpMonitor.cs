using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using VSharp.Constants;
using VSharp.Exceptions;
using VSharp.Models;
using VSharp.Models.Events;
using VSharp.Models.Monitoring;

namespace VSharp
{
    public class VSharpMonitor
    {
        public enum Monitor
        {
            Live,
            Upload,
            Subtitle
        }

        public event EventHandler<SubtitlesAvailableEventArgs> SubtitleAvailable;
        public event EventHandler<LiveFoundEventArgs> LiveFound;
        public event EventHandler<NewUploadEventArgs> NewUpload;
        public event EventHandler<Exception> ExceptionThrown;

        private static readonly object _lock = new object();

        private readonly VSharpService _service;
        private readonly ConcurrentDictionary<int, LiveMonitorTask> _liveTasks;
        private readonly ConcurrentDictionary<int, UploadMonitorTask> _uploadTasks;
        private readonly ConcurrentDictionary<string, SubtitleMonitorTask> _subtitleTasks;

        public VSharpMonitor(VSharpService service)
        {
            _service = service;
            _subtitleTasks = new ConcurrentDictionary<string, SubtitleMonitorTask>();
            _liveTasks = new ConcurrentDictionary<int, LiveMonitorTask>();
            _uploadTasks = new ConcurrentDictionary<int, UploadMonitorTask>();
        }

        #region LiveMonitor
        public void RegisterLiveMonitor(int channelSeq, int count, TimeSpan timeSpan)
        {
            ValidateStrictlyPostiveInteger(channelSeq, nameof(channelSeq));
            ValidateStrictlyPostiveInteger(count, nameof(count));

            lock (_lock)
            {
                LiveMonitorTask task = new LiveMonitorTask(channelSeq, count, timeSpan, _service);
                if (!_liveTasks.ContainsKey(task.ChannelSeq))
                {
                    task.ExceptionThrown += ExceptionThrownRepeater;
                    task.LiveFound += LiveFoundRepeater;
                    bool success = _liveTasks.TryAdd(task.ChannelSeq, task);
                    if (!success)
                        ExceptionThrown.Invoke(null, new FailedToRegisterLiveMonitorException(channelSeq, Monitor.Live, "Failed to register a task with the monitor service."));
                    else
                        task.StartTask();
                }
            }
        }

        public void UnregisterLiveMonitor(int channelSeq)
        {
            lock (_lock)
            {
                if (_liveTasks.ContainsKey(channelSeq))
                {
                    _liveTasks[channelSeq].StopTask();
                    bool success = _liveTasks.TryRemove(channelSeq, out LiveMonitorTask _);
                    if (!success)
                        ExceptionThrown.Invoke(null, new FailedToUnregisterLiveMonitorException(channelSeq, Monitor.Live, "Failed to unregister a task with the monitor service."));
                }
            }
        }
        #endregion

        #region UploadMonitor
        public void RegisterUploadMonitor(int channelSeq, int count, TimeSpan timeSpan)
        {
            ValidateStrictlyPostiveInteger(channelSeq, nameof(channelSeq));
            ValidateStrictlyPostiveInteger(count, nameof(count));

            lock (_lock)
            {
                UploadMonitorTask task = new UploadMonitorTask(channelSeq, count, timeSpan, _service);
                if (!_uploadTasks.ContainsKey(task.ChannelSeq))
                {
                    task.ExceptionThrown += ExceptionThrownRepeater;
                    task.NewUpload += NewUploadRepeater;
                    bool success = _uploadTasks.TryAdd(task.ChannelSeq, task);
                    if (!success)
                        ExceptionThrown.Invoke(null, new FailedToRegisterUploadMonitorException(channelSeq, Monitor.Upload, "Failed to register a task with the monitor service."));
                    else
                        task.StartTask();
                }
            }
        }
        
        public void UnregisterUploadMonitor(int channelSeq)
        {
            lock (_lock)
            {
                if (_uploadTasks.ContainsKey(channelSeq))
                {
                    _uploadTasks[channelSeq].StopTask();
                    bool success = _uploadTasks.TryRemove(channelSeq, out UploadMonitorTask _);
                    if (!success)
                        ExceptionThrown.Invoke(null, new FailedToUnregisterUploadMonitorException(channelSeq, Monitor.Live, "Failed to unregister a task with the monitor service."));
                }
            }
        }
        #endregion

        #region SubtitleMonitor
        public void RegisterSubtitleMonitor(int videoSeq, Language language, SubtitleType type, TimeSpan timespan)
        {
            ValidateStrictlyPostiveInteger(videoSeq, nameof(videoSeq));

            lock (_lock)
            {
                SubtitleMonitorTask task = new SubtitleMonitorTask(videoSeq, language, type, timespan, _service);
                if (!_subtitleTasks.ContainsKey(task.Id.Value))
                {
                    task.ExceptionThrown += ExceptionThrownRepeater;
                    task.SubtitleAvailable += SubtitleAvailableRepeater;
                    bool success = _subtitleTasks.TryAdd(task.Id.Value, task);
                    if (!success)
                        ExceptionThrown.Invoke(null, new FailedToRegisterSubtitleMonitorException(videoSeq, language.Value, type.Value, Monitor.Subtitle, "Failed to register a task with the monitor service."));
                    else
                        task.StartTask();
                }
            }
        }

        public void UnregisterSubtitleMonitor(int videoSeq, Language language, SubtitleType type)
            => UnregisterSubtitleMonitor(videoSeq, language.Value, type.Value);

        private void UnregisterSubtitleMonitor(int videoSeq, string locale, string type)
        {
            ValidateStrictlyPostiveInteger(videoSeq, nameof(videoSeq));

            lock (_lock)
            {
                string key = $"{videoSeq}_{locale}_{type}";
                if (_subtitleTasks.ContainsKey(key))
                {
                    _subtitleTasks[key].StopTask();
                    bool success = _subtitleTasks.TryRemove(key, out SubtitleMonitorTask _);
                    if (!success)
                        ExceptionThrown.Invoke(null, new FailedToUnregisterSubtitleMonitorException(videoSeq, locale, type, Monitor.Subtitle, "Failed to unregister a task with the monitor service."));
                }
            }
        }
        #endregion

        private void LiveFoundRepeater(object sender, LiveFoundEventArgs e)
        {
            LiveFound.Invoke(sender, e);
        }

        private void NewUploadRepeater(object sender, NewUploadEventArgs e)
        {
            NewUpload.Invoke(sender, e);
        }

        private void SubtitleAvailableRepeater(object sender, SubtitlesAvailableEventArgs e)
        {
            UnregisterSubtitleMonitor(e.VideoSeq, e.Locale, e.Type);
            SubtitleAvailable.Invoke(sender, e);
        }

        private void ExceptionThrownRepeater(object sender, Exception e)
        {
            ExceptionThrown.Invoke(sender, e);
        }

        private void ValidateStrictlyPostiveInteger(int value, string argumentName)
        {
            if (value <= 0)
                throw new ArgumentException($"{argumentName} must be a strictly positive integer.");
        }
    }
}
