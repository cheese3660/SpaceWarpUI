using System.Linq;
using I2.Loc;
using UnityEngine.UIElements;

namespace SpaceWarp.UI.Controls
{
    public class IntlFoldout: Foldout
    {
        public new class UxmlFactory : UxmlFactory<IntlFoldout, UxmlTraits>
        {
        }

        public new class UxmlTraits : Foldout.UxmlTraits
        {
            private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription { name = "text" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var foldout = (IntlFoldout) ve;
                foldout._localizedText = m_Text.GetValueFromBag(bag, cc);
                foldout.text = foldout._localizedText;
            }
        }

        private LocalizedString _localizedText;

        public new string text
        {
            // get => m_Toggle.text;
            // set => m_Toggle.text = value;
            get => hierarchy.Children().OfType<Toggle>().First().text;
            set => hierarchy.Children().OfType<Toggle>().First().text = value;
        }
    }
}