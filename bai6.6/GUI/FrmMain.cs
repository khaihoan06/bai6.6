
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.GUI
{
    public class FrmMain : Form
    {
        private Panel pnlMenu = null!;
        private Panel pnlContent = null!;
        private Label lblWelcome = null!;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "HỆ THỐNG QUẢN LÝ THƯ VIỆN";
            Size = new Size(1250, 780);
            MinimumSize = new Size(1000, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);

            // THANH TIEU DE
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(35, 55, 80)
            };

            Label lblTitle = new Label
            {
                Text = "LIBRARY MANAGEMENT SYSTEM",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18, FontStyle.Bold)
            };

            pnlHeader.Controls.Add(lblTitle);

            // MENU BEN TRAI
            pnlMenu = new Panel
            {
                Dock = DockStyle.Left,
                Width = 230,
                BackColor = Color.FromArgb(44, 62, 80)
            };

            Label lblMenu = new Label
            {
                Text = "DANH MỤC CHỨC NĂNG",
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            Button btnTrangChu = CreateMenuButton(
                "🏠  Trang chủ");

            Button btnDocGia = CreateMenuButton(
                "👤  Quản lý độc giả");

            Button btnDauSach = CreateMenuButton(
                "📚  Quản lý đầu sách");

            Button btnPhieuMuon = CreateMenuButton(
                "📖  Quản lý mượn trả");

            Button btnThongKe = CreateMenuButton(
                "📊  Thống kê thư viện");

            Button btnThoat = CreateMenuButton(
                "🚪  Thoát chương trình");

            btnTrangChu.Click += (s, e) => ShowHome();

            btnDocGia.Click += (s, e) =>
                OpenForm<FrmDocGia>();

            btnDauSach.Click += (s, e) =>
                OpenForm<FrmDauSach>();

            btnPhieuMuon.Click += (s, e) =>
                OpenForm<FrmPhieuMuon>();

            btnThongKe.Click += (s, e) =>
                OpenForm<FrmThongKe>();

            btnThoat.Click += BtnThoat_Click;

            pnlMenu.Controls.Add(btnThoat);
            pnlMenu.Controls.Add(btnThongKe);
            pnlMenu.Controls.Add(btnPhieuMuon);
            pnlMenu.Controls.Add(btnDauSach);
            pnlMenu.Controls.Add(btnDocGia);
            pnlMenu.Controls.Add(btnTrangChu);
            pnlMenu.Controls.Add(lblMenu);

            // KHU VUC NOI DUNG
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250)
            };

            Controls.Add(pnlContent);
            Controls.Add(pnlMenu);
            Controls.Add(pnlHeader);

            ShowHome();
        }

        // TAO NUT MENU
        private Button CreateMenuButton(string text)
        {
            return new Button
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 58,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(18, 0, 0, 0),
                BackColor = Color.FromArgb(44, 62, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        // MO GIAO DIEN TRONG FORM CHINH
        private void OpenForm<T>() where T : Form, new()
        {
            pnlContent.Controls.Clear();

            T form = new T
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };

            pnlContent.Controls.Add(form);
            form.Show();
        }

        // TRANG CHU
        private void ShowHome()
        {
            pnlContent.Controls.Clear();

            Panel pnlHome = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250)
            };

            Label lblWelcome = new Label
            {
                Text = "CHÀO MỪNG ĐẾN VỚI HỆ THỐNG\nQUẢN LÝ THƯ VIỆN",
                Dock = DockStyle.Top,
                Height = 180,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 23, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 55, 80)
            };

            Label lblDescription = new Label
            {
                Text =
                    "Hệ thống hỗ trợ quản lý độc giả, đầu sách,\n" +
                    "phiếu mượn, trả sách và thống kê thư viện.",
                Dock = DockStyle.Top,
                Height = 100,
                TextAlign = ContentAlignment.TopCenter,
                Font = new Font("Segoe UI", 13),
                ForeColor = Color.DimGray
            };

            pnlHome.Controls.Add(lblDescription);
            pnlHome.Controls.Add(lblWelcome);

            pnlContent.Controls.Add(pnlHome);
        }

        // THOAT CHUONG TRINH
        private void BtnThoat_Click(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát chương trình?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
