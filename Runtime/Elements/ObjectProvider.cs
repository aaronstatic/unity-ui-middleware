using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UUM.Data;

namespace UUM
{
    public class ObjectProvider : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<ObjectProvider, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription collectionName_String =
                new UxmlStringAttributeDescription { name = "collection-name", defaultValue = "" };
            
            UxmlStringAttributeDescription objectId_String =
                new UxmlStringAttributeDescription { name = "object-id", defaultValue = "" };
            
            
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                if (!(ve is ObjectProvider provider)) return;
                provider.CollectionName = collectionName_String.GetValueFromBag(bag, cc);
                provider.ObjectId = objectId_String.GetValueFromBag(bag, cc);

                provider.Init();
            }
        }
        
        public string CollectionName { get; set; }
        public string ObjectId { get; set; }

        private DataObject _obj;

        private void Init()
        {
            if (!Application.isPlaying) return;
            if (CollectionName == "" || ObjectId == "") return;
            
            if (Collections.Has(CollectionName))
            {
                var coll = Collections.Get(CollectionName);
                if (coll.Has(ObjectId))
                {
                    SetObject(coll.Get(ObjectId));
                }
                else
                {
                    //Requested object doesnt exist yet
                    coll.ObjectAdded += (o, dataObject) =>
                    {
                        if (dataObject.ID == ObjectId)
                        {
                            SetObject(dataObject);
                            dataObject.Update += (sender, args) =>
                            {
                                Refresh();        
                            };
                        }
                    };
                }
            }
            else
            {
                //Requested collection doesn't exist yet
                Collections.OnCollectionCreated += (sender, s) =>
                {
                    if (s != CollectionName) return;
                    var coll = Collections.Get(CollectionName);
                    coll.ObjectAdded += (o, dataObject) =>
                    {
                        if (dataObject.ID == ObjectId)
                        {
                            SetObject(dataObject);
                        }
                    };
                };
            }
        }

        public void SetObject(DataObject obj)
        {
            _obj = obj;
            Refresh();
        }

        public void Refresh()
        {
            if (_obj == null) return;
            var providers = this.Query<TextProvider>();
            providers.ForEach(el =>
            {
                el.SetObject(_obj);
            });
        }
    }
}