using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using GennadichGame.Enums;
using GennadichGame.Scenes;
using GennadichGame.Controls;

namespace GennadichGame.Manager
{
    public delegate void SceneChangedEventHandler(GameState previousState);
    public class SceneManager : GameModule, IEnumerable<KeyValuePair<GameState, Scene>>
    {
        public event SceneChangedEventHandler OnSceneChanged;
        private Dictionary<GameState, Scene> Scenes { get; } = new Dictionary<GameState, Scene>();
        public Scene ActiveScene => Scenes.Values.FirstOrDefault(scene => scene.Active);
        public GameState ActiveState
        {
            get { return Scenes.FirstOrDefault(scene => scene.Value.Active).Key; }
            set { SetActiveScene(value); }
        }
        public Scene this[GameState scene] => Scenes[scene];
        public SceneManager() { }
        public SceneManager(params KeyValuePair<GameState, Scene>[] scenes) => AddScene(scenes);
        public SceneManager(params (GameState state, Scene scene)[] scenes) => AddScene(scenes);
        public void AddScene(params KeyValuePair<GameState, Scene>[] scenes)
        {
            foreach (var scene in scenes) Scenes.Add(scene.Key, scene.Value);
        }
        public void AddScene(params (GameState state, Scene scene)[] scenes)
        {
            foreach (var scene in scenes) Scenes.Add(scene.state, scene.scene);
        }
        public void SetActiveScene(GameState scene)
        {
            if (ActiveState == scene && Scenes.Select(sc => sc.Value).FirstOrDefault(sc => sc.Active) != null) return;

            var currentState = ActiveState;
            var currentScene = ActiveScene;

            if (currentScene != null) currentScene.Deactivate();
            foreach (var sc in Scenes)
            {
                if (sc.Key == scene) sc.Value.Activate();
            }

            if (OnSceneChanged != null) OnSceneChanged.Invoke(currentState);
        }
        public Scene GetScene(GameState scene)
        {
            return Scenes[scene];
        }
        public T GetScene<T>(GameState scene) where T : Scene
        {
            return (T)Scenes[scene];
        }
        public IEnumerable<KeyValuePair<GameState, Scene>> GetScene<T>() where T : Scene
        {
            return Scenes.Where(scene => scene.Value.GetType() == typeof(T)).Select(scene => new KeyValuePair<GameState, Scene>(scene.Key, (T)scene.Value));
        }
        public IEnumerator<KeyValuePair<GameState, Scene>> GetEnumerator()
        {
            return Scenes.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Scenes.GetEnumerator();
        }
    }
}
