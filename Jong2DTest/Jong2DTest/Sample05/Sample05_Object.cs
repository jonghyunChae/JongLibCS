using Jong2D;
using Jong2D.Utility;

namespace Jong2DTest
{
    public class Grass
    {
        static Image image;
        Vector2D pos;
        static Grass()
        {
            Grass.image = Context.LoadImage(@"Resources\grass.png");
            Program.Resources.Add(Grass.image);
        }
        public Grass(int x, int y)
        {
            pos = new Vector2D(x, y);
        }

        public void Render()
        {
            Grass.image.Render(pos.x, pos.y);
        }
    }

    public class Text
    {
        private Font font;
        private Vector2D pos;

        public string Content { get; set; }
        public Color Color = new Color();

        public Text(int x, int y, string content, int size = 20)
        {
            pos = new Vector2D(x, y);
            font = Context.LoadFont(@"Resources\ConsolaMalgun.TTF", size);
            Program.Resources.Add(font);

            Content = content;
        }

        public void Render()
        {
            font.Render(pos.x, pos.y, Content, Color);
        }
    }

    public class Character
    {
        static Image image;
        public Vector2D Pos = new Vector2D();

        private int frame { get; set; }
        private Rectangle imageFrame = new Rectangle(0, 0, 100, 100);

        static Character()
        {
            Character.image = Context.LoadImage(@"Resources\run_animation.png");
            Program.Resources.Add(Character.image);
        }
        public Character(int x, int y)
        {
            Pos = new Vector2D(x, y);
        }
        public void Render()
        {
            imageFrame.x = frame * 100;
            Character.image.ClipRender(imageFrame, Pos);
        }

        public void Update()
        {
            Pos.x += 2;
            frame = (frame + 1) % 8;
        }
    }

}
