using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DG.Tweening;

public class BlockManager : MonoBehaviour
{
    static public BlockManager instance;
    private void Awake() => instance = this;
    private void OnDestroy() => instance = null;

    public List<Block> blocks = new List<Block>();
    void Start()
    {
        blocks.Clear();
        blocks.AddRange(FindObjectsOfType<Block>());

        blockMap = blocks.ToDictionary(x => x.Pos);
    }
    public Dictionary<Vector2Int, Block> blockMap;

    float nextEnableMoveTime;
    public float duration = 0.5f;
    public Ease ease = Ease.OutSine;
    internal void Move(Block block, int moveX, int moveY)
    {
        if (Time.time < nextEnableMoveTime) return;

        nextEnableMoveTime = Time.time + duration;
        var newPos = block.transform.position;
        newPos.x += moveX;
        newPos.y += moveY;
        block.transform.DOMove(newPos, duration)
            .SetEase(ease);
    }
}
