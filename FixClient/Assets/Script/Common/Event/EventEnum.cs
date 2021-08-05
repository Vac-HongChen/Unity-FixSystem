public enum EventEnum
{
    // 服务器主动下发给客户端的帧更新,没有参数
    FrameSync,
    // 客户端接受到帧更新,将当前帧的操作传递给服务器
    SendFrameInput,
    // 客户端上传帧数据
    SendFrameInfo,


    UplinkPacket,
    DownlinkPacket,
    InitPacket,


    // 操作请求
    OperationReq,
    // 操作同步
    OperationSync
}