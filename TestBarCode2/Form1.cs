using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZXing;
using NLog;
using System.Text.Json;
using System.IO;

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
        private TreeNode selectedNode = null;
        public Form1()
        {
            InitializeComponent();

            lvRoi.CheckBoxes = true;

            // 🔹 Thêm các loại barcode vào ComboBox
            cbxBarcodeFormat.Items.Add("Tự động (Auto)");
            cbxBarcodeFormat.Items.Add("CODE_128");
            cbxBarcodeFormat.Items.Add("CODE_39");
            cbxBarcodeFormat.Items.Add("CODE_93");
            cbxBarcodeFormat.Items.Add("EAN_8");
            cbxBarcodeFormat.Items.Add("EAN_13");
            cbxBarcodeFormat.Items.Add("UPC_A");
            cbxBarcodeFormat.Items.Add("UPC_E");
            cbxBarcodeFormat.Items.Add("ITF");
            cbxBarcodeFormat.Items.Add("CODABAR");
            cbxBarcodeFormat.Items.Add("QR_CODE");
            cbxBarcodeFormat.Items.Add("DATA_MATRIX");
            cbxBarcodeFormat.Items.Add("PDF_417");
            cbxBarcodeFormat.Items.Add("AZTEC");
            cbxBarcodeFormat.Items.Add("MAXICODE");
            cbxBarcodeFormat.Items.Add("MSI");
            cbxBarcodeFormat.Items.Add("PLESSEY");

            cbxBarcodeFormat.SelectedIndex = 0;
            cbxBarcodeFormat.SelectedIndexChanged += cbxBarcodeFormat_SelectedIndexChanged;

            lvRoi.CheckBoxes = true;
            lvRoi.AfterSelect += lvRoi_AfterSelect;

            roiX.KeyDown += RoiTextbox_KeyDown;
            roiY.KeyDown += RoiTextbox_KeyDown;
            roiW.KeyDown += RoiTextbox_KeyDown;
            roiH.KeyDown += RoiTextbox_KeyDown;

            // Gắn event
            picImage.MouseDown += picImage_MouseDown;
            picImage.MouseMove += picImage_MouseMove;
            picImage.MouseUp += picImage_MouseUp;
            picImage.Paint += picImage_Paint;

            btnDeleteROI.Click += btnDeleteROI_Click;
            
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
                selectedRoiIndex = e.Node.Index;
                selectedNode = e.Node;

                // Nếu node này đã có thuật toán thì hiển thị lại trên ComboBox
                if (selectedNode.Tag != null)
                {
                    string algo = selectedNode.Tag.ToString();
                    int index = cbxBarcodeFormat.Items.IndexOf(algo);
                    if (index >= 0)
                        cbxBarcodeFormat.SelectedIndex = index;
                    else
                        cbxBarcodeFormat.SelectedIndex = 0; // Mặc định "Tự động (Auto)"
                }

                picImage.Invalidate();
            }
            else
            {
                selectedNode = null;
                selectedRoiIndex = -1;
                picImage.Invalidate();
            }
            if (selectedRoiIndex >= 0 && selectedRoiIndex < roiList.Count)
            {
                Rectangle roi = roiList[selectedRoiIndex];
                roiX.Text = roi.X.ToString();
                roiY.Text = roi.Y.ToString();
                roiW.Text = roi.Width.ToString();
                roiH.Text = roi.Height.ToString();
            }
            else
            {
                roiX.Text = roiY.Text = roiW.Text = roiH.Text = "";
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
            node.Checked = true; // ✅ Mặc định được chọn để đọcgggggggggg
            lvRoi.Nodes[0].Nodes.Add(node);//htfhfgh
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
        private void SaveToJson(string filePath)
        {
            RootModel data = new RootModel();

            for (int i = 0; i < roiList.Count; i++)
            {
                var rect = roiList[i];
                bool enabled = false;
                string algo = "default";

                if (lvRoi.Nodes.Count > 0 && lvRoi.Nodes[0].Nodes.Count > i)
                {
                    var node = lvRoi.Nodes[0].Nodes[i];
                    enabled = node.Checked;
                    if (node.Tag != null)
                        algo = node.Tag.ToString();
                }

                data.Model.Roi.Add(new Roi
                {
                    Id = i + 1,
                    X = rect.X,
                    Y = rect.Y,
                    W = rect.Width,
                    H = rect.Height,
                    Enable = enabled,
                    Thuat_toan = algo
                });
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);

            MessageBox.Show("✅ Đã lưu file JSON!");
        }

        private void LoadFromJson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Không tìm thấy file JSON!");
                return;
            }

            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<RootModel>(json);

            roiList.Clear();
            lvRoi.Nodes.Clear();
            TreeNode root = new TreeNode("Mode");
            lvRoi.Nodes.Add(root);

            foreach (var roi in data.Model.Roi)
            {
                roiList.Add(new Rectangle(roi.X, roi.Y, roi.W, roi.H));

                TreeNode node = new TreeNode($"ROI{roi.Id}")
                {
                    Checked = roi.Enable,
                    Tag = roi.Thuat_toan ?? "default"
                };
                lvRoi.Nodes[0].Nodes.Add(node);
            }

            lvRoi.Nodes[0].Expand();
            picImage.Invalidate();
        }
        private void cbxBarcodeFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedNode != null)
            {
                string algo = cbxBarcodeFormat.SelectedItem.ToString();
                selectedNode.Tag = algo; // 🔹 Gán thuật toán vào ROI đang chọn
            }
        }
        private void RoiTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (selectedRoiIndex < 0 || selectedRoiIndex >= roiList.Count)
                return;

            TextBox tb = sender as TextBox;
            if (tb == null) return;

            // Lấy ROI hiện tại
            Rectangle roi = roiList[selectedRoiIndex];

            int delta = 0;
            if (e.KeyCode == Keys.Up) delta = 1;
            else if (e.KeyCode == Keys.Down) delta = -1;

            if (delta == 0) return;

            // Xác định textbox nào được chỉnh
            if (tb == roiX)
                roi.X += delta;
            else if (tb == roiY)
                roi.Y += delta;
            else if (tb == roiW)
                roi.Width = Math.Max(1, roi.Width + delta);
            else if (tb == roiH)
                roi.Height = Math.Max(1, roi.Height + delta);

            // Cập nhật lại ROI trong danh sách
            roiList[selectedRoiIndex] = roi;

            // Cập nhật text hiển thị
            roiX.Text = roi.X.ToString();
            roiY.Text = roi.Y.ToString();
            roiW.Text = roi.Width.ToString();
            roiH.Text = roi.Height.ToString();

            // Vẽ lại hình
            picImage.Invalidate();

            // Ngăn chặn tiếng "bíp" khi nhấn phím
            e.SuppressKeyPress = true;
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

        private void btnSaveJson_Click(object sender, EventArgs e)
        {
            SaveToJson("config.json");
        }

        private void btnLoadJson_Click(object sender, EventArgs e)
        {
            LoadFromJson("config.json");
        }
    }
}
