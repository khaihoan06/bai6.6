
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.BUS;

namespace LibraryManagement.GUI
{
    public class FrmThongKe : Form
    {
        private readonly PhieuMuonBUS bus = new PhieuMuonBUS();

        private Label lblTongPhieu = null!;
        private Label lblQuaHan = null!;

        private DataGridView dgvTheLoai = null!;
        private DataGridView dgvQuaHan = null!;

        public FrmThongKe()
        {
            InitializeComponent();
            LoadThongKe();
        }

        private void InitializeComponent()
        {
            Text = "THỐNG KÊ THƯ VIỆN";
            Size = new Size(1150, 750);
            MinimumSize = new Size(1000, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);

            Label lblTitle = new Label
            {
                Text = "THỐNG KÊ HOẠT ĐỘNG THƯ VIỆN",
                Dock = DockStyle.Top,
                Height = 65,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 19, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(35, 55, 80)
            };

            // PANEL THONG KE TONG QUAT
            Panel pnlTongQuan = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150,
                BackColor = Color.FromArgb(245, 247, 250)
            };

            Panel pnlTongPhieu = CreateStatPanel(
                "TỔNG SỐ PHIẾU MƯỢN",
                Color.FromArgb(41, 128, 185),
                30);

            lblTongPhieu = (Label)pnlTongPhieu.Controls[1];

            Panel pnlQuaHan = CreateStatPanel(
                "PHIẾU MƯỢN QUÁ HẠN",
                Color.FromArgb(211, 84, 0),
                390);

            lblQuaHan = (Label)pnlQuaHan.Controls[1];

            pnlTongQuan.Controls.Add(pnlTongPhieu);
            pnlTongQuan.Controls.Add(pnlQuaHan);

            // PANEL NOI DUNG
            Panel pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(15),
                BackColor = Color.White
            };

            SplitContainer split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 230,
                BackColor = Color.LightGray
            };

            // THONG KE THEO THE LOAI
            Panel pnlTheLoai = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            Label lblTheLoai = new Label
            {
                Text = "THỐNG KÊ SỐ LƯỢT MƯỢN THEO THỂ LOẠI",
                Dock = DockStyle.Top,
                Height = 35,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 55, 80)
            };

            dgvTheLoai = CreateGrid();

            pnlTheLoai.Controls.Add(dgvTheLoai);
            pnlTheLoai.Controls.Add(lblTheLoai);

            // DANH SACH QUA HAN
            Panel pnlDanhSachQuaHan = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            Label lblDanhSachQuaHan = new Label
            {
                Text = "DANH SÁCH PHIẾU MƯỢN QUÁ HẠN",
                Dock = DockStyle.Top,
                Height = 35,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(192, 57, 43)
            };

            dgvQuaHan = CreateGrid();

            pnlDanhSachQuaHan.Controls.Add(dgvQuaHan);
            pnlDanhSachQuaHan.Controls.Add(lblDanhSachQuaHan);

            split.Panel1.Controls.Add(pnlTheLoai);
            split.Panel2.Controls.Add(pnlDanhSachQuaHan);

            pnlContent.Controls.Add(split);

            // NUT CAP NHAT
            Panel pnlButton = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.White
            };

            Button btnCapNhat = new Button
            {
                Text = "CẬP NHẬT THỐNG KÊ",
                Size = new Size(190, 40),
                Location = new Point(20, 10),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnCapNhat.Click += BtnCapNhat_Click;

            pnlButton.Controls.Add(btnCapNhat);

            Controls.Add(pnlContent);
            Controls.Add(pnlButton);
            Controls.Add(pnlTongQuan);
            Controls.Add(lblTitle);
        }

        // TAO PANEL THONG KE
        private Panel CreateStatPanel(
            string title,
            Color color,
            int x)
        {
            Panel panel = new Panel
            {
                Location = new Point(x, 20),
                Size = new Size(320, 105),
                BackColor = color
            };

            Label lblName = new Label
            {
                Text = title,
                Location = new Point(15, 12),
                Size = new Size(290, 30),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Label lblValue = new Label
            {
                Text = "0",
                Location = new Point(15, 45),
                Size = new Size(290, 50),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 24, FontStyle.Bold)
            };

            panel.Controls.Add(lblName);
            panel.Controls.Add(lblValue);

            return panel;
        }

        // TAO BANG
        private DataGridView CreateGrid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                AutoGenerateColumns = true
            };
        }

        // TAI DU LIEU THONG KE
        private void LoadThongKe()
        {
            try
            {
                bus.UpdateOverdueStatus();

                DataTable dtPhieu = bus.GetAll();
                DataTable dtQuaHan = bus.GetOverdue();
                DataTable dtTheLoai = bus.ThongKeTheLoai();

                lblTongPhieu.Text = dtPhieu.Rows.Count.ToString();
                lblQuaHan.Text = dtQuaHan.Rows.Count.ToString();

                dgvTheLoai.DataSource = dtTheLoai;
                dgvQuaHan.DataSource = dtQuaHan;

                FormatColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải thống kê!\n" + ex.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // DINH DANG COT
        private void FormatColumns()
        {
            if (dgvQuaHan.Columns["MaPhieu"] != null)
                dgvQuaHan.Columns["MaPhieu"]!.HeaderText = "Mã phiếu";

            if (dgvQuaHan.Columns["MaDocGia"] != null)
                dgvQuaHan.Columns["MaDocGia"]!.HeaderText = "Mã độc giả";

            if (dgvQuaHan.Columns["HoTen"] != null)
                dgvQuaHan.Columns["HoTen"]!.HeaderText = "Tên độc giả";

            if (dgvQuaHan.Columns["HanTra"] != null)
            {
                dgvQuaHan.Columns["HanTra"]!.HeaderText = "Hạn trả";
                dgvQuaHan.Columns["HanTra"]!
                    .DefaultCellStyle.Format = "dd/MM/yyyy";
            }

            if (dgvQuaHan.Columns["SoNgayQuaHan"] != null)
                dgvQuaHan.Columns["SoNgayQuaHan"]!.HeaderText =
                    "Số ngày quá hạn";
        }

        // CAP NHAT
        private void BtnCapNhat_Click(object? sender, EventArgs e)
        {
            LoadThongKe();
            MessageBox.Show("Đã cập nhật thống kê!");
        }
    }
}
