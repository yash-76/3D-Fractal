using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Levels
{
    public static List<Level> All_levels = new List<Level>();

    static Level level1 = new Level {
        Scale = 1.28f,
        Offset = new Vector3(7.0f, 0.35f, 0.35f),
        Angle1 = 0,
        Angle2 = 0,
        n = 6,
        Colour1 = new Vector3(14, -1, 4),
        Colour2 = new Vector3(-4, 4, 0),
        BGColour1 = new Vector3(180, 80, 20),
        BGColour2 = new Vector3(-150, 10, 30),
        Darkness = 30,
        Black_White = 0,
        Ball_Pos = new Vector3(0, 22, 0),
        End_Pos = new Vector3(11, 4, 11),
        Ball_Rad = 0.5f,
        Cam_Dst = 15,
        Light_Pos = new Vector3(-30, 30, -6),
        Friction = 0.8f
    };

    static Level level2 = new Level
    {
        Scale = 1.28f,
        Offset = new Vector3(2.5f, 8f, -3f),
        Angle1 = -1.6f,
        Angle2 = 0,
        n = 6,
        Colour1 = new Vector3(14, -1, 4),
        Colour2 = new Vector3(-4, 4, 0),
        BGColour1 = new Vector3(180, 80, 20),
        BGColour2 = new Vector3(-150, 10, 30),
        Darkness = 30,
        Black_White = 0,
        Ball_Pos = new Vector3(-6, 10, -6),
        End_Pos = new Vector3(6, 9, 6),
        Ball_Rad = 0.25f,
        Cam_Dst = 8,
        Light_Pos = new Vector3(24, 20, 17),
        Friction = 0.8f
    };

    static Level level3 = new Level
    {
        Scale = 1.28f,
        Offset = new Vector3(5.0f, 6.0f, 8.0f),
        Angle1 = 0f,
        Angle2 = 0,
        n = 6,
        Colour1 = new Vector3(8, 1, 0),
        Colour2 = new Vector3(0, -1, 4),
        BGColour1 = new Vector3(15, 15, 15),
        BGColour2 = new Vector3(200, 200, 200),
        Darkness = 10,
        Black_White = 0,
        Ball_Pos = new Vector3(-11, 18, -11),
        End_Pos = new Vector3(11, 15.5f, 11),
        Ball_Rad = 0.2f,
        Cam_Dst = 12,
        Light_Pos = new Vector3(-30, 30, -6),
        Friction = 0.8f
    };

    static Level level4 = new Level
    {
        Scale = 1.35f,
        Offset = new Vector3(9.0f, 13.0f, 5.0f),
        Angle1 = -0.3f,
        Angle2 = 0,
        n = 6,
        Colour1 = new Vector3(250, 237, 39),
        Colour2 = new Vector3(-225, -213.3f, -35.1f),
        BGColour1 = new Vector3(250, 237, 39),
        BGColour2 = new Vector3(200, 25, 200),
        Darkness = 30,
        Black_White = 0.3f,
        Ball_Pos = new Vector3(-13, 24, -13),
        End_Pos = new Vector3(13, 23, 13),
        Ball_Rad = 0.2f,
        Cam_Dst = 13,
        Light_Pos = new Vector3(55, 65, 20),
        Friction = 0.8f
    };

    static Levels()
    {
        All_levels.Add(level1);
        All_levels.Add(level2);
        All_levels.Add(level3);
        All_levels.Add(level4);
    }




}

public struct Level
{
    public float Scale;
    public Vector3 Offset;
    public float Angle1;
    public float Angle2;
    public int n;
    public Vector3 Colour1;
    public Vector3 Colour2;
    public Vector3 BGColour1;
    public Vector3 BGColour2;
    public float Black_White;
    public float Darkness;
    public Vector3 Ball_Pos;
    public Vector3 End_Pos;
    public float Cam_Dst;
    public float Ball_Rad;
    public Vector3 Light_Pos;
    public float Friction;
}

