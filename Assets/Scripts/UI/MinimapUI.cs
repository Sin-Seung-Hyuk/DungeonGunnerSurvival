using Cinemachine;
using UnityEngine;

public class MinimapUI : MonoBehaviour
{
    [SerializeField] private GameObject minimapPlayer;
    private Transform playerTransform;

    private void Start()
    {
        // 게임매니저에서 플레이어 트랜스폼 가져오기
        playerTransform = GameManager.Instance.GetPlayer().transform;

        SpriteRenderer spriteRenderer = minimapPlayer.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = GameManager.Instance.GetPlayerMinimapIcon();
        }
    }

    private void Update()
    {
        if (playerTransform != null && minimapPlayer != null)
        {
            minimapPlayer.transform.position = playerTransform.position;
        }
    }
}
