﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEvents.Framework.FunctionEvents
{
    /// <summary>
    /// Used to handle events that happens when a mouse enters/leaves a specified position.
    /// </summary>
    class MouseEntryLeaveEvent
    {
        /// <summary>
        /// A function that is called when a mouse enters a certain position.
        /// </summary>
        public functionEvent onMouseEnter;
        /// <summary>
        /// A function that is called when a mouse leaves a certain position.
        /// </summary>
        public functionEvent onMouseLeave;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="OnMouseEnter">The function that occurs when the mouse enters a certain position.</param>
        /// <param name="OnMouseLeave">The function that occurs when the mouse leaves a certain position.</param>
        public MouseEntryLeaveEvent(functionEvent OnMouseEnter, functionEvent OnMouseLeave)
        {
            this.onMouseEnter = OnMouseEnter;
            this.onMouseLeave = OnMouseLeave;
        }
    }
}
