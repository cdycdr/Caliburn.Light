﻿using System;
using System.Collections.Generic;

namespace Caliburn.Light
{
    /// <summary>
    /// A weak event source that does not hold any strong reference to the event listeners.
    /// </summary>
    public sealed class WeakEventSource
    {
        private readonly WeakEventList _list = new WeakEventList();

        /// <summary>
        /// Adds the specified event handler.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        public void Add(EventHandler eventHandler)
        {
            if (eventHandler == null) return;
            lock (_list)
            {
                _list.AddHandler(eventHandler);
            }
        }

        /// <summary>
        /// Removes the specified event handler.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        public void Remove(EventHandler eventHandler)
        {
            if (eventHandler == null) return;
            lock (_list)
            {
                _list.RemoveHandler(eventHandler);
            }
        }

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        public void Raise(object sender, EventArgs e)
        {
            List<WeakEventListener> listeners;
            lock (_list)
            {
                listeners = _list.GetCopy();
            }

            var foundStaleEntries = DeliverEvent(listeners, sender, e);
            if (foundStaleEntries)
            {
                lock (_list)
                {
                    _list.Purge();
                }
            }
        }

        private static bool DeliverEvent(IEnumerable<WeakEventListener> listeners, object sender, EventArgs e)
        {
            var foundStaleEntries = false;

            foreach (var listener in listeners)
            {
                if (listener.Target != null)
                {
                    var handler = (EventHandler)listener.Handler;
                    if (handler != null)
                    {
                        handler(sender, e);
                    }
                }
                else
                {
                    foundStaleEntries = true;
                }
            }

            return foundStaleEntries;
        }
    }

    /// <summary>
    /// A weak event source that does not hold any strong reference to the event listeners.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
    public sealed class WeakEventSource<TEventArgs>
        where TEventArgs : EventArgs
    {
        private readonly WeakEventList _list = new WeakEventList();

        /// <summary>
        /// Adds the specified event handler.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        public void Add(EventHandler<TEventArgs> eventHandler)
        {
            if (eventHandler == null) return;
            lock (_list)
            {
                _list.AddHandler(eventHandler);
            }
        }

        /// <summary>
        /// Removes the specified event handler.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        public void Remove(EventHandler<TEventArgs> eventHandler)
        {
            if (eventHandler == null) return;
            lock (_list)
            {
                _list.RemoveHandler(eventHandler);
            }
        }

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        public void Raise(object sender, TEventArgs e)
        {
            List<WeakEventListener> listeners;
            lock (_list)
            {
                listeners = _list.GetCopy();
            }

            var foundStaleEntries = DeliverEvent(listeners, sender, e);
            if (foundStaleEntries)
            {
                lock (_list)
                {
                    _list.Purge();
                }
            }
        }

        private static bool DeliverEvent(IEnumerable<WeakEventListener> listeners, object sender, TEventArgs e)
        {
            var foundStaleEntries = false;

            foreach (var listener in listeners)
            {
                if (listener.Target != null)
                {
                    var handler = (EventHandler<TEventArgs>)listener.Handler;
                    if (handler != null)
                    {
                        handler(sender, e);
                    }
                }
                else
                {
                    foundStaleEntries = true;
                }
            }

            return foundStaleEntries;
        }
    }
}
