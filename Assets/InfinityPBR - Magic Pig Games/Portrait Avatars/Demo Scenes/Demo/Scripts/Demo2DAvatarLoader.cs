using UnityEngine;
using UnityEngine.UI;

/*
 * This is an example of the "do something" class which you can call once the avatar has been loaded. Often this may
 * be the step where you load all the customizations for the avatar -- materials, wardrobe, blend shapes, and so on.
 *
 * In this example, we will randomly select a new material for the head.
 */

namespace MagicPigGames.Portraits.Demo
{
    public class Demo2DAvatarLoader : MonoBehaviour
    {
        [Header("Required")]
        public Image image;
        public Sprite[] sprites;
        
        public void LoadAvatar() => image.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}