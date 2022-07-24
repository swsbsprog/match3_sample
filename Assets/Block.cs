using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector2Int pos;
    public SpriteRenderer sr;


    void OnMouseDown() => previousPos = Input.mousePosition;
    Vector3 previousPos;
    void OnMouseDrag()
    {
        if (Time.time < nextEnableMoveTime) return;
        Vector3 currentPos = Input.mousePosition;
        float absX = Mathf.Abs(currentPos.x - previousPos.x);
        float absY = Mathf.Abs(currentPos.y - previousPos.y);

        if (absX > absY)
        {
            if (currentPos.x > previousPos.x)
                Move(1, 0);// print("오른쪽");
            else
                Move(-1, 0);// print("왼쪽");
        }
        else if (absX < absY)
        {
            if (currentPos.y > previousPos.y)
                Move(0, 1);// print("위");
            else
                Move(0, -1);// print("아래");
        }
        previousPos = currentPos;
    }

    float nextEnableMoveTime;
    public float duration = 0.5f;
    private void Move(int moveX, int moveY)
    {
        nextEnableMoveTime = Time.time + duration;
        transform.Translate(moveX, moveY, 0);
    }
}
