using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZXing;
using NLog;

namespace TestBarCode2
{
    public partial class Form1 : Form
    {
        private static readonly NLog.Logger logger1 = NLog.LogManager.GetCurrentClassLogger();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private int selectedRoiIndex = -1;
        private List<Rectangle> roiList = new List<Rectangle>();
        private Point startPoint;
        private bool isDrawing = false;
        private Rectangle currentRect;

        public Form1()
        {
            InitializeComponent();

            lvRoi.CheckBoxes = true;

            // 🔹 Thêm các loại barcode vào ComboBox
            cbxBarcodeFormat.Items.Add("Tự động (Auto)");
            cbxBarcodeFormat.Items.Add("CODE_128");
            cbxBarcodeFormat.Items.Add("QR_CODE");
            cbxBarcodeFormat.Items.Add("EAN_13");
            cbxBarcodeFormat.Items.Add("CODE_39");
            cbxBarcodeFormat.Items.Add("PDF_417");
            cbxBarcodeFormat.Items.Add("DATA_MATRIX");
            cbxBarcodeFormat.SelectedIndex = 0;

            lvRoi.CheckBoxes = true;
            lvRoi.AfterSelect += lvRoi_AfterSelect;

            // Gắn event
            picImage.MouseDown += picImage_MouseDown;
            picImage.MouseMove += picImage_MouseMove;
            picImage.MouseUp += picImage_MouseUp;
            picImage.Paint += picImage_Paint;

            btnLoadImage.Click += btnLoadImage_Click;
            btnDeleteROI.Click += btnDeleteROI_Click;
            btnReadBarcode.Click += btnReadBarcode_Click;
        }

        // 🔹 Nút Load ảnh
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Ảnh (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                picImage.Image = Image.FromFile(ofd.FileName);

                roiList.Clear();
                lvRoi.Nodes.Clear();

                TreeNode root = new TreeNode("Mode");
                lvRoi.Nodes.Add(root);
            }
        }

        // 🔹 Nút Xóa ROI
        private void btnDeleteROI_Click(object sender, EventArgs e)
        {
            if (lvRoi.Nodes.Count == 0 || lvRoi.SelectedNode == null)
            {
                MessageBox.Show("Chưa chọn ROI để xóa!");
                return;
            }

            TreeNode selectedNode = lvRoi.SelectedNode;

            if (selectedNode.Parent != null)
            {
                int index = selectedNode.Index;

                if (index >= 0 && index < roiList.Count)
                    roiList.RemoveAt(index);

                selectedNode.Remove();

                // Cập nhật lại tên ROI
                lvRoi.Nodes[0].Nodes.Clear();
                for (int i = 0; i < roiList.Count; i++)
                    lvRoi.Nodes[0].Nodes.Add(new TreeNode($"ROI{i + 1}"));

                picImage.Invalidate();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn ROI cụ thể để xóa (không phải node gốc).");
            }
        }
        private void lvRoi_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent != null) // Bỏ qua node gốc "Mode"
            {
                selectedRoiIndex = e.Node.Index;  // Ghi lại ROI đang được chọn
                picImage.Invalidate();             // Vẽ lại ảnh để đổi màu ROI
            }
            else
            {
                selectedRoiIndex = -1; // Không chọn ROI nào
                picImage.Invalidate();
            }
        }
        // 🔹 Mouse event
        private void picImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (picImage.Image == null) return;
            isDrawing = true;
            startPoint = e.Location;
        }

        private void picImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                currentRect = GetRectFromPoints(startPoint, e.Location);
                picImage.Invalidate();
            }
        }

        private void picImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;
            isDrawing = false;

            Rectangle roi = GetRectFromPoints(startPoint, e.Location);
            roiList.Add(roi);

            if (lvRoi.Nodes.Count == 0)
                lvRoi.Nodes.Add("Mode");

            TreeNode node = new TreeNode($"ROI{roiList.Count}");
            node.Checked = true; // ✅ Mặc định được chọn để đọc
            lvRoi.Nodes[0].Nodes.Add(node);
            lvRoi.Nodes[0].Expand();

            picImage.Invalidate();
        }

        private Rectangle GetRectFromPoints(Point p1, Point p2)
        {
            return new Rectangle(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Abs(p1.X - p2.X),
                Math.Abs(p1.Y - p2.Y));
        }

        private void picImage_Paint(object sender, PaintEventArgs e)
        {
            if (picImage.Image == null) return;

            for (int i = 0; i < roiList.Count; i++)
            {
                var rect = roiList[i];
                using (Pen pen = new Pen(i == selectedRoiIndex ? Color.LimeGreen : Color.Red, 2))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }

            if (isDrawing)
            {
                using (Pen dashedPen = new Pen(Color.Blue, 1))
                {
                    dashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    e.Graphics.DrawRectangle(dashedPen, currentRect);
                }
            }
        }

        // 🔹 Đọc barcode
        private void btnReadBarcode_Click(object sender, EventArgs e)
        {
           
            if (picImage.Image == null)
            {
                MessageBox.Show("Vui lòng chọn ảnh trước!");
                return;
            }

            if (roiList.Count == 0)
            {
                MessageBox.Show("Chưa có ROI nào được chọn!");
                return;
            }

            Bitmap fullBitmap = new Bitmap(picImage.Image);
            StringBuilder results = new StringBuilder();

            // 🔸 Cấu hình ZXing reader
            var options = new ZXing.Common.DecodingOptions
            {
                TryHarder = true,
                PossibleFormats = new List<ZXing.BarcodeFormat>()
            };

            string selectedFormat = cbxBarcodeFormat.SelectedItem.ToString();

            if (selectedFormat != "Tự động (Auto)")
            {
                try
                {
                    var formatEnum = (ZXing.BarcodeFormat)Enum.Parse(typeof(ZXing.BarcodeFormat), selectedFormat);
                    options.PossibleFormats.Add(formatEnum);
                }
                catch
                {
                    MessageBox.Show("Định dạng không hợp lệ!");
                }
            }

            BarcodeReader reader = new BarcodeReader
            {
                Options = options
            };

            foreach (TreeNode node in lvRoi.Nodes[0].Nodes)
            {
                if (!node.Checked) continue; // ❌ Bỏ qua ROI không được chọn

                int index = node.Index;
                Rectangle roi = roiList[index];

                // Chuyển đổi tọa độ ROI sang vùng ảnh thực
                float scaleX = (float)fullBitmap.Width / picImage.ClientSize.Width;
                float scaleY = (float)fullBitmap.Height / picImage.ClientSize.Height;

                Rectangle scaledRoi = new Rectangle(
                    (int)(roi.X * scaleX),
                    (int)(roi.Y * scaleY),
                    (int)(roi.Width * scaleX),
                    (int)(roi.Height * scaleY)
                );

                Bitmap roiBitmap = new Bitmap(scaledRoi.Width, scaledRoi.Height);
                using (Graphics g = Graphics.FromImage(roiBitmap))
                {
                    g.DrawImage(fullBitmap,
                        new Rectangle(0, 0, roiBitmap.Width, roiBitmap.Height),
                        scaledRoi, GraphicsUnit.Pixel);
                }

                var result = reader.Decode(roiBitmap);

                if (result != null)
                {
                    string message = $"✅ {node.Text}: [{result.BarcodeFormat}] {result.Text}";
                    results.AppendLine(message);
                    logger.Info(message); // 📄 Ghi log thành công
                }
                else
                {
                    string message = $"❌ {node.Text}: Không đọc được mã.";
                    results.AppendLine(message);
                    logger.Warn(message); // ⚠️ Ghi log cảnh báo nếu không đọc được
                }

                roiBitmap.Dispose();
            }

            // 🔹 Sau khi quét hết ROIs — hiển thị toàn bộ kết quả cùng lúc
            MessageBox.Show(results.ToString(), "Kết quả đọc barcode", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}
