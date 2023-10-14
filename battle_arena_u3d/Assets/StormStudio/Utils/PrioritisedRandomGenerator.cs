using System;
using System.Collections.Generic;

public class PrioritisedRandomGenerator<T>
{
    private List<T> _list = new List<T>();
    protected Random _random = new Random((int)DateTime.Now.Ticks);

    public PrioritisedRandomGenerator(Dictionary<T, int> priorityDict)
    {
        foreach (T key in priorityDict.Keys)
        {
            int count = priorityDict[key];
            for (int i = 0; i < count; i++)
            {
                int index = _random.Next(0, _list.Count);
                _list.Insert(index, key);
            }
        }
    }

    public T GetRandom()
    {
        return _list[_random.Next(0, _list.Count)];
    }
}
