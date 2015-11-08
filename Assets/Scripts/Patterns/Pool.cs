using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Can be attached as a component or created as a container.
public class Pool : MonoBehaviour
{
    [SerializeField]
    private GameObject _template;

    [SerializeField]
    private int _capacity;

    private LinkedList<GameObject> _list = new LinkedList<GameObject>();
    private int _numActive = 0;

    public Pool(GameObject template, int capacity)
        : base()
    {
        _template = template;
        _capacity = 0;
        Resize(capacity);
    }

    public void Resize(int capacity)
    {
        // Make sure the number of active items is never larger than the number of items in the pool.
        if (_numActive > capacity)
        {
            _numActive = capacity;
        }

        // If the current capacity is smaller than the new capacity, increase the pool size.
        while (_capacity < capacity)
        {
            //Debug.Log("adding");
            GameObject instance = Instantiate<GameObject>(_template);
            instance.SetActive(false);
            _list.AddLast(instance);

            ++_capacity;
        }

        // If the current capacity is larger than the new capacity, decrease the pool size.
        while (_capacity > capacity)
        {
            //Debug.Log("removing");
            _list.RemoveLast();

            --_capacity;
        }

        //Debug.Log("capacity is now " + _capacity);
        //Debug.Log("list is now " + _list.Count);
    }

    // Items will be taken from the pool much less frequently than per-frame.
    public void Refresh()
    {
        //Debug.Log("num active was " + _numActive);
        LinkedListNode<GameObject> current = _list.First;
        for (int i = 0; i < _numActive; ++i)
        {
            if (!current.Value.activeSelf)
            {
                //Debug.Log("item inactive");
                LinkedListNode<GameObject> next = current.Next;

                // Put the inactive item at the end of the list and reduce the tally of active items.
                _list.Remove(current);
                _list.AddLast(current);
                --_numActive;

                // Move forward from the point in the list before the current node was moved.
                current = next;
            }
            else
            {
                // Move forward in the list.
                current = current.Next;
            }
        }
        //Debug.Log("num active is now " + _numActive);
    }

    public GameObject GetItem()
    {
        if (_capacity > 0)
        {
            // Update all items in the list.
            Refresh();

            // Increase the active tally if this value isn't active.
            if (!_list.Last.Value.activeSelf)
            {
                //Debug.Log("new active");
                ++_numActive;
            }

            // Make the last item inactive and put it at the front of the list.
            LinkedListNode<GameObject> last = _list.Last;
            last.Value.SetActive(true);
            _list.RemoveLast();
            _list.AddFirst(last);

            return last.Value;
        }

        return null;
    }

    public IList<GameObject> GetItems(int amount)
    {
        List<GameObject> items = new List<GameObject>();

        if (_capacity > 0)
        {
            // Update all items in the list.
            Refresh();

            LinkedListNode<GameObject> current = _list.Last;
            do
            {
                LinkedListNode<GameObject> previous = current.Previous;
                //Debug.Log(previous);

                // Increase the active tally if this value isn't active.
                if (!current.Value.activeSelf)
                {
                    //Debug.Log("new active");
                    ++_numActive;
                }

                current.Value.SetActive(true);
                _list.RemoveLast();
                _list.AddFirst(current);

                // Add to the list of items to return.
                items.Add(current.Value);

                // Move back in the list from the point before the current node was moved.
                current = previous;
            } while (current != null && items.Count < amount);
        }

        return items;
    }

    public void SetAllActive(bool active)
    {
        foreach (GameObject g in _list)
        {
            g.SetActive(active);
        }

        _numActive = (active ? _list.Count : 0);
    }

    public void Clear()
    {
        LinkedListNode<GameObject> current = _list.First;
        while (current != null)
        {
            Destroy(current.Value);
            current = current.Next;
        }
        _list.Clear();
        _capacity = 0;
        _numActive = 0;
    }

    #region Component
    // Pool can be used as a component too.
    public Pool()
        : base()
    {
    }

    private void Start()
    {
        //Debug.Log("Starting");
        // Reset the capacity to 0 and resize with the given value.
        int capacity = _capacity;
        _capacity = 0;
        Resize(capacity);
    }
    #endregion

    #region Accessors
    public int capacity
    {
        get { return _capacity; }
        set { Resize(value); }
    }

    public int numActive
    {
        get
        {
            Refresh();
            return _numActive;
        }
    }

    public GameObject template
    {
        get { return _template; }
    }
    #endregion
}
