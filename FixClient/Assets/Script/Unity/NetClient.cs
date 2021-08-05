using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixSystem;

/*
    收到网络同步帧时


    操作上传:
    理想状态下,玩家收到同步数据,将当前的操作传递过去
*/

public class NetClient : MonoBehaviour
{
    public LogicCore logicCore = new LogicCore();
    private List<DownlinkPacket> downlinkPackets = new List<DownlinkPacket>();
    private float curTime;
    private float inputInterval = 0.02f;
    public WorldMono world;
    public List<FrameOperation> operations = new List<FrameOperation>();


    private void Awake()
    {
        logicCore.name = "Client";
        world.BindLogic(logicCore);
        EventManager.RegistEvent(EventEnum.DownlinkPacket, OnDownlinkPacket);
        EventManager.RegistEvent(EventEnum.InitPacket, OnInitPacket);
    }

    private void Update()
    {
        HandleDownPacket();
        HandleInput();
    }

    /// <summary>
    /// 初始化第0帧状态
    /// </summary>
    private void OnInitPacket(object o)
    {
        var packet = o as InitPacket;
        logicCore.Start(packet);
    }

    /// <summary>
    /// 接收到新的网络帧
    /// 不能直接执行,先存放到一个队列中,在Update中执行
    /// 时序问题:当使用UDP时,会导致有时序问题的存在,则需要记录此时执行的逻辑帧的id,直到收到下个id
    /// </summary>
    private void OnDownlinkPacket(object o)
    {
        var packet = o as DownlinkPacket;
        // 如果此时有缓存,说明同步数据存在时序问题,将id较小的存在第一位
        downlinkPackets.Add(packet);
    }

    private void HandleDownPacket()
    {
        foreach (var item in downlinkPackets)
        {
            if (item.id == logicCore.frameId)
            {
                logicCore.LogicUpdate(item.operations);
                downlinkPackets.Remove(item);
                break;
            }
        }
    }


    /// <summary>
    /// 处理输入
    /// 在一个逻辑帧中可能产生很多操作
    /// 需要将这些操作整合在一起处理
    /// </summary>
    private void HandleInput()
    {
        curTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            FrameOperation operation = new FrameOperation();
            operation.keyCode = FrameOperation.KeyCode.LeftMouse;
            operation.mousePosition = FixHelper.ToFixVector2(mousePosition);
            operations.Add(operation);
        }
        if (Input.GetKeyDown(UnityEngine.KeyCode.Alpha1))
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            FrameOperation operation = new FrameOperation();
            operation.keyCode = FrameOperation.KeyCode.Skill1;
            operation.mousePosition = FixHelper.ToFixVector2(mousePosition);
            operations.Add(operation);
        }
        if (curTime > inputInterval)
        {
            curTime -= inputInterval;
            UplinkPacket uplinkPacket = new UplinkPacket();
            uplinkPacket.operations = operations;
            EventManager.CallEvent(EventEnum.UplinkPacket, uplinkPacket);
            operations.Clear();
        }
    }
}