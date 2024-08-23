using System;
using System.Linq;
using UnityEngine;

namespace MagicPigGames.Portraits
{
    public static class PortraitUtilities
    {
        
        
        public static Color RGBLerp(this Color valueFrom, Color valueTo, float t, bool includeAlpha = false)
        {
            var r = Mathf.Lerp(valueFrom.r, valueTo.r, t);
            var g = Mathf.Lerp(valueFrom.g, valueTo.g, t);
            var b = Mathf.Lerp(valueFrom.b, valueTo.b, t);
            var a = includeAlpha ?  Mathf.Lerp(valueFrom.a, valueTo.a, t) : valueFrom.a;
            return new Color(r, g, b, a);
        }
        
        
        
        // Checks the layerIndex to ensure it's not in use, will find one that is not in use if it is, or if -1 is 
        // provided. If none can be found, we'll return -1;
        public static int ComputeLayerIndex(int layerIndex)
        {
            if (!IsLayerInUse(layerIndex)) 
                return GetLayerIndex();
            
            Debug.LogWarning(
                $"Requested layer index {layerIndex} was in use. Will try to find one that is not currently in use.");
            return GetLayerIndex();
        }
        
        // Gets the first available layer that is not in use
        public static int GetLayerIndex()
        {
            var availableLayers = PortraitAvatars.instance.availableLayers;
            for (var i = 0; i < 32; i++)
            {
                if (availableLayers != (availableLayers | (1 << i))) continue;
                if (IsLayerInUse(i)) continue;
                
                return i;
            }

            return -1;
        }
        
        // Returns true if any of the active inGamePortrait objects are on the selected layer
        public static bool IsLayerInUse(int layer) 
            => PortraitAvatars.instance.InGamePortraits3D.Any(inGamePortrait => inGamePortrait.avatar.layer == layer);

        // Set the layer of the object and all of its children
        public static void SetLayerRecursively(GameObject obj, int newLayer)
        {
            if (obj == null)
                return;

            obj.layer = newLayer;

            foreach (Transform child in obj.transform)
            {
                if (child == null)
                    continue;

                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
        
        // Create a render texture with the specified width and height
        public static RenderTexture CreateRenderTexture(int width = 256, int height = 256)
        {
            var renderTexture = new RenderTexture(width, height, 24)
            {
                enableRandomWrite = true
            };
            renderTexture.Create();
            return renderTexture;
        }
        /*
        public static float CalculatePerceivedBrightness(Color color) 
            => Mathf.Max(color.r, Mathf.Max(color.g, color.b));

        public static Color ClampColorPerceivedBrightness(Color color, float minBrightness, float maxBrightness)
        {
            var perceivedBrightness = CalculatePerceivedBrightness(color);
            if (perceivedBrightness > minBrightness && perceivedBrightness < maxBrightness)
                return color;

            Debug.Log($"Perceived Brightness: {perceivedBrightness}, min/max: {minBrightness}/{maxBrightness}");
            
            return perceivedBrightness > maxBrightness 
                ? ClampBrightness(color, maxBrightness) 
                : ClampDimness(color, minBrightness);
        }
        
        public static Color ClampBrightness(Color originalColor, float maxBrightness)
        {
            // Calculate the perceived brightness of the original color
            float perceivedBrightness = CalculatePerceivedBrightness(originalColor);
            if (!(perceivedBrightness > maxBrightness)) 
                return originalColor;
            
            // Calculate the scaling factor
            float scaleFactor = maxBrightness / perceivedBrightness;

            // Scale down the R, G, B components to clamp brightness
            float r = originalColor.r * scaleFactor;
            float g = originalColor.g * scaleFactor;
            float b = originalColor.b * scaleFactor;

            // Construct and return the new color
            return new Color(r, g, b, originalColor.a);
        }
        
        public static Color ClampDimness(Color originalColor, float minBrightness)
        {
            double maxComponent = (double)CalculatePerceivedBrightness(originalColor);

            // If at least one component is greater than or equal to minBrightness, return original color
            if (maxComponent >= minBrightness)
            {
                return originalColor;
            }

            // If all components are less than minBrightness
            double scaleFactor = (double)minBrightness / maxComponent;
            Debug.Log($"Scale Factor: {scaleFactor}");
            Debug.Log($"Example R: {originalColor.r} * {scaleFactor} = {(double)originalColor.r * scaleFactor}");
            Debug.Log($"Example G: {originalColor.g} * {scaleFactor} = {(double)originalColor.g * scaleFactor}");
            Debug.Log($"Example B: {originalColor.b} * {scaleFactor} = {(double)originalColor.b * scaleFactor}");

            double r = (double)originalColor.r * scaleFactor;
            double g = (double)originalColor.g * scaleFactor;
            double b = (double)originalColor.b * scaleFactor;

            // Convert back to float when constructing the new Color
            return new Color((float)r, (float)g, (float)b, originalColor.a);
        }




        public static float CalculateBrightness(Color color)
        {
            // Assuming the color channels are in the range [0, 1]
            return 0.299f * color.r + 0.587f * color.g + 0.114f * color.b;
        }
        
        public static float CalculateLuminance(Color color)
        {
            return 0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b;
        }

        public static Color AdjustLuminance(Color color, float targetLuminance)
        {
            float currentLuminance = CalculateLuminance(color);

            if (Mathf.Approximately(currentLuminance, 0))
            {
                // Avoid division by zero or near-zero luminance
                return color;
            }

            float ratio = targetLuminance / currentLuminance;
        
            // Adjust the color by the ratio and clamp the components between 0 and 1
            return new Color(
                Mathf.Clamp01(color.r * ratio), 
                Mathf.Clamp01(color.g * ratio), 
                Mathf.Clamp01(color.b * ratio), 
                color.a);
        }

        public static Vector3[] GenerateUniformDirections(int numPoints)
        {
            var points = new Vector3[numPoints];
            var phi = Mathf.PI * (3f - Mathf.Sqrt(5f));  // golden angle in radians

            for (var i = 0; i < numPoints; i++)
            {
                var y = 1f - (i / (float)(numPoints - 1)) * 2f;  // y goes from 1 to -1
                var radius = Mathf.Sqrt(1f - y * y);  // radius at y

                var theta = phi * i;  // golden angle increment

                var x = Mathf.Cos(theta) * radius;
                var z = Mathf.Sin(theta) * radius;

                points[i] = new Vector3(x, y, z);
            }

            return points;
        }
        */
        
    }
}