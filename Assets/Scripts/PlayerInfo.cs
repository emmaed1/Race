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
    public Color colorId;

    public PlayerInfo(ulong id)
    {
        clientId = id;
        colorId = Color.magenta;
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

    public override string ToString() => Name.Value.ToString();
    public static implicit operator string(PlayerInfo name) => name.ToString();
    public static implicit operator PlayerInfo(string s) => new PlayerInfo { Name = new FixedString64Bytes(s) };
}
