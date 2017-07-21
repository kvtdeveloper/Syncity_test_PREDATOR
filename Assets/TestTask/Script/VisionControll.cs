using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionControll : MonoBehaviour
{

    public NightVision NightVision;
    public HeatVision HeatVision;
    public ThermalVision ThermalVision;

    public GameObject NightCamera;
    public GameObject HeatCamera;
    public GameObject ThermalCamera;

    void Update()
    {
         SwitchVision();
    }

    private void SwitchVision()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            NightVision.enabled = !NightVision.enabled;
            ThermalVision.enabled = false;
            HeatVision.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HeatVision.enabled = !HeatVision.enabled;
            NightVision.enabled = false;
            ThermalVision.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ThermalVision.enabled = !ThermalVision.enabled;
            HeatVision.enabled = false;
            NightVision.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            HeatVision.enabled = false;

            NightVision.enabled = false;
            ThermalVision.enabled = false;

            EnableCameraVision = !ThermalCamera.activeInHierarchy;
        }
    }

    private bool EnableCameraVision
    {
        set
        {
            NightCamera.SetActive(value);
            HeatCamera.SetActive(value);
            ThermalCamera.SetActive(value);
        }
    }
}
