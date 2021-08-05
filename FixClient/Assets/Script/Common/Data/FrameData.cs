using System.Collections.Generic;
using FixSystem;
using TrueSync;

/// <summary>
/// 上行包
/// </summary>
public class UplinkPacket
{
    public List<FrameOperation> operations = new List<FrameOperation>();
}


/// <summary>
/// 下行包
/// </summary>
public class DownlinkPacket
{
    public int id;
    public FrameOperation operation = new FrameOperation();
    public List<FrameOperation> operations = new List<FrameOperation>();
}

/// <summary>
/// 初始化
/// </summary>
public class InitPacket
{
    /// <summary>
    /// 网络帧的时间间隔
    /// </summary>
    public FP NetUpdateTime;
}

/// <summary>
/// 帧操作
/// 包括操作id和此时的操作数据
/// </summary>
public struct FrameOperation
{
    public KeyCode keyCode;
    public TSVector2 mousePosition;

    public enum KeyCode
    {
        None,
        LeftMouse,
        Skill1,
        Skill2
    }
}



public struct FrameState
{
    public int frameId;
    public TSVector2 position;
    public FrameState(int frameId)
    {
        this.frameId = frameId;
        this.position = TSVector2.zero;
    }
}