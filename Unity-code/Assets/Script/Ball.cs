using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float Gravity;
    public BallPlane plane;
    public Main main;
    private Vector3 FinalForce;
    private static bool MoveBall;

    void Start()
    {
        MoveBall = false;
        plane = gameObject.GetComponent<BallPlane>();
        main = gameObject.GetComponent<Main>();
        FinalForce = new Vector3(0, 0, 0);
        Debug.Log(Levels.All_levels[0].Scale);
    }

    float PlaneDistance(Vector3 Point)
    {
        Vector4 _Plane = plane.GetPlane();
        float temp1 = _Plane.x * Point.x + _Plane.y * Point.y + _Plane.z * Point.z + _Plane.w;
        float temp2 = Mathf.Sqrt(Mathf.Pow(_Plane.x, 2) + Mathf.Pow(_Plane.y, 2) + Mathf.Pow(_Plane.z, 2));
        return temp1 / temp2;
    }

    void Update()
    { 
        if (MoveBall)
        {
            float userForce = Input.GetAxisRaw("Horizontal")/100;
            Vector3 UserForceUltraMaxPro = -Vector3.Cross(new Vector3(0, 1, 0), plane.NormalVector).normalized * userForce;

            int n = main.Shadern;
            float max_delta_v = 0.0f;
            Vector3 np = NP(plane.BallPoint);
            for (int i = 0; i < n; i++)
            {
                float force = plane.BallRad * Gravity / n;
                force *= 0.25f;
                FinalForce.y -= force;
                plane.MoveBall(FinalForce / n);
                MarbleCollision(max_delta_v);
            }
            FinalForce += UserForceUltraMaxPro;
            plane.MoveBall(FinalForce);
            FinalForce *= Levels.All_levels[Main.LevelNo].Friction;
        }
    }

    public static void UpDateMoveBall(bool NewValue)
    {
        MoveBall = NewValue;
    }

    private bool MarbleCollision(float max_delta_v)
    {
        float de = DE(plane.BallPoint);
        if(de >= plane.BallRad)
        {
            return de < plane.BallRad * 1.5 ;
        }

        Vector3 np = NP(plane.BallPoint);
        Vector3 d = np - plane.BallPoint;
        Vector3 dn = d.normalized;

        float dv = Vector3.Dot(FinalForce, dn);
        max_delta_v = Mathf.Max(max_delta_v, dv);
        plane.MoveBall(-(dn * plane.BallRad - d));
        FinalForce -= dn * (dv * 1.2f);
        return true;

    }

    private Vector3 mengrFold(Vector3 vec)
    {
        float a = Mathf.Min(vec.x - vec.y, 0.0f);
        vec.x -= a;
        vec.y += a;
        a = Mathf.Min(vec.x - vec.z, 0.0f);
        vec.x -= a;
        vec.z += a;
        a = Mathf.Min(vec.y - vec.z, 0.0f);
        vec.y -= a;
        vec.z += a;
        return vec;
    }

    private Vector3 unMengrFold(Vector3 vec, Vector3 Point)
    {
        float mx = Mathf.Max(vec.x, vec.y);
        if (Mathf.Min(vec.x, vec.y) < Mathf.Min(mx, vec.z))
        {
            float something = Point.y;
            Point.y = Point.z;
            Point.z = something;
        }
        if (mx < vec.z)
        {
            float something = Point.x;
            Point.x = Point.z;
            Point.z = something;
        }
        if (vec.x < vec.y)
        {
            float something = Point.y;
            Point.y = Point.x;
            Point.x = something;
        }
        return Point;
    }

    private Vector3 rotZ(Vector3 vec, float angle)
    {
        float tempx = vec.x;
        float tempy = vec.y;
        vec.x = Mathf.Cos((angle)) * tempx + Mathf.Sin((angle)) * tempy;
        vec.y = Mathf.Cos((angle)) * tempy - Mathf.Sin((angle)) * tempx;

        return vec;
    }

    float de_box(Vector3 vec, Vector3 Scale)
    {
        vec.x = Mathf.Abs(vec.x);
        vec.y = Mathf.Abs(vec.y);
        vec.z = Mathf.Abs(vec.z);
        Vector3 temp = vec - Scale;
        return (Mathf.Min(Mathf.Max(Mathf.Max(temp.x, temp.y), temp.z), 0.0f) + (Vector3.Max(temp, new Vector3(0,0,0))).magnitude / Scale.x);
    }

    private Vector3 rotX(Vector3 vec, float angle)
    {
        float tempy = vec.y;
        float tempz = vec.z;
        vec.y = Mathf.Cos((angle)) * tempy + Mathf.Sin((angle)) * tempz;
        vec.z = Mathf.Cos((angle)) * tempz - Mathf.Sin((angle)) * tempy;

        return vec;
    }

    public float DE(Vector3 vec)
    {
        float planeDst = PlaneDistance(vec);
        float Angle1 = main.ShaderAngle1;
        float Angle2 = main.ShaderAngle2;
        float Scale = main.ShaderScale;
        Vector3 Offset = main.ShaderOffset;
        int n = main.Shadern;
        for (int i = 0; i < n; i++)
        {
            vec.x = Mathf.Abs(vec.x);
            vec.y = Mathf.Abs(vec.y);
            vec.z = Mathf.Abs(vec.z);
            vec = rotZ(vec, Angle2);
            vec = mengrFold(vec); 
            vec = rotX(vec, Angle1);
            vec = mengrFold(vec); 
            vec *= Scale;
            vec -= Offset;
        }
        return de_box(vec, new Vector3(6, 6, 6));
    }

    public Vector3 NP(Vector3 vec)
    {
        float Angle1 = main.ShaderAngle1;
        float Angle2 = main.ShaderAngle2;
        float Scale = main.ShaderScale;
        Vector3 Offset = main.ShaderOffset;
        int n = main.Shadern;
        List<Vector3> Hist = new List<Vector3>();
        Hist.Clear();
        for (int i = 0; i < n; i++)
        {
            Hist.Add(vec);
            vec.x = Mathf.Abs(vec.x);
            vec.y = Mathf.Abs(vec.y);
            vec.z = Mathf.Abs(vec.z);
            vec = rotZ(vec, Angle2);
            Hist.Add(vec);
            vec = mengrFold(vec);
            vec = rotX(vec, Angle1);
            Hist.Add(vec);
            vec = mengrFold(vec);
            vec *= Scale;
            vec -= Offset;
        }
        Vector3 Point = vec;
        Point.x = Mathf.Max(-6.0f, Mathf.Min(6.0f, Point.x));
        Point.y = Mathf.Max(-6.0f, Mathf.Min(6.0f, Point.y));
        Point.z = Mathf.Max(-6.0f, Mathf.Min(6.0f, Point.z));
        Vector3 temp = new Vector3();

        for (int i = 0; i < n; i++)
        {
            Point += Offset;
            Point /= Scale;
            temp = Hist[Hist.Count - 1];
            Hist.RemoveAt(Hist.Count - 1);
            Point = unMengrFold(temp, Point);
            Point = rotX(Point, -Angle1);
            temp = Hist[Hist.Count - 1];
            Hist.RemoveAt(Hist.Count - 1);
            Point = unMengrFold(temp, Point);
            Point = rotZ(Point, -Angle2);
            temp = Hist[Hist.Count - 1];
            Hist.RemoveAt(Hist.Count - 1);
            if (temp.x < 0.0f)
            {
                Point.x = -Point.x;
            }
            if (temp.y < 0.0f)
            {
                Point.y = -Point.y;
            }
            if (temp.z < 0.0f)
            {
                Point.z = -Point.z;
            }
        }
        return Point;
    }
}
