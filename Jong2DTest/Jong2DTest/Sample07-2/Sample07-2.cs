using Jong2D;
using Jong2D.Utility;
using SDL2;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace Jong2DTest
{
    /* 
        과제 :
        1. ENTER 키 입력을 하면 주인공의 정보를 저장한다.
        2. SPACE 키 입력을 하면 주인공의 정보를 로드하여 거기부터 시작한다.
        3. 게임 시작할 때, 로드할 정보가 있으면 로드하고, 아니면 지금처럼 시작한다
    */

    class Program
    {
        // 객체 저장 테스트를 위한 클래스
        class ForFileData
        {
            public Vector2D Pos { get; set; }
            private int data;
            // JSON 컨버터에게 힌트를 주는 어트리뷰트
            [JsonProperty(PropertyName = "PrivateData")]
            private int data2;

            public void init()
            {
                var rand = new Random();
                Pos = new Vector2D(rand.Next(0, 1000), rand.Next(0, 1000));
                data = rand.Next(0, 1000);
                data2 = rand.Next(0, 1000);
            }

            public override string ToString()
            {
                StringBuilder str = new StringBuilder();
                str.AppendFormat("Pos:{0}\n", Pos.ToString());
                str.AppendFormat("Data:{0}\n", data.ToString());
                str.AppendFormat("Data2:{0}\n", data2.ToString());
                return str.ToString();
            }
        }

        private const int SCREEN_WIDTH = 800;
        private const int SCREEN_HEIGHT = 480;
        private static bool CloseGame { get; set; }
        static void HandleEvents()
        {
            var events = Context.GetGameEvents();
            foreach (GameEvent e in events)
            {
                switch (e.Type)
                {
                    // 키보드 처리
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        {
                            if (e.Key == SDL.SDL_Keycode.SDLK_ESCAPE)
                                CloseGame = true;
                            if (e.Key == SDL.SDL_Keycode.SDLK_RETURN)
                            {
                                // 앤터 입력. 저장해보자
                                // 저장하기 위한 샘플 코드 예시
                                ForFileData data = new ForFileData();
                                data.init();
                                string json = JsonConvert.SerializeObject(data);
                                Console.WriteLine("Save");
                                Console.WriteLine(json);
                                Console.WriteLine("ClassData");
                                Console.WriteLine(data.ToString());
                                Console.WriteLine();
                                System.IO.File.WriteAllText("save.txt", json);
                            }

                            if (e.Key == SDL.SDL_Keycode.SDLK_SPACE)
                            {
                                // 스페이스바 입력. 로드해보자
                                // 로드하기 위한 샘플 코드 예시
                                string json = System.IO.File.ReadAllText("save.txt");
                                ForFileData data = JsonConvert.DeserializeObject<ForFileData>(json);
                                Console.WriteLine("Load");
                                Console.WriteLine(json);
                                Console.WriteLine("ClassData");
                                Console.WriteLine(data.ToString());
                                Console.WriteLine();
                            }
                        }
                        break;
                    case SDL.SDL_EventType.SDL_QUIT:
                        CloseGame = true;
                        break;
                    default:
                        break;
                }

            }

            foreach (GameEvent e in events)
            {
                EventHandle(e);
            }
        }

        static void EventHandle(GameEvent e)
        {
            foreach (var obj in GameObjects)
            {
                if (obj is IControllable)
                {
                    var target = obj as IControllable;
                    target.EventHandle(e);
                }
            }
        }

        static void Render()
        {
            Context.ClearWindow();
            foreach (var obj in GameObjects)
            {
                obj.Render();
            }
            Context.UpdateWindow();
        }

        static void Update()
        {
            foreach (var obj in GameObjects)
            {
                obj.Update();
            }
        }

        static List<IGameObject> GameObjects = new List<IGameObject>();
        public static List<IResource> Resources = new List<IResource>();
        static void Main(string[] args)
        {
            Context.CreateWindow(Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT);
            Context.OnClosed += Close;      // 종료 이벤트 등록

            // 리소스 생성
            Music music = Context.LoadMusic(@"Resources\background.mp3");
            Resources.Add(music);

            music.PlayRepeat();

            GameObjects.Add(new Text(100, 300, "Sample7")
            {
                Color = new Color(100, 25, 25),
            });
            GameObjects.Add(new Grass(Program.SCREEN_WIDTH / 2, 30));
            GameObjects.Add(new Boy(150, 80));

            // 게임 루프
            CloseGame = false;
            while (CloseGame == false)
            {
                HandleEvents();

                Update();

                Render();

                Context.Delay(0.05);
            }

            Context.CloseWindow();
        }

        static void Close()
        {
            Console.WriteLine("Close!");
            foreach (var resource in Resources)
            {
                resource.Dispose();
            }
        }
    }
}
