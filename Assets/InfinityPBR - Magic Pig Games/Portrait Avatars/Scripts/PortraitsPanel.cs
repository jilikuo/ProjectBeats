using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MagicPigGames.Portraits
{
    [HelpURL("https://infinitypbr.gitbook.io/infinity-pbr/other/portrait-avatars")]
    [System.Serializable]
    public class PortraitsPanel : MonoBehaviour
    {
        public static PortraitsPanel instance;
        
        [Header("Required")]
        public GameObject uiPortraitPrefab;
        
        [Header("Plumbing")]
        public GridLayoutGroup gridLayoutGroup;
        
        [Header("Runtime")]
        public List<GameObject> portraits = new List<GameObject>();

        
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }

        public GameObject Setup2DAvatarUI()
        {
            // Instantiate a uiPortraitPrefab
            var instantiatedObject = CreateObject();
            if (instantiatedObject == null)
                return default;
            
            // Set the render texture
            var portraitUi = instantiatedObject.GetComponent<PortraitUI>();
            SetImageActive(portraitUi);
            SetRawImageActive(portraitUi, false);
            
            portraits.Add(instantiatedObject);
            return instantiatedObject;
        }

        public GameObject Setup3DAvatarUI(GameObject avatar, RenderTexture newRenderTexture)
        {
            // Instantiate a uiPortraitPrefab
            var instantiatedObject = CreateObject();
            if (instantiatedObject == null)
                return default;
            
            // Set the render texture
            var portraitUi = instantiatedObject.GetComponent<PortraitUI>();
            SetRawImageActive(portraitUi);
            SetImageActive(portraitUi, false);
            
            var rawImage = portraitUi.rawImage;
            rawImage.texture = newRenderTexture;
            
            portraits.Add(instantiatedObject);
            return instantiatedObject;
        }

        private GameObject CreateObject()
        {
            if (uiPortraitPrefab != null) 
                return Instantiate(uiPortraitPrefab, transform);
            
            Debug.Log("No UI Portrait Prefab assigned!");
            return default;
        }
        
        private void SetImageActive(PortraitUI portraitUI, bool value = true) => portraitUI.image.gameObject.SetActive(value);
        private void SetRawImageActive(PortraitUI portraitUI, bool value = true) => portraitUI.rawImage.gameObject.SetActive(value);
    }

}
