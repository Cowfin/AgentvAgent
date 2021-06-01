using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>{

    T[] items;
    int currentItemCount;

    // Constructor, takes a heap size as parameter to create a new heap.
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    // Adds an item T to the heap, increases item count and calls sort up.
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    // Removes first element, lowest, on the heap. Decreases item count and
    // calls sort down on the new replacement item.
    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    // Updates location of item T by re-evaluating sort location.
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    // Returns current number of items in the heap.
    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    // Checks if an item T already exists within the heap.
    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    // Makes checks against item T's children and if they are
    // of a lower index, move item T down the heap until it
    // satisfies the conditions of the heap.
    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    // Makes checks against item T's parent and if it is 
    // of a higher index, move item T up the heap until it
    // satisfies the conditions of the heap.
    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    // Swap itemA's and itemB's positions within the heap
    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}