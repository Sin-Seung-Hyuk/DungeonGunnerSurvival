using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    [SerializeField] private Color materializeColor; // ������ ��
    [SerializeField] private float materializeTime = 3f; // ���ڰ� ��Ÿ���� �ð�
    [SerializeField] private Transform itemSpawnPoint; 
    [SerializeField] private GameObject itemPrefab;

    private MaterializeEffect materializeEffect;
    private Animator animator;
    private bool isEnable = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        materializeEffect = GetComponent<MaterializeEffect>();
    }

    private void OnEnable()
    {
        StartCoroutine(MaterializeChest());

        StaticEventHandler.OnRoomChanged += OnRoomChanged;
    }
    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= OnRoomChanged;
    }

    private void OnRoomChanged(RoomChangedArgs obj)
    {
        Destroy(this.gameObject); // ���� ����� �ı�
    }

    private IEnumerator MaterializeChest()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // ������ ��Ÿ���� ȿ�� ������ ���� �ڷ�ƾ ȣ��
        yield return StartCoroutine(materializeEffect.MaterializeRoutine(GameResources.Instance
            .materializeShader, materializeColor, materializeTime, spriteRenderer, GameResources.Instance.litMaterial, true));

        isEnable = true;
    }


    public void Interact(Interactor interator, out bool interactSuccessful)
    {
        if (isEnable)
        {
            isEnable = false;
            StartCoroutine(InstantiateWeapon());

            interactSuccessful = true;
        }

        else interactSuccessful = false;
    }

    private IEnumerator InstantiateWeapon()
    {
        animator.SetBool(Settings.use, true);

        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.chestOpen);

        yield return new WaitForSeconds(1.5f);

        int randomItem = Random.Range(1000, Settings.lastLegendItemID); // ����ũ~���� ������ �� ����

        ItemPickUp itemObj = (ItemPickUp)ObjectPoolManager.Instance.Release(itemPrefab, itemSpawnPoint.position, Quaternion.identity);

        // �����ͺ��̽����� ������ �������� �ϳ� ��������
        itemObj.GetComponent<ItemPickUp>().InitializeItem(GameResources.Instance.database.GetItem(randomItem));
    }


    public void EndInteraction()
    {

    }
}
