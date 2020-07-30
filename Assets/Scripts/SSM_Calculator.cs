using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SSM_Calculator : MonoBehaviour
{
	//宣告變數St, dis_rbt2op
    public double St, dis_rbt2op;
	//宣告變數Sh, Sr, Ss, C, Zd, Zr
    public double Sh, Sr, Ss, C, Zd, Zr;
	//宣告變數Vh, Vr, Vs
    public double Vh, Vr, Vs;
	//宣告變數T0, Tr, Ts
    public double T0, Tr, Ts;
	//宣告變數rbt_speed
    public static double rbt_speed;

    void Start()
    {
        //預設參數，將隨時間以及狀態更動
        Vh = 800; //人員行走速度
        Vr = 250; //機械手臂運行速度
        Vs = 250; //機械手臂接收停止命令時的運行速度

        //固定的變數
        C = 500;  //人員與機械手臂之間的自訂安全距離
        Zd = 100; //人員在空間中的偏移
        Zr = 50;  //機械手臂在空間中的偏移
        T0 = 0;   //初始時間
        Tr = 0.5; //ethercat響應時間,接收到停止命令到實際運作停止的時間差
        Ts = 0.1; //ethercat響應時間
    }

    void FixedUpdate()
    {
        //(mm/S)為單位
        St = SSM(Vh, Vr, Vs, T0, Tr, Ts, C, Zd, Zr);

        //caculate distance 毫米,所以 *1000
        dis_rbt2op = (Vector3.Distance(System_Parameter.Operator.transform.position, System_Parameter.TCP.transform.position)) *1000;
		
		//如果最小安全距離St大於等於人員到機械手臂的距離
        if (St >= dis_rbt2op)
        {
			//機械手臂設為停機
            rbt_speed = 0;
        }
		//如果最小安全距離St*1.5大於等於人員到機械手臂的距離
        else if (St*1.5 >= dis_rbt2op)
        {
			//機械手臂設為減速
            rbt_speed = 1;
        }
		//如果最小安全距離St*1.5小於人員到機械手臂的距離
        else if (St*1.5 < dis_rbt2op)
        {
			//機械手臂設為原速
            rbt_speed = 2;
        }
    }

    private double SSM(double vh, double vr, double vs, double t0, double tr, double ts, double c, double zd, double zr)
    {
        Sh = Vh * (Tr + Ts); //人員在空間中行走的距離
        Sr = Vr * Tr;       //機械手臂停止前運行的距離
        Ss = (Vs * Ts) / 2;       //機械手臂接收停機指令到實際停機所運行的距離
        double St = Sh + Sr + Ss + C + Zd + Zr; //SSM最小保護間隔距離

        return St;
    }

}

