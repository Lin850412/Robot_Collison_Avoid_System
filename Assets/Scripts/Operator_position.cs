using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Opc.UaFx.Client;
using System;
using System.Threading;

public class Operator_position : MonoBehaviour
{
	//宣告變數x_movedis, z_movedis, movedis, move_speed
    double x_movedis, z_movedis, movedis, move_speed;
	//宣告變數time
    public double time;
	//宣告變數x_var, z_var
    public float x_var, z_var;
	//宣告變數x_var_pre, z_var_pre
    float x_var_pre, z_var_pre;

    private void FixedUpdate()
    {
        DateTime time_start = DateTime.Now;

        //更新座標用於移動
        Vector3 move = System_Parameter.Operator.transform.position;
		//透過x_var,z_var更新最新的座標
        move = new Vector3(x_var * 0.01f, 1.5f, z_var * 0.01f);
		//將座標附加至實體模型
        System_Parameter.Operator.transform.position = move;

        //紀錄當前X軸的座標
        x_var_pre = x_var;
		//紀錄當前Z軸的座標
        z_var_pre = z_var;

        DateTime time_end = DateTime.Now;
        time = ((TimeSpan)(time_end - time_start)).TotalMilliseconds;
    }
}
