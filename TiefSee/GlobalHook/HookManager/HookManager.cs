using System;
using System.Windows.Forms;

namespace GlobalHook {
    /// <summary>
    /// This class monitors all mouse activities globally (also outside of the application) 
    /// and provides appropriate events.
    /// </summary>
    public static partial class HookManager {
        //################################################################

        //################################################################
        #region Keyboard events

        private static event KeyPressEventHandler s_KeyPress;

        /// <summary>
        /// Occurs when a key is pressed.
        /// </summary>
        /// <remarks>
        /// Key events occur in the following order: 
        /// <list type="number">
        /// <item>KeyDown</item>
        /// <item>KeyPress</item>
        /// <item>KeyUp</item>
        /// </list>
        ///The KeyPress event is not raised by noncharacter keys; however, the noncharacter keys do raise the KeyDown and KeyUp events. 
        ///Use the KeyChar property to sample keystrokes at run time and to consume or modify a subset of common keystrokes. 
        ///To handle keyboard events only in your application and not enable other applications to receive keyboard events, 
        /// set the KeyPressEventArgs.Handled property in your form's KeyPress event-handling method to <b>true</b>. 
        /// </remarks>
        public static event KeyPressEventHandler KeyPress {
            add {
                EnsureSubscribedToGlobalKeyboardEvents();
                s_KeyPress += value;
            }
            remove {
                s_KeyPress -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        private static event KeyEventHandler s_KeyUp;

        /// <summary>
        /// Occurs when a key is released. 
        /// </summary>
        public static event KeyEventHandler KeyUp {
            add {
                EnsureSubscribedToGlobalKeyboardEvents();
                s_KeyUp += value;
            }
            remove {
                s_KeyUp -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        private static event KeyEventHandler s_KeyDown;

        /// <summary>
        /// Occurs when a key is preseed. 
        /// </summary>
        public static event KeyEventHandler KeyDown {
            add {
                EnsureSubscribedToGlobalKeyboardEvents();
                s_KeyDown += value;
            }
            remove {
                s_KeyDown -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }


        #endregion
    }
}
