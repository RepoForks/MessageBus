using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSoft.Messaging.Collections
{
    interface IMessageBusEventHandlerCollection
    {
        void Add(MessageBusEventHandler handler);
        void Remove(MessageBusEventHandler handler);
        void RemoveAll(string eventId);
        MessageBusEventHandler[] HandlersForEvent(string eventId);
        MessageBusEventHandler[] HandlersForEvent(Type eventType);
        MessageBusEventHandler[] HandlersForEvent<T>() where T : MessageBusEvent;
    }
}
