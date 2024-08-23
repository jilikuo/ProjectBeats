using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MagicPigGames.Portraits
{
    [HelpURL("https://infinitypbr.gitbook.io/infinity-pbr/other/portrait-avatars/portrait-ui")]
    [System.Serializable]
    public class PortraitUI : MonoBehaviour
    {
        [Header("Required")]
        public RawImage rawImage;
        public Image image;
        [Tooltip("Populate this with any UI Image elements that you may wish to modify the color of at runtime.")]
        public Image[] uiImages;

        [FormerlySerializedAs("transitionTime")] [Header("Options")]
        public float uiTransitionTime = 1f;
        public float imageTransitionTime = 1f;
        
        private Color _uiCurrentColor;
        private Color _uiDesiredColor;
        private Color _imageCurrentColor;
        private Color _imageDesiredColor;
        private bool _onlySetFirstImage = false;
        private float _uiTransitionTimer = 0f;
        private float _imageTransitionTimer = 0f;
        private Coroutine _uiTransitionCoroutine;
        private Coroutine _imageTransitionCoroutine;
        
        public void SetImageColor(Color value, bool instant)
        {
            if (_imageTransitionCoroutine != null)
            {
                StopCoroutine(_imageTransitionCoroutine);
                _imageTransitionCoroutine = null;
            }
            
            if (instant)
            {
                SetImageColorInstant(value);
                return;
            }
            
            _imageCurrentColor = image == null ? rawImage.color : image.color;
            _imageDesiredColor = value;
            _imageTransitionTimer = 0;
            
            if (_imageCurrentColor != _imageDesiredColor && _imageTransitionCoroutine == null)
                _imageTransitionCoroutine = StartCoroutine(ImageTransitionToDesiredColor());
        }
        
        public void SetImageColorInstant(Color value)
        {
            if (rawImage != null)
                rawImage.color = value;
            
            if (image != null)
                image.color = value;
        }
        
        public virtual void SetUIColor(Color value, bool onlySetFirstImage = false, bool instant = false)
        {
            if (_uiTransitionCoroutine != null)
            {
                StopCoroutine(_uiTransitionCoroutine);
                _uiTransitionCoroutine = null;
            }
            
            if (instant)
            {
                SetUIColorInstant(value, onlySetFirstImage);
                return;
            }
            _onlySetFirstImage = onlySetFirstImage;
            _uiCurrentColor = uiImages[0].color;
            _uiDesiredColor = value;
            _uiTransitionTimer = 0;
            
            if (_uiCurrentColor != _uiDesiredColor && _uiTransitionCoroutine == null)
                _uiTransitionCoroutine = StartCoroutine(UITransitionToDesiredColor());
        }

        public virtual void SetUIColorInstant(Color value, bool onlySetFirstImage = false)
        {
            for (var i = 0; i < uiImages.Length; i++)
            {
                if (onlySetFirstImage && i > 0) continue;
                uiImages[i].color = value;
            }
        }

        protected virtual IEnumerator UITransitionToDesiredColor()
        {
            if (uiImages.Length == 0) yield break;

            var initialColor = _uiCurrentColor;

            while (_uiTransitionTimer < uiTransitionTime)
            {
                _uiTransitionTimer += Time.deltaTime;
                _uiCurrentColor = Color.Lerp(initialColor, _uiDesiredColor, _uiTransitionTimer / uiTransitionTime);

                SetUIColorInstant(_uiCurrentColor, _onlySetFirstImage);
                
                yield return null;
            }
            _uiCurrentColor = _uiDesiredColor;
            SetUIColorInstant(_uiCurrentColor, _onlySetFirstImage); // To ensure the final color is _desiredColor
            _uiTransitionCoroutine = null;
        }
        
        protected virtual IEnumerator ImageTransitionToDesiredColor()
        {
            if (rawImage == null && image == null) yield break;

            var initialColor = _imageCurrentColor;

            while (_imageTransitionTimer < imageTransitionTime)
            {
                _imageTransitionTimer += Time.deltaTime;
                _imageCurrentColor = Color.Lerp(initialColor, _imageDesiredColor, _imageTransitionTimer / imageTransitionTime);

                SetImageColorInstant(_imageCurrentColor);
                
                yield return null;
            }
            _imageCurrentColor = _imageDesiredColor;
            SetImageColorInstant(_imageCurrentColor); // To ensure the final color is _desiredColor
            _imageTransitionCoroutine = null;
        }

        private void OnValidate()
        {
            if (rawImage == null)
                rawImage = GetComponentInChildren<RawImage>();
            if (image == null)
                image = GetComponentInChildren<Image>();
        }
    }

}
