using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class QuestLogUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private QuestLogScrollingList scrollingList;
    [SerializeField] private TextMeshProUGUI questDisplayNameText;
    [SerializeField] private TextMeshProUGUI questStatusText;

    private Button firstSelectedButton;

    private void OnEnable()
    {
        GameEventManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        InputManager.UI.QuestLogToggleEvent.AddListener(QuestLogTogglePressed);
    }

    private void OnDisable()
    {
        GameEventManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        InputManager.UI.QuestLogToggleEvent.RemoveListener(QuestLogTogglePressed);
    }

    private void QuestLogTogglePressed()
    {
        if (contentParent.activeInHierarchy)
        {
            HideUI();
        }
        else
        {
            ShowUI();
        }
    }

    private void ShowUI()
    {
        contentParent.SetActive(true);
        InputManager.Instance.SetQuestLogInput();
        if (firstSelectedButton != null)
        {
            firstSelectedButton.Select();
        }
    }

    private void HideUI()
    {
        contentParent.SetActive(false);
        InputManager.Instance.SetPlayerInput();
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void QuestStateChange(Quest quest)
    {
        QuestLogButton questLogButton = scrollingList.CreateButtonIfNotExists(quest, () => {
            SetQuestLogInfo(quest);
        });
        if (firstSelectedButton == null)
        {
            firstSelectedButton = questLogButton.button;
        }
        questLogButton.SetState(quest.state);
    }

    private void SetQuestLogInfo(Quest quest)
    {
        questDisplayNameText.text = quest.info.displayName;
        questStatusText.text = quest.GetFullStatusText();

    }
}
