using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Leafy.Data
{
    public class Craft
    {
        public static Dictionary<int, List<int>> list = new Dictionary<int, List<int>>();
    
    
        public static int GetCraft(List<int> stack)
        {
            stack.Sort();
            foreach (var kvp in list)
            {
                kvp.Value.Sort();
                if (stack.Count == kvp.Value.Count && stack.SequenceEqual(kvp.Value))
                {
                    Debug.Log("Crafting ======" + kvp.Key);
                    foreach (var v in stack)
                    {
                        Debug.Log(v + " Value========");
                    }
                    return kvp.Key;
                }
            }
            
            return -1;
        }
    }
}

