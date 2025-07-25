using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;

public class UDPSender : MonoBehaviour
{
    public GeospatialDisplay geospatialDisplay;
    public InputField ipInputField;           
    public string remoteIP = "172.20.10.10";      
    public int remotePort = 12345;

    public Button reconnectButton;
    public Image statusIndicator;
    public Text statusText;
    public Text geospatialText;

    private UdpClient udpClient;
    private bool isConnected = false;
    private float sendInterval = 0.5f;
    private float lastSendTime = 0f;

    void Start()
    {
        reconnectButton.onClick.AddListener(() =>
        {
            if (!string.IsNullOrWhiteSpace(ipInputField.text))
            {
                remoteIP = ipInputField.text.Trim();
            }
            ConnectUDP();
        });

        ConnectUDP();
    }

    void Update()
    {
        if (!isConnected || geospatialDisplay == null) return;
        if (Time.time - lastSendTime < sendInterval) return;

        var poseOpt = geospatialDisplay.CurrentPose;
        if (poseOpt == null) return;

        var pose = poseOpt.Value;
        string message = $"{pose.Latitude},{pose.Longitude},{pose.Altitude},{pose.Heading}";
        geospatialText.text = message;
        byte[] data = Encoding.UTF8.GetBytes(message);

        try
        {
            udpClient.Send(data, data.Length, remoteIP, remotePort);
            UpdateStatus(true, "送信成功");
        }
        catch
        {
            UpdateStatus(false, "送信失敗");
        }

        lastSendTime = Time.time;
    }

    void ConnectUDP()
    {
        try
        {
            udpClient?.Close();
            udpClient = new UdpClient();
            isConnected = true;
            UpdateStatus(true, $"接続: {remoteIP}:{remotePort}");
        }
        catch
        {
            isConnected = false;
            UpdateStatus(false, "接続失敗");
        }
    }

    void UpdateStatus(bool success, string msg)
    {
        if (statusIndicator != null)
            statusIndicator.color = success ? Color.green : Color.red;

        if (statusText != null)
            statusText.text = msg;
    }

    void OnApplicationQuit()
    {
        udpClient?.Close();
    }
}
