using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
    public GameObject[] weapons;
    public int selectedWeapon = 0;

    public void NextWeapon()
    {
        weapons[selectedWeapon].SetActive(false);
        selectedWeapon = (selectedWeapon + 1) % weapons.Length;
        weapons[selectedWeapon].SetActive(true);
    }

    public void PreviousWeapon()
    {
        weapons[selectedWeapon].SetActive(false);
        selectedWeapon--;
        if(selectedWeapon < 0)
        {
            selectedWeapon += weapons.Length;
        }
        weapons[selectedWeapon].SetActive(true);
    }
}
