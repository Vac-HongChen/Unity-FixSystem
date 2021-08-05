using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixSystem;
using TrueSync;

/*
    一场游戏开始
    客户端和服务器通过同样的配置文件调用LogicCore.Init(),获取同样的初始状态

    服务器开启一个每秒执行一次的循环线程,定义为网络帧
    在网络帧中,先将玩家的上一帧的操作分发下去,然后将该帧的操作赋值给LogicCore,执行一次逻辑帧

    在第一帧中,玩家的操作都是Null

    客户端接收到服务器的网络帧后,在下一次渲染帧执行逻辑帧


    -- 客户端每次处理完网络帧都需要将该帧状态传递给服务器进行验证
        帧状态:网络帧编号,帧状态

    -- 服务器收到帧状态后
        1.判断是否跳帧
        2.判断对应在服务器的帧状态是否一致

    操作数据处理:
    1.在接收到当前网络帧后,立即将此时的操作上传
    2.


    网络帧长度定为1s
    上下行延迟定义为1.5s

    0s      服务器下发第一帧操作给客户端,内容为空操作,并以该操作执行帧逻辑          延迟定义为0.75s,客户端在0.75s接收到
    0.75s   客户端收到第一帧操作,为空操作,执行帧逻辑,并将此时的操作A传递给服务器    延迟定义为0.75s,服务器在1.5s接收到

    1s      服务器下发第二帧操作给客户端,内容为空操作,并以该操作执行帧逻辑          延迟定义为0.75s,客户端在1.75接收到
    1.5s    服务器收到客户端的操作A
    1.75s   客户端收到第二帧操作,为空操作,执行帧逻辑,并将此时的操作B传递给服务器    延迟定义为2s,服务器在3.75s接收到

    2s      服务器下发第三帧操作给客户端,内容为操作A,并以该操作执行帧逻辑
    2.75s   客户端收到第三帧操作,为操作A,执行帧逻辑,并将此时的操作C传递给服务器     此时延迟正常,为0.75s,服务器将在3.5s收到

    3s      服务器下发第四帧操作给客户端,内容为操作A,并以该操作执行帧逻辑           此时延迟为2.5s,客户端在5.5s接收到
    3.5s    服务器收到客户端的操作C
    3.75    服务器收到客户端的操作B                                                此时在服务器存储有操作C和操作B,按照服务器最后一次接受到的操作,则认定为操作B

    4s      服务器下发第五帧操作给客户端,内容为操作B,并以该操作执行帧逻辑           此时延迟正常,为0.75s
    4.75s   客户端收到第五帧操作,为操作B,执行帧逻辑,并将此时的操作E传递给服务器     此时客户端实际上应该执行第四帧的逻辑,但是由于延迟,先接到了第五帧的操作,缺少第四帧的操作,只能先将该操作存储下来,不执行逻辑帧


    5s      服务器下发第六帧操作给客户端,内容为操作E,并以该操作执行帧逻辑           此时服务器虽然已经收到
    5.5s    客户端收到第四帧操作,为操作A,执行帧逻辑,并将此时的操作F传递给服务器
    5.75s   客户端收到第六帧操作,为操作E,执行帧逻辑,并将此时的操作G传递给服务器



    客户端在0.75s发送的帧操作,最终在2.75s的时候开始执行,差了2个网络帧长度
    客户端的操作延迟为:(网络延迟相比于网络帧长度的整数倍 + 1) * 网络帧长度

    由于延迟产生的问题:
    1.客户端当前执行到了第三网络帧,但是先收到了第五网络帧
    需要在客户端将所有收到的网络帧先缓存下来,等收到需要执行的网络帧后再执行
*/
[System.Serializable]
public class LogicCore
{
    public World world = new World();
    public PlayerEntity player;
    public Action<Entity> OnAddEntity;
    public Action<Entity> OnRemoveEntity;
    /// <summary>
    /// 当前网络帧结束后调用
    /// </summary>
    public Action<FrameState> OnNetFrameComplete;
    public FP NetTime;
    /// <summary>
    /// 当前的逻辑帧id
    /// </summary>
    public int frameId { get; private set; }
    public string name;

    public void Start(InitPacket initPacket)
    {
        world.name = this.name;
        frameId = 0;
        this.NetTime = (FP)initPacket.NetUpdateTime;
        world.OnAddEntity += OnAddEntity;
        world.OnRemoveEntity += OnRemoveEntity;
        player = new PlayerEntity(world);


        MonsterEntity monster = new MonsterEntity(world);
    }
    public void LogicUpdate(List<FrameOperation> operations)
    {
        player.operations = operations.ToArray();
        world.LogicUpdate(NetTime);
        frameId++;
        // BattleDebug.LogFile(name + ":" + player.transform.position + "\n");
    }
}