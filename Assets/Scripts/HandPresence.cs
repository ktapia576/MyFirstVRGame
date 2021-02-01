using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public bool showController = false; // Toggle whether or not to show controller or hands
    public InputDeviceCharacteristics controllerCharacteristics;    // Characteristics are chosen via dropdown in Unity Inspector
    public List<GameObject> controllerPrefabs;  // ControllerPrefabs are set in Unity Inspector as an Array
    public GameObject handModelPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();    // try to initialize at start, Update() function tries again if not initialized
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var device in devices)
        {
            Debug.Log("name: " + device.name + " characteristic: " + device.characteristics);
        }

        // Make sure there is at least one device connected
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name); // Find the Prefab that matches the device name in the prefab Array and return false if not found
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform); // Create the Game object thats matches device connected and use the transform properties 
            }
            else
            {
                Debug.LogError("Did not find the corresponding controller model");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    void UpdateHandAnimation() 
    {
        // Check if Trigger pressed, TryGetFeature method returns false if not pressed
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Example for getting input values on controllers ---------------------------------------------------------------------------------------------------
        //// PrimaryButtonValue = true if pressed and TryGetFeatureValue returns false if the input does not exist
        //if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
        //{
        //    Debug.Log("Primary Button was pressed!");
        //}

        //// Trigger will return a float value based on its axis
        //if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
        //{
        //    Debug.Log("Trigger was pressed: "+triggerValue);
        //}

        //if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue) && primary2DAxisValue != Vector2.zero)
        //{
        //    Debug.Log("Touchpad was used: " + primary2DAxisValue);
        //}
        // ---------------------------------------------------------------------------------------------------------------------------------------------------


        // Check if controller device is initialized at start of game, if not try to intialize again
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);

                // If showing hand model, update hand animation
                UpdateHandAnimation();
            }
        }
    }
}
