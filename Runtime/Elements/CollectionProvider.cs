using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UUM.Data;

namespace UUM
{
    public class CollectionProvider : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<CollectionProvider, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription collectionName_String =
                new UxmlStringAttributeDescription { name = "collection-name", defaultValue = "" };
            
            UxmlStringAttributeDescription templatePath_String =
                new UxmlStringAttributeDescription { name = "template-path", defaultValue = "" };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                if (!(ve is CollectionProvider provider)) return;
                provider.CollectionName = collectionName_String.GetValueFromBag(bag, cc);
                provider.TemplatePath = templatePath_String.GetValueFromBag(bag, cc);
                provider.Init();
            }
        }

        public string CollectionName { get; set; }
        public string TemplatePath { get; set; }

        private Collection _collection;
        private VisualElement _container;
        private VisualTreeAsset _prototype;

        private Dictionary<string, ObjectProvider> _providers;
        
        private void Init()
        {
            if (!Application.isPlaying) return;
            _providers = new Dictionary<string, ObjectProvider>();
            
            if (CollectionName == "") return;
            if (Collections.Has(CollectionName))
            {
                SetCollection(Collections.Get(CollectionName));
            }
            else
            {
                //Requested collection doesn't exist yet
                Collections.OnCollectionCreated += (sender, s) =>
                {
                    if (s != CollectionName) return;
                    SetCollection(Collections.Get(CollectionName));
                };
            }
        }

        public void FindContainer()
        {
            if (_container != null) return;
            
            //Step down the heirarchy until we find an empty element or a template

            VisualElement el = this;
            while(el.contentContainer.childCount > 0 && el.GetType() != typeof(TemplateContainer))
            {
                el = el.contentContainer.ElementAt(0);
            }

            //If we found a template, use the parent of it, otherwise use the element
            if (el.GetType() == typeof(TemplateContainer))
            {
                _container = el.parent;
            }
            else
            {
                _container = el;
            }
        }

        private void LoadAsset()
        {
            _prototype = Resources.Load<VisualTreeAsset>(TemplatePath);
        }

        public void SetCollection(Collection coll)
        {
            _collection = coll;

            FindContainer();
            LoadAsset();
            Refresh();
            
            _collection.ObjectAdded += (sender, o) =>
            {
                Add(o);
            };
            
            _collection.ObjectUpdated += (sender, o) =>
            {
                UpdateObject(o);
            };
        }

        protected void Refresh()
        {
            if (_collection == null || _container == null) return;
            
            _container.Clear();
            _collection.GetAll().ForEach(Add);
        }

        private void UpdateObject(DataObject obj)
        {
            if (!_providers.ContainsKey(obj.ID)) return;
            _providers[obj.ID].Refresh();
        }

        public void Add(DataObject obj)
        {
            if (_container == null || _prototype == null) return;

            var clone = _prototype.CloneTree();
            var provider = new ObjectProvider {CollectionName = CollectionName, ObjectId = obj.ID};
            provider.Add(clone);
            _container.Add(provider);
            provider.SetObject(obj);

            _providers[obj.ID] = provider;
        }
    }
}