using Jong2D.Utility;
using System;
using System.Collections.Generic;

namespace Jong2D.Framework
{
    public interface ICamera
    {
        void SetCamera(ref Vector2D pos);
        Vector2D ToScreenPos(ref Vector2D pos);
    }

    public interface IGameObject
    {
        void Render();
        void Update(double frame_time);
        void EventHandle(GameEvent e, double frame_time);
    }

    public interface IScene
    {
        void Enter();
        void Exit();

        void Pause();
        void Resume();

        void Update(double frame_time);
        void HandleEvents(GameEvent e, double frame_time);
        void Render();
    }

    public class Framework
    {
        static Framework instance = new Framework();
        public static Framework Instance => instance;

        public bool Stop { get; set; }
        bool running { get; set; }
        Stack<IScene> scenes { get; set; }
        IScene nextScene { get; set; }
        IScene CurrentScene => scenes.Peek();

        public event Action Closed;

        private Framework()
        {
            Stop = false;
            running = false;
            scenes = new Stack<IScene>();
            nextScene = null;
        }

        public void Run(IScene start_scene)
        {
            if (running)
            {
                throw new Exception("Framework is already started!");
            }

            running = true;

            scenes.Push(start_scene);
            start_scene.Enter();

            DateTime current_time = DateTime.Now;
            while (running)
            {
                DateTime now = DateTime.Now;
                double frame_time = (now - current_time).TotalSeconds;
                if (frame_time <= 0)
                {
                    continue;
                }
                current_time = now;

                IScene scene = CurrentScene;
                handleEvent(scene, frame_time);
                update(scene, frame_time);
                render(scene);

                if (nextScene != null)
                {
                    setScene(nextScene);
                    nextScene = null;
                }
            }

            while (PopScene())
            {
            }

            Closed?.Invoke();
            Closed = null;
        }

        private void handleEvent(IScene scene, double frame_time)
        {
            foreach (var gameEvent in Context.GetGameEvents())
            {
                scene.HandleEvents(gameEvent, frame_time);
            }
        }

        private void update(IScene scene, double frame_time)
        {
            if (Stop == false)
            {
                scene.Update(frame_time);
            }
        }

        private void render(IScene scene)
        {
            Context.ClearWindow();
            scene.Render();
            Context.UpdateWindow();
        }

        private void setScene(IScene scene)
        {
            PopScene();

            scenes.Push(scene);
            scene.Enter();
        }

        public void ChangeScene(IScene scene)
        {
            nextScene = scene;
        }

        public void PushScene(IScene scene)
        {
            if (scenes.Count > 0)
            {
                CurrentScene.Pause();
            }

            scenes.Push(scene);
            scene.Enter();
        }

        public bool PopScene()
        {
            if (scenes.Count > 0)
            {
                CurrentScene.Exit();
                scenes.Pop();
                return true;
            }
            return false;
        }

        public void Quit()
        {
            running = false;
        }
    }
}
