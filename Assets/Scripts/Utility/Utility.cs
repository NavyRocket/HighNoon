using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI;
public class Utility 
{
    internal static float ProportionalRatio(float value, float min, float max)
    {
        if (min == max) return 0f;
        return (value - min) / (max - min);
    }

    internal static Vector3 Diffusion(Vector3 reference, float theta, float phi)
    {
        float alpha = theta + Random.Range(0f, 1f) * (phi - theta);
        float beta = 2f * Random.Range(0f, 1f) * Mathf.PI;

        float x = Mathf.Sin(alpha) * Mathf.Cos(beta);
        float y = Mathf.Sin(alpha) * Mathf.Sin(beta);
        float z = Mathf.Cos(alpha);

        Vector3 randomVector = new Vector3(x, y, z);

        Vector3 zAxis = new Vector3(0f, 0f, 1f);
        Vector3 axis = Vector3.Cross(zAxis, reference).normalized;
        float angle = Vector3.Angle(zAxis, reference);

        Quaternion rotation = Quaternion.AngleAxis(angle, axis);
        Vector3 resultVector = rotation * randomVector;

        return resultVector;

    }

    //  XMVECTOR Cone_DiffusionAngle(FXMVECTOR _vReference, _float _fTheta, _float _fPhi)
    //  {
    //      XMFLOAT3 vVector;
    //
    //      _float fAlpha = _fTheta + RandomFloat(0.f, 1.f) * (_fPhi - _fTheta);
    //      _float fBeta = 2.f * RandomFloat(0.f, 1.f) * XM_PI;
    //
    //      vVector.x = sin(fAlpha) * cos(fBeta);
    //      vVector.y = sin(fAlpha) * sin(fBeta);
    //      vVector.z = cos(fAlpha);
    //
    //      XMVECTOR vAxis = XMVector3Cross(XMVectorSet(0.f, 0.f, 1.f, 0.f), _vReference);
    //      _float fAngle = XMVectorGetX(XMVector3AngleBetweenVectors(XMVectorSet(0.f, 0.f, 1.f, 0.f), _vReference));
    //
    //      return XMVector3Transform(XMLoadFloat3(&vVector), XMMatrixRotationAxis(vAxis, fAngle));
    //  }
}
