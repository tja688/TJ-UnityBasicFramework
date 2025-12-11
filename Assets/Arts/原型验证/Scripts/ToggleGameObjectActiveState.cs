using UnityEngine;
using UnityEngine.UI; // Required for Button component

public class ToggleGameObjectActiveState : MonoBehaviour
{
    public GameObject targetGameObject; // The GameObject to be toggled

    /// <summary>
    /// Toggles the active state (active/inactive) of the target GameObject.
    /// This method can be called by a UI Button's OnClick event.
    /// </summary>
    public void ToggleActiveState()
    {
        if (targetGameObject != null)
        {
            targetGameObject.SetActive(!targetGameObject.activeSelf);
            Debug.Log($"GameObject '{targetGameObject.name}' active state toggled to: {targetGameObject.activeSelf}");
        }
        else
        {
            Debug.LogWarning("Target GameObject not assigned to ToggleGameObjectActiveState script.");
        }
    }
}
