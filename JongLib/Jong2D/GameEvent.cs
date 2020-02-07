using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace Jong2D
{
    public class GameEvent
    {
        public int x { get; set; }
        public int y { get; set; }
        public SDL.SDL_EventType Type { get; set; }
        public SDL.SDL_Keycode Key { get; set; }
        public byte Button { get; set; }

        public GameEvent(SDL.SDL_EventType eventType)
        {
            this.Type = eventType;
        }

    }

    public static partial class Context
    {
        public static List<GameEvent> GetGameEvents()
        {
            Context.PrintFPS();
            SDL.SDL_Delay(1);

            var result = new List<GameEvent>();
            SDL.SDL_Event sdl_event;
            while(SDL.SDL_PollEvent(out sdl_event) != 0)
            {
                var gameEvent = Context.CreateEvent(sdl_event);
                result.Add(gameEvent);
            } // while

            return result;
        }

        private static GameEvent CreateEvent(SDL.SDL_Event sdl_event)
        {
            var gameEvent = new GameEvent(sdl_event.type);
            switch (gameEvent.Type)
            {
                case SDL.SDL_EventType.SDL_KEYDOWN:
                case SDL.SDL_EventType.SDL_KEYUP:
                    {
                        if (sdl_event.key.repeat == 0)
                        {
                            gameEvent.Key = sdl_event.key.keysym.sym;
                        }
                    }
                    break;
                case SDL.SDL_EventType.SDL_MOUSEMOTION:
                    {
                        gameEvent.x = sdl_event.motion.x;
                        gameEvent.y = sdl_event.motion.y;
                    }
                    break;
                case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                    {
                        gameEvent.Button = sdl_event.button.button;
                        gameEvent.x = sdl_event.button.x;
                        gameEvent.y = sdl_event.button.y;
                    }
                    break;
                default:
                    break;
            }

            return gameEvent;
        }
    }

}
