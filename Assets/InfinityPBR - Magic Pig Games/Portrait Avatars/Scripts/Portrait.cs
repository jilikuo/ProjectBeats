using UnityEngine;

namespace MagicPigGames.Portraits
{
    [HelpURL("https://infinitypbr.gitbook.io/infinity-pbr/other/portrait-avatars")]
    [System.Serializable]
    public class Portrait : MonoBehaviour
    {
        public virtual RenderTexture SetupAvatar(int index, int layer)
        {
            Debug.LogError("Did you forget to Override this?");
            return default;
        }
        

        protected virtual void OnValidate()
        {
            
        }
    }
}