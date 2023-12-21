using RicKit.Common;

namespace RicKit.MakeEditorTools
{
    public class ClearSystem : MonoSingleton<ClearSystem>
    {
        protected override void GetAwake()
        {
            
        }
        public void Clear(ClearTrigger trigger)
        {
            trigger.onClear?.Invoke();
            Destroy(trigger.gameObject);
        }
    }
}