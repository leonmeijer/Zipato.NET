using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LVMS.Zipato.Model;
using PortableRest;

namespace LVMS.Zipato
{
    public partial class ZipatoClient
    {
        Scene[] _cachedScenesList;
        Dictionary<Guid, Scene> _cachedScenes;

        public async Task<Scene[]> GetScenesAsync(bool allowCache = true)
        {
            CheckInitialized();

            if (allowCache && _cachedScenesList != null)
                return _cachedScenesList;

            var request = new RestRequest("scenes", HttpMethod.Get);
            PrepareRequest(request);
            var result = await _httpClient.ExecuteAsync<Scene[]>(request);

            if (allowCache || _cachedScenesList != null)
                _cachedScenesList = result;
            return result;
        }

        public async Task<Scene> GetSceneAsync(Guid uuid, bool allowCache = true)
        {
            CheckInitialized();

            if (allowCache && _cachedEndpoints != null && _cachedEndpoints.ContainsKey(uuid))
                return _cachedScenes[uuid];

            var request = new RestRequest("scenes/" + uuid, HttpMethod.Get);
            
            PrepareRequest(request);
            var result = await _httpClient.ExecuteAsync<Scene>(request);


            if (_cachedScenes == null)
                _cachedScenes = new Dictionary<Guid, Scene>();

            if (_cachedScenes.ContainsKey(uuid))
                _cachedScenes.Remove(uuid);
            if (allowCache)
            {
                _cachedScenes.Add(uuid, result);
            }
            return result;
        }

        /// <summary>
        /// Finds a scene by name. Will call GetSceneAsync if no scenes are loaded yet.
        /// Will use cached data whenever possible.
        /// </summary>
        /// <param name="sceneName">Scene name</param>
        /// <returns>A Scene instance</returns>
        public async Task<Scene> GetSceneAsync(string sceneName)
        {
            var scenes = await GetScenesAsync(true);
            var scene = scenes.First(e => e.Name == sceneName);
            return scene;
        }
        /// <summary>
        /// Runs a scene
        /// </summary>
        /// <param name="sceneName">Scene name</param>
        /// <returns>Async Task</returns>
        public async Task RunScene(string sceneName)
        {
            var scene = await GetSceneAsync(sceneName);
            await RunScene(scene);
        }
        /// <summary>
        /// Runs a scene
        /// </summary>
        /// <param name="scene">Scene</param>
        /// <returns>Async Task</returns>
        public async Task RunScene(Scene scene)
        {
            await RunScene(scene.Uuid);
        }

        /// <summary>
        /// Runs a scene
        /// </summary>
        /// <param name="uuid">Scene UUID</param>
        /// <returns>Async Task</returns>
        public async Task RunScene(Guid uuid)
        {
            var request = new RestRequest("scenes/" + uuid + "/run", HttpMethod.Get);

            PrepareRequest(request);
            await _httpClient.ExecuteAsync<Scene>(request);
        }
    }
}
