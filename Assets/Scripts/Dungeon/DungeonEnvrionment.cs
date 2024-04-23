using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnvrionment : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask; // �浹���� üũ
    [SerializeField] private GameObject itemPrefab; // ������ ������ ������
    [SerializeField] private SoundEffectSO destroyedSoundEffect; // �ı� �Ҹ�

    private DestroyedEvent destroyedEvent;
    private Animator animator;

    private bool isDestroyed = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        destroyedEvent = GetComponent<DestroyedEvent>();
    }

    private void OnEnable()
    {
        destroyedEvent.OnDestroyed += DestroyedEvent_OnDestroyed;

    }
    private void OnDisable()
    {
        destroyedEvent.OnDestroyed -= DestroyedEvent_OnDestroyed;
    }

    private void DestroyedEvent_OnDestroyed(DestroyedEvent arg1, DestroyedEventArgs args)
    {
        // 102~106 : ����
        InventoryItemData randomPotion = GameResources.Instance.database.GetItem(Random.Range(102, 107));

        ItemPickUp itemObj = (ItemPickUp)ObjectPoolManager.Instance.Release(itemPrefab, args.point, Quaternion.identity);

        itemObj.InitializeItem(randomPotion); // �������� ���� �����ϱ�
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int collisionObjLayerMask = 1 << collision.gameObject.layer;

        if ((collisionObjLayerMask & layerMask.value) == 0)
            return;

        if (!isDestroyed)
        {
            isDestroyed = true;

            SoundEffectManager.Instance.PlaySoundEffect(destroyedSoundEffect);
            animator.SetBool(Settings.destroy, true);
            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Destroyed"))
            yield return null;

        destroyedEvent.CallDestroyedEvent(false, this.transform.position);
    }
}
