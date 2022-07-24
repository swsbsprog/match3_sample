﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public SpriteRenderer sr;
    public Vector2Int Pos
    {
        get
        {
            Vector2Int intPos = new Vector2Int(
                (int)MathF.Round(transform.position.x), // 1.1 ~ 1.9 -> 1 소수점 자르기,  1.9 -> 2
                (int)MathF.Round(transform.position.y)
                );
            return intPos;
        }
    }
    void OnMouseDown() => previousPos = Input.mousePosition;
    Vector3 previousPos;
    void OnMouseDrag()
    {
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

    private void Move(int moveX, int moveY)
    {
        BlockManager.instance.Move(this, moveX, moveY);
    }
}
