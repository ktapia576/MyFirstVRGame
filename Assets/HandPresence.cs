using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public List<GameObject> controllerPrefabs;
    private InputDevice targetDevice;
    private GameObject spawnedController;

    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        foreach (var device in devices)
        {
            Debug.Log("name: " + device.name + " characteristic: "+ device.characteristics);
        }

        // Make sure there is at least one device connected
        if (devices.Count > 0) 
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform); // Create the Game object thats matches device connected and use the transform properties 
            }
            else
            {
                Debug.LogError("Did not find the corresponding controller model");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // PrimaryButtonValue = true if pressed and TryGetFeatureValue returns false if the input does not exist
        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
        {
            Debug.Log("Primary Button was pressed!");
        }

        // Trigger will return a float value based on its axis
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
        {
            Debug.Log("Trigger was pressed: "+triggerValue);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue) && primary2DAxisValue != Vector2.zero)
        {
            Debug.Log("Touchpad was used: " + primary2DAxisValue);
        }
    }
}
