using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Guard_relation : MonoBehaviour
{
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
    private int Direction = 1;//プレイヤーの向いている方向//1前2右３左４後
    private bool Stop = true;
    private bool Stop_front = true;
    private bool Stop_back = true;
    private bool Stop_left = true;
    private bool Stop_right = true;
    private bool Clear_Block_Key_NPush = true;//押されていない場合真
    private bool Space_Key_NPush = true;//押されていない場合真
    private bool Clear_Block = false;//真の時に生成するブロックを透明ブロックにする



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
        if (Input.GetKey(KeyCode.N) == false)//Nキーが押されてない場合
        {
            if (Stop_front == true)
            {
                if (Input.GetKey(KeyCode.W) == true)
                {
                    switch (create_map.map_array[create_map.Guard_y + 1, create_map.Guard_x])
                    {
                        case 0://何も無い（空白）
                            create_map.map_array[create_map.Guard_y, create_map.Guard_x] = 0;//現在座標を空白に
                            create_map.Guard_y++;
                            create_map.map_array[create_map.Guard_y, create_map.Guard_x] = 2;//移動後座標を看守にする
                            this.transform.LookAt(Front, Vector3.up);//前向きにする
                            Direction = 1;//前向きにである事を記録
                            Stop = false;
                            Stop_front = false;
                            Vector3 tmp = GameObject.Find("Guard").transform.position;
                            GameObject.Find("Guard").transform.position = new Vector3(tmp.x, tmp.y, tmp.z + 0.5f);
                            Vector3 tmp_2 = GameObject.Find("Guard_Direction").transform.position;
                            GameObject.Find("Guard_Direction").transform.position = new Vector3(tmp_2.x, tmp_2.y, tmp_2.z + 0.5f);
                            break;
                        case 1://壁
                            this.transform.LookAt(Front, Vector3.up);//前向きにする
                            Direction = 1;//前向きにである事を記録
                            break;
                        case 3://囚人
                            this.transform.LookAt(Front, Vector3.up);//前向きにする
                            Direction = 1;//前向きにである事を記録
                            break;
                        case 10://透明壁
                            this.transform.LookAt(Front, Vector3.up);//前向きにする
                            Direction = 1;//前向きにである事を記録
                            create_map.map_array[create_map.Guard_y + 1, create_map.Guard_x] = 1;//（壁）マップ配列を更新
                            create_map.blockobjects[create_map.Guard_y + 1, create_map.Guard_x].SetActive(true);
                            break;

                    }
                }
            }
            if (Stop_left == true)
            {
                if (Input.GetKey(KeyCode.A) == true)
                {
                    switch (create_map.map_array[create_map.Guard_y, create_map.Guard_x - 1])
                    {
                        case 0://何も無い（空白）
                            create_map.map_array[create_map.Guard_y, create_map.Guard_x] = 0;//現在座標を空白に
                            create_map.Guard_x--;
                            create_map.map_array[create_map.Guard_y, create_map.Guard_x] = 2;//移動後座標を看守にする
                            this.transform.LookAt(Left, Vector3.up);//左向きにする
                            Direction = 3;//左向きであることを記録
                            Stop = false;
                            Stop_left = false;
                            Vector3 tmp = GameObject.Find("Guard").transform.position;
                            GameObject.Find("Guard").transform.position = new Vector3(tmp.x - 0.5f, tmp.y, tmp.z);
                            Vector3 tmp_2 = GameObject.Find("Guard_Direction").transform.position;
                            GameObject.Find("Guard_Direction").transform.position = new Vector3(tmp_2.x - 0.5f, tmp_2.y, tmp_2.z);
                            break;
                        case 1://壁
                            this.transform.LookAt(Left, Vector3.up);//左向きにする
                            Direction = 3;//左向きであることを記録
                            break;
                        case 3://囚人
                            this.transform.LookAt(Left, Vector3.up);//左向きにする
                            Direction = 3;//左向きであることを記録
                            break;
                        case 10:
                            this.transform.LookAt(Left, Vector3.up);//左向きにする
                            Direction = 3;//左向きであることを記録
                            create_map.map_array[create_map.Guard_y, create_map.Guard_x - 1] = 1;//（壁）マップ配列を更新
                            create_map.blockobjects[create_map.Guard_y, create_map.Guard_x - 1].SetActive(true);
                            break;

                    }
                }
            }
            if (Stop_back == true)
            {
                if (Input.GetKey(KeyCode.S) == true)
                {
                    switch (create_map.map_array[create_map.Guard_y - 1, create_map.Guard_x])
                    {
                        case 0://何も無い（空白）
                            create_map.map_array[create_map.Guard_y, create_map.Guard_x] = 0;//現在座標を空白に
                            create_map.Guard_y--;
                            create_map.map_array[create_map.Guard_y, create_map.Guard_x] = 2;//移動後座標を看守にする
                            this.transform.LookAt(Back, Vector3.up);//後ろ向きにする
                            Direction = 4;//後ろ向きである事を記録
                            Stop = false;
                            Stop_back = false;
                            Vector3 tmp = GameObject.Find("Guard").transform.position;
                            GameObject.Find("Guard").transform.position = new Vector3(tmp.x, tmp.y, tmp.z - 0.5f);
                            Vector3 tmp_2 = GameObject.Find("Guard_Direction").transform.position;
                            GameObject.Find("Guard_Direction").transform.position = new Vector3(tmp_2.x, tmp_2.y, tmp_2.z - 0.5f);
                            break;
                        case 1://壁
                            this.transform.LookAt(Back, Vector3.up);//後ろ向きにする
                            Direction = 4;//後ろ向きである事を記録
                            break;
                        case 3://囚人
                            this.transform.LookAt(Back, Vector3.up);//後ろ向きにする
                            Direction = 4;//後ろ向きである事を記録
                            break;
                        case 10:
                            this.transform.LookAt(Back, Vector3.up);//後ろ向きにする
                            Direction = 4;//後ろ向きである事を記録
                            create_map.map_array[create_map.Guard_y - 1, create_map.Guard_x] = 1;//（壁）マップ配列を更新
                            create_map.blockobjects[create_map.Guard_y - 1, create_map.Guard_x].SetActive(true);
                            break;

                    }
                }
            }
            if (Stop_right == true)
            {
                if (Input.GetKey(KeyCode.D) == true)
                {
                    switch (create_map.map_array[create_map.Guard_y, create_map.Guard_x + 1])
                    {
                        case 0://何も無い（空白）
                            create_map.map_array[create_map.Guard_y, create_map.Guard_x] = 0;//現在座標を空白に
                            create_map.Guard_x++;
                            create_map.map_array[create_map.Guard_y, create_map.Guard_x] = 2;//移動後座標を看守にする
                            this.transform.LookAt(Right, Vector3.up);//右向きにする
                            Direction = 2;//右向きである事を記録
                            Stop = false;
                            Stop_right = false;
                            Vector3 tmp = GameObject.Find("Guard").transform.position;
                            GameObject.Find("Guard").transform.position = new Vector3(tmp.x + 0.5f, tmp.y, tmp.z);
                            Vector3 tmp_2 = GameObject.Find("Guard_Direction").transform.position;
                            GameObject.Find("Guard_Direction").transform.position = new Vector3(tmp_2.x + 0.5f, tmp_2.y, tmp_2.z);
                            break;
                        case 1://壁
                            this.transform.LookAt(Right, Vector3.up);//右向きにする
                            Direction = 2;//右向きである事を記録
                            break;
                        case 3://囚人
                            this.transform.LookAt(Right, Vector3.up);//右向きにする
                            Direction = 2;//右向きである事を記録
                            break;
                        case 10:
                            this.transform.LookAt(Right, Vector3.up);//右向きにする
                            Direction = 2;//右向きである事を記録
                            create_map.map_array[create_map.Guard_y, create_map.Guard_x + 1] = 1;//（壁）マップ配列を更新
                            create_map.blockobjects[create_map.Guard_y, create_map.Guard_x + 1].SetActive(true);
                            break;

                    }

                }
            }
           
        }
        else
        if (Input.GetKey(KeyCode.N) == true)//向きだけを変える処理
        {
            if (Input.GetKey(KeyCode.W) == true)
            {
                this.transform.LookAt(Front, Vector3.up);//前向きにする
                Direction = 1;//前向きにである事を記録
            }
            if (Input.GetKey(KeyCode.A) == true)
            {
                this.transform.LookAt(Left, Vector3.up);//左向きにする
                Direction = 3;//左向きであることを記録
            }
            if (Input.GetKey(KeyCode.S) == true)
            {
                this.transform.LookAt(Back, Vector3.up);//後ろ向きにする
                Direction = 4;//後ろ向きである事を記録
            }
            if (Input.GetKey(KeyCode.D) == true)
            {
                this.transform.LookAt(Right, Vector3.up);//右向きにする
                Direction = 2;//右向きである事を記録　
            }

        }
        if (Clear_Block_Key_NPush == true)
        {
            if (Input.GetKey(KeyCode.Z) == true)//透明ブロックに切り替え処理
            {
                if (Clear_Block == false)
                {
                    Clear_Block = true;
                }
                else
                {
                    Clear_Block = false;
                }
                Clear_Block_Key_NPush = false;
            }
        }
        if (Space_Key_NPush == true)
        {
            if (Input.GetKey(KeyCode.Space) == true)
            {
                switch (Direction)//向いてる方向で分岐処理
                {
                    case 1://前向きだった場合
                        switch (create_map.map_array[create_map.Guard_y + 1, create_map.Guard_x])//ひとつ上の配列を参照
                        {
                            case 0://空白だった場合
                                if (Clear_Block == false)
                                {
                                    create_map.map_array[create_map.Guard_y + 1, create_map.Guard_x] = 1;//（壁）マップ配列を更新
                                    create_map.blockobjects[create_map.Guard_y + 1, create_map.Guard_x].SetActive(true);
                                    Space_Key_NPush = false;
                                }
                                else
                                {
                                    create_map.map_array[create_map.Guard_y + 1, create_map.Guard_x] = 10;//（透明ブロック）マップ配列を更新
                                    Space_Key_NPush = false;
                                }
                                break;
                            case 1://壁だった場合
                                   //エラー音を鳴らす
                                break;
                            case 3://囚人だった場合
                                   //エラー音を鳴らす
                                break;
                            case 10://透明壁
                                    //エラー音鳴らす？
                                break;

                        }
                        break;
                    case 2://右向きだった場合
                        switch (create_map.map_array[create_map.Guard_y, create_map.Guard_x + 1])//ひとつ上の配列を参照
                        {
                            case 0://空白だった場合
                                if (Clear_Block == false)
                                {
                                    create_map.map_array[create_map.Guard_y, create_map.Guard_x + 1] = 1;//（壁）マップ配列を更新
                                    create_map.blockobjects[create_map.Guard_y, create_map.Guard_x + 1].SetActive(true);
                                    Space_Key_NPush = false;
                                }
                                else
                                {
                                    create_map.map_array[create_map.Guard_y, create_map.Guard_x + 1] = 10;//（透明ブロック）マップ配列を更新
                                    Space_Key_NPush = false;
                                }
                                break;
                            case 1://壁だった場合
                                   //エラー音を鳴らす
                                break;
                            case 3://囚人だった場合
                                   //エラー音を鳴らす
                                break;
                            case 10://透明壁
                                    //エラー音鳴らす？
                                break;
                        }
                        break;
                    case 3://左向きだった場合
                        switch (create_map.map_array[create_map.Guard_y, create_map.Guard_x - 1])//ひとつ上の配列を参照
                        {
                            case 0://空白だった場合
                                if (Clear_Block == false)
                                {
                                    create_map.map_array[create_map.Guard_y, create_map.Guard_x - 1] = 1;//（壁）マップ配列を更新
                                    create_map.blockobjects[create_map.Guard_y, create_map.Guard_x - 1].SetActive(true);
                                    Space_Key_NPush = false;
                                }
                                else
                                {
                                    create_map.map_array[create_map.Guard_y, create_map.Guard_x - 1] = 10;//（透明ブロック）マップ配列を更新
                                    Space_Key_NPush = false;
                                }
                                break;
                            case 1://壁だった場合
                                   //エラー音を鳴らす
                                break;
                            case 3://囚人だった場合
                                   //エラー音を鳴らす
                                break;
                            case 10://透明壁
                                    //エラー音鳴らす？
                                break;
                        }
                        break;
                    case 4://後ろ向きだった場合
                        switch (create_map.map_array[create_map.Guard_y - 1, create_map.Guard_x])//ひとつ上の配列を参照
                        {
                            case 0://空白だった場合
                                if (Clear_Block == false)
                                {
                                    create_map.map_array[create_map.Guard_y - 1, create_map.Guard_x] = 1;//（壁）マップ配列を更新
                                    create_map.blockobjects[create_map.Guard_y - 1, create_map.Guard_x].SetActive(true);
                                    Space_Key_NPush = false;
                                }
                                else
                                {
                                    create_map.map_array[create_map.Guard_y - 1, create_map.Guard_x] = 10;//（透明ブロック）マップ配列を更新
                                    Space_Key_NPush = false;
                                }
                                break;
                            case 1://壁だった場合
                                   //エラー音を鳴らす
                                break;
                            case 3://囚人だった場合
                                   //エラー音を鳴らす
                                break;
                            case 10://透明壁
                                    //エラー音鳴らす？
                                break;
                        }
                        break;
                }
            }
        }
        if (Input.GetKey(KeyCode.W) == false)
        {
            Stop_front = true;
        }
        if(Input.GetKey(KeyCode.A) == false)
        {
            Stop_left = true;
        }
        if(Input.GetKey(KeyCode.S) == false)
        {
            Stop_back = true;
        }
        if(Input.GetKey(KeyCode.D) == false)
        {
            Stop_right = true;
        }
        if (Input.GetKey(KeyCode.Space) == false)
        {
            Space_Key_NPush = true;
        }
        if(Input.GetKey(KeyCode.Z) == false)
        {
            Clear_Block_Key_NPush = true;
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

    private void OnDisable()
    {
        Debug.Log("aa");
        //囚人の勝利シーン遷移を書く
    }
}
