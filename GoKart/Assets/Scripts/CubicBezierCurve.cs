using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class CubicBezierCurve
{
    Vector3[] controlVerts = new Vector3[4];

    public CubicBezierCurve(Vector3[] cvs) {
        Assert.IsTrue(cvs.Length == 4);
        for (int cv = 0; cv < 4; cv++)
            controlVerts[cv] = cvs[cv];
    }

    public void setControlPoint(int index, Vector3 value) {
        controlVerts[index] = value;
    }

    public Vector3 getControlPoint(int index) {
        return controlVerts[index];
    }

    public Vector3 GetPoint(float t)
    {
        Assert.IsTrue((t >= 0.0f) && (t <= 1.0f));
        float c = 1.0f - t;

        float bb0 = c * c * c;
        float bb1 = 3 * t * c * c;
        float bb2 = 3 * t * t * c;
        float bb3 = t * t * t;

        Vector3 point = controlVerts[0] * bb0 + controlVerts[1] * bb1 + controlVerts[2] * bb2 + controlVerts[3] * bb3;
        return point;
    }

    public Vector3 GetTangent(float t)
    {
        Assert.IsTrue((t >= 0.0f) && (t <= 1.0f));

        Vector3 q0 = controlVerts[0] + ((controlVerts[1] - controlVerts[0]) * t);
        Vector3 q1 = controlVerts[1] + ((controlVerts[2] - controlVerts[1]) * t);
        Vector3 q2 = controlVerts[2] + ((controlVerts[3] - controlVerts[2]) * t);

        Vector3 r0 = q0 + ((q1 - q0) * t);
        Vector3 r1 = q1 + ((q2 - q1) * t);
        Vector3 tangent = r1 - r0;
        return tangent;
    }

    public float GetClosestParam(Vector3 pos, float paramThreshold = 1e-6f) {
        return GetClosestParamRec(pos, 0.0f, 1.0f, paramThreshold);
    }

    float GetClosestParamRec(Vector3 pos, float beginT, float endT, float thresholdT) {
        float mid = (beginT + endT) / 2.0f;

        if ((endT - beginT) < thresholdT)
            return mid;

        float paramA = (beginT + mid) / 2.0f;
        float paramB = (mid + endT) / 2.0f;

        Vector3 posA = GetPoint(paramA);
        Vector3 posB = GetPoint(paramB);
        float distASq = (posA - pos).sqrMagnitude;
        float distBSq = (posB - pos).sqrMagnitude;

        if (distASq < distBSq)
            endT = mid;
        else
            beginT = mid;

        return GetClosestParamRec(pos, beginT, endT, thresholdT);
    }

}
