using Unity.Netcode;
using UnityEngine;
using UshiSoft.UACPF;

namespace Unity.Services.Samples.ServerlessMultiplayerGame
{
    public class NetworkCarPlayer : NetworkBehaviour
    {
        [SerializeField]
        PlayerCarControl carControl;

        [field: SerializeField]
        public NetworkObject networkObject { get; private set; }

        public int playerIndex { get; private set; }
        public string playerId { get; private set; }
        public string playerName { get; private set; }
        public ulong playerRelayId { get; private set; }
        public int score { get; private set; }

        bool m_IsMovementAllowed = false;

        void Update()
        {
            if (!IsOwner) return;
            if (m_IsMovementAllowed && carControl != null)
            {
                // PlayerCarControl handles input internally
            }
        }

        [ClientRpc]
        public void SetPlayerCarClientRpc(int playerIndex, string playerId, string playerName, ulong relayClientId)
        {
            this.playerIndex = playerIndex;
            this.playerId = playerId;
            this.playerName = playerName;
            this.playerRelayId = relayClientId;
            this.playerName = ProfanityManager.SanitizePlayerName(this.playerName);
            GameNetworkManager.instance?.AddCarPlayer(this, IsOwner);
			Debug.Log($"Set car player for player #{playerIndex}: id:'{playerId}' name:'{playerName}' relay:{relayClientId}");
        }

        public void AllowMovement()
        {
            m_IsMovementAllowed = true;
        }

        void OnTriggerEnter(Collider other)
        {
            var coin = other.GetComponent<Coin>();
            if (coin != null)
            {
                HandleCoinCollection(coin);
            }
        }

        void HandleCoinCollection(Coin coin)
        {
            if (IsHost)
            {
                //GameCoinManager.instance?.CollectCoin(this, coin);
            }
            else
            {
                coin.gameObject.SetActive(false);
            }
        }

        [ClientRpc]
        public void ScorePointClientRpc()
        {
            score++;
            GameSceneManager.instance?.UpdateScores();
        }

        public override string ToString()
        {
            return $"Car player #{playerIndex} '{playerName}' score:{score}.";
        }
    }
}
