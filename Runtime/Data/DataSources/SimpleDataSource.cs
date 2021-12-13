using System;
using System.Collections.Generic;

namespace UUM.Data
{
    public class SimpleDataSource : DataSource
    {
        [Serializable]
        public struct KeyValue
        {
            public string key;
            public string value;
        }

        [Serializable]
        public struct Row
        {
            public string id;
            public List<KeyValue> values;
        }

        public List<Row> data;

        private void Start()
        {
            Collection collection = Collections.Create(collectionName);
            
            data.ForEach(row =>
            {
                var obj = new DataObject();
                obj.ID = row.id;
                row.values.ForEach(field =>
                {
                    obj.Set(field.key, field.value);
                });
                collection.Add(obj);
            });
        }
    }
}