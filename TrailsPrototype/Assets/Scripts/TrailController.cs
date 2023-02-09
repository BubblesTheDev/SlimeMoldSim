using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{

    public struct particle
    {
        public Vector2 position;
        public float angle;
    }


    [Range(320, 1920)]
    public int textureWidth = 320;
    [Range(180,1080)]
    public int textureHeight = 180;

    public ComputeShader shader;
    public RenderTexture render;


    [Space]
    public int numParticles = 200;
    public float speed = 10;
    public particle[] particles;
    public float reductionSpeed;


    private void Awake()
    {
        setupTexture();

    }
    private void OnValidate()
    {
        setupTexture();

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        shader.Dispatch(0, textureWidth, textureHeight , 1);
        initalizeCompute();

        Graphics.Blit(render, destination);
    }

    void setupTexture()
    {
        render = new RenderTexture(textureWidth, textureHeight, 1);
        render.enableRandomWrite = true;
        render.Create();

        createFlyers();
    }

    void initalizeCompute()
    {
        shader.SetInt("width", textureWidth);
        shader.SetInt("height", textureHeight);
        shader.SetInt("numFlyers", numParticles);
        shader.SetFloat("flySpeed", speed);
        shader.SetTexture(0, "Texture", render);
        shader.SetFloat("deltaTime", Time.deltaTime);
        shader.SetFloat("reduceSpeed", reductionSpeed);
    }

    void createFlyers()
    {
        particles = new particle[numParticles];
        for (int i = 0; i < numParticles; i++)
        {
            particle temp = new particle();
            float angle = i * Mathf.PI * 2f / numParticles;
            temp.position = new Vector2(Mathf.Cos(angle) * 5 + textureWidth / 2, Mathf.Sin(angle) * 5 + textureHeight / 2);
            temp.angle = Random.Range(0, 360);
            particles[i] = temp;
        }

        int vector3Size = sizeof(float) * 2;
        int angleSize = sizeof(float);
        int totalSize = vector3Size + angleSize;

        ComputeBuffer particlesBuffer = new ComputeBuffer(particles.Length, totalSize);
        particlesBuffer.SetData(particles);

        shader.SetBuffer(0, "particles", particlesBuffer);
        shader.Dispatch(0, particles.Length / 16, 1, 1);
    }
}
