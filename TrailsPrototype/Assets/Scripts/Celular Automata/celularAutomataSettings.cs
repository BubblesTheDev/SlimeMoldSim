using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class celularAutomataSettings : MonoBehaviour
{
    [Header("Settings")]
    public ComputeShader shader;
    public Color slimeColor = Color.blue;

    [Space, Header("Graphic Information")]
    [Range(320, 1920)]
    public int width = 320;
    [Range(180,1080)]
    public int height = 180;
    [SerializeField] RenderTexture initialLayerTexture;
    RenderTexture finalLayerTexture;
    public int lessThanToKill, greaterThanToKill;
    [Range(0.01f,0.99f)]
    public float cutoffThreshold;
    public int rangeToCheckCells;

    private void Awake() {
        initShader();
    }


    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        shader.Dispatch(shader.FindKernel("automataUpdate"), width / 16, height, 1);

        Graphics.Blit(finalLayerTexture, destination);
    }

    void initialization() {
        setupTexture();
        loadVariables();
    }

    public void setupTexture() {
        initialLayerTexture = new RenderTexture(width, height, 1);
        initialLayerTexture.enableRandomWrite = true;
        initialLayerTexture.Create();

        finalLayerTexture = new RenderTexture(width, height, 1);
        finalLayerTexture.enableRandomWrite = true;
        finalLayerTexture.Create();
    }

    void loadVariables() {
        shader.SetTexture(shader.FindKernel("automataInit"), "initialLayer", initialLayerTexture);
        shader.SetTexture(shader.FindKernel("automataInit"), "finalLayer", finalLayerTexture);

        shader.SetFloat("width", width);
        shader.SetFloat("height", height);
        shader.SetFloat("cutoffThreshold", cutoffThreshold);
        shader.SetInt("lessThanToKill", lessThanToKill);
        shader.SetFloat("greaterThanToKill", greaterThanToKill);

    }

    void initShader() {
        initialization();
        shader.Dispatch(shader.FindKernel("automataInit"), width, height, 1);
    }

    private void OnValidate() {
        initShader();
    }
}