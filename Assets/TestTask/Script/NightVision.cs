//using UnityEngine;
//using System.Collections;
//[ExecuteInEditMode]
//[AddComponentMenu("Image Effects/Vision/NightVision")]
//public class Nightvision : MonoBehaviour {

//	public delegate void OnNightVisionEnabledDelegate(bool enabled);
//	public static event OnNightVisionEnabledDelegate onNightVisionEnabled;

//	public Shader shader;
//	public Texture2D _NoiseTex;
//	public Texture2D _MaskTex;
//	[Range(0f,1f)]
//	public float _LuminanceThreshold= 0.3f;
//	[Range(2f,20f)]
//	public float _ColorAmplification= 6.0f;
//	[Range(0.1f,1f)]
//	public float _LightTreshold = 0.2f;
//	[Range(0f,8f)]
//	public float _Zoom = 0f;

//	private Material m_Material;

//	void OnEnable()
//	{
//		if(onNightVisionEnabled!=null)onNightVisionEnabled(true);
//	}

//	protected virtual void Start ()
//	{
//		// Disable if we don't support image effects
//		if (!SystemInfo.supportsImageEffects) {
//			enabled = false;
//			return;
//		}
//		if(shader == null)shader = Shader.Find("Hidden/NightVision");
//		// Disable the image effect if the shader can't
//		// run on the users graphics card
//		if (!shader || !shader.isSupported)
//			enabled = false;
//	}

//	protected Material material {
//		get {
//			if (m_Material == null) {
//				m_Material = new Material (shader);
//				m_Material.hideFlags = HideFlags.HideAndDontSave;
//			}
//			return m_Material;
//		} 
//	}

//	protected virtual void OnDisable() {
//		if( m_Material ) {
//			DestroyImmediate( m_Material );
//		}
//		if(onNightVisionEnabled!=null)onNightVisionEnabled(false);
//	}
//	void OnRenderImage (RenderTexture source, RenderTexture destination) {
//		material.SetTexture("_NoiseTex", _NoiseTex);
//		material.SetTexture("_MaskTex", _MaskTex);
//		material.SetFloat("_LuminanceThreshold",_LuminanceThreshold);
//		material.SetFloat("_ColorAmplification",_ColorAmplification);
//		material.SetFloat("_LightTreshold",_LightTreshold);
//		material.SetFloat("_Zoom",_Zoom);
//		Graphics.Blit (source, destination, material);
//	}
//}

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
/// <summary>
/// Deferred night vision effect.
/// </summary>
public class NightVision : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The main color of the NV effect")]
    public Color m_NVColor = new Color(0f, 1f, 0.1724138f, 0f);

    [SerializeField]
    [Tooltip("The color that the NV effect will 'bleach' towards (white = default)")]
    public Color m_TargetBleachColor = new Color(1f, 1f, 1f, 0f);

    [Range(0f, 0.1f)]
    [Tooltip("How much base lighting does the NV effect pick up")]
    public float m_baseLightingContribution = 0.025f;

    [Range(0f, 128f)]
    [Tooltip("The higher this value, the more bright areas will get 'bleached out'")]
    public float m_LightSensitivityMultiplier = 100f;

    Material m_Material;
    Shader m_Shader;

    [Tooltip("Do we want to apply a vignette to the edges of the screen?")]
    public bool useVignetting = true;

    public Shader NightVisionShader
    {
        get { return m_Shader; }
    }

    private void DestroyMaterial(Material mat)
    {
        if (mat)
        {
            DestroyImmediate(mat);
            mat = null;
        }
    }

    private void CreateMaterials()
    {
        if (m_Shader == null)
        {
            m_Shader = Shader.Find("Custom/DeferredNightVisionShader");
        }

        if (m_Material == null && m_Shader != null && m_Shader.isSupported)
        {
            m_Material = CreateMaterial(m_Shader);
        }
    }

    private Material CreateMaterial(Shader shader)
    {
        if (!shader)
            return null;
        Material m = new Material(shader);
        m.hideFlags = HideFlags.HideAndDontSave;
        return m;
    }

    void OnDisable()
    {
        DestroyMaterial(m_Material); m_Material = null; m_Shader = null;
    }

    [ContextMenu("UpdateShaderValues")]
    public void UpdateShaderValues()
    {
        if (m_Material == null)
            return;

        m_Material.SetVector("_NVColor", m_NVColor);

        m_Material.SetVector("_TargetWhiteColor", m_TargetBleachColor);

        m_Material.SetFloat("_BaseLightingContribution", m_baseLightingContribution);

        m_Material.SetFloat("_LightSensitivityMultiplier", m_LightSensitivityMultiplier);

        // State switching		
        m_Material.shaderKeywords = null;

        if (useVignetting)
        {
            Shader.EnableKeyword("USE_VIGNETTE");
        }
        else
        {
            Shader.DisableKeyword("USE_VIGNETTE");
        }

    }


    void OnEnable()
    {
        CreateMaterials();
        UpdateShaderValues();
    }

    public void ReloadShaders()
    {
        OnDisable();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        UpdateShaderValues();
        CreateMaterials();

        Graphics.Blit(source, destination, m_Material);
    }
}

