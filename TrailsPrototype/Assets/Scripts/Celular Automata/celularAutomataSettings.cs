using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class celularAutomataSettings : MonoBehaviour
{
    [Header("Settings")]
    public ComputeShader shader;
    public Color slimeColor = Color.blue;
    public ring[] rings;
    public cell[,] cellGrid;

    [Space, Header("Graphic Information")]
    [Range(320, 1920)]
    public int width = 320;
    [Range(180,1080)]
    public int height = 180;
    [SerializeField] RenderTexture texture;

    private void Awake() {
        setupTexture();
    }


    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        shader.SetTexture(0, "texture", texture);

        Graphics.Blit(source, texture);
    }

    public void setupTexture() {
        texture = new RenderTexture(width, height, 1);
        texture.enableRandomWrite = true;
        texture.Create();
    }

    public void createGrid() {

    }
}

[System.Serializable]
public struct ring {
    public float minRadius;
    public float maxRadius;
    public float minAlive;
    public float maxAlive;
    public float minDead;
    public float maxDead;
}

[System.Serializable]
public struct cell {
    Vector2 position;
    float value;
}