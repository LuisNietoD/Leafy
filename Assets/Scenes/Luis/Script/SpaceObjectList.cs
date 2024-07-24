using System.Collections.Generic;
using UnityEngine;

namespace Leafy.Data
{
    public class SpaceObjectList: MonoBehaviour
    {
        public static SpaceObjectList instance;
        public List<GameObject> spaceObject = new List<GameObject>();
        public int actual = -1;
        
    
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
    }
}