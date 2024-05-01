using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    [SerializeField] private Color materializeColor; // 상자의 색
    [SerializeField] private float materializeTime = 3f; // 상자가 나타나는 시간
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
        Destroy(this.gameObject); // 던전 입장시 파괴
    }

    private IEnumerator MaterializeChest()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // 서서히 나타나는 효과 연출을 위한 코루틴 호출
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

        int randomItem = Random.Range(1000, Settings.lastLegendItemID); // 유니크~전설 아이템 중 랜덤

        ItemPickUp itemObj = (ItemPickUp)ObjectPoolManager.Instance.Release(itemPrefab, itemSpawnPoint.position, Quaternion.identity);

        // 데이터베이스에서 아이템 랜덤으로 하나 가져오기
        itemObj.GetComponent<ItemPickUp>().InitializeItem(GameResources.Instance.database.GetItem(randomItem));
    }


    public void EndInteraction()
    {

    }
}
