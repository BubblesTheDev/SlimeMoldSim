using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeMoldSimulator : MonoBehaviour
{
    public ComputeShader shader;

    [Header("Particle information")]
    public float numParticles = 100000;
    public float sensorDistance = 5f, sensorAngle = 30f;
    public float moveWeight = 1.5f, turnWeight = 2f;
    public float moveSpeed = 5f, turnAngle = 15f;
    public float pherimoneIntensity = 1f;


    [Space, Header("GraphicInformation")]
    [Range(320, 1920)]
    public int width = 320; 
    [Range(180,1080)]
    public int height = 180;
    public Color slimeColor = Color.white;
    public float decayRate = 3f;
    public RenderTexture texture;

    private void Awake() {
        setupTextures();
    }

    private void Update() {
        shader.SetTexture(0, "Shader", texture);
        shader.Dispatch(0, width / 16, height, 1);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(texture, destination);
    }

    void setupTextures() {
        texture = new RenderTexture(width, height, 1);
        texture.height = height;
        texture.width = width;
        texture.enableRandomWrite = true;
        texture.Create();
    }


    
}

public struct particle {
    Vector2 position;
    public float headingAngle;
}

public struct cell {
    public float intensity;
}

