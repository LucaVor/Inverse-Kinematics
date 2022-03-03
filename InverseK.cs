using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseK : MonoBehaviour
{
    public Vector3[] bonePoints;

    public Vector3 origin = Vector3.one;
    public Vector3 target = Vector3.one - Vector3.right * 8;
    public int iterations = 1;
    public bool useStep;
    public bool step;
    public int delay = 10;
    public int count = 200;

    public Transform targetObject;

    // Start is called before the first frame update
    void Start()
    {
        bonePoints = new Vector3[count];
        Vector3 current = Vector3.zero;
        for (int i = 0; i < count; i ++)
        {
            bonePoints[i] = current;
            current += Vector3.forward * 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        target = targetObject.position;
        origin = transform.position;
        IKin();
    }

    public void IKin()
    {
        bool canReachTarget = (origin - target).magnitude <= bonePoints.Length*2;
        for (int iter = 0; iter < iterations; iter ++) {
            for (int a = 0; a < 2; a ++) {
                bonePoints[bonePoints.Length-1] = a == 0 ? target:origin;
                for (int i = bonePoints.Length-2; i >= 0; i --)
                {
                    bonePoints[i] = bonePoints[i+1]+(
                        bonePoints[i]-bonePoints[i+1]).normalized*2;
                }
                System.Array.Reverse (bonePoints);
            }

            if((bonePoints[0] - origin).magnitude < 0.01f && !canReachTarget) break;
            if((bonePoints[0] - origin).magnitude < 0.01f 
                && (bonePoints[bonePoints.Length-1] - target).magnitude < 0.01f) break;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere (bonePoints[0], 0.1f);
        for (int i = 1; i < bonePoints.Length; i++)
        {
            Gizmos.DrawSphere (bonePoints[i], 0.1f);
            Gizmos.DrawLine (bonePoints[i-1], bonePoints[i]);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere (target, 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere (origin, 0.1f);
    }
}
