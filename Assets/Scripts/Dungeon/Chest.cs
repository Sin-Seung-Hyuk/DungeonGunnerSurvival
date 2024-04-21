using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    [SerializeField] private Color materializeColor; // 상자의 색
    [SerializeField] private float materializeTime = 3f; // 상자가 나타나는 시간
    [SerializeField] private Transform itemSpawnPoint; 
    [SerializeField] private ChestSpawner chestSpawner; 

    private WeaponDetailsSO weaponDetails;
    private GameObject chestItemGameObject;

    private MaterializeEffect materializeEffect;
    private Animator animator;
    private bool isEnable = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        materializeEffect = GetComponent<MaterializeEffect>();
        weaponDetails = chestSpawner.weaponDetail;
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

        chestItemGameObject = Instantiate(GameResources.Instance.chestItemPrefab, itemSpawnPoint);

        chestItemGameObject.GetComponent<SpriteRenderer>().sprite = weaponDetails.weaponSprite;

        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.chestOpen);

        yield return new WaitForSeconds(1.5f);

        GameManager.Instance.GetPlayer().AddWeaponToPlayer(weaponDetails);
        chestItemGameObject.gameObject.SetActive(false);
    }


    public void EndInteraction()
    {
    }
}
