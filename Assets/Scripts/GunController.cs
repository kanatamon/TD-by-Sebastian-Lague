using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {
    public Transform weaponHold;
    public Gun startingGun;
    public Gun[] inventoryGun;

    Gun equippedGun;

	// Use this for initialization
	void Start () {
        if (startingGun != null)
        {
            EquipGun(startingGun);
        }
	}
	
    public void EquipGun(Gun gunToEquip){
        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }

        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        equippedGun.transform.parent = weaponHold;
    }

    public void EquipGun(int gunNumber){
        if (gunNumber <= inventoryGun.Length)
        {
            EquipGun(inventoryGun[gunNumber - 1]);      
        }
    }

    public void Shoot(){
        if (equippedGun != null)
        {
            equippedGun.Shoot();
        }
    }
}
