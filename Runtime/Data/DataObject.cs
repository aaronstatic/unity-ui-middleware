using System;
using System.Collections.Generic;

namespace UUM.Data
{
    public class DataObject
    {
        public string ID;

        private Dictionary<string, string> _strings;
        private Dictionary<string, int> _ints;
        private Dictionary<string, bool> _bools;

        public List<string> Keys { get; }

        public event EventHandler Update;

        public DataObject()
        {
            ID = Guid.NewGuid().ToString("N");
            
            _strings = new Dictionary<string, string>();
            _ints = new Dictionary<string, int>();
            _bools = new Dictionary<string, bool>();
            Keys = new List<string>();
        }

        public void Set(string name, string value)
        {
            _strings[name] = value;
            OnKeySet(name);
        }
        
        public void Set(string name, int value)
        {
            _ints[name] = value;
            OnKeySet(name);
        }
        
        public void Set(string name, bool value)
        {
            _bools[name] = value;
            OnKeySet(name);
        }

        public bool HasKey(string name)
        {
            return Keys.Contains(name);
        }

        private void OnKeySet(string name)
        {
            if (!Keys.Contains(name))
            {
                Keys.Add(name);
            }
            Update?.Invoke(this, EventArgs.Empty);
        }

        public string GetString(string name, string format = "N")
        {
            if (!HasKey(name)) return "";
            if (_strings.ContainsKey(name))
            {
                return _strings[name];
            }
            if (_ints.ContainsKey(name)) return _ints[name].ToString(format);
            if (_bools.ContainsKey(name)) return _bools[name] ? "Yes" : "No";
            return "";
        }

        public int GetInt(string name)
        {
            return _ints.ContainsKey(name) ? _ints[name] : 0;
        }
        
        public bool GetBool(string name)
        {
            return _bools.ContainsKey(name) && _bools[name];
        }
    }
}