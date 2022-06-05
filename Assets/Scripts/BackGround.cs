using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float Speed;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;

    void Start()
    {
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        manager.isScroll = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("GameManager").GetComponent<GameManager>().isScroll == false)
        {
            Vector3 curPos = transform.position;
            Vector3 nextPos = Vector3.down * Speed * Time.deltaTime;
            transform.position = curPos + nextPos;

            if (sprites[endIndex].position.y < -10)
            {
                Vector3 backSrptiePos = sprites[startIndex].localPosition;
                Vector3 frontSpritePos = sprites[endIndex].localPosition;
                sprites[endIndex].transform.localPosition = backSrptiePos + Vector3.up * 10;

                int startIndexSave = startIndex;
                startIndex = endIndex;
                endIndex = (startIndexSave - 1) == -1 ? sprites.Length - 1 : startIndexSave - 1;
            }
        }
    }
}
