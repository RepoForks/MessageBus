using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DSoft.Messaging;
using System.Threading;

namespace TestProject
{
    [TestClass]
    public class TestConcurrency
    {

        private const int ConcurrentIterations = 1000;

        MessageBus target;

        object lastSender;
        MessageBusEvent lastEvent;

        MessageBusEventHandler handler;

        [TestInitialize]
        public void Init()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            target = new MessageBus();

            RegisterForEvent("event");

            ResetLastEvent();
        }

        void RegisterForEvent(string eventId)
        {
            handler = new MessageBusEventHandler()
            {
                EventAction = (obj, message) =>
                {
                    lastSender = obj;
                    lastEvent = message;
                },
                EventId = eventId
            };

            target.Register(handler);
        }

        void UnregisterEvent(string eventId)
        {
            target.DeRegister(handler);
        }

        void ResetLastEvent()
        {
            lastEvent = null;
            lastSender = null;
        }

        [TestMethod]
        public void PostDuringRegistration()
        {

            Thread t = new Thread(postEvent);
            t.Start();
            Thread.Sleep(10);
            for (int i = 0; i < ConcurrentIterations; i++)
            {
                target.Post("stickyEvent", this, null);
            }
        }

        [TestMethod]
        public void PostStickyDuringRegistration()
        {

            Thread t = new Thread(postEvent);
            t.Start();
            Thread.Sleep(10);
            for (int i = 0; i < ConcurrentIterations; i++)
            {
                target.PostSticky("stickyEvent", this, null);
            }
        }

        void postEvent()
        {
            for (int i = 0; i < ConcurrentIterations; i++)
            {
                RegisterForEvent("stickyEvent");
            }
        }



        bool receivedEvent
        {
            get
            {
                return lastEvent != null;
            }
        }
    }
}
