using System;
using System.Collections;
using UnityEngine;
using static MagicPigGames.Portraits.PortraitUtilities;

namespace MagicPigGames.Portraits
{
    [HelpURL("https://infinitypbr.gitbook.io/infinity-pbr/other/portrait-avatars/portrait3d")]
    public class Portrait3D : Portrait
    {
        [Header("3D Options")] 
        public int renderTextureWidth = 256;
        public int renderTextureHeight = 256;
        [Tooltip("Populate this with any lights that are intended to illuminate only the portrait object.")]
        public Light[] lights;

        [Header("Transition Options")] 
        public float lightColorTransitionTime = 1f;
        public float lightIntensityTransitionTime = 1f;
        
        [Header("3D Plumbing")]
        public AvatarCamera avatarCamera;
        //public Renderer mainRenderer;
        
        private Color _currentColor;
        private Color _desiredColor;
        private bool _colorOnlySetFirstImage = false;
        private float _colorTransitionTimer = 0f;
        private Coroutine _colorTransitionCoroutine;
        
        private float _currentIntensity;
        private float _desiredIntensity;
        private bool _intensityOnlySetFirstImage = false;
        private float _intensityTransitionTimer = 0f;
        private Coroutine _intensityTransitionCoroutine;
        
        public override RenderTexture SetupAvatar(int index, int layer)
        {
            if (PortraitsPanel.instance == null)
            {
                Debug.Log("PortraitsPanel.instance is not found, or has not yet been loaded. If you're getting " +
                          "this error when the level or scene is loading, you may need re-order the code, so that the " +
                          "code which called this happens after PortraitsPanel.instance is available.");
                return default;
            }
           
            SetLayerRecursively(gameObject, layer);
            SetupLights(layer);
            if (!avatarCamera.Setup(layer))
                return default;
            
            var renderTexture = CreateRenderTexture(renderTextureWidth, renderTextureHeight);
            avatarCamera.AddRenderTexture(renderTexture);
            
            return renderTexture;
        }
        
        protected virtual void SetupLights(int layer)
        {
            foreach (var lightObj in lights)
                lightObj.cullingMask = 1 << layer;
        }
        
        public virtual void SetLightIntensity(float value, bool onlySetFirstLight = false, bool instant = false)
        {
            if (_intensityTransitionCoroutine != null)
            {
                StopCoroutine(_intensityTransitionCoroutine);
                _intensityTransitionCoroutine = null;
            }
            
            if (instant)
            {
                SetLightIntensityInstant(value, onlySetFirstLight);
                return;
            }
            
            _intensityOnlySetFirstImage = onlySetFirstLight;
            _currentIntensity = lights[0].intensity;
            _desiredIntensity = value;
            _intensityTransitionTimer = 0;
            
            if (Math.Abs(_currentIntensity - _desiredIntensity) > 0.001f && _intensityTransitionCoroutine == null)
                _intensityTransitionCoroutine = StartCoroutine(TransitionToDesiredIntensity());
        }
        
        public virtual void SetLightIntensityInstant(float value, bool onlySetFirstLight = false)
        {
            if (onlySetFirstLight)
            {
                if (lights[0] == null)
                    return;
                
                lights[0].intensity = value;
                return;
            }
            
            foreach (var lightObj in lights)
            {
                if (lightObj == null)
                    continue;
                
                lightObj.intensity = value;
            }
        }
        
        protected virtual IEnumerator TransitionToDesiredIntensity()
        {
            if (lights.Length == 0) yield break;
            
            var initialIntensity = _currentIntensity;

            while (_intensityTransitionTimer < lightIntensityTransitionTime)
            {
                _intensityTransitionTimer += Time.deltaTime;
                _currentIntensity = Mathf.Lerp(initialIntensity, _desiredIntensity, _intensityTransitionTimer / lightIntensityTransitionTime);

                SetLightIntensityInstant(_currentIntensity, _intensityOnlySetFirstImage);

                yield return null;
            }

            _currentIntensity = _desiredIntensity;
            SetLightIntensityInstant(_currentIntensity, _intensityOnlySetFirstImage);
            _intensityTransitionCoroutine = null;
        }
        
        public virtual void SetLightColor(Color value, bool onlySetFirstLight = false, bool instant = false)
        {
            if (_colorTransitionCoroutine != null)
            {
                StopCoroutine(_colorTransitionCoroutine);
                _colorTransitionCoroutine = null;
            }
            
            if (instant)
            {
                SetLightColorInstant(value, onlySetFirstLight);
                return;
            }
            
            _colorOnlySetFirstImage = onlySetFirstLight;
            _currentColor = lights[0].color;
            _desiredColor = value;
            _colorTransitionTimer = 0;
            
            if (_currentColor != _desiredColor && _colorTransitionCoroutine == null)
                _colorTransitionCoroutine = StartCoroutine(TransitionToDesiredColor());
        }
        
        public virtual void SetLightColorInstant(Color value, bool onlySetFirstLight = false)
        {
            if (onlySetFirstLight)
            {
                if (lights[0] == null)
                    return;
                
                lights[0].color = value;
                return;
            }
            
            foreach (var lightObj in lights)
            {
                if (lightObj == null)
                    continue;
                
                lightObj.color = value;
            }
        }
        
        protected virtual IEnumerator TransitionToDesiredColor()
        {
            if (lights.Length == 0) yield break;
            
            var initialColor = _currentColor;

            while (_colorTransitionTimer < lightColorTransitionTime)
            {
                _colorTransitionTimer += Time.deltaTime;
                _currentColor = Color.Lerp(initialColor, _desiredColor, _colorTransitionTimer / lightColorTransitionTime);

                SetLightColorInstant(_currentColor, _colorOnlySetFirstImage);

                yield return null;
            }

            _currentColor = _desiredColor;
            SetLightColorInstant(_currentColor, _colorOnlySetFirstImage);
            _colorTransitionCoroutine = null;
        }
        
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if (avatarCamera == null)
                avatarCamera = GetComponentInChildren<AvatarCamera>();
        }
    }
}