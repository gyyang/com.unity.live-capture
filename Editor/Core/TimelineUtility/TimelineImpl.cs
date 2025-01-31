using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEditor;
using UnityEditor.Timeline;

namespace Unity.LiveCapture.Editor
{
    [InitializeOnLoad]
    /// <summary>
    /// An implementation of <see cref="ITimelineImpl"/> that uses the TimelineEditor state.
    /// </summary>
    class TimelineImpl : ITimelineImpl
    {
        const string k_TimelineMenuItem = "Window/Sequencing/Timeline";
        static TimelineImpl Instance { get; } = new TimelineImpl();
        static TimelineEditorWindow Window { get; set; }

        static TimelineImpl()
        {
            Timeline.Instance.SetImpl(Instance);
        }

        static TimelineEditorWindow GetOrCreateWindow()
        {
            if (Window == null)
            {
                EditorApplication.ExecuteMenuItem(k_TimelineMenuItem);

                Window = TimelineEditor.GetWindow();
            }

            return Window;
        }

        /// <inheritdoc />
        public PlayableDirector InspectedDirector => TimelineEditor.inspectedDirector;

        /// <inheritdoc />
        public PlayableDirector MasterDirector => TimelineEditor.masterDirector;

        /// <inheritdoc />
        public TimelineAsset InspectedAsset => TimelineEditor.inspectedAsset;

        /// <inheritdoc />
        public TimelineAsset MasterAsset => TimelineEditor.masterAsset;

        public bool TrySetAsMasterDirector(PlayableDirector director)
        {
            if (TimelineEditor.masterDirector == director &&
                TimelineEditor.masterAsset == director.playableAsset)
                return true;

            var window = GetOrCreateWindow();

            if (window == null)
                return false;

            if (window.locked && TimelineEditor.masterDirector != director)
                return false;

            window.SetTimeline(director);

            return true;
        }

        public void Play()
        {
            var window = GetOrCreateWindow();

            if (window == null)
                return;

            window.playbackControls.Play();
        }

        public void Pause()
        {
            var window = GetOrCreateWindow();

            if (window == null)
                return;

            window.playbackControls.Pause();
        }

        public void SetGlobalTime(double time)
        {
            var window = GetOrCreateWindow();

            if (window == null)
                return;

            window.playbackControls.SetCurrentTime(time, TimelinePlaybackControls.Context.Global);

            Repaint();
        }

        public void Repaint()
        {
            TimelineEditor.Refresh(RefreshReason.WindowNeedsRedraw);
        }
    }
}
