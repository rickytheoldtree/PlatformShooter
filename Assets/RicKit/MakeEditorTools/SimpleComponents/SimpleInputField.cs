using UnityEngine;
using UnityEngine.UI;

namespace RicKit.MakeEditorTools.SimpleComponents
{
    public class SimpleInputField : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private InputField inputField;
        public string Label
        {
            get => text.text;
            set => text.text = value;
        }
        public string Value
        {
            get => inputField.text;
            set => inputField.text = value;
        }
        public InputField.ContentType ContentType
        {
            get => inputField.contentType;
            set => inputField.contentType = value;
        }
        public InputField.OnChangeEvent OnValueChanged
        {
            get => inputField.onValueChanged;
            set => inputField.onValueChanged = value;
        }
    }
}