using System.Linq;
using UnityEditor;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

namespace _project.Editor.UI_Elements
{
    [CustomPropertyDrawer(typeof(LocalizedStringData))]
    public class LocalizedStringDataPropertyDrawer : PropertyDrawer
    {
        public VisualTreeAsset m_asset;
        public StyleSheet m_styleSheet;
        private DropdownField _dropDown;
        private SerializedProperty _property;
        private TextField _textField;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            _property = property;
            var currentLanguage = property.FindPropertyRelative("Language").stringValue;
            var text = property.FindPropertyRelative("TextData").stringValue;
            var locales = LocalizationSettings.AvailableLocales.Locales;
            var languages = new string[locales.Count];
            var currentLanguageIndex =0;
            for (var i = 0; i < locales.Count; i++)
            {
                languages[i] = locales[i].LocaleName;
                if (locales[i].LocaleName == currentLanguage) currentLanguageIndex = i;
            }

            var root = new VisualElement();
            root.styleSheets.Add(m_styleSheet);
            root.AddToClassList("mainContainer");
            _dropDown = new DropdownField(languages.ToList(), currentLanguageIndex);
            _textField = new TextField
            {
                value = text
            };
            _dropDown.RegisterValueChangedCallback(OnLanguageValueChanged);
            _textField.RegisterValueChangedCallback(OnTextChanged);
            root.Add(_dropDown);
            root.Add(_textField);
            return root;
        }

        private void OnTextChanged(ChangeEvent<string> evt)
        {
            UpdateData("TextData",evt.newValue);
        }

        private void OnLanguageValueChanged(ChangeEvent<string> evt)
        {
          
            UpdateData("Language",evt.newValue);
        }

        private void UpdateData(string _key, string _value)
        {
            _property.FindPropertyRelative(_key).stringValue =_value;
            _property.serializedObject.ApplyModifiedProperties();
            _property.serializedObject.Update();
        }
        
    }
}