
using UnityEngine;

public class AmmoPattern : MonoBehaviour, IFireable
{
    [SerializeField] private Ammo[] ammoArray;

    private float ammoRange;
    private float ammoSpeed;
    private Vector3 fireDirectionVector;

    public void InitializeAmmo(float aimAngle, Vector3 weaponAimDirectionVector, Weapon weapon)
    {
        SetFireDirection(aimAngle, weaponAimDirectionVector); // 탄 진행방향

        this.ammoRange = weapon.weaponDetail.weaponRange;
        this.ammoSpeed = weapon.weaponDetail.weaponAmmoSpeed;

        gameObject.SetActive(true); // 탄 초기화 후 활성화

        foreach (Ammo ammo in ammoArray)
        {
            ammo.InitializeAmmo(aimAngle, weaponAimDirectionVector, weapon);
        }
    }

    private void Update()
    {
        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;
        transform.position += distanceVector;
        transform.Rotate(new Vector3(0f, 0f, 45f * Time.deltaTime));

        ammoRange -= distanceVector.magnitude;

        if (ammoRange < 0f) gameObject.SetActive(false);
    }
    private void SetFireDirection(float aimAngle, Vector3 aimDirectionVector)
    {
        transform.eulerAngles = new Vector3(0f, 0f, aimAngle);
        fireDirectionVector = Utilities.GetDirectionVectorFromAngle(aimAngle);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
