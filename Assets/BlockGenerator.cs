using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    public List<Sprite> sprites;

    //블럭 자동 생성
    public Block baseBlock;
    public int MaxX = 7;
    public int MaxY = 5;

    [ContextMenu("생성")]
    public void GenerateBlocks()
    {
        DestroyExistBlocks();

        for (int y = 0; y < MaxY; y++)
        {
            for (int x = 0; x < MaxX; x++)
            {
                Block newBlock = Instantiate(baseBlock);
                //newBlock.pos = new Vector2Int(x, y);
                newBlock.transform.position = new Vector3(x, y, 0);
                newBlock.GetComponent<SpriteRenderer>().sprite =
                    sprites[Random.Range(0, sprites.Count)];
            }
        }
    }

    private void DestroyExistBlocks()
    {
        //FindObjectOfType<Block>()// 1개만 가져옴
        Block[] existBlocks = FindObjectsOfType<Block>(); // 전부다 가져옴
        foreach (var item in existBlocks)
        {
            if (Application.isPlaying)
                Destroy(item.gameObject); // 플레이 중일때만 가능
            else
                DestroyImmediate(item.gameObject); // 플레이중이 아닐때도 가능.
        }
    }
}
