using System.Collections;
using System.Collections.Generic;
using Sources.Scripts.Logic;
using UnityEngine;

public class TileProductScript : MonoBehaviour
{
    private DefaultTileProducter _defaultTileProducter;

    public Transform 底线;

    public Transform 加载顶端;

    public Transform 生成左线;

    public Transform 生成右线;

    public float 最大间隔 = 3f;
    
    public float 最小间隔 = 0.3f;
    
    public float 收缩 = 0.1f;

    public Transform 生成组;
    
    // Start is called before the first frame update
    void Start()
    {
        float startLine = 底线.position.y;
        Debug.Log("生成底线"+startLine);
        _defaultTileProducter = new DefaultTileProducter(startLine, startLine + 1000, 最大间隔,最小间隔, 收缩);
    }

    // Update is called once per frame
    void Update()
    {
        if (_defaultTileProducter.CurrentHigh < 加载顶端.position.y)
        {
            TileInfo tileInfo = _defaultTileProducter.Product();
            float x = 生成左线.position.x + (生成右线.position.x - 生成左线.position.x) * tileInfo.Width;
            GameObject obj=Instantiate((GameObject)Resources.Load("Prefabs/Tile/" + tileInfo.TileName),生成组);
            obj.transform.position=new Vector3(x,tileInfo.High);
            Debug.Log("生成Tile"+obj.transform.position);
        }
    }
}