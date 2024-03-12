using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _Scripts.Utilities {
    /// <summary>
    /// Handles transitioning between scenes with a loading screen
    /// </summary>
    public class BernaLoader : PersistentSingleton <BernaLoader> {
        [Required]
        public int loadingScene;

        [SerializeField, Required]
        private float fakeLoadingDelay = 1f;
        
        public void SoftUnload(string sceneName) {
            var sceneIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
            StartCoroutine(SoftUnloadCoroutine(sceneIndex));
        }
        
        public void SoftUnload(int sceneId) {
            StartCoroutine(SoftUnloadCoroutine(sceneId));
        }
        
        private IEnumerator SoftUnloadCoroutine(int sceneId) {
            yield return SceneManager.UnloadSceneAsync(sceneId);
        }
        
        public void SoftLoad(string sceneName, string sourceName, TransitionDirection direction = TransitionDirection.EntranceToExit) {
            var sceneIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
            var sourceIndex = SceneManager.GetSceneByName(sourceName).buildIndex;
            StartCoroutine(SoftLoadCoroutine(sceneIndex, sourceIndex, direction));
        }
        
        public void SoftLoad(int sceneId, int sourceSceneId, TransitionDirection direction = TransitionDirection.EntranceToExit) {
            StartCoroutine(SoftLoadCoroutine(sceneId, sourceSceneId, direction));
        }
        
        
        private IEnumerator SoftLoadCoroutine(int sceneId, int sourceId, TransitionDirection direction = TransitionDirection.EntranceToExit) {
            var sourceScene = SceneManager.GetSceneByBuildIndex(sourceId);
            yield return SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);
            
            var newScene = SceneManager.GetSceneByBuildIndex(sceneId);
            
            var oldSceneGameObjects = sourceScene
                                     .GetRootGameObjects();
            
            var newSceneGameObjects = newScene
                                     .GetRootGameObjects();

            SceneManager.SetActiveScene(newScene);

            EntranceNode entranceNode;
            ExitNode exitNode;

            switch (direction) {
                case TransitionDirection.EntranceToExit:
                    exitNode = oldSceneGameObjects
                              .SelectMany(go => go.GetComponentsInChildren<ExitNode>())
                              .FirstOrDefault(node => node);
                    entranceNode = newSceneGameObjects
                                  .SelectMany(go => go.GetComponentsInChildren<EntranceNode>())
                                  .FirstOrDefault(node => node);
                    break;
                case TransitionDirection.ExitToEntrance:
                    exitNode = newSceneGameObjects
                              .SelectMany(go => go.GetComponentsInChildren<ExitNode>())
                              .FirstOrDefault(node => node);
                    entranceNode = oldSceneGameObjects
                                  .SelectMany(go => go.GetComponentsInChildren<EntranceNode>())
                                  .FirstOrDefault(node => node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            if (exitNode != null && entranceNode != null) {
                var delta = exitNode.transform.position - entranceNode.transform.position;
                newSceneGameObjects.ForEach(go => go.transform.position += delta);
            }

            newSceneGameObjects
               .SelectMany(go => go.GetComponentsInChildren<EntranceNode>())
               .ForEach(node => node.SetPreviousSceneIndexOnce(sourceId));
            
            newSceneGameObjects
               .SelectMany(go => go.GetComponentsInChildren<DespawnNode>())
               .ForEach(node => node.SetSceneToDespawnOnce(sourceId));
        }
        
        public void TransitionTo(string sceneName) {
            var sceneIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
            StartCoroutine(TransitionToCoroutine(sceneIndex));
        }
        
        public void TransitionTo(int sceneIndex) {
            StartCoroutine(TransitionToCoroutine(sceneIndex));
        }
        
        private IEnumerator TransitionToCoroutine(int sceneIndex) {
            yield return SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Additive);
            LoadingSceneController.Instance.Enable();
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            // Wait for 5 seconds
            yield return new WaitForSeconds(fakeLoadingDelay);
            yield return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            LoadingSceneController.Instance.Disable();
            yield return SceneManager.UnloadSceneAsync(loadingScene);
        }
    }


}