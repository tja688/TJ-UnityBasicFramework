// Assets/Editor/TimelineValidation/Manipulators/PlayheadManipulator.cs
using UnityEngine;
using UnityEngine.UIElements;

namespace Cheggone.TimelineValidation
{
    public class PlayheadManipulator : PointerManipulator
    {
        private readonly TimelineValidationWindow window;
        private readonly TimelineModel model;

        private bool dragging;
        private Vector2 startPos;
        private float startTime;

        public PlayheadManipulator(TimelineValidationWindow window, TimelineModel model)
        {
            this.window = window;
            this.model = model;
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
            startTime = model.playheadTime;
            target.CapturePointer(evt.pointerId);
            evt.StopPropagation();
        }

        private void OnMove(PointerMoveEvent evt)
        {
            if (!dragging || !target.HasPointerCapture(evt.pointerId)) return;

            float dx = evt.position.x - startPos.x;
            float pps = model.pixelsPerSecond * model.zoom;

            float raw = startTime + dx / pps;
            float snapped = window.SnapTime(raw);
            window.SetPlayheadTime(snapped);

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
