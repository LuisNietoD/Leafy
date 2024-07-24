using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LTX.ChanneledProperties
{
    [System.Serializable]
    public struct ChannelKey
    {
        #region Static
        internal static ChannelKey None => _None;
        private static ChannelKey _None = new ChannelKey(0);

        private static Dictionary<Object, ChannelKey> unityObjectsKeys;

        private static ulong _internalChannelKeyCount = 1;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void InitializeOnLoad()
        {
            Debug.Log($"[Channeled properties] Initializing channelkeys...");
            unityObjectsKeys = new();
            
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded; ;
        }

        private static void SceneManager_sceneUnloaded(Scene scene)
        {
            CleanMissingReferences();
        }
        private static void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            CleanMissingReferences();
        }

        private static void CleanMissingReferences()
        {

            Debug.Log($"[Channeled properties] Clearing missing channelkeys...");
            Dictionary<Object, ChannelKey> _createdKeys = new();

            foreach (var item in ChannelKey.unityObjectsKeys)
            {
                if (item.Key != null)
                    _createdKeys.Add(item.Key, item.Value);
            }

            ChannelKey.unityObjectsKeys = _createdKeys;
        }

        public static ChannelKey GetUniqueChannelKey()
        {
            _internalChannelKeyCount++;
            ChannelKey channelKey = new ChannelKey(_internalChannelKeyCount);

            return channelKey;
        }


        public static ChannelKey GetUniqueChannelKey(string pointer)
        {
            var key = GetUniqueChannelKey();
            key.pointerTag = pointer;

            return key;
        }

        public static ChannelKey GetUniqueChannelKey(object pointer)
        {
            ChannelKey channelKey = GetUniqueChannelKey(pointer.GetType().Name);

            if (pointer is Object unityObject)
            {
                channelKey.pointer = unityObject;
                unityObjectsKeys.Add(unityObject, channelKey);
            }
            return channelKey;
        }

        public static implicit operator ChannelKey(Object unityObject)
        {
            if (unityObjectsKeys == null)
                return default;

            if (unityObjectsKeys.TryGetValue(unityObject, out ChannelKey channelKey))
                return channelKey;
            else
                return GetUniqueChannelKey(unityObject);
        }

        #endregion


        [SerializeField]
        internal ulong _id;

        public ulong ID => _id;
        [SerializeField]
        internal Object pointer;
        [SerializeField]
        internal string pointerTag;

        private int hashcode;


        private ChannelKey(ulong id)
        {
            _id = id;
            hashcode = id.GetHashCode();
            pointerTag = null;
            pointer = null;
        }

        public override int GetHashCode() => hashcode;

        public override bool Equals(object obj) => base.Equals(obj);


    }
}
