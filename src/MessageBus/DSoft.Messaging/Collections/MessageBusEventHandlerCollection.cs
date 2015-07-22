using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace DSoft.Messaging.Collections
{
	/// <summary>
	/// Collection of messagebuseventhandlers
	/// </summary>
    internal class MessageBusEventHandlerCollection :  IMessageBusEventHandlerCollection
	{

	    private Collection<MessageBusEventHandler> _collection;

	    public MessageBusEventHandlerCollection()
	    {
	        _collection = new Collection<MessageBusEventHandler>();
	    }

	    

	    /// <summary>
		/// Handlers for event.
		/// </summary>
		/// <param name="EventId">The event identifier.</param>
		/// <returns></returns>
        public MessageBusEventHandler[] HandlersForEvent(String EventId)
		{
		    lock (_collection)
		    {
		        var results = from item in _collection
		            where !String.IsNullOrWhiteSpace(item.EventId)
		            where item.EventId.ToLower().Equals(EventId.ToLower())
		            where item.EventAction != null
		            select item;

		        var array = results.ToArray();
		        return array;
		    }
		}

		/// <summary>
		/// Handlerses for event type
		/// </summary>
		/// <returns>The for event.</returns>
		/// <param name="eventType">Event type.</param>
		public MessageBusEventHandler[] HandlersForEvent (Type eventType)
		{
		    lock (_collection)
		    {
		        var results = from item in _collection
		            where item is TypedMessageBusEventHandler
		            where item.EventAction != null
		            select item;

		        var list = new List<MessageBusEventHandler>();

		        foreach (TypedMessageBusEventHandler item in results.ToArray())
		        {
		            if (item.EventType != null && item.EventType.Equals(eventType))
		            {
		                list.Add(item);
		            }
		        }

		        return list.ToArray();
		    }
		}

		/// <summary>
		/// Returns the event handlers for the specified Generic MessageBusEvent Type 
		/// </summary>
		/// <returns>The for event.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
        public MessageBusEventHandler[] HandlersForEvent<T>() where T : MessageBusEvent
		{
			return HandlersForEvent (typeof(T));
		}

        public void Add(MessageBusEventHandler handler)
        {
            lock (_collection)
            {
                if (_collection.Contains(handler) == false)
                {
                    _collection.Add(handler);
                }
            }
        }

        public void Remove(MessageBusEventHandler handler)
        {
            lock (_collection)
            {
                if (_collection.Contains(handler))
                {
                    _collection.Remove(handler);
                }
            }
        }

        public void RemoveAll(string eventId)
        {
            lock (_collection)
            {
                foreach (var item in HandlersForEvent(eventId))
                {
                    Remove(item);
                }
            }
        }
    }
}

