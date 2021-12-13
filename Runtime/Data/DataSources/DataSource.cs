//Abstract base monobehaviour for a source of data

using System;
using UnityEngine;

namespace UUM.Data
{
    public class DataSource : MonoBehaviour
    {
        public string collectionName;

        public void Awake()
        {
            if (collectionName == "")
            {
                collectionName = gameObject.name;
            }
        }
    }
}