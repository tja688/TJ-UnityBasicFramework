using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FlyRabbit.EventCenter.EditorToolkit
{
    public static class SceneScriptCollector
    {
        public static IReadOnlyList<string> CollectOpenSceneScriptAssetPaths()
        {
            var set = new HashSet<string>();
            var behaviours = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
            foreach (var behaviour in behaviours)
            {
                if (behaviour == null)
                {
                    continue;
                }
                if (EditorUtility.IsPersistent(behaviour))
                {
                    continue;
                }
                if (behaviour.gameObject == null)
                {
                    continue;
                }

                var scene = behaviour.gameObject.scene;
                if (!scene.IsValid() || !scene.isLoaded)
                {
                    continue;
                }

                var script = MonoScript.FromMonoBehaviour(behaviour);
                if (script == null)
                {
                    continue;
                }
                var path = AssetDatabase.GetAssetPath(script);
                if (string.IsNullOrWhiteSpace(path) || path.Contains("/Editor/"))
                {
                    continue;
                }
                set.Add(path);
            }

            return new List<string>(set);
        }
    }
}

