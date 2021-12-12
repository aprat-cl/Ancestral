using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class RoomInstance
{
    // Start is called before the first frame update

    public Doors Left, Right, Up, Down;
    public byte Spec;
    public bool bStartRoom = false;
    public Texture2D Icon;
    public RoomType type = RoomType.Initial;
    public TreasureRoom chest;
    public static RoomInstance CreateRoom(byte combo, Connector conector)
    {
        RoomInstance room = new RoomInstance();
        room.Spec = combo;
        room.Up = conector == Connector.Up ? Doors.Open : GetBit(combo, 0) ? Doors.Wide : GetBit(combo, 1) ? Doors.Open : Doors.Close;
        room.Left = conector == Connector.Left ? Doors.Open : GetBit(combo, 5) ? Doors.Wide : GetBit(combo, 3) ? Doors.Open : Doors.Close;
        room.Right = conector == Connector.Right ? Doors.Open : GetBit(combo, 2) ? Doors.Wide : GetBit(combo, 4) ? Doors.Open : Doors.Close;
        room.Down = conector == Connector.Down ? Doors.Open : GetBit(combo, 7) ? Doors.Wide : GetBit(combo, 7) ? Doors.Open : Doors.Close;

        return room;
    }

    public static bool GetBit(byte b, int bitNumber)
    {
        return (b & (1 << bitNumber)) != 0;
    }
    public enum Doors
    {
        Open, Close, Wide, Blocked
    }
    public enum Connector
    {
        Left, Up, Right, Down, None
    }
    public enum RoomType
    {
        Initial,
        Cave,  Praire,  Lava,   Sky, Mountains, Hellish,    Beach,  Underwater, Void, Light, None
    }

}
[Serializable]
public class TreasureRoom{
    public bool bHasTreasure, bIsOpen;
    public ItemCodes ItemCode;
    public float Qty;
}