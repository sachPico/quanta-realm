using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUtility : MonoBehaviour
{
    [System.Serializable]
    public struct Spawner
    {
        public Vector3 position;
        public float direction;
    }

    [System.Serializable]
    public struct WeaponType
    {
        public string weaponName;
        public List<int> weaponAttack;
        public List<float> weaponFireRate;
        public List<Spawner> spawner;
        public int initialPoolSize;
    }

    public Transform player;
    public WeaponType[] mainWeaponTypes;
    public WeaponType[] subWeaponTypes;

    [Header("Main Weapon")]
    public int mainWeaponID;
    public int mainWeaponPower;
    public int maxMainWeaponPower = 0;

    [Header("Sub Weapon")]
    public int subWeaponID;
    public int subWeaponPower;
    public int maxSubWeaponPower = 0;

    [Header("Weapon UI")]
    public Image cursor;
    public Image mainWeaponBar;
    public Image mainWeaponMaxBar;
    public Image subWeaponBar;
    public Image subWeaponMaxBar;
    public Image qbeFragmentBar;
    public Text qbeNumberText;

    [Header("Other")]
    public int activeMainWeaponAttack;
    public float activeMainWeaponFireRate;
    public int activeSubWeaponAttack;
    public float activeSubWeaponFireRate;
    public int qbeFragment;
    public int qbeNumber;
    public int upgradeID = 0;

    public static WeaponUtility instance;

    private WeaponType activeMainWeaponType;
    private WeaponType activeSubWeaponType;
    private float mainFireCounter = 0;
    private float subFireCounter = 0;

    public void Shoot()
    {
        FireMain();
        FireSub();
    }

    public void ResetFireCounter()
    {
        mainFireCounter = activeMainWeaponFireRate;
        subFireCounter = activeSubWeaponFireRate;
    }

    public void MainPowerChange(int val)
    {
        mainWeaponPower = Mathf.Clamp(mainWeaponPower + val, 0, maxMainWeaponPower);
        SetMainPowerRate(mainWeaponPower);

        mainWeaponBar.fillAmount = mainWeaponPower / 10f;
    }

    public void SubPowerChange(int val)
    {
        subWeaponPower = Mathf.Clamp(subWeaponPower + val, 0, maxSubWeaponPower);
        SetSubPowerRate(subWeaponPower);

        subWeaponBar.fillAmount = subWeaponPower / 10f;
    }

    public void MaxMainPowerExtend(int val)
    {
        mainWeaponMaxBar.fillAmount = val / 10f;
    }

    public void MaxSubPowerExtend(int val)
    {
        subWeaponMaxBar.fillAmount = val / 10f;
    }

    public void QbeFragmentAdd(int val)
    {
        qbeFragment += val;
        if(qbeFragment>=100)
        {
            qbeNumber++;
            qbeFragment -= 100;
            SetQbeNumberText(qbeNumber);
        }
        qbeFragmentBar.fillAmount = qbeFragment / 100f;
    }

    void Shoot(string poolName, Vector3 spawnPosition, float spawnDirection)
    {
        BulletProperty bp = PoolHandler.instance.RequestObject(poolName).GetComponent<BulletProperty>();
        Debug.Log(poolName+" "+spawnPosition);
        bp.transform.position = spawnPosition;
        bp.transform.localEulerAngles = Vector3.forward * spawnDirection;
        bp.gameObject.SetActive(true);
    }

    private void SetCursor(int id)
    {
        switch (id)
        {
            case 0:
                cursor.transform.SetParent(mainWeaponBar.transform.parent, false); break;
            case 1:
                cursor.transform.SetParent(subWeaponBar.transform.parent, false); break;
        }
    }

    private void FireMain()
    {
        if (mainFireCounter >= activeMainWeaponFireRate)
        {
            Vector3 spawnWorldPos;
            foreach (var spawners in activeMainWeaponType.spawner)
            {
                spawnWorldPos = Playfield.instance.playerPivot.TransformVector(spawners.position);
                Shoot(activeMainWeaponType.weaponName, player.position + spawnWorldPos, spawners.direction);
            }
            mainFireCounter = 0;
        }
        else
        {
            mainFireCounter += Time.deltaTime;
        }
    }

    private void FireSub()
    {
        if (subFireCounter >= activeSubWeaponFireRate)
        {
            Vector3 spawnWorldPos;
            foreach (var spawners in activeSubWeaponType.spawner)
            {
                spawnWorldPos = Playfield.instance.playerPivot.TransformVector(spawners.position);
                Shoot(activeSubWeaponType.weaponName, player.position + spawnWorldPos, spawners.direction);
            }
            subFireCounter = 0;
        }
        else
        {
            subFireCounter += Time.deltaTime;
        }
    }

    private void SetActiveWeapon()
    {
        activeMainWeaponType = mainWeaponTypes[mainWeaponID];
        activeSubWeaponType = subWeaponTypes[subWeaponID];
    }

    private void SetMainPowerRate(int id)
    {
        activeMainWeaponFireRate = activeMainWeaponType.weaponFireRate[id];
        activeMainWeaponAttack = activeMainWeaponType.weaponAttack[id];
    }

    private void SetQbeNumberText(int val)
    {
        qbeNumberText.text = val.ToString();
    }

    private void SetSubPowerRate(int id)
    {
        activeSubWeaponFireRate = activeSubWeaponType.weaponFireRate[id];
        activeSubWeaponAttack = activeSubWeaponType.weaponAttack[id];
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        SetActiveWeapon();
        SetCursor(0);
        SetMainPowerRate(0);
        SetSubPowerRate(0);

        mainWeaponBar.fillAmount = 0f;
        MaxMainPowerExtend(0);

        subWeaponBar.fillAmount = 0f;
        MaxSubPowerExtend(0);

        qbeFragmentBar.fillAmount = 0f;

        if (mainWeaponID >= mainWeaponTypes.Length)
        {
            Debug.LogException(new System.Exception("A weaponID is larger than the weapon type array size"));
        }
        else
        {
            SHMUPObjectPool.playfieldTransform = GameObject.FindGameObjectWithTag("Playfield").transform;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            upgradeID = Mathf.Abs(upgradeID - 1);
            SetCursor(upgradeID);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            upgradeID += 1;
            upgradeID %= 2;
            SetCursor(upgradeID);
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            if (qbeNumber != 0)
            {
                switch (upgradeID)
                {
                    case 1:
                        if (maxSubWeaponPower < 10)
                        {
                            maxSubWeaponPower += 1;
                        }
                        MaxSubPowerExtend(maxSubWeaponPower);
                        break;
                    case 0:
                        if (maxMainWeaponPower < 10)
                        {
                            maxMainWeaponPower += 1;
                        }
                        MaxMainPowerExtend(maxMainWeaponPower);
                        break;
                }
                qbeNumber--;
                SetQbeNumberText(qbeNumber);
            }
            else
            {
                Debug.Log("EMPTY");
            }
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            switch (upgradeID)
            {
                case 0:
                    MainPowerChange(1);
                    break;
                case 1:
                    SubPowerChange(1);
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            switch (upgradeID)
            {
                case 0:
                    MainPowerChange(-1);
                    break;
                case 1:
                    SubPowerChange(-1);
                    break;
            }
        }
    }
}