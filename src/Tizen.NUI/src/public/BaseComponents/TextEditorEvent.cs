﻿/*
 * Copyright(c) 2021 Samsung Electronics Co., Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Tizen.NUI.BaseComponents
{
    /// <summary>
    /// A control which provides a multi-line editable text editor.
    /// </summary>
    /// <since_tizen> 3 </since_tizen>
    public partial class TextEditor
    {
        private EventHandler<TextChangedEventArgs> textEditorTextChangedEventHandler;
        private TextChangedCallbackDelegate textEditorTextChangedCallbackDelegate;

        private EventHandler<ScrollStateChangedEventArgs> textEditorScrollStateChangedEventHandler;
        private ScrollStateChangedCallbackDelegate textEditorScrollStateChangedCallbackDelegate;

        private EventHandler textEditorCursorPositionChangedEventHandler;
        private CursorPositionChangedCallbackDelegate textEditorCursorPositionChangedCallbackDelegate;

        private EventHandler<MaxLengthReachedEventArgs> textEditorMaxLengthReachedEventHandler;
        private MaxLengthReachedCallbackDelegate textEditorMaxLengthReachedCallbackDelegate;

        private EventHandler<AnchorClickedEventArgs> textEditorAnchorClickedEventHandler;
        private AnchorClickedCallbackDelegate textEditorAnchorClickedCallbackDelegate;

        private EventHandler textEditorSelectionClearedEventHandler;
        private SelectionClearedCallbackDelegate textEditorSelectionClearedCallbackDelegate;

        private EventHandler textEditorSelectionStartedEventHandler;
        private SelectionStartedCallbackDelegate textEditorSelectionStartedCallbackDelegate;

        private EventHandler textEditorSelectionChangedEventHandler;
        private SelectionChangedCallbackDelegate textEditorSelectionChangedCallbackDelegate;

        private EventHandler<InputFilteredEventArgs> textEditorInputFilteredEventHandler;
        private InputFilteredCallbackDelegate textEditorInputFilteredCallbackDelegate;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void TextChangedCallbackDelegate(IntPtr textEditor);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ScrollStateChangedCallbackDelegate(IntPtr textEditor, ScrollState state);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void CursorPositionChangedCallbackDelegate(IntPtr textEditor, uint oldPosition);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void MaxLengthReachedCallbackDelegate(IntPtr textEditor);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SelectionClearedCallbackDelegate(IntPtr textEditor);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SelectionStartedCallbackDelegate(IntPtr textEditor);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void AnchorClickedCallbackDelegate(IntPtr textEditor, IntPtr href, uint hrefLength);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SelectionChangedCallbackDelegate(IntPtr textEditor, uint oldStart, uint oldEnd);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void InputFilteredCallbackDelegate(IntPtr textEditor, InputFilterType type);

        private bool invokeTextChanged = true;

        /// <summary>
        /// An event for the TextChanged signal which can be used to subscribe or unsubscribe the event handler
        /// provided by the user. The TextChanged signal is emitted when the text changes.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public event EventHandler<TextChangedEventArgs> TextChanged
        {
            add
            {
                if (textEditorTextChangedEventHandler == null)
                {
                    textEditorTextChangedCallbackDelegate = (OnTextChanged);
                    using var signal = TextChangedSignal();
                    signal.Connect(textEditorTextChangedCallbackDelegate);
                }
                textEditorTextChangedEventHandler += value;
            }
            remove
            {
                textEditorTextChangedEventHandler -= value;
                using var signal = TextChangedSignal();
                if (textEditorTextChangedEventHandler == null && signal.Empty() == false)
                {
                    signal.Disconnect(textEditorTextChangedCallbackDelegate);
                }
            }
        }

        /// <summary>
        /// Event for the ScrollStateChanged signal which can be used to subscribe or unsubscribe the event handler
        /// provided by the user. The ScrollStateChanged signal is emitted when the scroll state changes.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public event EventHandler<ScrollStateChangedEventArgs> ScrollStateChanged
        {
            add
            {
                if (textEditorScrollStateChangedEventHandler == null)
                {
                    textEditorScrollStateChangedCallbackDelegate = OnScrollStateChanged;
                    using var signal = ScrollStateChangedSignal(this);
                    signal.Connect(textEditorScrollStateChangedCallbackDelegate);
                }
                textEditorScrollStateChangedEventHandler += value;
            }
            remove
            {
                textEditorScrollStateChangedEventHandler -= value;
                using var signal = ScrollStateChangedSignal(this);
                if (textEditorScrollStateChangedEventHandler == null && signal.Empty() == false)
                {
                    signal.Disconnect(textEditorScrollStateChangedCallbackDelegate);
                }
            }
        }

        /// <summary>
        /// The CursorPositionChanged event is emitted whenever the primary cursor position changed.
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public event EventHandler CursorPositionChanged
        {
            add
            {
                if (textEditorCursorPositionChangedEventHandler == null)
                {
                    textEditorCursorPositionChangedCallbackDelegate = (OnCursorPositionChanged);
                    using var signal = CursorPositionChangedSignal();
                    signal.Connect(textEditorCursorPositionChangedCallbackDelegate);
                }
                textEditorCursorPositionChangedEventHandler += value;
            }
            remove
            {
                using var signal = CursorPositionChangedSignal();
                if (textEditorCursorPositionChangedEventHandler == null && signal.Empty() == false)
                {
                    signal.Disconnect(textEditorCursorPositionChangedCallbackDelegate);
                }
                textEditorCursorPositionChangedEventHandler -= value;
            }
        }

        /// <summary>
        /// The MaxLengthReached event.
        /// </summary>
        /// This will be public opened after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler<MaxLengthReachedEventArgs> MaxLengthReached
        {
            add
            {
                if (textEditorMaxLengthReachedEventHandler == null)
                {
                    textEditorMaxLengthReachedCallbackDelegate = (OnMaxLengthReached);
                    using var signal = MaxLengthReachedSignal();
                    signal.Connect(textEditorMaxLengthReachedCallbackDelegate);
                }
                textEditorMaxLengthReachedEventHandler += value;
            }
            remove
            {
                using var signal = MaxLengthReachedSignal();
                if (textEditorMaxLengthReachedEventHandler == null && signal.Empty() == false)
                {
                    signal.Disconnect(textEditorMaxLengthReachedCallbackDelegate);
                }
                textEditorMaxLengthReachedEventHandler -= value;
            }
        }

        /// <summary>
        /// The AnchorClicked signal is emitted when the anchor is clicked.
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public event EventHandler<AnchorClickedEventArgs> AnchorClicked
        {
            add
            {
                if (textEditorAnchorClickedEventHandler == null)
                {
                    textEditorAnchorClickedCallbackDelegate = (OnAnchorClicked);
                    using var signal = AnchorClickedSignal();
                    signal.Connect(textEditorAnchorClickedCallbackDelegate);
                }
                textEditorAnchorClickedEventHandler += value;
            }
            remove
            {
                textEditorAnchorClickedEventHandler -= value;
                using var signal = AnchorClickedSignal();
                if (textEditorAnchorClickedEventHandler == null && signal.Empty() == false)
                {
                    signal.Disconnect(textEditorAnchorClickedCallbackDelegate);
                }
            }
        }

        /// <summary>
        /// The SelectionStarted event is emitted when the selection has been started.
        /// </summary>
        /// <since_tizen> 10 </since_tizen>
        public event EventHandler SelectionStarted
        {
            add
            {
                if (textEditorSelectionStartedEventHandler == null)
                {
                    textEditorSelectionStartedCallbackDelegate = (OnSelectionStarted);
                    using var signal = SelectionStartedSignal();
                    signal.Connect(textEditorSelectionStartedCallbackDelegate);
                }
                textEditorSelectionStartedEventHandler += value;
            }
            remove
            {
                using var signal = SelectionStartedSignal();
                if (textEditorSelectionStartedEventHandler == null && signal.Empty() == false)
                {
                    signal.Disconnect(textEditorSelectionStartedCallbackDelegate);
                }
                textEditorSelectionStartedEventHandler -= value;
            }
        }

        /// <summary>
        /// The SelectionCleared signal is emitted when selection is cleared.
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public event EventHandler SelectionCleared
        {
            add
            {
                if (textEditorSelectionClearedEventHandler == null)
                {
                    textEditorSelectionClearedCallbackDelegate = (OnSelectionCleared);
                    using var signal = SelectionClearedSignal();
                    signal.Connect(textEditorSelectionClearedCallbackDelegate);
                }
                textEditorSelectionClearedEventHandler += value;
            }
            remove
            {
                using var signal = SelectionClearedSignal();
                if (textEditorSelectionClearedEventHandler == null && signal.Empty() == false)
                {
                    signal.Disconnect(textEditorSelectionClearedCallbackDelegate);
                }
                textEditorSelectionClearedEventHandler -= value;
            }
        }

        /// <summary>
        /// The SelectionChanged event is emitted whenever the selected text is changed.
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public event EventHandler SelectionChanged
        {
            add
            {
                if (textEditorSelectionChangedEventHandler == null)
                {
                    textEditorSelectionChangedCallbackDelegate = (OnSelectionChanged);
                    using var signal = SelectionChangedSignal();
                    signal.Connect(textEditorSelectionChangedCallbackDelegate);
                }
                textEditorSelectionChangedEventHandler += value;
            }
            remove
            {
                using var signal = SelectionChangedSignal();
                if (textEditorSelectionChangedEventHandler == null && signal.Empty() == false)
                {
                    signal.Disconnect(textEditorSelectionChangedCallbackDelegate);
                }
                textEditorSelectionChangedEventHandler -= value;
            }
        }

        /// <summary>
        /// The InputFiltered signal is emitted when the input is filtered by InputFilter. <br />
        /// </summary>
        /// <remarks>
        /// See <see cref="InputFilterType"/> and <see cref="InputFilteredEventArgs"/> for a detailed description. <br />
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the InputFiltered event.
        /// <code>
        /// editor.InputFiltered += (s, e) =>
        /// {
        ///     if (e.Type == InputFilterType.Accept)
        ///     {
        ///         // If input is filtered by InputFilter of Accept type.
        ///     }
        ///     else if (e.Type == InputFilterType.Reject)
        ///     {
        ///         // If input is filtered by InputFilter of Reject type.
        ///     }
        /// };
        /// </code>
        /// </example>
        /// <since_tizen> 9 </since_tizen>
        public event EventHandler<InputFilteredEventArgs> InputFiltered
        {
            add
            {
                if (textEditorInputFilteredEventHandler == null)
                {
                    textEditorInputFilteredCallbackDelegate = (OnInputFiltered);
                    using var signal = InputFilteredSignal();
                    signal.Connect(textEditorInputFilteredCallbackDelegate);
                }
                textEditorInputFilteredEventHandler += value;
            }
            remove
            {
                textEditorInputFilteredEventHandler -= value;
                using var signal = InputFilteredSignal();
                if (textEditorInputFilteredEventHandler == null && signal.Empty() == false)
                {
                    signal.Disconnect(textEditorInputFilteredCallbackDelegate);
                }
            }
        }

        internal TextEditorSignal SelectionStartedSignal()
        {
            TextEditorSignal ret = new TextEditorSignal(Interop.TextEditor.SelectionStartedSignal(SwigCPtr), false);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        internal TextEditorSignal SelectionClearedSignal()
        {
            TextEditorSignal ret = new TextEditorSignal(Interop.TextEditor.SelectionClearedSignal(SwigCPtr), false);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        internal TextEditorSignal TextChangedSignal()
        {
            TextEditorSignal ret = new TextEditorSignal(Interop.TextEditor.TextChangedSignal(SwigCPtr), false);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        internal ScrollStateChangedSignal ScrollStateChangedSignal(TextEditor textEditor)
        {
            ScrollStateChangedSignal ret = new ScrollStateChangedSignal(Interop.TextEditor.ScrollStateChangedSignal(TextEditor.getCPtr(textEditor)), false);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        internal TextEditorSignal CursorPositionChangedSignal()
        {
            TextEditorSignal ret = new TextEditorSignal(Interop.TextEditor.CursorPositionChangedSignal(SwigCPtr), false);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        internal TextEditorSignal MaxLengthReachedSignal()
        {
            TextEditorSignal ret = new TextEditorSignal(Interop.TextEditor.MaxLengthReachedSignal(SwigCPtr), false);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        internal TextEditorSignal AnchorClickedSignal()
        {
            TextEditorSignal ret = new TextEditorSignal(Interop.TextEditor.AnchorClickedSignal(SwigCPtr), false);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        internal TextEditorSignal SelectionChangedSignal()
        {
            TextEditorSignal ret = new TextEditorSignal(Interop.TextEditor.SelectionChangedSignal(SwigCPtr), false);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        internal TextEditorSignal InputFilteredSignal()
        {
            TextEditorSignal ret = new TextEditorSignal(Interop.TextEditor.InputFilteredSignal(SwigCPtr), false);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        private void OnTextChanged(IntPtr textEditor)
        {
            if (Disposed || IsDisposeQueued)
            {
                // Ignore native callback if the view is disposed or queued for disposal.
                return;
            }

            if (textEditorTextChangedEventHandler != null && invokeTextChanged)
            {
                TextChangedEventArgs e = new TextChangedEventArgs();

                // Populate all members of "e" (TextChangedEventArgs) with real data
                e.TextEditor = Registry.GetManagedBaseHandleFromNativePtr(textEditor) as TextEditor;
                //here we send all data to user event handlers
                textEditorTextChangedEventHandler(this, e);
            }
        }

        private void OnSelectionStarted(IntPtr textEditor)
        {
            if (Disposed || IsDisposeQueued)
            {
                // Ignore native callback if the view is disposed or queued for disposal.
                return;
            }

            //no data to be sent to the user
            textEditorSelectionStartedEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void OnSelectionCleared(IntPtr textEditor)
        {
            if (Disposed || IsDisposeQueued)
            {
                // Ignore native callback if the view is disposed or queued for disposal.
                return;
            }

            //no data to be sent to the user
            textEditorSelectionClearedEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void OnScrollStateChanged(IntPtr textEditor, ScrollState state)
        {
            if (Disposed || IsDisposeQueued)
            {
                // Ignore native callback if the view is disposed or queued for disposal.
                return;
            }

            if (textEditorScrollStateChangedEventHandler != null)
            {
                ScrollStateChangedEventArgs e = new ScrollStateChangedEventArgs();

                if (textEditor != global::System.IntPtr.Zero)
                {
                    // Populate all members of "e" (ScrollStateChangedEventArgs) with real data
                    e.TextEditor = Registry.GetManagedBaseHandleFromNativePtr(textEditor) as TextEditor;
                    e.ScrollState = state;
                }
                //here we send all data to user event handlers
                textEditorScrollStateChangedEventHandler(this, e);
            }
        }

        private void OnCursorPositionChanged(IntPtr textEditor, uint oldPosition)
        {
            if (Disposed || IsDisposeQueued)
            {
                // Ignore native callback if the view is disposed or queued for disposal.
                return;
            }

            // no data to be sent to the user, as in NUI there is no event provide old values.
            textEditorCursorPositionChangedEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void OnMaxLengthReached(IntPtr textEditor)
        {
            if (Disposed || IsDisposeQueued)
            {
                // Ignore native callback if the view is disposed or queued for disposal.
                return;
            }

            if (textEditorMaxLengthReachedEventHandler != null)
            {
                MaxLengthReachedEventArgs e = new MaxLengthReachedEventArgs();

                // Populate all members of "e" (MaxLengthReachedEventArgs) with real data
                e.TextEditor = Registry.GetManagedBaseHandleFromNativePtr(textEditor) as TextEditor;
                //here we send all data to user event handlers
                textEditorMaxLengthReachedEventHandler(this, e);
            }
        }

        private void OnAnchorClicked(IntPtr textEditor, IntPtr href, uint hrefLength)
        {
            if (Disposed || IsDisposeQueued)
            {
                // Ignore native callback if the view is disposed or queued for disposal.
                return;
            }

            // Note: hrefLength is useful for get the length of a const char* (href) in dali-toolkit.
            // But NUI can get the length of string (href), so hrefLength is not necessary in NUI.
            AnchorClickedEventArgs e = new AnchorClickedEventArgs();

            // Populate all members of "e" (AnchorClickedEventArgs) with real data
            e.Href = Marshal.PtrToStringAnsi(href);
            //here we send all data to user event handlers
            textEditorAnchorClickedEventHandler?.Invoke(this, e);
        }

        private void OnSelectionChanged(IntPtr textEditor, uint oldStart, uint oldEnd)
        {
            if (Disposed || IsDisposeQueued)
            {
                // Ignore native callback if the view is disposed or queued for disposal.
                return;
            }

            // no data to be sent to the user, as in NUI there is no event provide old values.
            textEditorSelectionChangedEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void OnInputFiltered(IntPtr textEditor, InputFilterType type)
        {
            if (Disposed || IsDisposeQueued)
            {
                // Ignore native callback if the view is disposed or queued for disposal.
                return;
            }

            InputFilteredEventArgs e = new InputFilteredEventArgs();

            // Populate all members of "e" (InputFilteredEventArgs) with real data
            e.Type = type;
            //here we send all data to user event handlers
            textEditorInputFilteredEventHandler?.Invoke(this, e);
        }

        /// <summary>
        /// Event arguments that passed via the TextChanged signal.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public class TextChangedEventArgs : EventArgs
        {
            private TextEditor textEditor;

            /// <summary>
            /// TextEditor - is the texteditor control which has the text contents changed.
            /// </summary>
            /// <since_tizen> 3 </since_tizen>
            public TextEditor TextEditor
            {
                get
                {
                    return textEditor;
                }
                set
                {
                    textEditor = value;
                }
            }
        }

        /// <summary>
        /// Event arguments that passed via the ScrollStateChanged signal.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public class ScrollStateChangedEventArgs : EventArgs
        {
            private TextEditor textEditor;
            private ScrollState scrollState;

            /// <summary>
            /// TextEditor - is the texteditor control which has the scroll state changed.
            /// </summary>
            /// <since_tizen> 3 </since_tizen>
            public TextEditor TextEditor
            {
                get
                {
                    return textEditor;
                }
                set
                {
                    textEditor = value;
                }
            }

            /// <summary>
            /// ScrollState - is the texteditor control scroll state.
            /// </summary>
            /// <since_tizen> 3 </since_tizen>
            public ScrollState ScrollState
            {
                get
                {
                    return scrollState;
                }
                set
                {
                    scrollState = value;
                }
            }
        }

        /// <summary>
        /// The MaxLengthReached event arguments.
        /// </summary>
        /// This will be public opened after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class MaxLengthReachedEventArgs : EventArgs
        {
            private TextEditor textEditor;

            /// <summary>
            /// TextEditor.
            /// </summary>
            /// This will be public opened after ACR done. Before ACR, need to be hidden as inhouse API.
            [EditorBrowsable(EditorBrowsableState.Never)]
            public TextEditor TextEditor
            {
                get
                {
                    return textEditor;
                }
                set
                {
                    textEditor = value;
                }
            }
        }
    }
}
