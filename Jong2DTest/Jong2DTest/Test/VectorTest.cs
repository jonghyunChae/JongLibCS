using Jong2D;
using Jong2D.Utility;
using System;
using System.Collections.Generic;

namespace Jong2DTest
{
    class Program
    {
        //Screen dimension constants
        private const int SCREEN_WIDTH = 800;
        private const int SCREEN_HEIGHT = 480;
        private static List<IResource> Resources = new List<IResource>();

        static void Main(string[] args)
        {
            Context.CreateWindow(Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT);
            // 종료 이벤트 등록
            Context.OnClosed += Close;

            // 리소스 생성
            Font font = Context.LoadFont(@"Resources\ConsolaMalgun.TTF", 16);
            Image grass = Context.LoadImage(@"Resources\grass.png");
            Image character = Context.LoadImage(@"Resources\character.png");
            // Music music = Context.LoadMusic(@"Resources\background.mp3");

            Resources.Add(font);
            Resources.Add(grass);
            Resources.Add(character);
            // Resources.Add(music);

            // 게임 루프
            // music.PlayRepeat();
            var pos1 = new Vector2D(0, 80);
            var target = new Vector2D(180, 190);
            var pos1Rotator = (target - pos1).ToRotator();

            // var rectangle = new Rectangle(new Vector2D(0, 0), new Size2D(100, 100));
            while (pos1.x < 800)
            {
                // 이벤트 처리
                Context.GetGameEvents();

                // 로직 처리
                pos1.x += 1;
                pos1.y += 1;

                // 화면 초기화
                Context.ClearWindow();

                // 렌더링 
                font.Render(100, 300, "Sample1", new Color(100, 25, 25));
                grass.Render(Program.SCREEN_WIDTH / 2, 30);

                // Pos1 - red 점
                DrawPos1();

                // Pos1의 x방향벡터 : green, y방향벡터 : blue
                DrawRotator(pos1, pos1Rotator, 200);
                
                // Target : cyan
                DrawTarget();

                // 접점 : pink
                DrawTargetToConcatPointOfPitch();

                Context.DrawLine(pos1, target, new Color(255, 0, 0));

                // 페이지 플리핑
                Context.UpdateWindow();

                Context.Delay(0.05);
            }

            Context.CloseWindow();

            void DrawPos1()
            {
                Context.DrawPoint(pos1, 10, new Color(255, 0, 0));
            }

            void DrawRotator(Vector2D pos, Rotator2D rotator, int size)
            {
                Context.DrawLine(pos, pos + rotator.Pitch * size, new Color(0, 255, 0));
                Context.DrawLine(pos, pos + rotator.Yaw * size, new Color(0, 0, 255));
            }

            void DrawTarget()
            {
                Context.DrawPoint(target, 10, new Color(0, 255, 255));
            }

            void DrawTargetToConcatPointOfPitch()
            {
                var toTarget = target - pos1;
                var proj = toTarget.Dot(pos1Rotator.Pitch);
                var contactPointOfPitch = pos1 + (pos1Rotator.Pitch * proj);

                Context.DrawLine(target, contactPointOfPitch, new Color(255, 50, 255));
            }
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