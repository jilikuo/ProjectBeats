using System.Collections;
using UnityEngine;
using static MagicPigGames.Portraits.PortraitUtilities;

/*
 * This handles the camera assigned to the object featured in the portrait.
 */

namespace MagicPigGames.Portraits
{
    [HelpURL("https://infinitypbr.gitbook.io/infinity-pbr/other/portrait-avatars/avatar-camera")]
    [System.Serializable]
    public class AvatarCamera : MonoBehaviour
    {
        [Header("Options")] 
        public float transitionTime = 1f;
        
        [Header("Plumbing")] 
        [SerializeField] private Camera portraitCamera;

        private RenderTexture _renderTexture;
        
        private Color _currentColor;
        private Color _desiredColor;
        private float _transitionTimer = 0f;
        private Coroutine _transitionCoroutine;
        
        public bool Setup(int portraitLayer)
        {
            // Add the layer to the camera's culling mask
            if (portraitCamera == null)
            {
                Debug.Log("No camera assigned!");
                return false;
            }
            
            portraitCamera.cullingMask |= 1 << portraitLayer; // Add to the existing layers
            
            _renderTexture = CreateRenderTexture();
            portraitCamera.targetTexture = _renderTexture;
            
            return true;
        }
        
        public void SetBackgroundColorInstant(Color value) => portraitCamera.backgroundColor = value;

        public virtual void SetBackgroundColor(Color value, bool instant = false)
        {
            if (instant)
            {
                SetBackgroundColorInstant(value);
                return;
            }
            
            _currentColor = portraitCamera.backgroundColor;
            _desiredColor = value;
            _transitionTimer = 0;
            
            if (_currentColor != _desiredColor && _transitionCoroutine == null)
                _transitionCoroutine = StartCoroutine(TransitionToDesiredColor());
        }

        public void AddRenderTexture(RenderTexture renderTexture) 
            => portraitCamera.targetTexture = renderTexture;

        private void OnValidate()
        {
            if (portraitCamera == null)
                portraitCamera = GetComponentInChildren<Camera>();
        }
        
        protected virtual IEnumerator TransitionToDesiredColor()
        {
            while (_currentColor != _desiredColor)
            {
                _transitionTimer += Time.deltaTime;
                _currentColor = Color.Lerp(_currentColor, _desiredColor, _transitionTimer / transitionTime);

                SetBackgroundColorInstant(_currentColor);
                
                yield return null;
            }
            
            _transitionCoroutine = null;
        }
    }
}