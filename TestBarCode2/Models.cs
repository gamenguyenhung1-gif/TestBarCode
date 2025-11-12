using System.Collections.Generic;

namespace TestBarCode2
{
    public class Roi
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public bool Enable { get; set; }
        public string Thuat_toan { get; set; }  // 👈 thuật toán riêng cho ROI
    }

    public class Model
    {
        public List<Roi> Roi { get; set; } = new List<Roi>();
    }

    public class RootModel
    {
        public Model Model { get; set; } = new Model();
    }
}
