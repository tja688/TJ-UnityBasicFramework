using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Arts.原型验证.Scripts
{
    public class GlobalLightControl : MonoBehaviour
    {
        [Tooltip("Assign the Global Light 2D here. If null, it will try to find one automatically.")]
        [SerializeField] private Light2D _targetLight;

        private float _initialIntensity;
        private bool _isOff;

        private void Start()
        {
            if (_targetLight == null)
            {
                // Try to find a global light in the scene
                // Note: FindObjectsOfType includes inactive objects if we pass true? No, usually safer to just look for active ones first.
                // Light2D is usually in UnityEngine.Rendering.Universal
                var lights = FindObjectsOfType<Light2D>();
                foreach (var light in lights)
                {
                    if (light.lightType == Light2D.LightType.Global)
                    {
                        _targetLight = light;
                        break;
                    }
                }
            }

            if (_targetLight != null)
            {
                _initialIntensity = _targetLight.intensity;
            }
            else
            {
                Debug.LogWarning("GlobalLightControl: No Global Light 2D found or assigned! The toggle button will not work.");
            }
        }

        /// <summary>
        /// Toggles the global light intensity between 0 and its initial value.
        /// Bind this method to a UI Button's OnClick event.
        /// </summary>
        public void ToggleGlobalLight()
        {
            if (_targetLight == null) return;

            _isOff = !_isOff;
            _targetLight.intensity = _isOff ? 0f : _initialIntensity;
        }
    }
}
