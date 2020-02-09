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
            this.pos = new Vector2D(x, y);
        }

        public void Render()
        {
            Grass.image.Render(this.pos.x, this.pos.y);
        }
    }

    public class Text
    {
        private Font font;
        private Vector2D pos;

        public string Content { get; set; }
        public Color Color = new Color();

        public Text(int x, int y, int size = 20)
        {
            pos = new Vector2D(x, y);
            this.font = Context.LoadFont(@"Resources\ConsolaMalgun.TTF", size);
            Program.Resources.Add(this.font);
        }

        public void Render()
        {
            if (string.IsNullOrEmpty(this.Content) == false)
            {
                this.font.Render(pos.x, pos.y, this.Content, this.Color);
            }
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
            this.Pos = new Vector2D(x, y);
        }
        public void Render()
        {
            this.imageFrame.x = this.frame * 100;
            Character.image.ClipRender(this.imageFrame, this.Pos);
        }

        public void Update()
        {
            Pos.x += 2;
            this.frame = (this.frame + 1) % 8;
        }
    }

}
