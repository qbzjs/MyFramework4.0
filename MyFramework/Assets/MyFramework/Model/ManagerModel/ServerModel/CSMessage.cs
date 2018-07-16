using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MyFramework
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CSMessageHead
    {
        public byte ComId;
        public byte MsgId;
        public Int32 MsgLength;

        public CSMessageHead(byte _ComId,byte _MsgId,int _Length)
        {
            ComId = _ComId;
            MsgId = _MsgId;
            MsgLength = _Length;
        }
    }
    public class CSMessage
    {
        public CSMessageHead Head;
        public byte[] Msg;

        public int ReadMsg(byte[] msg)
        {
            byte[] headdata = msg.Skip(0).Take(6).ToArray();
            Head = DataTools.ByteaToStruct<CSMessageHead>(headdata);
            if (msg.Length >= Head.MsgLength + 6)
            {
                Msg = msg.Skip(6).Take(Head.MsgLength).ToArray();
                return Head.MsgLength + 6;
            }
            else
            {
                throw new ArgumentOutOfRangeException("CSMessage读取异常 " + Head.ComId + "  Data = " + Head.MsgId+ " Data.Length ="+ msg.Length);
            }
        }

        public void WriteMsg(byte _ComId, byte _MsgId, byte[] _Msg)
        {
            Msg = _Msg;
            Head = new CSMessageHead(_ComId, _MsgId, Msg.Length);
        }

        public byte[] ToBytes()
        {
            byte[] msg = DataTools.StructToBytes(Head).Concat(Msg).ToArray();
            return msg;
        }
    }
}
