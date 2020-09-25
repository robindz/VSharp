using System;
using System.Collections.Concurrent;
using VSharp.Constants;
using VSharp.Exceptions;
using VSharp.Models;

namespace VSharp
{
    public class VSharpMonitor
    {
        public enum Monitor
        {
            Subtitle
        }

        public event EventHandler<SubtitlesAvailableEventArgs> SubtitleAvailable;
        public event EventHandler<Exception> ErrorOccured;

        private static readonly object _lock = new object();

        private readonly VSharpService _service;
        private readonly ConcurrentDictionary<string, SubtitleMonitorTask> _tasks;

        public VSharpMonitor(VSharpService service)
        {
            _service = service;
            _tasks = new ConcurrentDictionary<string, SubtitleMonitorTask>();
        }

        public void RegisterSubtitleMonitor(int videoSeq, Language language, SubtitleType type, TimeSpan timespan)
        {
            lock (_lock)
            {
                SubtitleMonitorTask task = new SubtitleMonitorTask(videoSeq, language, type, timespan, _service);
                if (!_tasks.ContainsKey(task.Id.Value))
                {
                    task.ErrorOccured += SubtitleTaskExceptionOccured;
                    task.SubtitleAvailable += SubtitleTaskCompleted;
                    bool success = _tasks.TryAdd(task.Id.Value, task);
                    if (!success)
                        ErrorOccured.Invoke(null, new FailedToRegisterSubtitleMonitorException(videoSeq, language.Value, type.Value, Monitor.Subtitle, "Failed to register a task with the monitor service."));
                    else
                        task.StartTask();
                }
            }
        }

        public void UnregisterSubtitleMonitor(int videoSeq, Language language, SubtitleType type)
            => UnregisterSubtitleMonitor(videoSeq, language.Value, type.Value);

        private void UnregisterSubtitleMonitor(int videoSeq, string language, string type)
        {
            lock (_lock)
            {
                string key = $"{videoSeq}_{language}_{type}";
                if (_tasks.ContainsKey(key))
                {
                    _tasks[key].StopTask();
                    bool success = _tasks.TryRemove(key, out SubtitleMonitorTask _);
                    if (!success)
                        ErrorOccured.Invoke(null, new FailedToUnregisterSubtitleMonitorException(videoSeq, language, type, Monitor.Subtitle, "Failed to unregister a task with the monitor service."));
                }
            }
        }

        private void SubtitleTaskCompleted(object sender, SubtitlesAvailableEventArgs e)
        {
            UnregisterSubtitleMonitor(e.VideoSeq, e.Language, e.Type);
            SubtitleAvailable.Invoke(sender, e);
        }

        private void SubtitleTaskExceptionOccured(object sender, Exception e)
        {
            ErrorOccured.Invoke(sender, e);
        }
    }
}
