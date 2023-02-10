using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeMoldSimulator : MonoBehaviour {
    public ComputeShader shader;

    [Header("Particle information")]
    public int numParticles = 100000;
    public float sensorDistance = 5f, sensorAngle = 30f;
    public float moveWeight = 1.5f, turnWeight = 2f;
    public float moveSpeed = 5f, turnAngle = 15f;
    public float pherimoneIntensity = 1f;
    public particle[] particles;


    [Space, Header("GraphicInformation")]
    [Range(320, 1920)]
    public int width = 320;
    [Range(180,1080)]
    public int height = 180;
    public Color slimeColor = Color.white;
    public float decayRate = 3f;
    public float diffuseSpeed;
    public RenderTexture texture;
    [SerializeField] RenderTexture diffusedTexture;

    private void Awake() {
        setupTextures();
        loadVariables();

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        shader.Dispatch(0, width / 16, height, 1);

        Graphics.Blit(texture, destination);
    }

    void setupTextures() {
        texture = new RenderTexture(width, height, 1);
        texture.enableRandomWrite = true;
        texture.Create();

        diffusedTexture = new RenderTexture(width, height, 1);
        diffusedTexture.enableRandomWrite = true;
        diffusedTexture.Create();
    }

    void loadVariables() {
        shader.SetTexture(0, "shader", texture);
        shader.SetTexture(0, "diffusedShader", diffusedTexture);

        shader.SetInt("numParticles", numParticles);
        shader.SetFloat("sensorDistance", sensorDistance);
        shader.SetFloat("sensorAngle", sensorAngle);
        shader.SetFloat("moveWeight", moveWeight);
        shader.SetFloat("turnWeight", turnWeight);
        shader.SetFloat("moveSpeed", moveSpeed);
        shader.SetFloat("turnAngle", turnAngle);
        shader.SetFloat("pherimoneIntensity", pherimoneIntensity);
        shader.SetFloat("deltaTime", Time.deltaTime);
        shader.SetFloat("screenWidth", width);
        shader.SetFloat("screenHeight", height);
        shader.SetFloat("diffuseSpeed", diffuseSpeed);

        createParticles();
    }

    void createParticles() {
        particles = new particle[numParticles];
        for (int i = 0; i < numParticles; i++) {
            particle temp = new particle();
            float angle = i * Mathf.PI * 2f / numParticles;
            temp.position = new Vector2(Mathf.Cos(angle) * 5 + width / 2, Mathf.Sin(angle) * 5 + height / 2);
            temp.headingAngle = Random.Range(0, 360);
            particles[i] = temp;
        }

        int vector2Size = sizeof(float) * 2;
        int angleSize = sizeof(float);
        int totalSize = vector2Size + angleSize;

        ComputeBuffer particlesBuffer = new ComputeBuffer(numParticles, totalSize);
        particlesBuffer.SetData(particles);

        shader.SetBuffer(0, "particles", particlesBuffer);
        shader.Dispatch(0, particles.Length / 16, 1, 1);

    }
}

[SerializeField]
public struct particle {
    public Vector2 position;
    public float headingAngle;
}

