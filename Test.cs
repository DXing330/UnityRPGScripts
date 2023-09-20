using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // heap implementation.
    // private int capacity = 10;
    private int size = 0;
    public List<int> nodes = new List<int>();
    private int getLeftChildIndex(int parentIndex)
    {
        return ((parentIndex*2)+1);
    }

    private int getRightChildIndex(int parentIndex)
    {
        return ((parentIndex*2)+2);
    }

    private int getParentIndex(int childIndex)
    {
        return ((childIndex-1)/2);
    }

    private bool hasLeftChild(int index)
    {
        return (getLeftChildIndex(index) < size);
    }

    private bool hasRightChild(int index)
    {
        return (getRightChildIndex(index) < size);
    }

    private bool hasParent(int index)
    {
        return (getParentIndex(index) >= 0);
    }

    private int leftChild(int index)
    {
        return nodes[getLeftChildIndex(index)];
    }

    private int rightChild(int index)
    {
        return nodes[getRightChildIndex(index)];
    }

    private int parent(int index)
    {
        return nodes[getParentIndex(index)];
    }

    private void Swap(int indexOne, int indexTwo)
    {
        int temp = nodes[indexOne];
        nodes[indexOne] = nodes[indexTwo];
        nodes[indexTwo] = temp;
    }

    private void EnsureCapacity()
    {
        while (nodes.Count < size)
        {
            nodes.Add(-1);
        }
    }

    public int Peek()
    {
        if (size == 0)
        {
            return -1;
        }
        return nodes[0];
    }

    public int Pull()
    {
        if (size == 0)
        {
            return -1;
        }
        int node = nodes[0];
        nodes[0] = nodes[size-1];
        size--;
        HeapifyDown();
        return node;
    }

    public void AddNode(int newNode)
    {
        EnsureCapacity();
        nodes[size] = newNode;
        size++;
        HeapifyUp();
    }

    private void HeapifyUp()
    {
        int index = size - 1;
        while (hasParent(index) && parent(index) > nodes[index])
        {
            Swap(getParentIndex(index), index);
            index = getParentIndex(index);
        }
    }

    private void HeapifyDown()
    {
        int index = 0;
        while (hasLeftChild(index))
        {
            int smallerChildIndex = getLeftChildIndex(index);
            if (hasRightChild(index) && getRightChildIndex(index) < smallerChildIndex)
            {
                smallerChildIndex = getRightChildIndex(index);
            }
            if (nodes[index] < nodes[smallerChildIndex])
            {
                break;
            }
            Swap(index, smallerChildIndex);
            index = smallerChildIndex;
        }
    }
}
