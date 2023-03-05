using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject poi; //������� ������
    public GameObject[] panels; //�������������� ������ ��������� �����
    public float scrollSpeed = -30f;
    public float motionMultiplier = 0.25f; //���������� ������� ������� �� �������� ������

    private float panelHeight;
    private float panelDepth; //pos.z �������

    private void Start()
    {
        panelHeight = panels[0].transform.localScale.y;
        panelDepth = panels[0].transform.position.z;

        panels[0].transform.position = new Vector3(0, 0, panelDepth);
        panels[1].transform.position = new Vector3(0, panelHeight, panelDepth);
    }

    private void Update()
    {
        float tY, tX = 0;

        tY = Time.time * scrollSpeed % panelHeight + (panelHeight * 0.5f);

        if (poi != null)
            tX = -poi.transform.position.x * motionMultiplier;

        panels[0].transform.position = new Vector3(tX, tY, panelDepth);

        if (tY >= 0)
            panels[1].transform.position = new Vector3(tX, tY - panelHeight, panelDepth);
        else
            panels[1].transform.position = new Vector3(tX, tY + panelHeight, panelDepth);
    }
}
