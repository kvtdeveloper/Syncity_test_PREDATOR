using UnityEngine;
using System.Collections;

public class ThermalVisionPlayerEventReceiver : MonoBehaviour {
	private Material m_Material;
	void Start()
	{
		m_Material = GetComponent<Renderer>().material;
	}
	// Use this for initialization
	void OnEnable () {
		ThermalVision.onThermalVisionEnabled+= OnThermalModeInit;
		HeatVision.onHeatVisionEnabled+= OnThermalMode2Init;
	}
	
	// Update is called once per frame
	void OnDisable () {
		ThermalVision.onThermalVisionEnabled-= OnThermalModeInit;
		HeatVision.onHeatVisionEnabled-= OnThermalMode2Init;
	}

	void OnThermalModeInit (bool enabled)
	{
		if(enabled)
		{
            m_Material.SetColor("_EmissionColor", Color.white*5f);
		}
		else 
		{
            m_Material.SetColor("_EmissionColor", Color.black);
		}
	}

	void OnThermalMode2Init (bool enabled)
	{
		if(enabled)
		{
            m_Material.SetColor("_EmissionColor", Color.white*5f);
		}
		else 
		{
            m_Material.SetColor("_EmissionColor", Color.black);
		}
	}
}
