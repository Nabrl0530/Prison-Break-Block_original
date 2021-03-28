using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Prisoner_relation : MonoBehaviour
{
    public Transform Guard_tra;
    public GameObject Block;
    private Transform mTrans;
    private Rigidbody mRigid;
    private PlayerAnimation mAnim;
    public Rigidbody dRigid;
    public Transform Front;
    public Transform Right;
    public Transform Left;
    public Transform Back;
    public Create_Map create_map;
    private int Direction = 4;//プレイヤーの向いている方向//1前2右３左４後
    private bool Stop = true;
    private bool Stop_front = true;
    private bool Stop_back = true;
    private bool Stop_left = true;
    private bool Stop_right = true;
    private bool Stop_push = true;



    // Start is called before the first frame update
    void Start()
    {
        mTrans = GetComponent<Transform>();
        mRigid = GetComponent<Rigidbody>();
        mAnim = GetComponent<PlayerAnimation>();
        State = STATE.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputMouse();

        switch (State)
        {
            case STATE.IDLE:
            case STATE.RUN:
                CheckInputMove();
                break;
        }

    }
    void FixedUpdate()
    {
        switch (State)
        {
            case STATE.IDLE:
            case STATE.RUN:
                UpdateMove();
                break;
        }
    }
    //--------
    // 状態 //
    //---------------------------------------------------------------------------------

    public enum STATE
    {
        DEFAULT = 0,
        IDLE,
        RUN,
        ROLL,
        DOWN
    }
    public STATE State { get; private set; }

    //-------------
    // 入力の監視 //
    //---------------------------------------------------------------------------------
    /// <summary>
    /// wasdの入力監視
    /// 8方向移動
    /// </summary>
    private void CheckInputMove()
    {
        Vector3 velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.RightShift) == false)//Nキーが押されてない場合
        {
            if (Stop_front == true)
            {
                if (Input.GetKey(KeyCode.UpArrow) == true)
                {
                    switch (create_map.map_array[create_map.Prisoner_y + 1, create_map.Prisoner_x])
                    {
                        case 0://何も無い（空白）
                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                            create_map.Prisoner_y++;
                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                            this.transform.LookAt(Front, Vector3.up);//前向きにする
                            Direction = 1;//前向きにである事を記録
                            Stop = false;
                            Stop_front = false;
                            Vector3 tmp = GameObject.Find("Prisoner").transform.position;
                            GameObject.Find("Prisoner").transform.position = new Vector3(tmp.x, tmp.y, tmp.z + 0.5f);
                            Vector3 tmp_2 = GameObject.Find("Prisoner_Direction").transform.position;
                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_2.x, tmp_2.y, tmp_2.z + 0.5f);
                            break;
                        case 1://壁
                            this.transform.LookAt(Front, Vector3.up);//前向きにする
                            Direction = 1;//前向きにである事を記録
                            break;
                        case 10://透明壁
                            this.transform.LookAt(Front, Vector3.up);//前向きにする
                            Direction = 1;//前向きにである事を記録
                            create_map.map_array[create_map.Prisoner_y + 1, create_map.Prisoner_x] = 1;//（壁）マップ配列を更新
                            create_map.blockobjects[create_map.Prisoner_y + 1, create_map.Prisoner_x].SetActive(true);
                            break;

                    }
                }
            }
            if (Stop_left == true)
            {
                if (Input.GetKey(KeyCode.LeftArrow) == true)
                {
                    switch (create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x - 1])
                    {
                        case 0://何も無い（空白）
                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                            create_map.Prisoner_x--;
                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                            this.transform.LookAt(Left, Vector3.up);//左向きにする
                            Direction = 3;//左向きであることを記録
                            Stop = false;
                            Stop_left = false;
                            Vector3 tmp = GameObject.Find("Prisoner").transform.position;
                            GameObject.Find("Prisoner").transform.position = new Vector3(tmp.x - 0.5f, tmp.y, tmp.z);
                            Vector3 tmp_2 = GameObject.Find("Prisoner_Direction").transform.position;
                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_2.x - 0.5f, tmp_2.y, tmp_2.z);
                            break;
                        case 1://壁
                            this.transform.LookAt(Left, Vector3.up);//左向きにする
                            Direction = 3;//左向きであることを記録
                            break;
                        case 10:
                            this.transform.LookAt(Left, Vector3.up);//左向きにする
                            Direction = 3;//左向きであることを記録
                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x - 1] = 1;//（壁）マップ配列を更新
                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x - 1].SetActive(true);
                            break;

                    }
                }
            }
            if (Stop_back == true)
            {
                if (Input.GetKey(KeyCode.DownArrow) == true)
                {
                    switch (create_map.map_array[create_map.Prisoner_y - 1, create_map.Prisoner_x])
                    {
                        case 0://何も無い（空白）
                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                            create_map.Prisoner_y--;
                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                            this.transform.LookAt(Back, Vector3.up);//後ろ向きにする
                            Direction = 4;//後ろ向きである事を記録
                            Stop = false;
                            Stop_back = false;
                            Vector3 tmp = GameObject.Find("Prisoner").transform.position;
                            GameObject.Find("Prisoner").transform.position = new Vector3(tmp.x, tmp.y, tmp.z - 0.5f);
                            Vector3 tmp_2 = GameObject.Find("Prisoner_Direction").transform.position;
                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_2.x, tmp_2.y, tmp_2.z - 0.5f);
                            break;
                        case 1://壁
                            this.transform.LookAt(Back, Vector3.up);//後ろ向きにする
                            Direction = 4;//後ろ向きである事を記録
                            break;
                        case 10:
                            this.transform.LookAt(Back, Vector3.up);//後ろ向きにする
                            Direction = 4;//後ろ向きである事を記録
                            create_map.map_array[create_map.Prisoner_y - 1, create_map.Prisoner_x] = 1;//（壁）マップ配列を更新
                            create_map.blockobjects[create_map.Prisoner_y - 1, create_map.Prisoner_x].SetActive(true);
                            break;

                    }
                }
            }
            if (Stop_right == true)
            {
                if (Input.GetKey(KeyCode.RightArrow) == true)
                {
                    switch (create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x + 1])
                    {
                        case 0://何も無い（空白）
                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                            create_map.Prisoner_x++;
                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                            this.transform.LookAt(Right, Vector3.up);//右向きにする
                            Direction = 2;//右向きである事を記録
                            Stop = false;
                            Stop_right = false;
                            Vector3 tmp = GameObject.Find("Prisoner").transform.position;
                            GameObject.Find("Prisoner").transform.position = new Vector3(tmp.x + 0.5f, tmp.y, tmp.z);
                            Vector3 tmp_2 = GameObject.Find("Prisoner_Direction").transform.position;
                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_2.x + 0.5f, tmp_2.y, tmp_2.z);
                            break;
                        case 1://壁
                            this.transform.LookAt(Right, Vector3.up);//右向きにする
                            Direction = 2;//右向きである事を記録
                            break;
                        case 10:
                            this.transform.LookAt(Right, Vector3.up);//右向きにする
                            Direction = 2;//右向きである事を記録
                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x + 1] = 1;//（壁）マップ配列を更新
                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x + 1].SetActive(true);
                            break;

                    }

                }
            }
        }
        else
        if (Input.GetKey(KeyCode.RightShift) == true)//向きだけを変える処理
        {
            if (Input.GetKey(KeyCode.UpArrow) == true)
            {
                this.transform.LookAt(Front, Vector3.up);//前向きにする
                Direction = 1;//前向きにである事を記録
            }
            if (Input.GetKey(KeyCode.LeftArrow) == true)
            {
                this.transform.LookAt(Left, Vector3.up);//左向きにする
                Direction = 3;//左向きであることを記録
            }
            if (Input.GetKey(KeyCode.DownArrow) == true)
            {
                this.transform.LookAt(Back, Vector3.up);//後ろ向きにする
                Direction = 4;//後ろ向きである事を記録
            }
            if (Input.GetKey(KeyCode.RightArrow) == true)
            {
                this.transform.LookAt(Right, Vector3.up);//右向きにする
                Direction = 2;//右向きである事を記録　
            }

        }
        if (Stop_push == true)
        {
            if (Input.GetKey(KeyCode.Return) == true)
            {
                if (Direction == 1)//前方向
                {
                    switch (create_map.map_array[create_map.Prisoner_y + 1, create_map.Prisoner_x])
                    {
                        case 0://何も無い（空白）
                            Stop_push = false;
                            break;
                        case 1://壁
                               //囚人の前方向移動
                            switch (create_map.map_array[create_map.Prisoner_y + 2, create_map.Prisoner_x])
                            {
                                case 0://何も無い
                                    create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                    create_map.Prisoner_y++;
                                    create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                    Transform mytrans = this.transform;
                                    mytrans.position = new Vector3(mytrans.position.x, mytrans.position.y, (mytrans.position.z + 0.5f));
                                    Vector3 tmp_2 = GameObject.Find("Prisoner_Direction").transform.position;
                                    GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_2.x, tmp_2.y, tmp_2.z + 0.5f);
                                    create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                    create_map.map_array[create_map.Prisoner_y + 1, create_map.Prisoner_x] = 1;//（壁）マップ配列を更新
                                    create_map.blockobjects[create_map.Prisoner_y + 1, create_map.Prisoner_x].SetActive(true);//ブロックを表示する
                                    Stop_push = false;
                                    break;
                                case 1://ブロック
                                    //エラー音
                                    break;
                                case 2://看守
                                    switch (create_map.map_array[create_map.Prisoner_y + 3, create_map.Prisoner_x])
                                    {
                                        case 0://何も無い
                                            //看守を1ます前に動かす
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                            create_map.Prisoner_y++;
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                            Transform mytrans_2 = this.transform;
                                            mytrans_2.position = new Vector3(mytrans_2.position.x, mytrans_2.position.y, (mytrans_2.position.z + 0.5f));
                                            Vector3 tmp_3 = GameObject.Find("Prisoner_Direction").transform.position;
                                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_3.x, tmp_3.y, tmp_3.z + 0.5f);
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                            create_map.map_array[create_map.Prisoner_y + 1, create_map.Prisoner_x] = 1;//（壁）マップ配列を更新
                                            create_map.blockobjects[create_map.Prisoner_y + 1, create_map.Prisoner_x].SetActive(true);//ブロックを表示する
                                            create_map.Guard_y++;//ガード座標を１増加
                                            create_map.map_array[create_map.Guard_y, create_map.Guard_x] = 2;//看守で更新
                                            Transform guard_tra = Guard_tra.transform;
                                            Guard_tra.position = new Vector3(guard_tra.position.x, guard_tra.position.y, (guard_tra.position.z + 0.5f));
                                            Vector3 tmp_gua = GameObject.Find("Guard_Direction").transform.position;
                                            GameObject.Find("Guard_Direction").transform.position = new Vector3(tmp_gua.x, tmp_gua.y, tmp_gua.z + 0.5f);
                                            Stop_push = false;
                                            break;
                                        case 1://ブロック
                                            //囚人の勝利処理
                                            var obj_gover = GameObject.Find("Guard");
                                            obj_gover.SetActive(false);
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                            create_map.Prisoner_y++;
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                            Transform mytrans_4 = this.transform;
                                            mytrans_4.position = new Vector3(mytrans_4.position.x, mytrans_4.position.y, (mytrans_4.position.z + 0.5f));
                                            Vector3 tmp_4 = GameObject.Find("Prisoner_Direction").transform.position;
                                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_4.x, tmp_4.y, tmp_4.z + 0.5f);
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                            create_map.map_array[create_map.Prisoner_y + 1, create_map.Prisoner_x] = 1;//（壁）マップ配列を更新
                                            create_map.blockobjects[create_map.Prisoner_y + 1, create_map.Prisoner_x].SetActive(true);//ブロックを表示する
                                            Stop_push = false;
                                            break;
                                        case 10://透明ブロック
                                            //ブロック表示
                                            create_map.blockobjects[create_map.Guard_y + 1, create_map.Guard_x].SetActive(true);
                                            create_map.map_array[create_map.Guard_y + 1, create_map.Guard_x] = 1;//壁に更新
                                            //囚人の勝利処理
                                            var obj_gover_2 = GameObject.Find("Guard");
                                            obj_gover_2.SetActive(false);
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                            create_map.Prisoner_y++;
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                            Transform mytrans_5 = this.transform;
                                            mytrans_5.position = new Vector3(mytrans_5.position.x, mytrans_5.position.y, (mytrans_5.position.z + 0.5f));
                                            Vector3 tmp_5 = GameObject.Find("Prisoner_Direction").transform.position;
                                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_5.x, tmp_5.y, tmp_5.z + 0.5f);
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                            create_map.map_array[create_map.Prisoner_y + 1, create_map.Prisoner_x] = 1;//（壁）マップ配列を更新
                                            create_map.blockobjects[create_map.Prisoner_y + 1, create_map.Prisoner_x].SetActive(true);//ブロックを表示する
                                            Stop_push = false;
                                            break;
                                    }
                                    break;
                                case 10://透明ブロック
                                    //ブロックを生成
                                    create_map.map_array[create_map.Prisoner_y + 2, create_map.Prisoner_x] = 1;//ブロックに更新
                                    create_map.blockobjects[create_map.Prisoner_y + 2, create_map.Prisoner_x].SetActive(true);
                                    Stop_push = false;
                                    //エラー音
                                    break;
                            }
                            break;
                        case 2://看守
                            //エラー音
                            Stop_push = false;
                            break;
                        case 10://透明壁
                            create_map.map_array[create_map.Prisoner_y + 1, create_map.Prisoner_x] = 1;//（壁）マップ配列を更新
                            create_map.blockobjects[create_map.Prisoner_y + 1, create_map.Prisoner_x].SetActive(true);
                            Stop_push = false;
                            break;

                    }
                }
                if(Direction == 2)//右向き
                {
                    switch (create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x + 1])
                    {
                        case 0://何も無い（空白）
                            Stop_push = false;
                            break;
                        case 1://壁
                               //囚人の前方向移動
                            switch (create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x + 2])
                            {
                                case 0://何も無い
                                    create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                    create_map.Prisoner_x++;
                                    create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                    Transform mytrans = this.transform;
                                    mytrans.position = new Vector3((mytrans.position.x + 0.5f), mytrans.position.y, (mytrans.position.z));
                                    Vector3 tmp_2 = GameObject.Find("Prisoner_Direction").transform.position;
                                    GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_2.x + 0.5f, tmp_2.y, tmp_2.z);
                                    create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                    create_map.map_array[create_map.Prisoner_y , create_map.Prisoner_x + 1] = 1;//（壁）マップ配列を更新
                                    create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x + 1].SetActive(true);//ブロックを表示する
                                    Stop_push = false;
                                    break;
                                case 1://ブロック
                                    //エラー音
                                    break;
                                case 2://看守
                                    switch (create_map.map_array[create_map.Prisoner_y , create_map.Prisoner_x + 3])
                                    {
                                        case 0://何も無い
                                            //看守を1ます前に動かす
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                            create_map.Prisoner_x++;
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                            Transform mytrans_2 = this.transform;
                                            mytrans_2.position = new Vector3(mytrans_2.position.x + 0.5f, mytrans_2.position.y, (mytrans_2.position.z ));
                                            Vector3 tmp_3 = GameObject.Find("Prisoner_Direction").transform.position;
                                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_3.x + 0.5f, tmp_3.y, tmp_3.z );
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                            create_map.map_array[create_map.Prisoner_y , create_map.Prisoner_x + 1] = 1;//（壁）マップ配列を更新
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x + 1].SetActive(true);//ブロックを表示する
                                            create_map.Guard_x++;//ガード座標を１増加
                                            create_map.map_array[create_map.Guard_y, create_map.Guard_x] = 2;//看守で更新
                                            Transform guard_tra = Guard_tra.transform;
                                            Guard_tra.position = new Vector3(guard_tra.position.x + 0.5f, guard_tra.position.y, (guard_tra.position.z ));
                                            Vector3 tmp_gua = GameObject.Find("Guard_Direction").transform.position;
                                            GameObject.Find("Guard_Direction").transform.position = new Vector3(tmp_gua.x + 0.5f, tmp_gua.y, tmp_gua.z);
                                            Stop_push = false;
                                            break;
                                        case 1://ブロック
                                            //囚人の勝利処理
                                            var obj_gover = GameObject.Find("Guard");
                                            obj_gover.SetActive(false);
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                            create_map.Prisoner_x++;
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                            Transform mytrans_4 = this.transform;
                                            mytrans_4.position = new Vector3(mytrans_4.position.x + 0.5f, mytrans_4.position.y, (mytrans_4.position.z ));
                                            Vector3 tmp_4 = GameObject.Find("Prisoner_Direction").transform.position;
                                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_4.x + 0.5f, tmp_4.y, tmp_4.z );
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                            create_map.map_array[create_map.Prisoner_y , create_map.Prisoner_x + 1] = 1;//（壁）マップ配列を更新
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x + 1].SetActive(true);//ブロックを表示する
                                            Stop_push = false;
                                            break;
                                        case 10://透明ブロック
                                            //ブロック表示
                                            create_map.blockobjects[create_map.Guard_y , create_map.Guard_x + 1].SetActive(true);
                                            create_map.map_array[create_map.Guard_y , create_map.Guard_x + 1] = 1;//壁に更新
                                            //囚人の勝利処理
                                            var obj_gover_2 = GameObject.Find("Guard");
                                            obj_gover_2.SetActive(false);
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                            create_map.Prisoner_x++;
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                            Transform mytrans_5 = this.transform;
                                            mytrans_5.position = new Vector3(mytrans_5.position.x + 0.5f, mytrans_5.position.y, (mytrans_5.position.z ));
                                            Vector3 tmp_5 = GameObject.Find("Prisoner_Direction").transform.position;
                                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_5.x + 0.5f, tmp_5.y, tmp_5.z );
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                            create_map.map_array[create_map.Prisoner_y , create_map.Prisoner_x + 1] = 1;//（壁）マップ配列を更新
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x + 1].SetActive(true);//ブロックを表示する
                                            Stop_push = false;
                                            break;
                                    }
                                    break;
                                case 10://透明ブロック
                                    //ブロックを生成
                                    create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x  + 2] = 1;//ブロックに更新
                                    create_map.blockobjects[create_map.Prisoner_y , create_map.Prisoner_x + 2].SetActive(true);
                                    Stop_push = false;
                                    //エラー音
                                    break;
                            }
                            break;
                        case 2://看守
                            //エラー音
                            Stop_push = false;
                            break;
                        case 10://透明壁
                            create_map.map_array[create_map.Prisoner_y , create_map.Prisoner_x + 1 ]= 1;//（壁）マップ配列を更新
                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x + 1].SetActive(true);
                            Stop_push = false;
                            break;

                    }
                }
                if (Direction == 3)//左向き
                {
                    switch (create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x - 1])
                    {
                        case 0://何も無い（空白）
                            Stop_push = false;
                            break;
                        case 1://壁
                               //囚人の前方向移動
                            switch (create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x - 2])
                            {
                                case 0://何も無い
                                    create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                    create_map.Prisoner_x--;
                                    create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                    Transform mytrans = this.transform;
                                    mytrans.position = new Vector3((mytrans.position.x - 0.5f), mytrans.position.y, (mytrans.position.z));
                                    Vector3 tmp_2 = GameObject.Find("Prisoner_Direction").transform.position;
                                    GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_2.x - 0.5f, tmp_2.y, tmp_2.z);
                                    create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                    create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x - 1] = 1;//（壁）マップ配列を更新
                                    create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x - 1].SetActive(true);//ブロックを表示する
                                    Stop_push = false;
                                    break;
                                case 1://ブロック
                                    //エラー音
                                    break;
                                case 2://看守
                                    switch (create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x - 3])
                                    {
                                        case 0://何も無い
                                            //看守を1ます前に動かす
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                            create_map.Prisoner_x--;
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                            Transform mytrans_2 = this.transform;
                                            mytrans_2.position = new Vector3(mytrans_2.position.x - 0.5f, mytrans_2.position.y, (mytrans_2.position.z));
                                            Vector3 tmp_3 = GameObject.Find("Prisoner_Direction").transform.position;
                                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_3.x - 0.5f, tmp_3.y, tmp_3.z);
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x - 1] = 1;//（壁）マップ配列を更新
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x - 1].SetActive(true);//ブロックを表示する
                                            create_map.Guard_x--;//ガード座標を１減少
                                            create_map.map_array[create_map.Guard_y, create_map.Guard_x] = 2;//看守で更新
                                            Transform guard_tra = Guard_tra.transform;
                                            Guard_tra.position = new Vector3(guard_tra.position.x - 0.5f, guard_tra.position.y, (guard_tra.position.z));
                                            Vector3 tmp_gua = GameObject.Find("Guard_Direction").transform.position;
                                            GameObject.Find("Guard_Direction").transform.position = new Vector3(tmp_gua.x - 0.5f, tmp_gua.y, tmp_gua.z);
                                            Stop_push = false;
                                            break;
                                        case 1://ブロック
                                            //囚人の勝利処理
                                            var obj_gover = GameObject.Find("Guard");
                                            obj_gover.SetActive(false);
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                            create_map.Prisoner_x--;
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                            Transform mytrans_4 = this.transform;
                                            mytrans_4.position = new Vector3(mytrans_4.position.x - 0.5f, mytrans_4.position.y, (mytrans_4.position.z));
                                            Vector3 tmp_4 = GameObject.Find("Prisoner_Direction").transform.position;
                                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_4.x - 0.5f, tmp_4.y, tmp_4.z);
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x - 1] = 1;//（壁）マップ配列を更新
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x - 1].SetActive(true);//ブロックを表示する
                                            Stop_push = false;
                                            break;
                                        case 10://透明ブロック
                                            //ブロック表示
                                            create_map.blockobjects[create_map.Guard_y, create_map.Guard_x - 1].SetActive(true);
                                            create_map.map_array[create_map.Guard_y, create_map.Guard_x - 1] = 1;//壁に更新
                                            //囚人の勝利処理
                                            var obj_gover_2 = GameObject.Find("Guard");
                                            obj_gover_2.SetActive(false);
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                            create_map.Prisoner_x--;
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                            Transform mytrans_5 = this.transform;
                                            mytrans_5.position = new Vector3(mytrans_5.position.x - 0.5f, mytrans_5.position.y, (mytrans_5.position.z));
                                            Vector3 tmp_5 = GameObject.Find("Prisoner_Direction").transform.position;
                                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_5.x - 0.5f, tmp_5.y, tmp_5.z);
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x - 1] = 1;//（壁）マップ配列を更新
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x - 1].SetActive(true);//ブロックを表示する
                                            Stop_push = false;
                                            break;
                                    }
                                    break;
                                case 10://透明ブロック
                                    //ブロックを生成
                                    create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x - 2] = 1;//ブロックに更新
                                    create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x - 2].SetActive(true);
                                    Stop_push = false;
                                    //エラー音
                                    break;
                            }
                            break;
                        case 2://看守
                            //エラー音
                            Stop_push = false;
                            break;
                        case 10://透明壁
                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x - 1] = 1;//（壁）マップ配列を更新
                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x - 1].SetActive(true);
                            Stop_push = false;
                            break;

                    }
                }
                if (Direction == 4)//後ろ向き
                {
                    switch (create_map.map_array[create_map.Prisoner_y - 1, create_map.Prisoner_x])
                    {
                        case 0://何も無い（空白）
                            Stop_push = false;
                            break;
                        case 1://壁
                               //囚人の前方向移動
                            switch (create_map.map_array[create_map.Prisoner_y - 2, create_map.Prisoner_x])
                            {
                                case 0://何も無い
                                    create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                    create_map.Prisoner_y--;
                                    create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                    Transform mytrans = this.transform;
                                    mytrans.position = new Vector3(mytrans.position.x, mytrans.position.y, (mytrans.position.z - 0.5f));
                                    Vector3 tmp_2 = GameObject.Find("Prisoner_Direction").transform.position;
                                    GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_2.x, tmp_2.y, tmp_2.z - 0.5f);
                                    create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                    create_map.map_array[create_map.Prisoner_y - 1, create_map.Prisoner_x] = 1;//（壁）マップ配列を更新
                                    create_map.blockobjects[create_map.Prisoner_y - 1, create_map.Prisoner_x].SetActive(true);//ブロックを表示する
                                    Stop_push = false;
                                    break;
                                case 1://ブロック
                                    //エラー音
                                    break;
                                case 2://看守
                                    switch (create_map.map_array[create_map.Prisoner_y - 3, create_map.Prisoner_x])
                                    {
                                        case 0://何も無い
                                            //看守を1ます前に動かす
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                            create_map.Prisoner_y--;
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                            Transform mytrans_2 = this.transform;
                                            mytrans_2.position = new Vector3(mytrans_2.position.x, mytrans_2.position.y, (mytrans_2.position.z - 0.5f));
                                            Vector3 tmp_3 = GameObject.Find("Prisoner_Direction").transform.position;
                                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_3.x, tmp_3.y, tmp_3.z - 0.5f);
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                            create_map.map_array[create_map.Prisoner_y - 1, create_map.Prisoner_x] = 1;//（壁）マップ配列を更新
                                            create_map.blockobjects[create_map.Prisoner_y - 1, create_map.Prisoner_x].SetActive(true);//ブロックを表示する
                                            create_map.Guard_y--;//ガード座標を1減少
                                            create_map.map_array[create_map.Guard_y, create_map.Guard_x] = 2;//看守で更新
                                            Transform guard_tra = Guard_tra.transform;
                                            Guard_tra.position = new Vector3(guard_tra.position.x, guard_tra.position.y, (guard_tra.position.z - 0.5f));
                                            Vector3 tmp_gua = GameObject.Find("Guard_Direction").transform.position;
                                            GameObject.Find("Guard_Direction").transform.position = new Vector3(tmp_gua.x, tmp_gua.y, tmp_gua.z - 0.5f);
                                            Stop_push = false;
                                            break;
                                        case 1://ブロック
                                            //囚人の勝利処理
                                            var obj_gover = GameObject.Find("Guard");
                                            obj_gover.SetActive(false);
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                            create_map.Prisoner_y--;
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                            Transform mytrans_4 = this.transform;
                                            mytrans_4.position = new Vector3(mytrans_4.position.x, mytrans_4.position.y, (mytrans_4.position.z - 0.5f));
                                            Vector3 tmp_4 = GameObject.Find("Prisoner_Direction").transform.position;
                                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_4.x, tmp_4.y, tmp_4.z - 0.5f);
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                            create_map.map_array[create_map.Prisoner_y - 1, create_map.Prisoner_x] = 1;//（壁）マップ配列を更新
                                            create_map.blockobjects[create_map.Prisoner_y - 1, create_map.Prisoner_x].SetActive(true);//ブロックを表示する
                                            Stop_push = false;
                                            break;
                                        case 10://透明ブロック
                                            //ブロック表示
                                            create_map.blockobjects[create_map.Guard_y - 1, create_map.Guard_x].SetActive(true);
                                            create_map.map_array[create_map.Guard_y - 1, create_map.Guard_x] = 1;//壁に更新
                                            //囚人の勝利処理
                                            var obj_gover_2 = GameObject.Find("Guard");
                                            obj_gover_2.SetActive(false);
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 0;//現在座標を空白に
                                            create_map.Prisoner_y--;
                                            create_map.map_array[create_map.Prisoner_y, create_map.Prisoner_x] = 3;//移動後座標を囚人にする
                                            Transform mytrans_5 = this.transform;
                                            mytrans_5.position = new Vector3(mytrans_5.position.x, mytrans_5.position.y, (mytrans_5.position.z - 0.5f));
                                            Vector3 tmp_5 = GameObject.Find("Prisoner_Direction").transform.position;
                                            GameObject.Find("Prisoner_Direction").transform.position = new Vector3(tmp_5.x, tmp_5.y, tmp_5.z - 0.5f);
                                            create_map.blockobjects[create_map.Prisoner_y, create_map.Prisoner_x].SetActive(false);//ブロックを非表示
                                            create_map.map_array[create_map.Prisoner_y - 1, create_map.Prisoner_x] = 1;//（壁）マップ配列を更新
                                            create_map.blockobjects[create_map.Prisoner_y - 1, create_map.Prisoner_x].SetActive(true);//ブロックを表示する
                                            Stop_push = false;
                                            break;
                                    }
                                    break;
                                case 10://透明ブロック
                                    //ブロックを生成
                                    create_map.map_array[create_map.Prisoner_y - 2, create_map.Prisoner_x] = 1;//ブロックに更新
                                    create_map.blockobjects[create_map.Prisoner_y - 2, create_map.Prisoner_x].SetActive(true);
                                    Stop_push = false;
                                    //エラー音
                                    break;
                            }
                            break;
                        case 2://看守
                            //エラー音
                            Stop_push = false;
                            break;
                        case 10://透明壁
                            create_map.map_array[create_map.Prisoner_y - 1, create_map.Prisoner_x] = 1;//（壁）マップ配列を更新
                            create_map.blockobjects[create_map.Prisoner_y - 1, create_map.Prisoner_x].SetActive(true);
                            Stop_push = false;
                            break;

                    }
                }

            }
        }
        if (Input.GetKey(KeyCode.UpArrow) == false)
        {
            Stop_front = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow) == false)
        {
            Stop_left = true;
        }
        if (Input.GetKey(KeyCode.DownArrow) == false)
        {
            Stop_back = true;
        }
        if (Input.GetKey(KeyCode.RightArrow) == false)
        {
            Stop_right = true;
        }
        if(Input.GetKey(KeyCode.Return) == false)
        {
            Stop_push = true;
        }


        //if (Input.GetKey(KeyCode.W)) velocity.z += 1.0f;
        //if (Input.GetKey(KeyCode.A)) velocity.x -= 1.0f;
        //if (Input.GetKey(KeyCode.S)) velocity.z -= 1.0f;
        //if (Input.GetKey(KeyCode.D)) velocity.x += 1.0f;

        // 移動力が0でない場合は移動の初期化
        if (velocity.magnitude > 0.0f)
        {
            OnMove(velocity);
            return;
        }

        // 移動力が0の場合は移動停止の初期化
        OffMove();
    }

    //--------
    // 移動 //
    //---------------------------------------------------------------------------------

    private const float MAX_SPEED = 4.0f;
    private const float DEC_SPEED_VALUE = 0.4f;
    private const float ROTATE_SPEED = 0.2f;

    private float mSpeed = MAX_SPEED; // 現在の速度
    private Vector3 mMoveVelocity; // 入力によって決まる移動力を一時的に保持

    /// <summary>
    /// 移動の初期化
    /// 
    /// アニメーションを開始
    /// 状態を遷移
    /// 移動力を保持
    /// </summary>
    /// <param name="velocity"></param>
    private void OnMove(Vector3 velocity)
    {
        mAnim.Play(PlayerAnimation.ANIM_ID.RUN);
        State = STATE.RUN;
        mMoveVelocity = velocity.normalized * mSpeed;
    }

    /// <summary>
    /// 移動停止の初期化
    /// 
    /// アニメーションを開始
    /// 状態を遷移
    /// 移動力を保持
    /// </summary>
    private void OffMove()
    {
        mAnim.Play(PlayerAnimation.ANIM_ID.IDLE);
        State = STATE.IDLE;
        mMoveVelocity = Vector3.zero;
    }

    /// <summary>
    /// 実際の移動処理
    /// 
    /// Rigidbodyに保持している移動力を反映する
    /// </summary>
    private void UpdateMove()
    {
        mRigid.velocity = mMoveVelocity;
        dRigid.velocity = mMoveVelocity;

        //// 移動力が0でない場合はその方向に向きを変える（スムーズに）
        //if (mMoveVelocity.magnitude > 0.0f)
        //{
        //    mRigid.angularVelocity = Vector3.zero; // （重要）これが無いと壁と接触しながら移動する際に回転力が相殺される
        //    mRigid.rotation = Quaternion.Slerp(mRigid.rotation, Quaternion.LookRotation(mMoveVelocity), ROTATE_SPEED);
        //}
        //else
        //{
        //    // 移動終了後は地面との摩擦で回転する力がかかってしまう場合があるので、それをゼロにする
        //    mRigid.angularVelocity = Vector3.zero;
        //}
    }
    private void CheckInputMouse()
    {
        if (Input.GetMouseButtonDown(1)) OnDown();
    }

    /// <summary>
    /// ダウンアニメーションの初期化
    /// </summary>
    public void OnDown()
    {
        State = STATE.DOWN;
        mRigid.velocity = Vector3.zero;
        mRigid.angularVelocity = Vector3.zero;
        mAnim.Play(PlayerAnimation.ANIM_ID.DOWN);
    }
}
