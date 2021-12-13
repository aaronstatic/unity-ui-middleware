using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace UUM.Data
{
    public class JSONDataSource : DataSource
    {
        public TextAsset asset;
        [TextArea(15,20)]
        public string json;
        
        private bool inArray = false;

        private void Start()
        {
            if (asset != null)
            {
                json = asset.text;
            }
            Collection collection = Collections.Create(collectionName);
            JsonTextReader reader = new JsonTextReader(new StringReader(json));

            DataObject obj = new DataObject();
            string key = "";
            
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        key = reader.Value as string;
                    }

                    if (reader.TokenType == JsonToken.String && key == "id")
                    {
                        obj.ID = reader.Value as string;
                    }else if (reader.TokenType == JsonToken.String && key != "")
                    {
                        obj.Set(key, reader.Value as string);
                    }
                    if (reader.TokenType == JsonToken.Integer && key != "")
                    {
                        obj.Set(key, int.Parse(reader.Value.ToString()));
                    }
                    if (reader.TokenType == JsonToken.Boolean && key != "")
                    {
                        obj.Set(key, reader.Value.ToString() == "true");
                    }
                }
                else
                {
                    if (reader.TokenType == JsonToken.StartArray)
                    {
                        inArray = true;
                    }

                    if (reader.TokenType == JsonToken.EndArray)
                    {
                        inArray = false;
                    }
                    
                    if (reader.TokenType == JsonToken.StartObject && inArray)
                    {
                        obj = new DataObject();
                    }
                    
                    if(reader.TokenType == JsonToken.EndObject)
                    {
                        collection.Add(obj);
                    }
                }
            }
        }
    }
}