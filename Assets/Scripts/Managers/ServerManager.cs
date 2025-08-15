using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class ServerManager : NetworkBehaviour
{
    public static ServerManager Instance { get; private set; }

    // 플레이어 정보 관리
    private Dictionary<ulong, PlayerData> playerDataMap = new Dictionary<ulong, PlayerData>();
    public enum GamePhase { Preperation, Combat, Corousel } // 구매, 전투, 공동 선택
    // NetworkVariable 서버만 변경 가능
    public NetworkVariable<GamePhase> CurrentGamePhase = new NetworkVariable<GamePhase>(
        GamePhase.Preperation,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    // 시작 시 1회만 실행
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

    // 서버 성공적 시작시 호출됨.
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            this.enabled = false;
            return;
        }

        Debug.Log("Server Manager Spawn: Server Start");

        // 서버 시작 시 필요한 로직 추가
        // 클라이언트 연결/해제 이벤트 함수 등록
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    // 클라이언트 서버 연결시 호출됨
    void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client connected: {clientId}");

        // 신규 접속일 경우만
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
        // 게임 시작시 로직 구현
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
