// Assets/Editor/TimelineValidation/TimelineModel.cs
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cheggone.TimelineValidation
{
    [Serializable]
    public class ClipModel
    {
        public string id;
        public string name;
        public float start;     // seconds (or frames if useFrameScale)
        public float duration;  // seconds
        public string typeHint; // placeholder config hint

        public float End => start + duration;

        public ClipModel(string name, float start, float duration, string typeHint)
        {
            this.id = Guid.NewGuid().ToString();
            this.name = name;
            this.start = start;
            this.duration = duration;
            this.typeHint = typeHint;
        }
    }

    [Serializable]
    public class TrackModel
    {
        public string id;
        public string headerName;
        public List<ClipModel> clips = new();

        public TrackModel(string headerName)
        {
            this.id = Guid.NewGuid().ToString();
            this.headerName = headerName;
        }
    }

    public class TimelineModel
    {
        public bool useFrameScale = false;
        public float fps = 30f;

        public float zoom = 1f;             // zoom factor
        public float pixelsPerSecond = 100; // base scale
        public float playheadTime = 0f;

        public List<TrackModel> tracks = new();

        public float MaxEndTime
        {
            get
            {
                float max = 0;
                foreach (var t in tracks)
                foreach (var c in t.clips)
                    max = Mathf.Max(max, c.End);
                return max;
            }
        }

        public TimelineModel()
        {
            // 默认一条轨
            tracks.Add(new TrackModel("Track Group 1"));
        }
    }
}