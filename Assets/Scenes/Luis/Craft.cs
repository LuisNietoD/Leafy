using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Leafy.Data
{
    public class Craft
    {
        public static Dictionary<int, List<int>> list = new Dictionary<int, List<int>>()
        {
            //Stick
            { 5, new List<int>() { 1, 1, 1 } },
            //Wood
            { 1, new List<int>() { 2, 1 } }
        };
    
    
        public static int GetCraft(List<int> stack)
        {
            foreach (var kvp in list)
            {
                if (stack.Count == kvp.Value.Count && stack.All(kvp.Value.Contains))
                {
                    return kvp.Key;
                }
            }
            
            return -1;
        }
    }
}

