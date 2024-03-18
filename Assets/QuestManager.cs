using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class objective
{
    public string objectif;
    public int goal;
    public int progress;
    public bool done;
}

[Serializable]
public class Quest
{
    public int ID;
    [TextArea(1,20)]
    public string desc;
    public List<objective> objectives;
    public bool done;
}

[Serializable]
public class QuestCategory
{
    public string name;
    public List<Quest> quests;
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public List<QuestCategory> QuestCategories;
    public Quest actualQuest;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDesc;
    public Animator questPanel;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        actualQuest = QuestCategories[0].quests[0];
        DisplayQuest();
    }

    public void UpdateQuest(int id, int objID = 0)
    {
        if (id == actualQuest.ID)
        {
            actualQuest.objectives[objID].progress++;
            if (actualQuest.objectives[objID].progress >= actualQuest.objectives[objID].goal)
            {
                actualQuest.objectives[objID].progress = actualQuest.objectives[objID].goal;
                actualQuest.objectives[objID].done = true;
            }
            DisplayQuest();
            bool nextQuest = true;
            foreach (objective o in actualQuest.objectives)
            {
                if (!o.done)
                    nextQuest = false;
            }

            if (nextQuest)
            {
                questPanel.Play("Change");
            }
        }
    }

    public void DisplayQuest()
    {
        questTitle.text = QuestCategories[0].name;
        string desc = actualQuest.desc + "<size=50>";
        foreach (objective o in actualQuest.objectives)
        {
            desc += "\n";
            if (o.done)
            {
                desc += "-" + o.objectif + " " + "<color=green>" + o.progress + "/" + o.goal+"</color>";
            }
            else
            {
                desc += "-" + o.objectif + " " + o.progress + "/" + o.goal;
            }
        }

        questDesc.text = desc;
    }

    public void ChangeQuest()
    {
        actualQuest = QuestCategories[0].quests[actualQuest.ID + 1];
        DisplayQuest();
    }
}
