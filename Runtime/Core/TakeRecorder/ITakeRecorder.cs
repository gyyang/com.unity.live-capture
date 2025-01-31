using UnityObject = UnityEngine.Object;

namespace Unity.LiveCapture
{
    /// <summary>
    /// Represents a take recorder.
    /// </summary>
    public interface ITakeRecorder
    {
        /// <summary>
        /// The frame rate to use for recording.
        /// </summary>
        FrameRate FrameRate { get; set; }

        /// <summary>
        /// The current <see cref="IShot"/> to record to.
        /// </summary>
        IShot Shot { get; }

        /// <summary>
        /// Indicates whether the take recorder is ready for recording.
        /// </summary>
        /// <returns>
        /// true if ready for recording; otherwise, false.
        /// </returns>
        bool IsLive();

        /// <summary>
        /// Use this method to set the take recorder ready for recording.
        /// </summary>
        /// <param name="value">true to set ready; otherwise, false.</param>
        void SetLive(bool value);

        /// <summary>
        /// Checks whether the take recorder is recording or not.
        /// </summary>
        /// <returns>
        /// true if playing; otherwise, false.
        /// </returns>
        bool IsRecording();

        /// <summary>
        /// Starts the recording of a new take for the selected slate.
        /// </summary>
        void StartRecording();

        /// <summary>
        /// Stops the recording.
        /// </summary>
        void StopRecording();

        /// <summary>
        /// Checks whether the take recorder is playing the selected take or not.
        /// </summary>
        /// <returns>
        /// true if playing; otherwise, false.
        /// </returns>
        bool IsPreviewPlaying();

        /// <summary>
        /// Starts playing the selected take.
        /// </summary>
        void PlayPreview();

        /// <summary>
        /// Pauses the playback of the selected take.
        /// </summary>
        void PausePreview();

        /// <summary>
        /// Returns the current playback time of the selected take.
        /// </summary>
        /// <returns>The current time in seconds.</returns>
        double GetPreviewTime();

        /// <summary>
        /// Changes the current playback time of the selected take.
        /// </summary>
        /// <param name="time">The current time in seconds.</param>
        void SetPreviewTime(double time);

        /// <summary>
        /// Returns the current playback duration of the selected take.
        /// </summary>
        /// <returns>The current duration in seconds.</returns>
        double GetPreviewDuration();

        /// <summary>
        /// Returns the time elapsed since the start of the recording.
        /// </summary>
        /// <returns>The time elapsed since the start of the recording, in seconds.</returns>
        double GetRecordingElapsedTime();
    }

    interface ITakeRecorderInternal : ITakeRecorder
    {
        /// <summary>
        /// The enabled state of this take recorder.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Activates the shot associated with the provided object and sets the time.
        /// </summary>
        /// <param name="obj">The object associated with the shot.</param>
        /// <param name="time">The time in seconds.</param>
        void SetPreviewTime(UnityObject obj, double time);
    }
}
