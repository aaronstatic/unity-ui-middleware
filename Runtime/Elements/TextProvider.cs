using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UUM.Data;

namespace UUM
{
    public class TextProvider : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TextProvider, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription fieldName_String =
                new UxmlStringAttributeDescription { name = "field-name", defaultValue = "" };
            
            UxmlStringAttributeDescription numberFormat_String =
                new UxmlStringAttributeDescription { name = "number-format", defaultValue = "N" };
            
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                if (!(ve is TextProvider provider)) return;
                provider.FieldName = fieldName_String.GetValueFromBag(bag, cc);
                provider.NumberFormat = numberFormat_String.GetValueFromBag(bag, cc);

                if (provider.FieldName == "")
                {
                    Debug.LogWarning("[TextProvider] No Field Name specified");
                }
            }
        }
        
        public string FieldName { get; set; }
        public string NumberFormat { get; set; }

        public void SetObject(DataObject obj)
        {
            var text = obj.GetString(FieldName, NumberFormat);
            UQueryBuilder<TextElement> textElements = this.Query<TextElement>();
            textElements.ForEach((el) =>
            {
                el.text = text;
            });
        }
    }
}