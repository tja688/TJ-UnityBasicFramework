using UnityEngine;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// This class helps the Dialogue System's demo work with the New Input System. It
    /// registers the inputs defined in DemoInputControls_Custom for use with the Dialogue 
    /// System's Input Device Manager.
    /// </summary>
    public class DemoInputRegistration : MonoBehaviour
    {

#if USE_NEW_INPUT

        private DemoInputControls_Custom _controlsCustom;

        // Track which instance of this script registered the inputs, to prevent
        // another instance from accidentally unregistering them.
        protected static bool isRegistered = false;
        private bool didIRegister = false;

        void Awake()
        {
            _controlsCustom = new DemoInputControls_Custom();
        }

        void OnEnable()
        {
            if (!isRegistered)
            {
                isRegistered = true;
                didIRegister = true;
                _controlsCustom.Enable();
                InputDeviceManager.RegisterInputAction("Horizontal", _controlsCustom.DemoActionMap.Horizontal);
                InputDeviceManager.RegisterInputAction("Vertical", _controlsCustom.DemoActionMap.Vertical);
                InputDeviceManager.RegisterInputAction("Fire1", _controlsCustom.DemoActionMap.Fire1);
            }
        }

        void OnDisable()
        {
            if (didIRegister)
            {
                isRegistered = false;
                didIRegister = false;
                _controlsCustom.Disable();
                InputDeviceManager.UnregisterInputAction("Horizontal");
                InputDeviceManager.UnregisterInputAction("Vertical");
                InputDeviceManager.UnregisterInputAction("Fire1");
            }
        }

#endif

    }
}
