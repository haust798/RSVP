/*
 * BodyDataReceiver.cs
 *
 * Receives body data from the network
 * Requires CustomMessages2.cs
 */

using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Tests;
using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;

// Receives the body data messages
public class ConditionReceiver : Singleton<ConditionReceiver>
{

    private Dictionary<long, long> _condition = new Dictionary<long, long>();
    public static long public_condition;

    public Dictionary<long, long> GetData()
    {
        return _condition;
    }

    void Start()
    {
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.condition] =
            this.UpdateCondition;
    }

    // Called when reading in Kinect body data
    void UpdateCondition(NetworkInMessage msg)
    {
        // Parse the message
        long surprise = msg.ReadInt64();
        long trackingID = msg.ReadInt64();
        public_condition = msg.ReadInt64();
        //Debug.Log("Condition Messge reieved: "+trackingID+", "+_condition[trackingID]);
    }
}