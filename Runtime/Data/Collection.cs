using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UUM.Data
{
    public class Collection
    {
        public string Name;
        private List<DataObject> _objects;
        private Dictionary<string, DataObject> _index;
        
        public event EventHandler<DataObject> ObjectAdded;
        public event EventHandler<DataObject> ObjectUpdated;

        public Collection()
        {
            Init();
            Name = "Temp";
        }

        public Collection(string name)
        {
            Init();
            Name = name;
        }

        private void Init()
        {
            _objects = new List<DataObject>();
            _index = new Dictionary<string, DataObject>();
        }

        public void Add(DataObject obj)
        {
            if (!_index.ContainsKey(obj.ID))
            {
                _index[obj.ID] = obj;
                _objects.Add(obj);
                ObjectAdded?.Invoke(this, obj);
                obj.Update += ObjOnUpdate;
            }
        }

        private void ObjOnUpdate(object sender, EventArgs e)
        {
            var obj = sender as DataObject;
            ObjectUpdated?.Invoke(this, obj);
        }

        public DataObject Get(string id)
        {
            return _index.ContainsKey(id) ? _index[id] : null;
        }
        
        public bool Has(string id)
        {
            return _index.ContainsKey(id);
        }

        public List<DataObject> GetAll()
        {
            return _objects;
        }
    }
}