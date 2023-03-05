using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    //==========������� ��� ������ � �����������==========//

    /// <returns>������ ���������� GameObject � ���� ��� �������� ��������</returns>
    public static Material[] GetAllMaterials(GameObject go)
    {
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        Material[] materials = new Material[renderers.Length];
        for(int i = 0; i < materials.Length; i++)
            materials[i] = renderers[i].material;
        return materials;   
    }

    //==========������� ��� �������� ������������==========//

    /// <summary>
    /// �������� �������� ������������ ��� ������������ ����������� ����� �� ������ �����.
    /// </summary>
    /// <param name="u">����������� ������������.</param>
    /// <param name="points">������ �����.</param>
    /// <param name="iLeft">��������� �� ������ ������� � ������.</param>
    /// <param name="iRight">��������� �� ��������� ������� � ������.</param>
    /// <returns></returns>
    public static Vector3 BezierRecursive(float u, List<Vector3> points, int iLeft = 0, int iRight = -1)
    {
        if (iRight == -1)
            iRight = points.Count - 1;

        Vector3 p0, p1, p01;

        if (iRight - iLeft != 1)
        {
            p0 = BezierRecursive(u, points, iLeft, iRight - 1);
            p1 = BezierRecursive(u, points, iLeft + 1, iRight);
        }
        else
        {
            p0 = points[iLeft];
            p1 = points[iRight];
        }

        p01 = (1 - u) * p0 + u * p1;
        return p01;
    }

    /// <summary>
    /// �������� �������� ������������ ��� ������������ ����������� ����� �� ������ �����.
    /// </summary>
    /// <param name="u">����������� ������������.</param>
    /// <param name="points">������ �����.</param>
    /// <returns></returns>
    public static Vector3 BezierRecursive(float u, params Vector3[] points)
    {
        Vector3 p01 = BezierRecursive(u, new List<Vector3>(points));
        return p01;
    }


}
