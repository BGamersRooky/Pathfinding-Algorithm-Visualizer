using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<TElement>
{
    List<ElementList> List = new List<ElementList>();
    ElementList result;
    System.Random rand = new System.Random();

    public void Enqueue(TElement e, int p = 0)
    {
        List.Add(new ElementList(e, p));
    }

    public TElement Dequeue()
    {       
        result = List[0];        

        foreach (ElementList e in List) {
            if (result.priority > e.priority)
            {
                result = e;
            }
        }
        List.Remove(result);

        return result.element;
    }

    public bool Contains(TElement e)
    {
        foreach(ElementList el in List)
        {
            if(el.element.Equals(e))
            {
                return true;
            }
        }

        return false;
    }

    public void SetPriority(TElement e, int p)
    {
        foreach (ElementList el in List)
        {
            if (el.element.Equals(e))
            {
                el.priority = p;
                break;
            }
        }
    }

    public int Count()
    {
        return List.Count;
    }

    class ElementList
    {
        public TElement element;
        public int priority;

        public ElementList(TElement e, int priority)
        {
            this.element = e;
            this.priority = priority;
        }       
        public override string ToString()
        {
            return element.ToString() + " and priority is:" + priority;
        }
    }
}
