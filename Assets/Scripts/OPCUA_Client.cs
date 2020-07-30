using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel;

using UnityEngine;
using Opc.UaFx.Client;

public class OPCUA_Client: MonoBehaviour
{
	//實體化一個叫做bgwThread_rbt的BackgroundWorker
    BackgroundWorker bgwThread_rbt = new BackgroundWorker();
	//實體化一個叫做bgwThread_op的BackgroundWorker
    BackgroundWorker bgwThread_op = new BackgroundWorker();

    //實體化並賦予opc.tcp ip 位址opc.tcp://192.168.0.11:4830
    public OpcClient OpcUaClient = new OpcClient("opc.tcp://192.168.0.11:4830");
	//宣告變數Time,Time_2,operator_speed
    public double BGW_Time, BGW2_Time, engineer_speed;
	//宣告變數J1_opc,J2_opc,J3_opc,J4_opc,J5_opc,J6_opc
    string J1_opc, J2_opc, J3_opc, J4_opc, J5_opc, J6_opc;
	//宣告變數pos_a_x_var, pos_a_y_var, machine_vision_speed
    string pos_a_x_var, pos_a_y_var, machine_vision_speed;
	//宣告變數Vector3 operator_position, operator_position_pre, operator_position_dif
    public Vector3 engineer_position, engineer_position_pre, engineer_position_dif;
    public double collision_detection_result;

    private void Awake()
    {
        {
            //將R700實體模型賦予
            System_Parameter.R700 = GameObject.Find("R700");
			//將Operator實體模型賦予
            System_Parameter.Operator = GameObject.Find("Operator");
			//將TCP實體模型賦予
            System_Parameter.TCP = GameObject.Find("TCP");

            try
            {
                //opcua進行連線
                OpcUaClient.Connect();
                Debug.Log("Sucess");
            }
            catch (System.Exception)
            {
                Debug.Log("OPCUA Connection Fail");
                throw;
            }
   
        }
    }

    private void Start()
    {
		//開啟bgwThread_rbt中的屬性WorkerReportsProgress
        bgwThread_rbt.WorkerReportsProgress = true;
        //bgwThread_rbt.WorkerSupportsCancellation = true;
		//將bgwThread_rbt_DoWork事件加載到DoWorkEventHandler
        bgwThread_rbt.DoWork += new DoWorkEventHandler(bgwThread_rbt_DoWork);
        //bgwThread_rbt.ProgressChanged += new ProgressChangedEventHandler(bgwThread_rbt_ProgressChanged);
		//將bgwThread_rbt_RunWorkerCompleted事件加載到RunWorkerCompletedEventHandler
        bgwThread_rbt.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwThread_rbt_RunWorkerCompleted);

		//開啟bgwThread_op中的屬性WorkerReportsProgress
        bgwThread_op.WorkerReportsProgress = true;
        //bgwThread_op.WorkerSupportsCancellation = true;
		//將bgwThread_op_DoWork事件加載到DoWorkEventHandler
        bgwThread_op.DoWork += new DoWorkEventHandler(bgwThread_op_DoWork);
        //bgwThread_op.ProgressChanged += new ProgressChangedEventHandler(bgwThread_op_ProgressChanged);
		//將bgwThread_op_RunWorkerCompleted事件加載到RunWorkerCompletedEventHandler
        bgwThread_op.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwThread_op_RunWorkerCompleted);

        //bgwThread_rbt開啟，執行bgwThread_rbt_DoWork
        bgwThread_rbt.RunWorkerAsync();
		//bgwThread_op開啟，執行bgwThread_op_DoWork
        bgwThread_op.RunWorkerAsync();
    }

    private void bgwThread_rbt_DoWork(object sender, DoWorkEventArgs e)
    {
        DateTime time_start = DateTime.Now;

        try
        {
            //自opcua server獲取各關節的value
			//自node id("ns=2;s=3"),獲取J1_opc的數值
            J1_opc = OpcUaClient.ReadNode("ns=2;s=3").Value.ToString();
			//自node id("ns=2;s=4"),獲取J2_opc的數值
            J2_opc = OpcUaClient.ReadNode("ns=2;s=4").Value.ToString();
			//自node id("ns=2;s=5"),獲取J3_opc的數值
            J3_opc = OpcUaClient.ReadNode("ns=2;s=5").Value.ToString();
			//自node id("ns=2;s=6"),獲取J4_opc的數值
            J4_opc = OpcUaClient.ReadNode("ns=2;s=6").Value.ToString();
			//自node id("ns=2;s=7"),獲取J5_opc的數值
            J5_opc = OpcUaClient.ReadNode("ns=2;s=7").Value.ToString();
			//自node id("ns=2;s=8"),獲取J6_opc的數值
            J6_opc = OpcUaClient.ReadNode("ns=2;s=8").Value.ToString();
        }
        catch (Exception)
        {
            throw;
        }

        DateTime time_end = DateTime.Now;
        BGW_Time = ((TimeSpan)(time_end - time_start)).TotalMilliseconds;
		
		//觸發bgwThread_rbt_ProgressChanged
        bgwThread_rbt.ReportProgress(1);
    }

    private void bgwThread_rbt_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
		//do nothing
    }

    private void bgwThread_rbt_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        try
        {
            //將J1_opc存取至Virtual_Robot_Controller腳本中的J1變數
            System_Parameter.R700.GetComponent<Virtual_Robot_Controller>().J1 = float.Parse(J1_opc);
			//將J2_opc存取至Virtual_Robot_Controller腳本中的J2變數
            System_Parameter.R700.GetComponent<Virtual_Robot_Controller>().J2 = float.Parse(J2_opc);
			//將J3_opc存取至Virtual_Robot_Controller腳本中的J3變數
            System_Parameter.R700.GetComponent<Virtual_Robot_Controller>().J3 = float.Parse(J3_opc);
			//將J4_opc存取至Virtual_Robot_Controller腳本中的J4變數
            System_Parameter.R700.GetComponent<Virtual_Robot_Controller>().J4 = float.Parse(J4_opc);
			//將J5_opc存取至Virtual_Robot_Controller腳本中的J5變數
            System_Parameter.R700.GetComponent<Virtual_Robot_Controller>().J5 = float.Parse(J5_opc);
			//將J6_opc存取至Virtual_Robot_Controller腳本中的J6變數
            System_Parameter.R700.GetComponent<Virtual_Robot_Controller>().J6 = float.Parse(J6_opc);
        }
        catch (Exception)
        {
            throw;
        }

        //bgwThread_rbt開啟，執行bgwThread_rbt_DoWork
        bgwThread_rbt.RunWorkerAsync();
    }

    private void bgwThread_op_DoWork(object sender, DoWorkEventArgs e)
    {
        DateTime time_start2 = DateTime.Now;

        try
        {
            //自Node ID("ns=3;s=4")獲取人員位置的X軸數值
            pos_a_x_var = OpcUaClient.ReadNode("ns=3;s=4").Value.ToString();
			//自Node ID("ns=3;s=5")獲取人員位置的Y軸數值
            pos_a_y_var = OpcUaClient.ReadNode("ns=3;s=5").Value.ToString();

            //上傳至Node ID("ns=2;s=17"),SSM判別後的結果，分別為0,1,2，代表停止、減速以及常速
            collision_detection_result = SSM_Calculator.rbt_speed;
            OpcUaClient.WriteNode("ns=2;s=17", collision_detection_result);

        }
        catch (Exception)
        {

            throw;
        }

        DateTime time_end2 = DateTime.Now;
        BGW2_Time = ((TimeSpan)(time_end2 - time_start2)).TotalMilliseconds;
		
		//觸發bgwThread_op_ProgressChanged
        bgwThread_rbt.ReportProgress(1);
    }

    private void bgwThread_op_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
		//Do nothing
    }

    private void bgwThread_op_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        try
        {
            engineer_position_pre = System_Parameter.Operator.transform.position;

            //存取至unity參數
            System_Parameter.Operator.GetComponent<Operator_position>().z_var = float.Parse(pos_a_x_var);
            System_Parameter.Operator.GetComponent<Operator_position>().x_var = float.Parse(pos_a_y_var);

            //計算人員移動差異向量
            engineer_position = new Vector3(float.Parse(pos_a_y_var) * 0.01f, 1.5f, float.Parse(pos_a_x_var) * 0.01f);
            engineer_position_dif = engineer_position - engineer_position_pre;

            //計算人員移動速度,*1000是為了將公尺轉換成釐米
            engineer_speed = ((Vector3.Distance(engineer_position, engineer_position_pre))*1000) / (BGW_Time/1000);

            //將operator_speed傳遞至SSM_Calculator腳本的Vh參數
            System_Parameter.R700.GetComponent<SSM_Calculator>().Vh = engineer_speed;
        }
        catch (Exception)
        {
            throw;
        }
		
		//執行bgwThread_rbt_DoWork
        bgwThread_op.RunWorkerAsync();
        
    }

}
