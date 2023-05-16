using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManagerVillages : MonoBehaviour
{
    protected VillageDataManager villagedatamanager;
    public Village village;

    protected void Start()
    {
        villagedatamanager = GameManager.instance.villages;
    }
}
