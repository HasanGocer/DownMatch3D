using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskUISystem : MonoSingleton<TaskUISystem>
{
    public GameObject TaskPanel;
    [SerializeField] List<GameObject> _taskImage = new List<GameObject>();
    public List<GameObject> taskObject = new List<GameObject>();
    [SerializeField] List<TMP_Text> _taskText = new List<TMP_Text>();

    public void UIPlacement()
    {
        TaskSystem.Contract contract = TaskSystem.Instance.focusContract;

        TaskPanel.SetActive(true);

        for (int i = 0; i < contract.objectTypeCount.Count; i++)
        {
            _taskImage[i].gameObject.transform.GetChild(contract.objectTypeCount[i]).gameObject.SetActive(true);
            _taskText[i].gameObject.transform.parent.gameObject.SetActive(true);
            _taskText[i].text = contract.objectCount[i].ToString();
        }
    }

    public void TaskDown(int typeCount)
    {
        TaskSystem.Contract contract = TaskSystem.Instance.focusContract;
        if (contract.objectTypeCount.Contains(typeCount))
        {
            int tempTypeCount = contract.objectTypeCount.IndexOf(typeCount);
            TaskSystem.Instance.ObjectCountUpdate(tempTypeCount);

            _taskText[tempTypeCount].text = contract.objectCount[tempTypeCount].ToString();

            if (contract.objectCount[tempTypeCount] <= 0)
            {
                _taskText[tempTypeCount].gameObject.transform.parent.gameObject.SetActive(false);
                _taskImage[tempTypeCount].SetActive(true);
            }
        }
    }
}
