// Assets/Editor/TimelineValidation/Manipulators/ClipResizeManipulator.cs
using UnityEngine;
using UnityEngine.UIElements;

namespace Cheggone.TimelineValidation
{
    public class ClipResizeManipulator : PointerManipulator
    {
        private readonly TimelineValidationWindow window;
        private readonly TimelineModel model;
        private readonly ClipModel clip;
        private readonly bool isLeft;

        private bool dragging;
        private Vector2 startPos;
        private float startStart;
        private float startDur;

        public ClipResizeManipulator(TimelineValidationWindow window, TimelineModel model, ClipModel clip, bool isLeft)
        {
            this.window = window;
            this.model = model;
            this.clip = clip;
            this.isLeft = isLeft;
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
            startStart = clip.start;
            startDur = clip.duration;
            target.CapturePointer(evt.pointerId);
            evt.StopPropagation();
        }

        private void OnMove(PointerMoveEvent evt)
        {
            if (!dragging || !target.HasPointerCapture(evt.pointerId)) return;

            float dx = evt.position.x - startPos.x;
            float pps = model.pixelsPerSecond * model.zoom;
            float dSec = dx / pps;

            if (isLeft)
            {
                float rawStart = startStart + dSec;
                float rawDur = startDur - dSec;

                rawStart = Mathf.Max(0, rawStart);
                rawDur = Mathf.Max(0.1f, rawDur);

                float snappedStart = window.SnapTime(rawStart, draggingClip: clip);
                float end = startStart + startDur;
                snappedStart = Mathf.Min(snappedStart, end - 0.1f);

                clip.start = snappedStart;
                clip.duration = Mathf.Max(0.1f, end - snappedStart);
            }
            else
            {
                float rawEnd = startStart + startDur + dSec;
                rawEnd = Mathf.Max(startStart + 0.1f, rawEnd);

                float snappedEnd = window.SnapTime(rawEnd, draggingClip: clip);
                clip.duration = Mathf.Max(0.1f, snappedEnd - clip.start);
            }

            window.LayoutClip(target.parent, clip); // handle is child; parent is clip
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
