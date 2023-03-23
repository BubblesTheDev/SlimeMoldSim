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
    [SerializeField] RenderTexture texture;
    public int lessThanToKill, greaterThanToKill;
    [Range(0.01f,0.99f)]
    public float cutoffThreshold;
    public int rangeToCheckCells;

    private void Awake() {
        initialization();
    }


    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        shader.Dispatch(shader.FindKernel("automataUpdate"), width / 16, height, 1);

        Graphics.Blit(texture, destination);
    }

    void initialization() {
        setupTexture();
        loadVariables();
    }

    public void setupTexture() {
        texture = new RenderTexture(width, height, 1);
        texture.enableRandomWrite = true;
        texture.Create();
    }

    void loadVariables() {
        shader.SetTexture(shader.FindKernel("automataUpdate"), "finalLayer", texture);

        shader.SetFloat("width", width);
        shader.SetFloat("height", height);
        shader.SetFloat("cutoffThreshold", cutoffThreshold);
        shader.SetInt("lessThanToKill", lessThanToKill);
        shader.SetFloat("greaterThanToKill", greaterThanToKill);

    }

    void initShader() {

    }

    private void OnValidate() {
        initialization();
    }
}