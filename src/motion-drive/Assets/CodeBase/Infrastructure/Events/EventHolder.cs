using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure.Events
{
  public static class EventHolder<T> where T : class
  {
    private static readonly List<Action<T>> _listeners = new List<Action<T>>();

    private static T _currentInfoState;

    public static void AddListener(Action<T> listener, bool instantNotify)
    {
      _listeners.Add(listener);
      
      if (instantNotify && _currentInfoState != null) 
        listener?.Invoke(_currentInfoState);
    }

    public static void RaiseRegistrationInfo(T state)
    {
      _currentInfoState = state;

      foreach (var listener in _listeners) 
        listener?.Invoke(state);
    }

    public static void RemoveListener(Action<T> listener)
    {
      if (_listeners.Contains(listener)) 
        _listeners.Remove(listener);
    }

  }
}