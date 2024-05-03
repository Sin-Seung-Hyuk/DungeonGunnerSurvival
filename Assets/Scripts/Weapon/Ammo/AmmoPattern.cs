
using UnityEngine;

public class AmmoPattern : MonoBehaviour, IFireable
{
    [SerializeField] private Ammo[] ammoArray;
    [SerializeField] private float rotateValue = 45f;

    private float ammoRange;
    private float ammoSpeed;
    private Vector3 fireDirectionVector;

    public void InitializeAmmo(float aimAngle, Weapon weapon, bool isAmmoPattern)
    {
        SetFireDirection(aimAngle); // ź �������

        this.ammoRange = weapon.weaponDetail.weaponRange;
        this.ammoSpeed = weapon.weaponDetail.weaponAmmoSpeed;

        gameObject.SetActive(true); // ź �ʱ�ȭ �� Ȱ��ȭ

        foreach (Ammo ammo in ammoArray) // �迭�� ����ִ� �� ź�ະ�� �ʱ�ȭ ����
        {
            ammo.InitializeAmmo(aimAngle, weapon, true); // isAmmoPattern = true
        }
    }

    private void Update()
    {
        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;
        transform.position += distanceVector;
        transform.Rotate(new Vector3(0f, 0f, rotateValue * Time.deltaTime));

        ammoRange -= distanceVector.magnitude;

        if (ammoRange < 0f)
        {
            gameObject.SetActive(false);
        }
    }
    private void SetFireDirection(float aimAngle)
    {
        transform.eulerAngles = new Vector3(0f, 0f, aimAngle);
        fireDirectionVector = Utilities.GetDirectionVectorFromAngle(aimAngle);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
