using System;
using System.Collections.Generic;

namespace UUM.Data
{
    public static class Collections
    {
        public static Collection Null;
        private static Dictionary<string, Collection> _collections;

        public static event EventHandler<string> OnCollectionCreated;

        static Collections()
        {
            _collections = new Dictionary<string, Collection>();
            Null = new Collection();
        }

        public static bool Has(string name)
        {
            return _collections.ContainsKey(name);
        }

        public static Collection Get(string name)
        {
            return _collections.ContainsKey(name) ? _collections[name] : Null;
        }
        
        public static Collection Create(string name)
        {
            if (!_collections.ContainsKey(name))
            {
                var coll = new Collection(name);
                _collections[name] = coll;
                OnCollectionCreated?.Invoke(coll, name);
                return coll;
            }
            else
            {
                return _collections[name];
            }
        }
    }
}