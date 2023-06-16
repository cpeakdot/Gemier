namespace Gemier.GridSpace
{
    public class Grid
    {
        int x;
        int z;
        UnityEngine.GameObject visual;
        private GridSystem gridSystem;
        public int GetX => x;
        public int GetZ => z;
        public GridSystem GetGridSystem => gridSystem;
        public UnityEngine.GameObject GetVisual => visual;
        

        public Grid(int x, int z, GridSystem gridSystem)
        {
            this.x = x;
            this.z = z;
            this.gridSystem = gridSystem;
        }

        public void SetVisual(UnityEngine.GameObject visual)
        {
            this.visual = visual;
        }

        public override string ToString()
        {
            return "x = " + x + ", z = " + z;
        }

    }
}

