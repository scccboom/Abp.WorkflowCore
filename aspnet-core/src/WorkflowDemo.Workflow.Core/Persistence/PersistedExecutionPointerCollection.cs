﻿using System.Collections;
using System.Collections.Generic;

namespace WorkflowDemo.Workflows
{
    public class PersistedExecutionPointerCollection : ICollection<PersistedExecutionPointer>
    {
        private readonly Dictionary<string, PersistedExecutionPointer> _dictionary;

        public int Count => _dictionary.Count;

        public bool IsReadOnly => false;

        public PersistedExecutionPointerCollection()
        {
            _dictionary = new Dictionary<string, PersistedExecutionPointer>();
        }

        public PersistedExecutionPointerCollection(int capacity)
        {
            _dictionary = new Dictionary<string, PersistedExecutionPointer>(capacity);
        }

        public IEnumerator<PersistedExecutionPointer> GetEnumerator()
        {
            return _dictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public PersistedExecutionPointer FindById(string id)
        {
            if (!_dictionary.ContainsKey(id))
                return null;

            return _dictionary[id];
        }

        public void Add(PersistedExecutionPointer item)
        {
            _dictionary.Add(item.Id, item);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(PersistedExecutionPointer item)
        {
            return _dictionary.ContainsValue(item);
        }

        public void CopyTo(PersistedExecutionPointer[] array, int arrayIndex)
        {
            _dictionary.Values.CopyTo(array, arrayIndex);
        }

        public bool Remove(PersistedExecutionPointer item)
        {
            return _dictionary.Remove(item.Id);
        }
    }
}
