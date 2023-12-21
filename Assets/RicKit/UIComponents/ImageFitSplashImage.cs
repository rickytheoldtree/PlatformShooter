using UnityEngine;
using UnityEngine.UI;

namespace RicKit.UIComponents
{
    [RequireComponent(typeof(Image))]
    public class ImageFitSplashImage : MonoBehaviour
    {
        private Image img;

        private void Awake()
        {
            img = GetComponent<Image>();
            var screenRatio = (float)Screen.width / Screen.height;
            var bgRatio = img.sprite.rect.width / img.sprite.rect.height;
            if (screenRatio > bgRatio)
                transform.localScale = Vector3.one * screenRatio / bgRatio;
            else
                transform.localScale = Vector3.one * bgRatio / screenRatio;
        }
    }
}