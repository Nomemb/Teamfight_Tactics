using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class ServerManager : NetworkBehaviour
{
    public static ServerManager Instance { get; private set; }

    // �÷��̾� ���� ����
    private Dictionary<ulong, PlayerData> playerDataMap = new Dictionary<ulong, PlayerData>();
    public enum GamePhase { Preperation, Combat, Corousel } // ����, ����, ���� ����
    // NetworkVariable ������ ���� ����
    public NetworkVariable<GamePhase> CurrentGamePhase = new NetworkVariable<GamePhase>(
        GamePhase.Preperation,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    // ���� �� 1ȸ�� ����
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    // ���� ������ ���۽� ȣ���.
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            this.enabled = false;
            return;
        }

        Debug.Log("Server Manager Spawn: Server Start");

        // ���� ���� �� �ʿ��� ���� �߰�
        // Ŭ���̾�Ʈ ����/���� �̺�Ʈ �Լ� ���
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    // Ŭ���̾�Ʈ ���� ����� ȣ���
    void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client connected: {clientId}");

        // �ű� ������ ��츸
        if (!playerDataMap.ContainsKey(clientId))
        {
            PlayerData newPlayerData = new PlayerData(clientId);
            playerDataMap.Add(clientId, newPlayerData);
            Debug.Log($"Client Add: {clientId}");
            Debug.Log($"Cur PlayerCount: {playerDataMap.Count}");
        }

        if (playerDataMap.Count == 8)
        {
            Debug.Log($"{playerDataMap.Count} players Ready, Game Start");
            StartGame();
        }
    }

    void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client disconnected: {clientId}");

        if (playerDataMap.ContainsKey(clientId))
        {
            playerDataMap.Remove(clientId);
        }
    }

    void StartGame()
    {
        // ���� ���۽� ���� ����
    }

    void StartCombatPhase()
    {
        if (IsServer)
        {
            CurrentGamePhase.Value = GamePhase.Combat;
        }
    }

    public PlayerData GetPlayerData(ulong clientId)
    {
        if (playerDataMap.TryGetValue(clientId, out PlayerData data))
        {
            return data;
        }
        return null;
    }
}
