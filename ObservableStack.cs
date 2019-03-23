using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Observers
{
    public class StackOperationsLogger
    {
        private readonly Observer observer = new Observer();
        internal void SubscribeOn<T>(ObservableStack<T> stack)
        {
            stack.Add(observer);
        }

        public string GetLog()
        {
            return observer.Log.ToString();
        }
    }

    public interface IObserver
    {
        void HandleEvent(object eventData);
    }

    internal class Observer : IObserver
    {
        public StringBuilder Log = new StringBuilder();

        public void HandleEvent(object eventData)
        {
            Log.Append(eventData);
        }
    }

    public delegate void Chahge(object eventData);
    internal class ObservableStack<T> //: IObservable
    {
        public event Chahge Notify;
        readonly List<IObserver> observers = new List<IObserver>();
        internal void Add(IObserver observer)
        {
            observers.Add(observer);
            Notify += observer.HandleEvent;
        }

        private void Remove(IObserver observer)
        {
            observers.Remove(observer);
            Notify += observer.HandleEvent;
        }

        List<T> data = new List<T>();
        internal void Push(T obj)
        {
            data.Add(obj);
            Notify?.Invoke(new StackEventData<T> { IsPushed = true, Value = obj });
        }

        internal T Pop()
        {
            if (data.Count == 0)
                throw new InvalidOperationException();
            var result = data[data.Count - 1];
            Notify?.Invoke(new StackEventData<T> { IsPushed = false, Value = result });
            return result;
        }
    }
}
