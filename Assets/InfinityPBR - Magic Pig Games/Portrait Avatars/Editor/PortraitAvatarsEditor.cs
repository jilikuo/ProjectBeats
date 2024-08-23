using System;
using InfinityPBR;
using MagicPigGames.Portraits;
using UnityEditor;
using UnityEngine;

/*
 * Nov 4 2023 -- why does this look like this?
 * EASY: I may update it later. Need to make a version of the LayerMask field, but don't have the time right now. I
 * did want to include the link to the docs though, so, basically drawing that, then the default inspector.
 */

namespace MagicPigGames
{
    [CustomEditor(typeof(PortraitAvatars))]
    [CanEditMultipleObjects]
    [Serializable]
    public class PortraitAvatarsEditor : InfinityEditor
    {
        private PortraitAvatars Target => GetTarget();
        
        private PortraitAvatars _target;
        
        private PortraitAvatars GetTarget()
        {
            if (_target != null) return _target;
            _target = (PortraitAvatars) target;
            return _target;
        }

        public override void OnInspectorGUI()
        {
            LinkToDocs("https://infinitypbr.gitbook.io/infinity-pbr/work-in-progress-coming-soon/portraits/portrait-avatars");
            Header1("Portrait Avatars");
            Space();
            
            ShowRequired();
            GreyLine();
            ShowColors();
            GreyLine();
            ShowLights();

            GreyLine();
            DrawDefaultInspectorToggle("Portrait Avatars Draw Default Inspector");
            EditorUtility.SetDirty(Target);
        }

        private void ShowRequired()
        {
            SectionHeader("Required for 3D Avatars", false);
            
            StartRow();
            Label($"Portrait Avatar 3D Prefab {symbolInfo}", $"This is the 3D prefab which will instantiate at runtime " +
                                                          $"as the avatar. Your 3D Portrait Avatar likely will have " +
                                                          $"custom code to handle the look and other logic, such as " +
                                                          $"setting up the character, the wardrobe, and so on.", 200);
            if (Target.portraitAvatar3DPrefab == null)
                ImportantField();
            Target.portraitAvatar3DPrefab = Object(Target.portraitAvatar3DPrefab, typeof(GameObject), 250) as GameObject;
            EndRow();

            StartRow();
            Label($"Layermask {symbolInfo}", $"This should include all of the layers you've set up for each " +
                                             $"portrait, and no others. The number of layers selected here is also the " +
                                             $"maximum number of portraits available to display at once.", 200);
            Target.availableLayers = LayerMaskField(Target.availableLayers, 250);
            EndRow();
            
            StartRow();
            Label($"Local Spawn Position {symbolInfo}", $"This is the position, relative to the object this " +
                                                        $"component is attached to, where the avatar will spawn.", 200);
            Target.avatarLocalSpawnPosition = Vector3Field(Target.avatarLocalSpawnPosition, 250);
            EndRow();
        }

        private void ShowColors()
        {
            if (!SectionHeader("Colors"))
                return;
            
            Label($"{textNormal}Image / RawImage Color {symbolInfo}{textColorEnd}", $"This handles the Image for 2D Portraits, and the RawImage for " +
                $"3D Portraits.", 300, true, false, true);
            StartRow();
            Target.cacheImageColor = LeftCheck($"Cache Image Color {symbolInfo}", $"When true, anytime the color is updated, the value " +
                $"will be cached. This means that new Portraits instantiated " +
                $"will have the same color when they are created.", Target.cacheImageColor, 200);
            
            Label("Color", 40);
            Target.cachedImageColor = ColorField(Target.cachedImageColor, 100);
            EndRow();
            Space();
            
            
            Label($"{textNormal}UI Color {symbolInfo}{textColorEnd}", $"This handles all of the UI Image components added to the " +
                                                                      $"UI Portrait object.", 350, true, false, true);
            StartRow();
            Target.cacheUIColor = LeftCheck($"Cache UI Color {symbolInfo}", $"When true, anytime the color is updated, the value " +
                                                                               $"will be cached. This means that new Portraits instantiated " +
                                                                               $"will have the same color when they are created.", Target.cacheUIColor, 200);
            
            Label("Color", 40);
            Target.cachedUIColor = ColorField(Target.cachedUIColor, 100);
            EndRow();
            StartRow();
            Target.newPortraitsOnlySetFirstUIImage = LeftCheck($"New Portraits Only Set First UI Image {symbolInfo}", $"When true, only the first UI Image in the " +
                $"array will be set, when a portrait is created.", Target.newPortraitsOnlySetFirstUIImage, 400);
            EndRow();
            Space();
            
            
            Label($"{textNormal}Background Color {symbolInfo}{textColorEnd}", $"This handles the background color for " +
                                                                              $"3D Portraits.", 300, true, false, true);
            StartRow();
            Target.cacheBackgroundColor = LeftCheck($"Cache Background Color {symbolInfo}", $"When true, anytime the color is updated, the value " +
                $"will be cached. This means that new 3D Portraits instantiated " +
                $"will have the same color when they are created.", Target.cacheBackgroundColor, 200);
            
            Label("Color", 40);
            Target.cachedBackgroundColor = ColorField(Target.cachedBackgroundColor, 100);
            EndRow();
            Space();
        }

        private void ShowLights()
        {
            if (!SectionHeader("Lights"))
                return;
            
            Label($"{textNormal}Lighting {symbolInfo}{textColorEnd}", $"This handles the Lights on 3D portraits", 300, true, false, true);
            StartRow();
            Target.cacheLightColor = LeftCheck($"Cache Light Color {symbolInfo}", $"When true, anytime the light color is updated, the value " +
                $"will be cached. This means that new Portraits instantiated " +
                $"will have the same light color when they are created.", Target.cacheLightColor, 200);
            
            Label("Color", 40);
            Target.cachedLightColor = ColorField(Target.cachedLightColor, 100);
            EndRow();
            StartRow();
            Target.cacheLightIntensity = LeftCheck($"Cache Light Intensity {symbolInfo}", $"When true, anytime the light intensity is updated, the value " +
                $"will be cached. This means that new Portraits instantiated " +
                $"will have the same light intensity when they are created.", Target.cacheLightIntensity, 200);
            
            Label("Color", 40);
            Target.cachedLightIntensity = Float(Target.cachedLightIntensity, 50);
            EndRow();
            
            StartRow();
            Target.newPortraitsOnlySetFirstLight = LeftCheck($"New Portraits Only Set First Light {symbolInfo}", $"When true, only the first light in the " +
                $"array will be set, when a portrait is created.", Target.newPortraitsOnlySetFirstLight, 400);
            EndRow();
            Space();
        }

        private bool SectionHeader(string title, bool showButton = true)
        {
            StartRow();
            var returnValue = false;
            if (showButton)
                returnValue = ButtonOpenClose($"Portrait Avatars {title}");
            Header2(title);
            EndRow();

            return returnValue;
        }
        
    }
}