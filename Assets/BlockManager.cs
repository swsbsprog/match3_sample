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
    public Dictionary<Vector2Int, Block> blockDic; //
    public BlockGenerator blockGenerator;

    float nextEnableMoveTime;
    public float duration = 0.5f;
    public Ease ease = Ease.OutSine;
    private int MaxX;
    private int MaxY;
    void Start()
    {
        blockList.Clear();
        blockList.AddRange(FindObjectsOfType<Block>());

        blockDic = blockList.ToDictionary(x => x.Pos);
        MaxX = blockGenerator.MaxX;
        MaxY = blockGenerator.MaxY;
    }

    internal void Move(Block targetBlock, int moveX, int moveY)
    {
        if (CanMove(targetBlock, moveX, moveY) == false)
            return;

        if (Time.time < nextEnableMoveTime) return;
        nextEnableMoveTime = Time.time + duration;


        StartCoroutine(MoveCo(targetBlock, moveX, moveY));
    }

    private IEnumerator MoveCo(Block targetBlock, int moveX, int moveY)
    {
        Vector2Int key = targetBlock.Pos;
        print(blockDic[key] == targetBlock);
        Vector2Int otherKey = key + new Vector2Int(moveX, moveY);
        Block otherBlock = blockDic[otherKey];
        Vector2Int targetEndPos = otherKey;
        Vector2Int otherEndPos = key;

        Move(targetBlock, targetEndPos);
        Move(otherBlock, otherEndPos);

        yield return new WaitForSeconds(duration);

        CheckMatch3();
        DestroyMatchedBlocks();
        NewBlocks();
    }

    private void NewBlocks()
    {
        for (int x = 0; x < MaxX; x++)
        {
            // 비어 있는 갯수 확인.
            int emptyCount = 0;
            for (int y = 0; y < MaxY; y++)
            {
                var checkBloc = blockDic[new Vector2Int(x, y)];
                if(checkBloc == null)
                    emptyCount++;
            }

            // 갯수만큼 위쪽에 생성.
            int newPosY = MaxY;
            for (int i = 0; i < emptyCount; i++)
            {
                int y = newPosY + i;
                var newBlock = blockGenerator.NewBlock(x, y);
                blockDic[new Vector2Int(x, y)] = newBlock;
            }
        }
    }

    private void DestroyMatchedBlocks()
    {
        matchListList.ForEach(list => list.ForEach(block => Destroy(block.gameObject)));
        matchListList.Clear();
    }

    private void CheckMatch3()
    {
        CheckMatchX();// 가로 체크

        CheckMatchY(); // 세로 체크
    }

    private void CheckMatchY()
    {
        for (int x = 0; x < MaxX; x++)
        {
            List<Block> matchList = new List<Block>();
            Block previousBlock = blockDic[new Vector2Int(x, 0)];   // 첫번째 블락 할당
            for (int y = 1; y < MaxY; y++)
            {
                Block currentBlock = blockDic[new Vector2Int(x, y)];
                // 매칭계산진행
                if (previousBlock.iconType == currentBlock.iconType) // 같은거다
                {
                    if (matchList.Count == 0)       // 기본구현리스트가 비었으면 매치 시작된 첫번째 블락 넣어야한다
                        matchList.Add(previousBlock);
                    matchList.Add(currentBlock);
                }
                else
                {
                    // 다른게 나왔다. 
                    if (matchList.Count >= 3)
                        matchListList.Add(matchList.ToList()); // 보관하자.

                    matchList.Clear();
                }

                if (matchList.Count >= 3)
                    matchListList.Add(matchList.ToList()); // 보관하자.

                previousBlock = currentBlock;
            }
        }
    }

    List<List<Block>> matchListList = new List<List<Block>>();
    private void CheckMatchX()
    {
        for (int y = 0; y < MaxY; y++)
        {
            List<Block> matchList = new List<Block>();
            Block previousBlock = blockDic[new Vector2Int(0, y)];   // 첫번째 블락 할당
            for (int x = 1; x < MaxX; x++)
            {
                Block currentBlock = blockDic[new Vector2Int(x, y)];
                // 매칭계산진행
                if(previousBlock.iconType == currentBlock.iconType) // 같은거다
                {
                    if (matchList.Count == 0)       // 기본구현리스트가 비었으면 매치 시작된 첫번째 블락 넣어야한다
                        matchList.Add(previousBlock);
                    matchList.Add(currentBlock);
                }
                else
                {
                    // 다른게 나왔다. 
                    if (matchList.Count >= 3)
                        matchListList.Add(matchList.ToList()); // 보관하자.

                    matchList.Clear();
                }

                if (matchList.Count >= 3)
                    matchListList.Add(matchList.ToList()); // 보관하자.

                previousBlock = currentBlock;
            }
        }
    }

    private bool CanMove(Block targetBlock, int moveX, int moveY)
    {
        var endPos = targetBlock.Pos + new Vector2Int(moveX, moveY);
        if (endPos.x < 0 || endPos.y < 0)
            return false;
        if (endPos.x >= MaxX || endPos.y >= MaxY)
            return false;
        return true;
    }

    private void Move(Block block, Vector2Int endPos)
    {
        block.transform.DOMove(new Vector3(endPos.x, endPos.y, 0), duration)
            .SetEase(ease);
        block.endPos = endPos;
        blockDic[endPos] = block;
    }
}
