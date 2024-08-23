using InfinityPBR;
using UnityEngine;

/*
 * This is a demo on how to use the Portraits from a controller object. Boostrapper, controller, whatever you
 * want to call it, you'll probably have some object which "does the stuff" that needs to happen at loading.
 *
 * Well, that's this script for the demo!
 */

namespace MagicPigGames.Portraits.Demo
{
    public class DemoGameController : MonoBehaviour
    {
        [Header("Plumbing")]
        public GridCellAutoHeight gridCellAutoHeight;
        
        [Header("Settings")]
        public Color[] backgroundColors;
        public Color[] borderColors;
        public Color[] lightColors;
        
        [Header("2D Required")]
        public Sprite[] sprites;

        public void SetRandomBackgroundColor() 
            => PortraitAvatars.instance.SetBackgroundColor(backgroundColors.TakeRandom());
        
        public void SetRandomBorderColor() 
            => PortraitAvatars.instance.SetUIColor(borderColors.TakeRandom());

        public void SetLightColor()
            => PortraitAvatars.instance.SetLightColor(lightColors.TakeRandom());
        
        public void SetLightIntensity(float value)
            => PortraitAvatars.instance.SetLightIntensity(value);

        public void Load3DPortrait()
        {
            var newAvatar = PortraitAvatars.instance.Create3DPortrait();
            if (newAvatar == null)
            {
                Debug.LogWarning("3D Avatar was not returned!");
                return;
            }
            
            newAvatar.avatar.GetComponent<DemoAvatarLoader>().LoadAvatar();
        }

        public void Load2DPortrait()
        {
            if (sprites.Length == 0)
            {
                Debug.LogError("No sprites available!");
                return;
            }
            
            var newAvatar = PortraitAvatars.instance.Create2DPortrait();
            if (newAvatar == null)
            {
                Debug.LogWarning("2D Avatar was not returned!");
                return;
            }

            newAvatar.SetSprite(sprites.TakeRandom());
        }

        public void SetRandomSprites()
        {
            foreach (var inGamePortrait in PortraitAvatars.instance.inGamePortraits)
                inGamePortrait.SetSprite(sprites.TakeRandom());
        }

        public void ScalePortraitHeight(float value) => gridCellAutoHeight.StartTransition(value);
        
        public void ResetImageColor() => PortraitAvatars.instance.SetImageColor(Color.white);
        
    }
}
