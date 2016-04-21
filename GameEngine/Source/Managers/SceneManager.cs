using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    public class SceneManager
    {
        private string activeScene = "";        

        public Dictionary<string, Scene> sceneDictionary = new Dictionary<string, Scene>();

        private static SceneManager instance;
        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SceneManager();
                return instance;
            }
        }        
        
        private SceneManager() { }

        /// <summary>
        /// Creates and adds a new Scene to the scene dictionary
        /// </summary>
        /// <param name="sceneName"></param>
        public void NewScene(string sceneName)
        {
            if(!sceneDictionary.ContainsKey(sceneName))
                sceneDictionary.Add(sceneName, new Scene());
        }

        /// <summary>
        /// Adds a entity to a layer
        /// </summary>
        /// <param name="sceneName"> The scene in which the entity is to be added </param>
        /// <param name="layer"> The layer in which the entity is to be added in </param>
        /// <param name="entity"> The entity to be added </param>
        public void AddEntityToSceneOnLayer(string sceneName, int layer, Entity entity)
        {
            if (activeScene.Equals(""))
            {
                activeScene = sceneName;
            }
            if(!sceneDictionary.ContainsKey(sceneName))
            {
                sceneDictionary.Add(sceneName, new Scene());
                sceneDictionary[sceneName].AddEntityToLayer(layer, entity);
            }
            else
            {
                sceneDictionary[sceneName].AddEntityToLayer(layer, entity);
            }
        }

        /// <summary>
        /// Removes a layer from a scene
        /// </summary>
        /// <param name="sceneName"> Scene name for the scene that contains layer </param>
        /// <param name="layer"> Layer to be removed </param>
        public void RemoveLayerFromScene(string sceneName, int layer)
        {
            if(sceneDictionary.ContainsKey(sceneName))
                sceneDictionary[sceneName].RemoveLayer(layer);
        }

        /// <summary>
        /// Removes an entity from a layer
        /// </summary>
        /// <param name="sceneName"> Scene name for the scene that contains the entity </param>
        /// <param name="layer"> The layer which contains the entity</param>
        /// <param name="entity"> The entity to be removed </param>
        public void RemoveEntityFromLayer(string sceneName, int layer, Entity entity)
        {
            if(sceneDictionary.ContainsKey(sceneName))
            {
                sceneDictionary[sceneName].RemoveEntityFromLayer(layer, entity);
            }
        }

        /// <summary>
        /// This function sets the active scene
        /// </summary>
        /// <param name="sceneName">The name of the scene to set as the active one.</param>
        public void SetActiveScene(string sceneName)
        {
            if (!sceneDictionary.ContainsKey(sceneName))
            {
                return;
            }
            activeScene = sceneName;
        }


        /// <summary>
        /// Gets the active Scene
        /// </summary>
        /// <returns> Active scene </returns>
        public Scene GetActiveScene()
        {
            if(!string.IsNullOrEmpty(activeScene))
                return sceneDictionary[activeScene];
            return null;
        }
    }
}
