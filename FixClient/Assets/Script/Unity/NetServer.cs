using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TrueSync;

public class NetServer : MonoBehaviour
{
    private LogicCore logicCore = new LogicCore();
    public float logicTime = 0;
    public int logicFrame = 15;
    public List<FrameOperation> operations = new List<FrameOperation>();
    private void Awake()
    {
        logicCore.name = "Server";
        BattleDebug.Log += Debug.Log;
        BattleDebug.LogError += Debug.LogError;
        BattleDebug.LogFile += (o) =>
        {
            System.IO.File.AppendAllText("Log.txt", o.ToString());
        };
        EventManager.RegistEvent(EventEnum.UplinkPacket, OnUplinkPacket);

        logicTime = (float)1 / logicFrame;
    }
    private void Start()
    {
        InitPacket packet = new InitPacket();
        packet.NetUpdateTime = (FP)logicTime;
        logicCore.Start(packet);
        EventManager.CallEvent(EventEnum.InitPacket, packet);
        StartCoroutine(TimingFunction());

    }


    /// <summary>
    /// 定时函数
    /// </summary>
    private IEnumerator TimingFunction()
    {
        while (true)
        {
            yield return new WaitForSeconds(logicTime);
            NetUpdate();
        }
    }


    /// <summary>
    /// 网络帧:定时执行
    /// 1.对比客户端的帧状态和服务器存储的帧状态
    /// 2.对玩家的帧操作进行转发
    /// 3.根据玩家的帧操作推进一次逻辑帧
    /// </summary>
    private void NetUpdate()
    {
        // 将上一帧的操作下发出去,在第一帧的情况下,上一帧没有操作,则可以在玩家收到的初始包的时候传递一次操作
        // 发送一次下行包
        // 包内容为:上一个上行包中的操作数据
        DownlinkPacket downlinkPacket = new DownlinkPacket();
        downlinkPacket.id = logicCore.frameId;
        downlinkPacket.operations.AddRange(operations);
        EventManager.CallEvent(EventEnum.DownlinkPacket, downlinkPacket);
        // 使用上一次的上行包的操作数据执行一次逻辑更新
        logicCore.LogicUpdate(operations);
        operations.Clear();
    }


    private void OnUplinkPacket(object o)
    {
        var packet = (UplinkPacket)o;
        operations.AddRange(packet.operations);
    }

    private void OnDestroy()
    {
        ThreadManager.Close();
    }
}