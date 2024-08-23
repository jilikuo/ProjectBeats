using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MagicPigGames
{
    [System.Serializable]
    public class ImageScaleOverTime : MonoBehaviour
    {
        [Header("Options")]
        public float scaleSpeed = 0.5f;
        [Range(1f,5f)]
        [Tooltip("The multiplier -- 1f is the original scale.")]
        public float scaleAmount = 1.3f;
        
        private RectTransform _rectTransform;
        private Vector3 _startScale;
        private Coroutine _scaleCoroutine;
        
        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _startScale = _rectTransform.localScale;

            StartScaling();
        }

        // Call this method to start scaling
        public void StartScaling()
        {
            StopTheCoroutine();
            _scaleCoroutine = StartCoroutine(ScaleOverTime());
        }

        private IEnumerator ScaleOverTime()
        {
            while(true)
            {
                Vector3 targetScale = _startScale * scaleAmount;
                Vector3 originalScale = _rectTransform.localScale;

                // Scale Up
                for (float t = 0; t <= 1; t += Time.deltaTime * scaleSpeed)
                {
                    _rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, t);
                    yield return null;
                }

                _rectTransform.localScale = targetScale;

                // Scale Down
                for (float t = 0; t <= 1; t += Time.deltaTime * scaleSpeed)
                {
                    _rectTransform.localScale = Vector3.Lerp(targetScale, _startScale, t);
                    yield return null;
                }

                _rectTransform.localScale = _startScale;
            }
        }

        private void OnDisable() => StopTheCoroutine();

        private void StopTheCoroutine()
        {
            if (_scaleCoroutine != null)
                StopCoroutine(_scaleCoroutine);
        }
    }

}
