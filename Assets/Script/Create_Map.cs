using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Map : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject floor;
    public GameObject Block;
    public int[,] map_array = new int[17,19];//マップ管理用二次元配列
    public GameObject[,] mapObjects = new GameObject[17, 19];
    public GameObject[,] blockobjects = new GameObject[17,19];
    public int Guard_x = 2, Guard_y = 2;//看守のxy座標
    public int Prisoner_x = 16, Prisoner_y = 14;//囚人のxy座標
    private int i, k;
    void Start()
    {
        //マップ配列の初期化
        //0は何も無い状態
        //1はブロックがある状態
        //2は看守
        //3は囚人
        for (int i = 0; i < 17; i++)
        {
            for (int k = 0; k < 19; k++)
            {
                map_array[i, k] = 0;//マップ配列の0クリア
                if (i < 2　|| i > 14)//0と１と15と16行目にブロックがあることを書き込む
                {
                    map_array[i, k] = 1;
                }
                if (k < 2 || k > 16)//0と１と17と18列めにブロックがあることを書き込む
                {
                    map_array[i, k] = 1;
                }
                if(i == 2 && k==2)//看守の初期座標を代入
                {
                    map_array[i, k] = 2;
                }
                if(i == 14 && k == 16)//囚人の初期座標を代入
                {
                    map_array[i, k] = 3;
                }
            }
                
        }
        int n = 0, m = 0;
        for (Vector3 v_1 = new Vector3(-6.0f, 0.0f, -2.3f); v_1.z <= 6; v_1.z += 0.5f)//床の生成
        {
            m = 0;
            for (Vector3 v_2 = new Vector3(-6.0f, 0.0f, v_1.z); v_2.x <= 3; v_2.x += 0.5f)
            {
                var obj = Instantiate(floor, new Vector3(v_2.x, v_2.y, v_2.z), Quaternion.identity);
                mapObjects[n, m] = obj;
            m++;
            }
            n++;
        }
        i = 0;
        k = 0;
        for (Vector3 v_1 = new Vector3(-6.0f, 0.5f, -2.3f); v_1.z <= 6; v_1.z += 0.5f)//外壁ブロックの生成
        {
            k = 0;
            for (Vector3 v_2 = new Vector3(-6.0f, 0.5f, v_1.z); v_2.x <= 3; v_2.x += 0.5f)
            {
                var obj = Instantiate(Block, new Vector3(v_2.x, v_2.y, v_2.z), Quaternion.identity);
                blockobjects[i,k] = obj;
                if(map_array[i,k] == 0 || map_array[i,k] == 2 || map_array[i , k] == 3)//空白とプレイヤーの場合ブロックを出現させない
                {
                    blockobjects[i, k].SetActive(false);
                }
                k++;
            }
            i++;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //看守の勝利判定
       switch(map_array[Prisoner_y+1,Prisoner_x] )//囚人の上方向検索
        {
            case 1://壁なら
                switch(map_array[Prisoner_y + 2, Prisoner_x])
                {
                    case 1://壁なら
                        switch(map_array[Prisoner_y , Prisoner_x + 1])//囚人の右方向
                        {
                            case 1:
                                switch (map_array[Prisoner_y, Prisoner_x + 2])
                                {
                                    case 1:
                                        switch(map_array[Prisoner_y-1, Prisoner_x ])//囚人の後ろ方向検索
                                        {
                                            case 1://壁なら
                                                switch(map_array[Prisoner_y - 2, Prisoner_x])
                                                {
                                                    case 1:
                                                        switch(map_array[Prisoner_y, Prisoner_x - 1])//囚人の左方向検索
                                                        {
                                                            case 1:
                                                                switch (map_array[Prisoner_y, Prisoner_x - 2])
                                                                {
                                                                    case 1:
                                                                        {
                                                                            //ここに看守勝利シーン繊維を書く
                                                                        }
                                                                        break;
                                                                }

                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;

                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

}
