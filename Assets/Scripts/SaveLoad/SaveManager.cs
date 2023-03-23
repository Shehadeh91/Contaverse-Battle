using System.Security;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Contaquest.Metaverse.Data;
using System;
using Contaquest.Server;

public class SaveManager : PersistentGenericSingleton<SaveManager>
{
    // [SerializeField] private List<GlobalVariable> globalVariables = new List<GlobalVariable>();
    private bool hasSaveBeenRequested = false;

    [SerializeField]
    private Dictionary<int, GlobalVariable> variablesToSave = new Dictionary<int, GlobalVariable>();

    private void Start()
    {
        Application.targetFrameRate = 24;
        // foreach (var globalVariable in globalVariables)
        // {
        //     globalVariable.InitializeVariable();
        // }
    }

    public void RegisterSaveableVariable(GlobalVariable variable)
    {
        variablesToSave.Add(variable.variableName.GetHashCode(), variable);
    }

    public void SaveGameState()
    {
        foreach (KeyValuePair<int, GlobalVariable> variable in variablesToSave)
        {
            variable.Value.Save();
        }
    }

    public void VariableSaveRequested()
    {
        if(!hasSaveBeenRequested)
        {
            hasSaveBeenRequested = true;
            StartCoroutine(SavePrefs());
        }
    }

    public void SaveRobotData(RobotData robotData, int saveSlot)
    {
        Debug.Log("Saving Robot data");

        foreach (var equippableData in robotData.equippableDatas)
        {
            string playerPrefsKey = saveSlot + equippableData.equipSlot.ToString();
            string nftID = equippableData.nftID;

            Debug.Log($"Saving slot {equippableData.equipSlot} prefname:{playerPrefsKey}: prefvalue:{nftID}. Robotpart: {equippableData.robotPartData.itemAddress}, nftID: {equippableData.robotPartData.nftData.nftID}");
            PlayerPrefs.SetString(playerPrefsKey, nftID);
        }
        VariableSaveRequested();
        // try
        // {
        //     UserManager.Instance.SetPlayerParts(robotData);
        // }
        // catch (System.Exception e)
        // {
        //     Debug.Log(e.Message);
        // }
    }

    public RobotData LoadRobotData(int saveSlot)
    {
        string debug = "Loading RobotData";

        EquipSlot[] equipSlots = EnumUtil.GetEnumList<EquipSlot>();
        EquippableData[] equippableDatas = new EquippableData[equipSlots.Length];

        for (int i = 0; i < equipSlots.Length; i++)
        {
            equippableDatas[i] = new EquippableData(equipSlots[i], "");
            string playerPrefsKey = saveSlot + equipSlots[i].ToString();

            if (PlayerPrefs.HasKey(playerPrefsKey))
                equippableDatas[i].nftID = PlayerPrefs.GetString(playerPrefsKey);
            else
            {
                PlayerPrefs.SetString(playerPrefsKey, "");
            }
            debug += "\n" + equippableDatas[i].nftID;
        }
        RobotData robotData = new RobotData(equippableDatas);
        Debug.Log(debug);
        return robotData;
    }

    public Vector3 GetVector3(string variableName, Vector3 value)
    {
        Vector3 returnVector = Vector3.zero;

        if (PlayerPrefs.HasKey($"{variableName}X"))
        {
            returnVector.x = PlayerPrefs.GetFloat($"{variableName}X");
            returnVector.y = PlayerPrefs.GetFloat($"{variableName}Y");
            returnVector.z = PlayerPrefs.GetFloat($"{variableName}Z");
        }
        return returnVector;
    }
    public void SetVector3(string variableName, Vector3 value)
    {
        PlayerPrefs.SetFloat($"{variableName}X", value.x);
        PlayerPrefs.SetFloat($"{variableName}Y", value.y);
        PlayerPrefs.SetFloat($"{variableName}Z", value.z);
    }

    public bool GetBool(string variableName)
    {
        if (PlayerPrefs.HasKey(variableName))
        {
            int integerValue = PlayerPrefs.GetInt(variableName);
            return integerValue != 0;
        }
        return false;
    }
    public void SetBool(string variableName, bool value)
    {
        PlayerPrefs.SetInt(variableName, value ? 1 : 0);
        VariableSaveRequested();
    }

    private IEnumerator SavePrefs()
    {
        yield return new WaitForEndOfFrame();

        PlayerPrefs.Save();
        hasSaveBeenRequested = false;
    }
}