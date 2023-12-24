using System;

namespace RicKit.Common
{
    public class EventProperty<T>
    {
        private Action<T,T> onValueChanged;
        private T value;
        public T Value
        {
            get => value;
            set
            {
                var oldValue = this.value;
                this.value = value;
                onValueChanged?.Invoke(value, oldValue);
            }
        }

        public EventProperty(T value = default, Action<T, T> setter = null)
        { 
            this.value = value;
            onValueChanged += setter;
        }
        public EventProperty<T> AddListener(Action<T, T> onValueChanged)
        {
            this.onValueChanged += onValueChanged;
            return this;
        }
        public EventProperty<T> RemoveListener(Action<T, T> onValueChanged)
        {
            this.onValueChanged -= onValueChanged;
            return this;
        }
        public void SetWithoutNotify(T value)
        {
            this.value = value;
        }
    }
}