// using System;
// using Sirenix.OdinInspector;
// using UnityEditor;
// using UnityEditor.SceneManagement;
// using UnityEngine;
// using Object = UnityEngine.Object;
//
// namespace Bernadetta {
//     public abstract class UniqueDataEditor <T,TG> : Editor where T : Object, UniqueData where TG : MonoBehaviour
//     {
//         internal static readonly string folderPath = "Assets/Resources/" + typeof(T).Name;
//
//         protected string key;
//
//         protected TG Target {
//             get => target as TG;
//         }
//
//         protected T Data {
//             get => (((UniqueObject<T>)target)).data;
//             set {
//                 UniqueObject <T> uniqueObject = target as UniqueObject <T>;
//                 uniqueObject.data = value;
//             }
//         }
//
//         protected virtual void Awake() {
//             if (AssetDatabase.IsValidFolder(folderPath) == false) {
//                 // Create the folder
//                 AssetDatabase.CreateFolder("Assets/Resources", typeof(T).Name);
//                 AssetDatabase.SaveAssets();
//             }
//
//             if (PrefabStageUtility.GetCurrentPrefabStage() != null) {
//                 return;
//             }
//
//             if (Data == null) {
//                 // Search for existing asset using the key in folderPath
//                 string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
//                 foreach (string guid in guids) {
//                     string path = AssetDatabase.GUIDToAssetPath(guid);
//                     T      data = AssetDatabase.LoadAssetAtPath<T>(path);
//                     if (data.key == key) {
//                         Data = data;
//                         break;
//                     }
//                 }
//             
//             
//                 // Generate a new data asset
//                 Data = CreateInstance<T>();
//                 // Generate a key
//                 key = System.Guid.NewGuid().ToString();
//
//                 // Save the asset
//                 AssetDatabase.CreateAsset(Data, $"{folderPath}/{key}.asset");
//
//                 // Set the key
//                 Data.key = key;
//
//                 EditorUtility.SetDirty(Data);
//                 AssetDatabase.SaveAssets();
//             }
//
//             key = Data.key;
//         }
//
//         public override void OnInspectorGUI()
//         {
//             base.OnInspectorGUI();
//             
//             PrefabStage prefabStage           = PrefabStageUtility.GetCurrentPrefabStage();
//             bool isVariant                    = prefabStage != null && (prefabStage.assetPath.Contains("Variant") || prefabStage.assetPath.Contains("variant"));
//             if (prefabStage != null && !isVariant) {
//                 // generic help box for prefab mode
//                 EditorGUILayout.HelpBox("Script is not supported in Prefab Mode as it's unique to each game object", MessageType.Info);
//
//                 // Button to remove link to Data
//                 if (Data != null && GUILayout.Button("Remove link to Data")) { Data = null; }
//                 return;
//             }
//
//             if (Data == null) {
//                 return;
//             }
//
//             // Display the key
//             EditorGUILayout.LabelField("Key", Data.key);
//             
//             string assetPath = AssetDatabase.GetAssetPath(Data);
//             if (GUILayout.Button("Open Asset"))
//             {
//                 T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
//                 if (asset != null)
//                 {
//                     Selection.activeObject = asset;
//                 }
//                 else
//                 {
//                     Debug.LogError("Could not find asset at path: " + assetPath);
//                 }
//             }
//         }
//
//
//         #if UNITY_EDITOR
//
//         // TODO: Disabled for now as it causes issues when stopping playing mode where the Data will get deleted
//         protected virtual void OnDisable() {
//             // If the target is null, then the component has been removed or the game object has been destroyed
//             // However, we only want to delete the backing asset if the game is not playing (i.e. we are in the editor)
//             if (target == null && EditorApplication.isPlayingOrWillChangePlaymode == false) {
//                 // The target has been removed, so let's delete the asset
//                 // AssetDatabase.DeleteAsset($"{folderPath}/{key}.asset");
//             }
//         }
//
//         #endif
//     }
//
//     public abstract class UniqueObject <T> : MonoBehaviour where T : UniqueData
//     {
//         [HideInInspector]
//         public T data;
//     }
//
//     public abstract class UniqueData : SerializedScriptableObject
//     {
//         [HideInInspector]
//         public string key;
//     }
// }