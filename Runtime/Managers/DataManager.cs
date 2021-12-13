using System;
using UnityEngine;
using UnityEngine.UIElements;
using UUM.Data;

namespace UUM
{
    [RequireComponent(typeof(UIDocument))]
    public class DataManager : MonoBehaviour
    {
        private UIDocument _document;
        private VisualElement _body;

        private void OnEnable()
        {
            _document = GetComponent<UIDocument>();
            _body = _document.rootVisualElement;
            
            BindCollections();
        }

        private void BindCollections()
        {
            UQueryBuilder<CollectionProvider> collectionProviders = _body.Query<CollectionProvider>();
            collectionProviders.ForEach(BindCollection);
        }

        private void BindCollection(CollectionProvider provider)
        {
            if (Collections.Has(provider.CollectionName))
            {
                var collection = Collections.Get(provider.CollectionName);
                provider.SetCollection(collection);
            }
            else
            {
                Collections.OnCollectionCreated += ((sender, s) =>
                {
                    if (s == provider.CollectionName)
                    {
                        var collection = sender as Collection;
                        provider.SetCollection(collection);
                    }
                });
            }
        }
    }
}