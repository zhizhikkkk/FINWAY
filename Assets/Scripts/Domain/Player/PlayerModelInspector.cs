using UnityEngine;
using Zenject;

public class PlayerModelInspector : MonoBehaviour
{
    [Header("������������� ������ (������� ����� � ����������, � Play-������)")]
    public PlayerData editableData;        

    [Inject] PlayerModel _model;
    [Inject] PlayerDataManager _dataMgr;   

#if UNITY_EDITOR
    [ContextMenu("Load from Model")]
    void LoadFromModel()
    {
        editableData = _model.ToData();
    }

    [ContextMenu("Apply To Model")]
    void ApplyToModel()
    {
        _model.ApplyData(editableData);
    }

    [ContextMenu("Apply + Save JSON")]
    void ApplyAndSave()
    {
        ApplyToModel();
        _dataMgr.Save(_model);          
    }
#endif
}
