using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct PlayerInfo : INetworkSerializable, IEquatable<PlayerInfo>
{
    public ulong clientId;
    public FixedString64Bytes Name;
    public bool isPlayerReady;
    public int colorId;

    public PlayerInfo(ulong id)
    {
        clientId = id;
        colorId = 0;
        Name = "";
        isPlayerReady = false;
    }

    public bool Equals(PlayerInfo other)
    {
        return clientId == other.clientId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref Name);
        serializer.SerializeValue(ref isPlayerReady);
        serializer.SerializeValue(ref colorId);        
    }
}
