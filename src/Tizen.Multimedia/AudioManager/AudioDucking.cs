/*
 * Copyright (c) 2019 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Diagnostics;

namespace Tizen.Multimedia
{
    /// <summary>
    /// Controls audio ducking for specific audio streams, allowing for dynamic
    /// adjustment of audio levels during playback. This class enables the activation
    /// and deactivation of ducking, monitors ducking state changes, and ensures proper
    /// privileges are in place for volume adjustments.
    /// </summary>
    /// <seealso cref="AudioManager"/>
    /// <since_tizen> 6 </since_tizen>
    public sealed class AudioDucking : IDisposable
    {
        private AudioDuckingHandle _handle;
        private bool _disposed = false;
        private Interop.AudioDucking.DuckingStateChangedCallback _duckingStateChangedCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioDucking"/> class for a specified audio stream type.
        /// This constructor sets up the audio ducking mechanism for the target stream, allowing
        /// the application to respond to ducking state changes.
        /// </summary>
        /// <param name="targetType">The type of sound stream affected by this new instance.</param>
        /// <exception cref="ArgumentException"><paramref name="targetType"/> is invalid.</exception>
        /// <exception cref="InvalidOperationException">Operation failed; internal error.</exception>
        /// <since_tizen> 6 </since_tizen>
        public AudioDucking(AudioStreamType targetType)
        {
            ValidationUtil.ValidateEnum(typeof(AudioStreamType), targetType, nameof(targetType));

            _duckingStateChangedCallback = (IntPtr ducking, bool isDucked, IntPtr _) =>
            {
                DuckingStateChanged?.Invoke(this,
                    new AudioDuckingStateChangedEventArgs(isDucked));
            };

            Interop.AudioDucking.Create(targetType, _duckingStateChangedCallback,
                IntPtr.Zero, out _handle).ThrowIfError("Unable to create stream ducking");

            Debug.Assert(_handle != null);
        }

        /// <summary>
        /// Occurs when the ducking state of the audio stream changes,
        /// notifying subscribers of the current ducking status.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public event EventHandler<AudioDuckingStateChangedEventArgs> DuckingStateChanged;

        /// <summary>
        /// Gets a value indicating whether the audio stream is currently ducked.
        /// This property checks the ducking state and returns true if the audio
        /// level is lowered; otherwise, false.
        /// </summary>
        /// <value>true if the audio stream is ducked; otherwise, false.</value>
        /// <exception cref="InvalidOperationException">Operation failed; internal error.</exception>
        /// <exception cref="ObjectDisposedException">The <see cref="AudioDucking"/> has already been disposed of.</exception>
        /// <since_tizen> 6 </since_tizen>
        public bool IsDucked
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(AudioDucking));
                }

                Interop.AudioDucking.IsDucked(Handle, out bool isDucked).
                    ThrowIfError("Failed to get the running state of the device");

                return isDucked;
            }
        }

        /// <summary>
        /// Activates audio ducking for a specified duration and volume ratio.
        /// This method lowers the audio level of the target stream for a defined
        /// period, enabling a smoother audio experience during events like notifications.
        /// </summary>
        /// <param name="duration">The duration for ducking in milliseconds.</param>
        /// <param name="ratio">The volume ratio when ducked.</param>
        /// <remarks>To activate ducking, the specified privilege is required.</remarks>
        /// <privilege>http://tizen.org/privilege/volume.set</privilege>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="duration"/> is less than 0 or greater than 3000.<br/>
        ///     -or-<br/>
        ///     <paramref name="ratio"/> is less than 0.0 or greater than or equal to 1.0.<br/>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Operation failed; internal error.<br/>
        ///     -or-<br/>
        ///     The target stream is already ducked.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have required privilege to set volume.</exception>
        /// <exception cref="ObjectDisposedException">The <see cref="AudioDucking"/> has already been disposed of.</exception>
        /// <since_tizen> 6 </since_tizen>
        public void Activate(uint duration, double ratio)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(AudioDucking));
            }

            if (duration < 0 || duration > 3000)
            {
                throw new ArgumentOutOfRangeException(nameof(duration), duration, "Valid range : 0 <= duration <= 3000");
            }

            if (ratio < 0.0 || ratio >= 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(ratio), ratio, "Valid range : 0 <= ratio < 1.0");
            }

            Interop.AudioDucking.Activate(Handle, duration, ratio).
                ThrowIfError("Failed to activate ducking");
        }

        /// <summary>
        /// Deactivates audio ducking, restoring the audio levels of the target stream
        /// to their original state. This method is used to stop the ducking effect
        /// applied earlier when needed.
        /// </summary>
        /// <remarks>To deactivate ducking, the specified privilege is required.</remarks>
        /// <privilege>http://tizen.org/privilege/volume.set</privilege>
        /// <exception cref="InvalidOperationException">
        ///     Operation failed; internal error.<br/>
        ///     -or-<br/>
        ///     The target stream is already unducked.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have required privilege to set volume.</exception>
        /// <exception cref="ObjectDisposedException">The <see cref="AudioDucking"/> has already been disposed of.</exception>
        /// <since_tizen> 6 </since_tizen>
        public void Deactivate()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(AudioDucking));
            }

            Interop.AudioDucking.Deactivate(Handle).
                ThrowIfError("Failed to deactivate ducking");
        }

        /// <summary>
        /// Releases all resources used by the <see cref="AudioDucking"/> instance.
        /// This method clears any allocated resources and should be called when
        /// the <see cref="AudioDucking"/> object is no longer needed.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            if (_handle != null)
            {
                _handle.Dispose();
            }

            _disposed = true;
            GC.SuppressFinalize(this);
        }

        internal AudioDuckingHandle Handle
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(AudioDucking));
                }
                return _handle;
            }
        }
    }
}
