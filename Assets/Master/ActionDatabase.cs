using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ActionDatabase")]
public class ActionDatabase : SerializedScriptableObject
{
    public TextAsset Labels;
#if UNITY_EDITOR
    [OnValueChanged(nameof(AddList))]
#endif
    public List<SubNode> actions = new List<SubNode>();
    private void AddList()
    {
        if (actions == null) return;
        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i].textAsset == null)
            {
                actions[i] = new SubNode(Labels);
            }
        }
    }
}
public class SubNode {
    
    public TextAsset textAsset {get; private set; }

    public SubNode() {;}
    public SubNode(TextAsset asset)
    {
        textAsset = asset;
    }

    public ActionNode actionType;
#if UNITY_EDITOR
    [ValueDropdown("_labels")]
#endif
    public string Label;
    public ValueDropdownList<string> _labels
    {
        get
        {
            if (textAsset == null)
            {
                return new ValueDropdownList<string>();
            }
            var dropdownList = new ValueDropdownList<string> { };
            var strokes = textAsset.text.Split('\n');
            foreach (var stroke in strokes)
            {
                dropdownList.Add(stroke);
            }

            return dropdownList;
        }
    }
}
