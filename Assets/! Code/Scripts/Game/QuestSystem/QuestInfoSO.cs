using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Quests/QuestInfoSO", order = 1)]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("General")]
    public string displayName;
    public string inkStoryStateVariable; 
    public string inkStoryStateVariableForSteps;


    [Header("Requirements")]
    public int levelRequirement;
    public QuestInfoSO[] questPrerequisites;

    [Header("Steps")]
    public GameObject[] questStepPrefabs;

    [Header("Rewards")]
    public int goldReward;
    public int experienceReward;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = this.name;
        }
        if (string.IsNullOrEmpty(inkStoryStateVariable))
        {
            inkStoryStateVariable = SanitizeInkVariableName(this.name) + "State";
        }
        if (string.IsNullOrEmpty(inkStoryStateVariableForSteps))
        {
            inkStoryStateVariableForSteps = SanitizeInkVariableName(this.name) + "_CurrentStep";
        }
        UnityEditor.EditorUtility.SetDirty(this);
    }

    private string SanitizeInkVariableName(string originalName)
    {
        if (string.IsNullOrEmpty(originalName)) return "DefaultQuest";
        System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9_]");
        string sanitized = rgx.Replace(originalName, "");
        if (string.IsNullOrEmpty(sanitized) || !char.IsLetter(sanitized[0]))
        {
            sanitized = "Quest_" + sanitized;
        }
        return sanitized;
    }
#endif
}