using System;
using System.Collections;
using System.Collections.Generic;

public class ObservableList<T> : IEnumerable<T>
{
    public delegate void            ListChangedHandler(object sender, ListChangedEventArgs e);
    public event ListChangedHandler ListChanged;

    private List<T> _internalList = new List<T>();

    public void Add(T item)
    {
        _internalList.Add(item);
        ListChanged?.Invoke(this, new ListChangedEventArgs(ListChangedOperation.Add, item));
    }

    public bool Remove(T item)
    {
        var result = _internalList.Remove(item);
        if(result)
        {
            ListChanged?.Invoke(this, new ListChangedEventArgs(ListChangedOperation.Remove, item));
        }
        return result;
    }

    public T this[int index]
    {
        get => _internalList[index];
        set
        {
            _internalList[index] = value;
            ListChanged?.Invoke(this, new ListChangedEventArgs(ListChangedOperation.Modify, value));
        }
    }
    
    public bool Contains(T item)
    {
        return _internalList.Contains(item);
    }
    
    public int IndexOf(T item)
    {
        return _internalList.IndexOf(item);
    }
    
    public void Clear()
    {
        _internalList.Clear();
    }
    
    public void CopyTo(T[] array, int arrayIndex)
    {
        _internalList.CopyTo(array, arrayIndex);
    }
    
    public void Insert(int index, T item)
    {
        _internalList.Insert(index, item);
    }
    
    public void RemoveAt(int index)
    {
        _internalList.RemoveAt(index);
    }
    
    public void RemoveAll(Predicate<T> match)
    {
        _internalList.RemoveAll(match);
    }
    
    public void Sort(Comparison<T> comparison)
    {
        _internalList.Sort(comparison);
    }
    
    public void Sort(IComparer<T> comparer)
    {
        _internalList.Sort(comparer);
    }
    
    public void Sort(int index, int count, IComparer<T> comparer)
    {
        _internalList.Sort(index, count, comparer);
    }
    
    public T Find(Predicate<T> match)
    {
        return _internalList.Find(match);
    }
    
    public List<T> FindAll(Predicate<T> match)
    {
        return _internalList.FindAll(match);
    }
    
    public int FindIndex(Predicate<T> match)
    {
        return _internalList.FindIndex(match);
    }
    
    public int FindIndex(int startIndex, Predicate<T> match)
    {
        return _internalList.FindIndex(startIndex, match);
    }
    
    public int FindIndex(int startIndex, int count, Predicate<T> match)
    {
        return _internalList.FindIndex(startIndex, count, match);
    }
    
    public T FindLast(Predicate<T> match)
    {
        return _internalList.FindLast(match);
    }
    
    public int FindLastIndex(Predicate<T> match)
    {
        return _internalList.FindLastIndex(match);
    }
    
    public int FindLastIndex(int startIndex, Predicate<T> match)
    {
        return _internalList.FindLastIndex(startIndex, match);
    }
    
    public int FindLastIndex(int startIndex, int count, Predicate<T> match)
    {
        return _internalList.FindLastIndex(startIndex, count, match);
    }
    
    public void ForEach(Action<T> action)
    {
        _internalList.ForEach(action);
    }
    
    public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
    {
        return _internalList.ConvertAll(converter);
    }
    
    public void CopyTo(T[] array)
    {
        _internalList.CopyTo(array);
    }
    
    public void CopyTo(int index, T[] array, int arrayIndex, int count)
    {
        _internalList.CopyTo(index, array, arrayIndex, count);
    }
    
    public bool Exists(Predicate<T> match)
    {
        return _internalList.Exists(match);
    }
    
    public T[] ToArray()
    {
        return _internalList.ToArray();
    }
    
    public void TrimExcess()
    {
        _internalList.TrimExcess();
    }
    
    public bool TrueForAll(Predicate<T> match)
    {
        return _internalList.TrueForAll(match);
    }
    
    public ObservableList<TOutput> Map<TOutput>(Func<T, TOutput> converter)
    {
        ObservableList<TOutput> newList = new ObservableList<TOutput>();
        foreach (T item in _internalList)
        {
            newList.Add(converter(item));
        }
        return newList;
    }

    public int Count => _internalList.Count;

    // Implement IEnumerable<T> and IEnumerable
    public IEnumerator<T> GetEnumerator()
    {
        return _internalList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _internalList.GetEnumerator();
    }

    public class ListChangedEventArgs : EventArgs
    {
        public ListChangedOperation Operation { get; }
        public T                    Item      { get; }

        public ListChangedEventArgs(ListChangedOperation operation, T item)
        {
            Operation = operation;
            Item      = item;
        }
    }

    public enum ListChangedOperation
    {
        Add,
        Remove,
        Modify
    }
}