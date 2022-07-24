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

    public List<Block> blockList = new List<Block>();
    void Start()
    {
        blockList.Clear();
        blockList.AddRange(FindObjectsOfType<Block>());

        blockDic = blockList.ToDictionary(x => x.Pos);
    }
    public Dictionary<Vector2Int, Block> blockDic; //

    float nextEnableMoveTime;
    public float duration = 0.5f;
    public Ease ease = Ease.OutSine;
    internal void Move(Block targetBlock, int moveX, int moveY)
    {
        if (Time.time < nextEnableMoveTime) return;

        nextEnableMoveTime = Time.time + duration;

        Vector2Int key = targetBlock.Pos;
        print(blockDic[key] == targetBlock);
        Vector2Int otherKey = key + new Vector2Int(moveX, moveY);
        Block otherBlock = blockDic[otherKey];
        Vector2Int targetEndPos = otherKey;
        Vector2Int otherEndPos = key;

        Move(targetBlock, targetEndPos);
        Move(otherBlock, otherEndPos);
    }

    private void Move(Block block, Vector2Int endPos)
    {
        block.transform.DOMove(new Vector3(endPos.x, endPos.y, 0), duration)
            .SetEase(ease);
    }
}
