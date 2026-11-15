
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.BUS;

namespace LibraryManagement.GUI
{
    public class FrmDocGia : Form
    {
        private readonly DocGiaBUS bus = new DocGiaBUS();

        private TextBox txtMaDocGia = null!;
        private TextBox txtHoTen = null!;
        private TextBox txtEmail = null!;
        private TextBox txtTimKiem = null!;

        private DateTimePicker dtpNgayHetHan = null!;

        private DataGridView dgvDocGia = null!;

        public FrmDocGia()
        {
            InitializeComponent();
            LoadDocGia();
        }

        private void InitializeComponent()
        {
            Text = "QUẢN LÝ ĐỘC GIẢ";
            Size = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);

            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ ĐỘC GIẢ",
                Dock = DockStyle.Top,
                Height = 65,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 19, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(35, 55, 80)
            };

            // PANEL NHAP THONG TIN
            Panel pnlInput = new Panel
            {
                Dock = DockStyle.Top,
                Height = 190,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblMa = new Label
            {
                Text = "Mã độc giả:",
                Location = new Point(20, 20),
                AutoSize = true
            };

            txtMaDocGia = new TextBox
            {
                Location = new Point(130, 16),
                Width = 220,
                ReadOnly = true
            };

            Label lblHoTen = new Label
            {
                Text = "Họ tên:",
                Location = new Point(20, 60),
                AutoSize = true
            };

            txtHoTen = new TextBox
            {
                Location = new Point(130, 56),
                Width = 220
            };

            Label lblEmail = new Label
            {
                Text = "Email:",
                Location = new Point(400, 20),
                AutoSize = true
            };

            txtEmail = new TextBox
            {
                Location = new Point(500, 16),
                Width = 250
            };

            Label lblNgayHetHan = new Label
            {
                Text = "Ngày hết hạn:",
                Location = new Point(400, 60),
                AutoSize = true
            };

            dtpNgayHetHan = new DateTimePicker
            {
                Location = new Point(500, 56),
                Width = 250,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy"
            };

            Button btnThem = CreateButton(
                "Thêm", Color.SeaGreen, 20, 110);

            Button btnSua = CreateButton(
                "Sửa", Color.RoyalBlue, 145, 110);

            Button btnXoa = CreateButton(
                "Xóa", Color.IndianRed, 270, 110);

            Button btnLamMoi = CreateButton(
                "Làm mới", Color.Gray, 395, 110);

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;

            pnlInput.Controls.AddRange(new Control[]
            {
                lblMa, txtMaDocGia,
                lblHoTen, txtHoTen,
                lblEmail, txtEmail,
                lblNgayHetHan, dtpNgayHetHan,
                btnThem, btnSua, btnXoa, btnLamMoi
            });

            // PANEL TIM KIEM
            Panel pnlSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            Label lblTimKiem = new Label
            {
                Text = "Tìm kiếm:",
                Location = new Point(20, 22),
                AutoSize = true
            };

            txtTimKiem = new TextBox
            {
                Location = new Point(110, 17),
                Width = 300
            };

            Button btnTimKiem = CreateButton(
                "Tìm kiếm", Color.DarkCyan, 430, 12);

            Button btnHienThi = CreateButton(
                "Hiển thị tất cả", Color.SlateBlue, 555, 12);

            btnTimKiem.Click += BtnTimKiem_Click;
            btnHienThi.Click += BtnHienThi_Click;

            pnlSearch.Controls.AddRange(new Control[]
            {
                lblTimKiem, txtTimKiem,
                btnTimKiem, btnHienThi
            });

            // BANG DU LIEU
            dgvDocGia = new DataGridView
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

            dgvDocGia.CellClick += DgvDocGia_CellClick;

            Controls.Add(dgvDocGia);
            Controls.Add(pnlSearch);
            Controls.Add(pnlInput);
            Controls.Add(lblTitle);
        }

        private Button CreateButton(
            string text,
            Color color,
            int x,
            int y)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(115, 35),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        // TAI DANH SACH DOC GIA
        private void LoadDocGia()
        {
            try
            {
                dgvDocGia.DataSource = bus.GetAll();

                if (dgvDocGia.Columns["MaDocGia"] != null)
                    dgvDocGia.Columns["MaDocGia"].HeaderText =
                        "Mã độc giả";

                if (dgvDocGia.Columns["HoTen"] != null)
                    dgvDocGia.Columns["HoTen"].HeaderText =
                        "Họ tên";

                if (dgvDocGia.Columns["Email"] != null)
                    dgvDocGia.Columns["Email"].HeaderText =
                        "Email";

                if (dgvDocGia.Columns["NgayHetHanThe"] != null)
                {
                    dgvDocGia.Columns["NgayHetHanThe"].HeaderText =
                        "Ngày hết hạn thẻ";

                    dgvDocGia.Columns["NgayHetHanThe"]
                        .DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải danh sách độc giả!\n" + ex.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // THEM DOC GIA
        private void BtnThem_Click(object? sender, EventArgs e)
        {
            try
            {
                int result = bus.Insert(
                    txtHoTen.Text,
                    txtEmail.Text,
                    dtpNgayHetHan.Value);

                if (result > 0)
                {
                    MessageBox.Show("Thêm độc giả thành công!");
                    LoadDocGia();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Không thể thêm độc giả!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // SUA DOC GIA
        private void BtnSua_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtMaDocGia.Text, out int ma))
                {
                    MessageBox.Show("Vui lòng chọn độc giả cần sửa!");
                    return;
                }

                int result = bus.Update(
                    ma,
                    txtHoTen.Text,
                    txtEmail.Text,
                    dtpNgayHetHan.Value);

                if (result > 0)
                {
                    MessageBox.Show("Cập nhật độc giả thành công!");
                    LoadDocGia();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy độc giả!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // XOA DOC GIA
        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtMaDocGia.Text, out int ma))
                {
                    MessageBox.Show("Vui lòng chọn độc giả cần xóa!");
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa độc giả này?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                int result = bus.Delete(ma);

                if (result > 0)
                {
                    MessageBox.Show("Xóa độc giả thành công!");
                    LoadDocGia();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy độc giả!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xóa độc giả!\n" + ex.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // TIM KIEM
        private void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            try
            {
                dgvDocGia.DataSource =
                    bus.Search(txtTimKiem.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm!\n" + ex.Message);
            }
        }

        // HIEN THI TAT CA
        private void BtnHienThi_Click(object? sender, EventArgs e)
        {
            txtTimKiem.Clear();
            LoadDocGia();
        }

        // LAM MOI
        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            ClearInputs();
            LoadDocGia();
        }

        // CHON DOC GIA TREN BANG
        private void DgvDocGia_CellClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvDocGia.Rows[e.RowIndex];

            txtMaDocGia.Text =
                Convert.ToString(row.Cells["MaDocGia"].Value) ?? "";

            txtHoTen.Text =
                Convert.ToString(row.Cells["HoTen"].Value) ?? "";

            txtEmail.Text =
                Convert.ToString(row.Cells["Email"].Value) ?? "";

            if (row.Cells["NgayHetHanThe"].Value != DBNull.Value)
            {
                dtpNgayHetHan.Value =
                    Convert.ToDateTime(
                        row.Cells["NgayHetHanThe"].Value);
            }
        }

        // XOA TRANG NHAP
        private void ClearInputs()
        {
            txtMaDocGia.Clear();
            txtHoTen.Clear();
            txtEmail.Clear();

            dtpNgayHetHan.Value = DateTime.Today.AddYears(1);

            dgvDocGia.ClearSelection();
        }
    }
}
