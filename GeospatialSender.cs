using UnityEngine;
using System.Net.Sockets;
using System.Text;
using Google.XR.ARCoreExtensions;

public class GeospatialSender : MonoBehaviour
{
    public ARCoreExtensions arCoreExtensions;
    public AREarthManager earthManager;
    public string remoteIP = "127.0.0.1"; // 送信先（同一マシンならlocalhost）
    public int remotePort = 12345;

    UdpClient udpClient;

    void Start()
    {
        udpClient = new UdpClient();
    }

    void Update()
    {
        if (earthManager == null || earthManager.EarthTrackingState != UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            return;

        var pose = earthManager.CameraGeospatialPose;

        string message = $"{pose.Latitude},{pose.Longitude},{pose.Altitude},{pose.Heading}";
        byte[] data = Encoding.UTF8.GetBytes(message);
        udpClient.Send(data, data.Length, remoteIP, remotePort);
    }

    void OnApplicationQuit()
    {
        udpClient?.Close();
    }
}
