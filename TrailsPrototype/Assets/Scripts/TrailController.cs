using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{

    public struct flyer
    {
        public Vector2 position;
        public float angle;
    }


    [Range(320, 1920)]
    public int textureWidth = 320;
    [Range(180,1080)]
    public int textureHeight = 180;

    public ComputeShader trailShader;
    public RenderTexture render;


    [Space]
    public int numOfFlyers = 200;
    public float speed = 10;
    public flyer[] flyers;
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
        trailShader.Dispatch(0, textureWidth, textureHeight , 1);
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
        trailShader.SetInt("width", textureWidth);
        trailShader.SetInt("height", textureHeight);
        trailShader.SetInt("numFlyers", numOfFlyers);
        trailShader.SetFloat("flySpeed", speed);
        trailShader.SetTexture(0, "Texture", render);
        trailShader.SetFloat("deltaTime", Time.deltaTime);
        trailShader.SetFloat("reduceSpeed", reductionSpeed);
    }

    void createFlyers()
    {
        flyers = new flyer[numOfFlyers];
        for (int i = 0; i < numOfFlyers; i++)
        {
            flyer temp = new flyer();
            float angle = i * Mathf.PI * 2f / numOfFlyers;
            temp.position = new Vector2(Mathf.Cos(angle) * 5 + textureWidth / 2, Mathf.Sin(angle) * 5 + textureHeight / 2);
            temp.angle = Random.Range(0, 360);
            flyers[i] = temp;
        }

        int vector3Size = sizeof(float) * 2;
        int angleSize = sizeof(float);
        int totalSize = vector3Size + angleSize;

        ComputeBuffer flyersBuffer = new ComputeBuffer(flyers.Length, totalSize);
        flyersBuffer.SetData(flyers);

        trailShader.SetBuffer(0, "flyers", flyersBuffer);
        trailShader.Dispatch(0, flyers.Length / 16, 1, 1);
    }

    
}
