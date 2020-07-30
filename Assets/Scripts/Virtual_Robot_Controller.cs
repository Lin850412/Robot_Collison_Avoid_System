using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virtual_Robot_Controller : MonoBehaviour
{
    //宣告J1的關節限制在負180到正180
    [Range(-180, 180)]
    public float J1;

    //宣告J2的關節限制負60到正135
    [Range(135, -60)]
    public float J2;

    //宣告J3的關節限制負170到正40
    [Range(-170,40)]
    public float J3;

    //宣告J4的關節限制負180到正180
    [Range(180, -180)]
    public float J4;

    //宣告J5的關節限制負100到正100
    [Range(100, -100)]
    public float J5;

    //宣告J6的關節限制負360到正360
    [Range(360, -360)]
    public float J6;

    //宣告向量TCP_vector
    public Vector3 TCP_vector;

    //宣告變數J1_rotation, J2_rotation, J3_rotation, J4_rotation, J5_rotation, J6_rotation，各關節的旋轉量
    float J1_rotation, J2_rotation, J3_rotation, J4_rotation, J5_rotation, J6_rotation;

    //宣告變數J1_pre, J2_pre, J3_pre, J4_pre, J5_pre, J6_pre，上一個關節座標
    float J1_pre, J2_pre, J3_pre, J4_pre, J5_pre, J6_pre;

    // Start is called before the first frame update
    void Start()
    {
        //初始化的J1-J6關節位置
        J1 = 0;
        J2 = 90;
        J3 = 0;
        J4 = 0;
        J5 = -90;
        J6 = 0;
    }


    void FixedUpdate()
    {
        //計算J1改變的角度量
        J1_rotation = J1 - J1_pre;
		//根據變化的角度量更新至模型上
        System_Parameter.RJ2.transform.Rotate(0f, -J1_rotation, 0f);
        J1_pre = J1;

        //計算J2改變的角度量
        J2_rotation = J2 - J2_pre;
		//根據變化的角度量更新至模型上
        System_Parameter.RJ3.transform.Rotate(J2_rotation, 0f, 0f);
        J2_pre = J2;

        //計算J3改變的角度量
        J3_rotation = J3 - J3_pre;
		//根據變化的角度量更新至模型上
        System_Parameter.RJ4.transform.Rotate(-J3_rotation, 0f, 0f);
        J3_pre = J3;

        //計算J4改變的角度量
        J4_rotation = J4 - J4_pre;
		//根據變化的角度量更新至模型上
        System_Parameter.RJ5.transform.Rotate(0f, 0f, -J4_rotation);
        J4_pre = J4;

        //計算J5改變的角度量
        J5_rotation = J5 - J5_pre;
		//根據變化的角度量更新至模型上
        System_Parameter.RJ6.transform.Rotate(J5_rotation, 0f, 0f);
        J5_pre = J5;

        //更新TCP座標
        TCP_vector = System_Parameter.TCP.transform.position;
    }

}

