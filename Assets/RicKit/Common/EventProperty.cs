using System;

namespace RicKit.Common
{
    public class EventProperty<T>
    {
        private Action<T> onValueChanged;
        private T value;
        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                onValueChanged?.Invoke(value);
            }
        }

        public EventProperty(T value = default, Action<T> setter = null)
        { 
            this.value = value;
            onValueChanged += setter;
        }
        public EventProperty<T> AddListener(Action<T> onValueChanged)
        {
            this.onValueChanged += onValueChanged;
            return this;
        }
        public EventProperty<T> RemoveListener(Action<T> onValueChanged)
        {
            this.onValueChanged -= onValueChanged;
            return this;
        }
    }
}