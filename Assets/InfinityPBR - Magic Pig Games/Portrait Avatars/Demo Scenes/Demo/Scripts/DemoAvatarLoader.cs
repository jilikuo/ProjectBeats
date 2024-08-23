using UnityEngine;

/*
 * This is an example of the "do something" class which you can call once the avatar has been loaded. Often this may
 * be the step where you load all the customizations for the avatar -- materials, wardrobe, blend shapes, and so on.
 *
 * In this example, we will randomly select a new material for the head.
 */

namespace MagicPigGames.Portraits.Demo
{
    public class DemoAvatarLoader : MonoBehaviour
    {
        [Header("3D Required")]
        public MeshRenderer headRenderer;
        public Material[] headMaterials;
        
        public void LoadAvatar() => headRenderer.material = headMaterials[Random.Range(0, headMaterials.Length)];
    }
}