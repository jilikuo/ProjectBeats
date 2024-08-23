using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static MagicPigGames.Portraits.PortraitUtilities;
using static InfinityPBR.InfinityExtensions;

/*
 * This is the main class for the Portrait Avatars. It handles the creation of new portraits, and the management of
 * the existing portraits, which are in the inGamePortraits List<InGamePortrait>.
 *
 * You can override this to add additional functionality, or customize the methods.
 */

namespace MagicPigGames.Portraits
{
    [HelpURL("https://infinitypbr.gitbook.io/infinity-pbr/other/portrait-avatars/portrait-avatars")]
    [System.Serializable]
    public class PortraitAvatars : MonoBehaviour
    {
        public static PortraitAvatars instance;
        
        [Header("Required - 3D Avatars")] 
        [Tooltip("The prefab that will load. This can be overridden at runtime.")]
        public GameObject portraitAvatar3DPrefab;
        [Tooltip("Each portrait must be in its own layer. You can not have more portraits than the number of layers " +
                 "set here. Create one layer for each portrait, such as \"Portrait0\", \"Portrait1\" etc.")]
        public LayerMask availableLayers;
        public Vector3 avatarLocalSpawnPosition = new Vector3(0, 0, 0);

        [Header("Image / RawImage Color")]
        public bool cacheImageColor = true;
        public Color cachedImageColor = Color.white;
        
        [Header("UI Images Color")]
        public bool cacheUIColor = true;
        public Color cachedUIColor = Color.grey;
        public bool newPortraitsOnlySetFirstUIImage = false;
        
        [Header("Background Color (3D)")]
        [Tooltip("When true, each time the background color is set, it will be cached. Any new portrait will then " +
                 "be created and have the background color set to this value.")]
        public bool cacheBackgroundColor = true;
        public Color cachedBackgroundColor = Color.grey;
        
        [Header("Light Settings (3D)")]
        public bool cacheLightColor = true;
        public Color cachedLightColor = Color.white;
        public bool newPortraitsOnlySetFirstLight = false;
        public bool cacheLightIntensity = true;
        public float cachedLightIntensity = 1f;
        
        [Header("In Game Data")] 
        public List<InGamePortrait> inGamePortraits = new List<InGamePortrait>();
        
        public List<InGamePortrait> InGamePortraits3D => inGamePortraits.Where(inGamePortrait => inGamePortrait.Is3D).ToList();
        public List<InGamePortrait> InGamePortraits2D => inGamePortraits.Where(inGamePortrait => inGamePortrait.Is2D).ToList();

        public InGamePortrait Create2DPortrait()
        {
            if (PortraitsPanel.instance == null)
            {
                Debug.LogError("PortraitsPanel instance is null.");
                return default;
            }
            var newUiObject = PortraitsPanel.instance.Setup2DAvatarUI();
            inGamePortraits.Add(new InGamePortrait(newUiObject));
            
            if (cacheUIColor)
                SetUIColor(cachedUIColor, inGamePortraits.Count - 1, newPortraitsOnlySetFirstUIImage, true);
            
            if (cacheImageColor)
                SetImageColor(cachedImageColor, inGamePortraits.Count - 1, true);

            return inGamePortraits[^1];
        }
        
        public InGamePortrait Create3DPortrait(GameObject customPortraitAvatarPrefab = null, int layerIndex = -1)
        {
            // Make sure we have layers set
            if (availableLayers == 0)
            {
                Debug.LogError("No layers set! Please set layers for the number of portraits you may have.");
                return default;
            }

            // Compute the next layer, or check if the provided one (optional) is available
            layerIndex = ComputeLayerIndex(layerIndex);
            if (layerIndex < 0)
            {
                Debug.LogWarning($"No layers are available for new portraits");
                return default;
            }
            
            // Make sure we have at least one available layer
            if (InGamePortraits3D.Count == availableLayers.CountLayers())
            {
                Debug.LogWarning("There are no more layers available for new portraits!");
                return default;
            }

            // Set the prefab based on if one was provided or not
            var avatarPrefab = customPortraitAvatarPrefab == null
                ? portraitAvatar3DPrefab
                : customPortraitAvatarPrefab;

            if (avatarPrefab == null)
            {
                Debug.LogWarning("3D Avatar Prefab is null!");
                return default;
            }
            
            // Spawn, then set up, the avatar
            var newAvatar = Instantiate(avatarPrefab, transform);
            newAvatar.transform.localPosition = avatarLocalSpawnPosition;
            var renderTexture = newAvatar.GetComponent<Portrait>().SetupAvatar(inGamePortraits.Count, layerIndex);

            if (renderTexture == null)
            {
                Debug.LogError("Render Texture null after Avatar Setup");
                return null;
            }

            // Setup the 3D Avatar UI in the Portraits Panel
            var newUiObject = PortraitsPanel.instance.Setup3DAvatarUI(newAvatar, renderTexture);
            inGamePortraits.Add(new InGamePortrait(newAvatar, newUiObject));
            
            if (cacheBackgroundColor)
                SetBackgroundColor(cachedBackgroundColor, inGamePortraits.Count - 1, true);
            
            if (cacheUIColor)
                SetUIColor(cachedUIColor, inGamePortraits.Count - 1, newPortraitsOnlySetFirstUIImage, true);
            
            if (cacheImageColor)
                SetImageColor(cachedImageColor, inGamePortraits.Count - 1, true);
            
            if (cacheLightColor)
                SetLightColor(cachedLightColor, inGamePortraits.Count - 1, newPortraitsOnlySetFirstLight, true);
            
            if (cacheLightIntensity)
                SetLightIntensity(cachedLightIntensity, inGamePortraits.Count - 1, newPortraitsOnlySetFirstLight, true);

            return inGamePortraits[^1];
        }
        
        public virtual void CacheBackgroundColor(Color value) 
            => cachedBackgroundColor = cacheBackgroundColor ? value : cachedBackgroundColor;
        
        public virtual void CacheLightColor(Color value) 
            => cachedLightColor = cacheLightColor ? value : cachedLightColor;
        
        public virtual void CacheLightIntensity(float value) 
            => cachedLightIntensity = cacheLightIntensity ? value : cachedLightIntensity;
        
        public virtual void CacheUIColor(Color value) 
            => cachedUIColor = cacheUIColor ? value : cachedUIColor;
        
        public virtual void CacheImageColor(Color value) 
            => cachedImageColor = cacheImageColor ? value : cachedImageColor;

        public virtual void SetBackgroundColor(Color value, bool instant = false)
        {
            CacheBackgroundColor(value);
            foreach (var inGamePortrait in inGamePortraits.Where(inGamePortrait => inGamePortrait.Is3D))
                inGamePortrait.Portrait3D.avatarCamera.SetBackgroundColor(value, instant);
        }

        public virtual void SetBackgroundColor(Color value, int index, bool instant = false)
        {
            CacheBackgroundColor(value);
            if (!inGamePortraits[index].Is3D)
                return;
            inGamePortraits[index].Portrait3D.avatarCamera.SetBackgroundColor(value, instant);
        }
        
        public virtual void SetLightColor(Color value, bool onlySetFirstItem = false, bool instant = false)
        {
            CacheLightColor(value);
            foreach (var inGamePortrait in inGamePortraits.Where(inGamePortrait => inGamePortrait.Is3D))
                inGamePortrait.Portrait3D.SetLightColor(value, onlySetFirstItem, instant);
        }

        public virtual void SetLightColor(Color value, int index, bool onlySetFirstItem = false, bool instant = false)
        {
            CacheLightColor(value);
            if (!inGamePortraits[index].Is3D)
                return;
            inGamePortraits[index].Portrait3D.SetLightColor(value, onlySetFirstItem, instant);
        }
        
        public virtual void SetLightIntensity(float value, bool onlySetFirstItem = false, bool instant = false)
        {
            CacheLightIntensity(value);
            foreach (var inGamePortrait in inGamePortraits.Where(inGamePortrait => inGamePortrait.Is3D))
                inGamePortrait.Portrait3D.SetLightIntensity(value, onlySetFirstItem, instant);
        }

        public virtual void SetLightIntensity(float value, int index, bool onlySetFirstItem = false, bool instant = false)
        {
            CacheLightIntensity(value);
            if (!inGamePortraits[index].Is3D)
                return;
            inGamePortraits[index].Portrait3D.SetLightIntensity(value, onlySetFirstItem, instant);
        }

        public virtual void SetUIColor(Color value, bool onlySetFirstItem = false, bool instant = false)
        {
            CacheUIColor(value);
            foreach (var inGamePortrait in inGamePortraits)
                inGamePortrait.PortraitUI.SetUIColor(value, onlySetFirstItem, instant);
        }

        public virtual void SetUIColor(Color value, int index, bool onlySetFirstItem = false, bool instant = false)
        {
            CacheUIColor(value);
            inGamePortraits[index].PortraitUI.SetUIColor(value, onlySetFirstItem, instant);
        }
        
        public virtual void SetImageColor(Color value, bool instant = false)
        {
            CacheImageColor(value);
            foreach (var inGamePortrait in inGamePortraits)
                inGamePortrait.PortraitUI.SetImageColor(value, instant);
        }
        
        public virtual void SetImageColor(Color value, int index, bool instant = false)
        {
            CacheImageColor(value);
            inGamePortraits[index].PortraitUI.SetImageColor(value, instant);
        }

        protected virtual void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
    }
}