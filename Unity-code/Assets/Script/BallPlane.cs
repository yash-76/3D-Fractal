using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPlane : MonoBehaviour
{
    public Camera cam;
    public Transform camTransform;
    public Vector3 BallPoint = new Vector3(0.0f, 0.0f, 0.0f);
    public float BallRad;
    public Vector3 NormalVector = new Vector3(1.0f, 0.0f, 0.0f);
    public float CamDst = 24;
    public float offsetDst = 10;
    public bool RenderOnPlane = false;
    public float camSpeed = 20;
    public Vector3 EndPoint;
    private Vector3 LastPos;


    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    public Vector3 GetBallPos()
    {
        return BallPoint;
    }

    public void NewLevel()
    {
        RenderOnPlane = false;
        Ball.UpDateMoveBall(false);
        EndPoint = Levels.All_levels[Main.LevelNo].End_Pos;
        BallPoint = Levels.All_levels[Main.LevelNo].Ball_Pos;
        print(Main.LevelNo);
        BallRad = Levels.All_levels[Main.LevelNo].Ball_Rad;
    }

    private void Update()
    {
        if(BallPoint.y <= -100)
        {
            GameUI.IsGameOver = true;
        }
        if (Vector3.Distance(EndPoint, BallPoint) < BallRad * 3)
        {
            if(Main.LevelNo != 3)
            {
                GameUI.LevelOver = true;
            }
            else
            {
                Main.GameFinish();
                Ball.UpDateMoveBall(false);
            }
        }
        CamDst = Levels.All_levels[Main.LevelNo].Cam_Dst;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RenderOnPlane = !RenderOnPlane;
            if (RenderOnPlane)
            {
                Ball.UpDateMoveBall(true);
            }
            else
            {
                Ball.UpDateMoveBall(false);
            }
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            RenderOnPlane = true;
            float userInput = Input.GetAxisRaw("Horizontal");
            Ball.UpDateMoveBall(false);
            NormalVector.z += userInput/50;
            CamDst = Levels.All_levels[Main.LevelNo].Cam_Dst * 3;

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Ball.UpDateMoveBall(true);
        }
    

        if (RenderOnPlane)
        {
            Vector3 MovePosition = NormalVector.normalized * CamDst;
            camTransform.position = Vector3.MoveTowards(camTransform.position, BallPoint + (NormalVector.normalized * CamDst), camSpeed * Time.deltaTime);
            camTransform.LookAt(new Vector3(BallPoint.x, BallPoint.y, BallPoint.z));
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                LastPos = cam.ScreenToViewportPoint(Input.mousePosition);
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 dst = LastPos - cam.ScreenToViewportPoint(Input.mousePosition);

                cam.transform.position = new Vector3(0,0,0);
                cam.transform.Rotate(new Vector3(1, 0, 0), dst.y * 180);
                cam.transform.Rotate(new Vector3(0, 1, 0), -dst.x * 180, Space.World);
                cam.transform.Translate(new Vector3(0, 0, -(Levels.All_levels[Main.LevelNo].Cam_Dst * 5)));

                LastPos = cam.ScreenToViewportPoint(Input.mousePosition);
            }
            camTransform.LookAt(new Vector3(0, 0, 0));
    
        }


    }
 
    public Vector4 GetPlane()
    {
        Vector4 plan = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        plan.x = NormalVector.x;
        plan.y = NormalVector.y;
        plan.z = NormalVector.z;
        plan.w = -((BallPoint.x * NormalVector.x) + (BallPoint.y * NormalVector.y) + (BallPoint.z * NormalVector.z));
        return plan;
    }


    public bool IsPlaneRenderOnly()
    {
        return RenderOnPlane;
    }

    public void MoveBall(Vector3 dir)
    {
        BallPoint += dir;
    }
}
