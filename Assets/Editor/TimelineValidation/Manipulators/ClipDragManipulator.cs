// Assets/Editor/TimelineValidation/Manipulators/ClipDragManipulator.cs
using UnityEngine;
using UnityEngine.UIElements;

namespace Cheggone.TimelineValidation
{
    public class ClipDragManipulator : PointerManipulator
    {
        private readonly TimelineValidationWindow window;
        private readonly TimelineModel model;
        private readonly TrackModel track;
        private readonly ClipModel clip;

        private bool dragging;
        private Vector2 startPos;
        private float startTime;

        public ClipDragManipulator(TimelineValidationWindow window, TimelineModel model, TrackModel track, ClipModel clip)
        {
            this.window = window;
            this.model = model;
            this.track = track;
            this.clip = clip;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(OnDown);
            target.RegisterCallback<PointerMoveEvent>(OnMove);
            target.RegisterCallback<PointerUpEvent>(OnUp);
        }
        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnDown);
            target.UnregisterCallback<PointerMoveEvent>(OnMove);
            target.UnregisterCallback<PointerUpEvent>(OnUp);
        }

        private void OnDown(PointerDownEvent evt)
        {
            if (evt.button != 0) return;
            dragging = true;
            startPos = evt.position;
            startTime = clip.start;
            target.CapturePointer(evt.pointerId);
            evt.StopPropagation();
        }

        private void OnMove(PointerMoveEvent evt)
        {
            if (!dragging || !target.HasPointerCapture(evt.pointerId)) return;

            float dx = evt.position.x - startPos.x;
            float pps = model.pixelsPerSecond * model.zoom;

            float raw = startTime + dx / pps;
            raw = Mathf.Max(0, raw);

            float snapped = window.SnapTime(raw, draggingClip: clip);
            clip.start = snapped;

            window.LayoutClip(target, clip);
            evt.StopPropagation();
        }

        private void OnUp(PointerUpEvent evt)
        {
            if (evt.button != 0) return;
            dragging = false;
            target.ReleasePointer(evt.pointerId);
            evt.StopPropagation();
        }
    }
}
