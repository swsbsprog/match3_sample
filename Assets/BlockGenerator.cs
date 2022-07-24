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
}
