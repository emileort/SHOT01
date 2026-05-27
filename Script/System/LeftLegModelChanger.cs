using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class LeftLegModelChanger : MonoBehaviour
    {
        public List<GameObject> leftLegModels;

        private void Awake()
        {
            GetAllLeftLegModels();
        }
        private void GetAllLeftLegModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                leftLegModels.Add(transform.GetChild(i).gameObject);
            }

        }

        public void UnEquipAllLeftLegModels()
        {
            foreach (GameObject leftlegModel in leftLegModels)
            {
                leftlegModel.SetActive(false);
            }
        }

        public void EquipLeftLegModelByName(string leftlegName)
        {
            for (int i = 0; i < leftLegModels.Count; i++)
            {
                if (leftLegModels[i].name == leftlegName)
                {
                    leftLegModels[i].SetActive(true);
                }
            }
        }
    }
}

