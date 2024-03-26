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
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDesc;
    public Animator questPanel;
    public int actualIndex = -1;
    public int actualCategories;
    
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
        ES3AutoSaveMgr._current.Load();
        DisplayQuest();
    }

    public void UpdateQuest(int cat, int id, int objID)
    {
        if (id == actualIndex && cat == actualCategories)
        {
            QuestCategories[actualCategories].quests[actualIndex].objectives[objID].progress++;
            if (QuestCategories[actualCategories].quests[actualIndex].objectives[objID].progress >= QuestCategories[actualCategories].quests[actualIndex].objectives[objID].goal)
            {
                QuestCategories[actualCategories].quests[actualIndex].objectives[objID].progress = QuestCategories[actualCategories].quests[actualIndex].objectives[objID].goal;
                QuestCategories[actualCategories].quests[actualIndex].objectives[objID].done = true;
            }
            DisplayQuest();
            bool nextQuest = true;
            foreach (objective o in QuestCategories[actualCategories].quests[actualIndex].objectives)
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
        questTitle.text = QuestCategories[actualCategories].name;
        string desc = QuestCategories[actualCategories].quests[actualIndex].desc + "<size=50>";
        foreach (objective o in QuestCategories[actualCategories].quests[actualIndex].objectives)
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
        if (QuestCategories[actualCategories].quests.Count > actualIndex + 1)
        {
            actualIndex++;
            DisplayQuest();
        }
        else if(QuestCategories.Count > actualCategories+1)
        {
            actualCategories++;
            actualIndex = 0;
            DisplayQuest();
        }
        else
        {
            questPanel.gameObject.SetActive(false);
        }
    }
}
