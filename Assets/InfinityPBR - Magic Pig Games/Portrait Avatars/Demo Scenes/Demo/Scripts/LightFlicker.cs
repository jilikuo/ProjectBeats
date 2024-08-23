using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MagicPigGames
{
    [RequireComponent(typeof(Light))] // Ensure there is a Light component attached
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField]
        private float minIntensity = 0.5f; // Minimum light intensity
        [SerializeField]
        private float maxIntensity = 1.5f; // Maximum light intensity
        [SerializeField]
        private float flickerDuration = 0.1f; // Duration between flickers

        private Light _light;
        private float _timer; // Timer to keep track of flicker duration
        
        [SerializeField]
        private Color minColor = Color.yellow;
        [SerializeField]
        private Color maxColor = Color.red;

        private Coroutine _flickerCoroutine;

        private void Start()
        {
            _flickerCoroutine = StartCoroutine(FlickerCoroutine());
        }

        private IEnumerator FlickerCoroutine()
        {
            while (true)
            {
                _light.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time, 0.0f));
                _light.color = Color.Lerp(minColor, maxColor, Mathf.PerlinNoise(Time.time, 1.0f));
                yield return new WaitForSeconds(flickerDuration);
            }
        }

        private void OnDisable()
        {
            StopCoroutine(_flickerCoroutine);
        }

        private void Awake()
        {
            _light = GetComponent<Light>();
        }

        private void Update()
        {
            /*
            // Flicker the light at intervals of flickerDuration
            _timer += Time.deltaTime;
            if (_timer >= flickerDuration)
            {
                _timer = 0f;
                FlickerLight();
            }
            */
        }

        private void FlickerLight()
        {
            // Randomize the intensity within the specified range
            _light.intensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}