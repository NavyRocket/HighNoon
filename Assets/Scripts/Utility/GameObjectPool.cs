using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : class
{
    public delegate T CreateFuncDel();
    CreateFuncDel createFunc;

    Queue<T> poolQueue = new Queue<T>();

    public GameObjectPool(CreateFuncDel createFuncDel, int count)
    {
        createFunc = createFuncDel;
        Allocation(count);
    }

    void Allocation(int count)
    {
        for(int i = 0; i < count; i++)
            poolQueue.Enqueue(createFunc());
    }

    public T Get()
    {
        if (poolQueue.Count > 0)
            return poolQueue.Dequeue();
        else
            return createFunc();
    }

    public void Set(T obj)
    {
        poolQueue.Enqueue(obj);
    }
}
