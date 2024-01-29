using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace Scenes.GUS.Script
{
    public class EasySaveManager : MonoBehaviour
    {
        ES3.Save("myList", myList);
        myList = ES3.Load("myList", defaultValue);
    }
}

