using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class Main : MonoBehaviour
{
    public ComputeShader raymarching;

    RenderTexture target;
    Camera cam;
    Light lightSource;
    List<ComputeBuffer> buffersToDispose;

    [Header("Ball Plane")]
    public BallPlane plane;
    
    [Header("Fractal Propertys")]
    [Range(0, 2)] public float ShaderScale = 2.0f;
    public Vector3 ShaderOffset = new Vector3(1.0f, 1.0f, 1.0f);
    [Range(-90, 90)] public float ShaderAngle1 = 30.0f;
    [Range(-90, 90)] public float ShaderAngle2 = 0.0f;
    [Range(1, 10)] public int Shadern = 5;

    public Vector3 ColourAMix = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 ColourBMix = new Vector3(1.0f, 1.0f, 1.0f);


    public Vector3 BackGroundColourA = new Vector3(51.0f, 3.0f, 20.0f);
    public Vector3 BackGroundColourB = new Vector3(16.0f, 6.0f, 28.0f);

    [Range(0, 200)] public float darkness = 70.0f;
    [Range(0, 1)] public float blackAndWhite = 0.0f;
   
    public float ChangeSpeed;
    
    /*
    public float   ShaderScale;
    public Vector3 ShaderOffset;
    public float   ShaderAngle1;
    public float   ShaderAngle2;
    public int     Shadern;
    public Vector3 ColourAMix;
    public Vector3 ColourBMix;
    public Vector3 BackGroundColourA;
    public Vector3 BackGroundColourB;
    public float   darkness;
    public float   blackAndWhite;
    */
    static public int LevelNo;
    private int LastLevel;
    private static bool Restart;
    static private int NumOfLevels;
  
    void Init()
    {
        cam = Camera.current;
        lightSource = FindObjectOfType<Light>();
        NumOfLevels = Levels.All_levels.Count;
    }

    private void Start()
    {
        LevelNo = 0;
        LastLevel = 10;
        ChangeSpeed = 0.5f;

        ShaderScale = 0;
        ShaderOffset = new Vector3(0,0,0);
        ShaderAngle1 = 0;
        ShaderAngle2 = 0;
        Shadern = 0;
        ColourAMix = new Vector3(0,0,0);
        ColourBMix = new Vector3(0,0,0);
        BackGroundColourA = new Vector3(0,0,0);
        BackGroundColourB = new Vector3(0, 0, 0);
        darkness = 0;
        blackAndWhite = 0;
        lightSource.transform.position = new Vector3(0, 0, 0);

        plane.NewLevel();
    }

    private void Update()
    {
        ShaderScale = Mathf.Lerp(ShaderScale, Levels.All_levels[LevelNo].Scale, ChangeSpeed * Time.deltaTime);
        ShaderOffset = Vector3.Lerp(ShaderOffset, Levels.All_levels[LevelNo].Offset, ChangeSpeed * Time.deltaTime);
        ShaderAngle1 = Mathf.Lerp(ShaderAngle1, Levels.All_levels[LevelNo].Angle1, ChangeSpeed * Time.deltaTime);
        ShaderAngle2 = Mathf.Lerp(ShaderAngle2, Levels.All_levels[LevelNo].Angle2, ChangeSpeed * Time.deltaTime);
        Shadern = Levels.All_levels[LevelNo].n;
        ColourAMix = Vector3.Lerp(ColourAMix, Levels.All_levels[LevelNo].Colour1, ChangeSpeed * Time.deltaTime);
        ColourBMix = Vector3.Lerp(ColourBMix, Levels.All_levels[LevelNo].Colour2, ChangeSpeed * Time.deltaTime);
        BackGroundColourA = Vector3.Lerp(BackGroundColourA, Levels.All_levels[LevelNo].BGColour1, ChangeSpeed * Time.deltaTime);
        BackGroundColourB = Vector3.Lerp(BackGroundColourB, Levels.All_levels[LevelNo].BGColour2, ChangeSpeed * Time.deltaTime);
        darkness = Mathf.Lerp(darkness, Levels.All_levels[LevelNo].Darkness, ChangeSpeed * Time.deltaTime);
        blackAndWhite = Mathf.Lerp(blackAndWhite, Levels.All_levels[LevelNo].Black_White, ChangeSpeed * Time.deltaTime);
        lightSource.transform.position = Vector3.Lerp(lightSource.transform.position, Levels.All_levels[LevelNo].Light_Pos, ChangeSpeed * Time.deltaTime);
        if (LastLevel != LevelNo)
        {
            plane.NewLevel();
            LastLevel = LevelNo;
        }
        if (Restart)
        {
            plane.NewLevel();
            Restart = false;
        }
    }

    public static void Restart_Level()
    {
        Restart = true;
    }

    public static void Next_Level()
    {
        if (NumOfLevels != 3)
        {
            LevelNo++;
        }
    }

    public static void GameFinish()
    {
        GameUI.IsGameComplete = true;
    }


    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Init();

        InitRenderTexture();
        SetParameters();

        raymarching.SetTexture(0, "Source", source);
        raymarching.SetTexture(0, "Destination", target);

        int threadGroupsX = Mathf.CeilToInt(cam.pixelWidth / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(cam.pixelHeight / 8.0f);
        raymarching.Dispatch(0, threadGroupsX, threadGroupsY, 1);
        Graphics.Blit(target, destination);
    }


    void SetParameters()
    {
        raymarching.SetFloat("_Scale", ShaderScale);
        raymarching.SetVector("_Offset", ShaderOffset);
        raymarching.SetFloat("_Angle1", ShaderAngle1);
        raymarching.SetFloat("_Angle2", ShaderAngle2);
        raymarching.SetInt("_n", Shadern);
        raymarching.SetVector("_colourAMix", ColourAMix);
        raymarching.SetVector("_colourBMix", ColourBMix);
        raymarching.SetVector("_BackGroundColourA", BackGroundColourA);
        raymarching.SetVector("_BackGroundColourB", BackGroundColourB);
        raymarching.SetFloat("_darkness", darkness);
        raymarching.SetFloat("_blackAndWhite", blackAndWhite);
        raymarching.SetBool("_onPlaneRenderOnly", plane.IsPlaneRenderOnly());
        raymarching.SetVector("_BallCenter", plane.BallPoint);
        raymarching.SetFloat("_BallRad", plane.BallRad);
        bool lightIsDirectional = lightSource.type == LightType.Directional;
        raymarching.SetMatrix("_CameraToWorld", cam.cameraToWorldMatrix);
        raymarching.SetMatrix("_CameraInverseProjection", cam.projectionMatrix.inverse);
        raymarching.SetVector("_Light", (!lightIsDirectional) ? lightSource.transform.forward : lightSource.transform.position);
        raymarching.SetBool("positionLight", lightIsDirectional);
        raymarching.SetVector("_Plane", plane.GetPlane());
        raymarching.SetVector("_EndPoint", plane.EndPoint);
        raymarching.SetFloat("_EndRad", (plane.BallRad * 3));
    }

    void InitRenderTexture()
    {
        if (target == null || target.width != cam.pixelWidth || target.height != cam.pixelHeight)
        {
            if (target != null)
            {
                target.Release();
            }
            target = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            target.enableRandomWrite = true;
            target.Create();
        }
    }
}
