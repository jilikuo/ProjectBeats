using UnityEngine;
using UnityEngine.UI;

namespace MagicPigGames.Portraits
{
    [HelpURL("https://infinitypbr.gitbook.io/infinity-pbr/other/portrait-avatars")]
    [System.Serializable]
    public class InGamePortrait
    {
        public GameObject avatar;
        public GameObject uiPortrait;
        public Portrait Portrait => GetPortrait();
        public Portrait3D Portrait3D => GetPortrait3D();
        public Portrait2D Portrait2D => GetPortrait2D();
        public PortraitUI PortraitUI => GetPortraitUI();
        public RawImage RawImage => GetRawImage();
        public Image Image => GetImage();

        private RawImage _rawImage;
        private Image _image;
        private Portrait _portrait;
        private Portrait2D _portrait2D;
        private Portrait3D _portrait3D;
        private PortraitUI _portraitUI;
        private Renderer _mainRenderer;

        public bool Is2D => Portrait2D != null;
        public bool Is3D => Portrait3D != null;
        
        public void SetBackgroundColor(Color value) => Portrait3D.avatarCamera.SetBackgroundColor(value);

        // Gets the RawImage, caching it if it hasn't been cached yet.
        private RawImage GetRawImage()
        {
            if (_rawImage != null)
                return _rawImage;

            _rawImage = PortraitUI.rawImage;
            if (_rawImage != null) return _rawImage;
            
            Debug.LogWarning("PortraitUI does not have a RawImage!");
            return null;
        }
        
        // Gets the Image, caching it if it hasn't been cached yet.
        private Image GetImage()
        {
            if (_image != null)
                return _image;

            _image = PortraitUI.image;
            if (_image != null) return _image;
            
            Debug.LogWarning("PortraitUI does not have an Image!");
            return null;
        }
        
        // This will return either the Portrait3D or the Portrait2D. Useful for when you want to get the portrait but
        // don't know if it'll be a 2D or 3D portrait...or just because it's easier!
        private Portrait GetPortrait()
        {
            if (_portrait != null)
                return _portrait;
            
            if (Portrait3D != null)
            {
                _portrait = Portrait3D;
                return Portrait3D;
            }
            
            if (Portrait2D != null)
            {
                _portrait = Portrait2D;
                return Portrait2D;
            }

            Debug.LogWarning("Avatar does not have a Portrait component!");
            return null;
        }
        
        // Will return the Portrait3D, and cache it for future use.
        private Portrait3D GetPortrait3D()
        {
            if (_portrait3D != null)
                return _portrait3D;

            if (avatar == null)
                return default;
            
            _portrait3D = avatar.GetComponent<Portrait3D>();
            if (_portrait3D != null) return _portrait3D;
            
            Debug.LogWarning("Avatar does not have a Portrait3D component!");
            return null;
        }
        
        // Will return the Portrait2D, and cache it for future use.
        private Portrait2D GetPortrait2D()
        {
            if (_portrait2D != null)
                return _portrait2D;
            
            _portrait2D = avatar.GetComponent<Portrait2D>();
            if (_portrait2D != null) return _portrait2D;
            
            Debug.LogWarning("Avatar does not have a Portrait2D component!");
            return null;
        }
        
        // Will return the PortraitUI, and cache it for future use.
        private PortraitUI GetPortraitUI()
        {
            if (_portraitUI != null)
                return _portraitUI;
            
            _portraitUI = uiPortrait.GetComponent<PortraitUI>();
            if (_portraitUI != null) return _portraitUI;
            
            Debug.LogWarning("uiPortrait does not have a Portrait UI component!");
            return null;
        }

        public InGamePortrait(GameObject avatar, GameObject uiPortrait)
        {
            this.avatar = avatar;
            this.uiPortrait = uiPortrait;
        }
        
        public InGamePortrait(GameObject uiPortrait) => this.uiPortrait = uiPortrait;

        public void SetSprite(Sprite value) => Image.sprite = value;
    }
}