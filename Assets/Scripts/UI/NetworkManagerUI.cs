using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;


public class NetworkManagerUI : MonoBehaviour
{
    public Button HostButton;
    public Button ClientButton;

    private void Awake()
    {
        HostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            Debug.Log("Host GameStart");
        });

        ClientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            Debug.Log("Client GameStart");
        });
    }
}
