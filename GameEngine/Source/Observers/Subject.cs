using System.Collections.Generic;

namespace GameEngine.Source.Observers
{
    public abstract class Subject
    {
        private readonly List<Observer> _observers = new List<Observers.Observer>();

        public void Subscribe(Observers.Observer observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(Observers.Observer observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
