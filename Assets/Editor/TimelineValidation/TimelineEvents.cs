// Assets/Editor/TimelineValidation/TimelineEvents.cs
using System;

namespace Cheggone.TimelineValidation
{
    public static class TimelineEvents
    {
        public static Action<TimelineModel> OnTimelineSelected;
        public static Action<TrackModel> OnTrackSelected;
        public static Action<ClipModel> OnClipSelected;

        public static void SelectTimeline(TimelineModel model) => OnTimelineSelected?.Invoke(model);
        public static void SelectTrack(TrackModel track) => OnTrackSelected?.Invoke(track);
        public static void SelectClip(ClipModel clip) => OnClipSelected?.Invoke(clip);
    }
}