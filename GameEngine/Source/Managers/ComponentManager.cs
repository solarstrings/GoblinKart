using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Interfaces;

namespace GameEngine.Managers
{
    /// <summary>
    /// Class to handle components: Add, remove and  retrive components 
    /// Thread safe singleton without using locks
    /// See link: "http://csharpindepth.com/Articles/General/Singleton.aspx#nested-cctor"
    /// </summary>
    public sealed class ComponentManager
    {
        static ComponentManager() { }
        private ComponentManager() { }

        public static ComponentManager Instance { get; } = new ComponentManager();

        public ConcurrentDictionary <Type, ConcurrentDictionary<Entity, IComponent>> ComponentsDictionary = new ConcurrentDictionary<Type, ConcurrentDictionary<Entity, IComponent>>();

        /// <summary>
        /// This method register a component "to" an entity 
        /// </summary>
        /// <param name="entity"> Entity to be connected to the component </param>
        /// <param name="component">Component to be added </param>
        public void AddComponentToEntity(Entity entity, IComponent component)
        {
            Type type = component.GetType();

            if (!ComponentsDictionary.ContainsKey(type))
            {
                ComponentsDictionary.TryAdd(type, new ConcurrentDictionary<Entity, IComponent>());
            }

            ComponentsDictionary[type][entity] = component;
        }

        /// <summary>
        /// This method removes a component "from" an entity
        /// </summary>
        /// <typeparam name="T"> The type of the component to be removedd</typeparam>
        /// <param name="entity"> The entity from which the component is removed from </param>
        public void RemoveComponentFromEntity<T>(Entity entity) where T : IComponent
        {
            var type = typeof(T);
            if (ComponentsDictionary.ContainsKey(type))
                if (ComponentsDictionary[type].ContainsKey(entity))
                {
                    IComponent c;
                    Console.WriteLine(ComponentsDictionary[type].TryRemove(entity, out c) == false
                        ? "failed to remove component"
                        : "removed component");
                }
        }

        /// <summary>
        /// This method returns a specific component connected to a user given entity
        /// </summary>
        /// <typeparam name="T"> The type of the component to be retrived </typeparam>
        /// <param name="entity"> The entity the component is connected to </param>
        /// <returns> The requested component, else null</returns>
        public T GetEntityComponent<T>(Entity entity) where T : class, IComponent
        {
            Type type = typeof(T);
            if (ComponentsDictionary.ContainsKey(type))
                if (ComponentsDictionary[type].ContainsKey(entity))
                    return (T)ComponentsDictionary[type][entity];

            return null;
        }

        /// <summary>
        /// This method returns an Entity with the given tagname from the given list of entities
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="entities"></param>
        /// <returns>The Entity with the given tag - if it exist</returns>
        /// <returns>null - if no entity with the tagName was found</returns>
        public Entity GetEntityWithTag(string tagName, List<Entity> entities)
        {
            var type = typeof(TagComponent);
            if (ComponentsDictionary.ContainsKey(type))
            {
                foreach(var e in entities)
                {
                    var t = GetEntityComponent<TagComponent>(e);
                    if (t == null) continue;
                    if (t.tagName.Equals(tagName)) return e;
                }
            }
            return null;
        }

        public Entity GetEntityOfComponent<T>(IComponent component) where T : IComponent
        {
            Type type = typeof(T);
            if (ComponentsDictionary.ContainsKey(type))
            {
                if (ComponentsDictionary[type].Values.Contains(component))
                {
                    foreach (KeyValuePair<Entity, IComponent> kv in ComponentsDictionary[type])
                    {
                        if (kv.Value == component)
                        {
                            return kv.Key;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the first entity of the given type
        /// </summary>
        /// <typeparam name="T"> The type of the component </typeparam>
        /// <returns>A list with entities, else null </returns>
        public Entity GetFirstEntityOfType<T>() where T : IComponent
        {
            Type type = typeof(T);

            if (ComponentsDictionary.ContainsKey(type))
                return ComponentsDictionary[type].Keys.First();

            return null;
        }
        /// <summary>
        /// This method returns a List of components that belonged to any of the entities
        /// that was passed in through the list.
        /// </summary>
        /// <typeparam name="T">The type of component that is to be retrived.</typeparam>
        /// <param name="entities">A list of Entity you want to check for a component from</param>
        /// <returns>A list of components or null if no was found.</returns>
        public List<T> GetComponentsFromEntities<T>(List<Entity> entities) where T : class, IComponent
        {
            Type type = typeof(T);
            if (ComponentsDictionary.ContainsKey(type))
            {
                return ComponentsDictionary[type].Where(x => entities.Contains(x.Key)).Select(x => x.Value).Cast<T>().ToList();
            }
            return null;
        }

        /// <summary>
        /// Returns all the components saved in the component managers with a specific
        /// component type
        /// </summary>
        /// <typeparam name="T"> Type of the component</typeparam>
        /// <returns> A list with all the components, else null </returns>
        public List<T> GetAllComponentsOfType<T>() where T : class, IComponent
        {
            Type type = typeof(T);
            if (ComponentsDictionary.ContainsKey(type))
            {                
                List<T> list = new List<T>();
                foreach (KeyValuePair<Entity, IComponent> entry in ComponentsDictionary[type])
                {
                    list.Add((T)entry.Value);
                }
                return list;                
            }
            return null;
        }

        /// <summary>
        /// Returns all the entities that have a specific component
        /// </summary>
        /// <typeparam name="T"> The type of the component </typeparam>
        /// <returns>A list with entities, else null </returns>
        public List<Entity> GetAllEntitiesWithComponentType<T>() where T : IComponent
        {
            Type type = typeof(T);

            if (ComponentsDictionary.ContainsKey(type))
                return ComponentsDictionary[type].Keys.ToList();

            return null;
        }

        /// <summary>
        /// Removes an entity
        /// </summary>
        /// <param name="entity"> The entity to be removed </param>
        public void RemoveEntity(Entity entity)
        {
            foreach (KeyValuePair<Type, ConcurrentDictionary<Entity, IComponent>> entry in ComponentsDictionary)
            {
                if (entry.Value.ContainsKey(entity))
                {
                    IComponent c;
                    ComponentsDictionary[entry.Key].TryRemove(entity, out c);
                }
            }
        }
    }
}
