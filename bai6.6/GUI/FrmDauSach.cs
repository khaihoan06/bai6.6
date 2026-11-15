
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.BUS;
using LibraryManagement.DTO;

namespace LibraryManagement.GUI
{
    public class FrmDauSach : Form
    {
        private readonly DauSachBUS bus = new DauSachBUS();

        private TextBox txtMaSach = null!;
        private TextBox txtTenSach = null!;
        private TextBox txtTheLoai = null!;
        private TextBox txtSoLuongCon = null!;
        private TextBox txtTimKiem = null!;

        private DataGridView dgvDauSach = null!;

        public FrmDauSach()
        {
            InitializeComponent();
            LoadDauSach();
        }

        private void InitializeComponent()
        {
            Text = "QUẢN LÝ ĐẦU SÁCH";
            Size = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);

            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ ĐẦU SÁCH",
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

            Label lblMaSach = new Label
            {
                Text = "Mã sách:",
                Location = new Point(20, 20),
                AutoSize = true
            };

            txtMaSach = new TextBox
            {
                Location = new Point(130, 16),
                Width = 220,
                ReadOnly = true
            };

            Label lblTenSach = new Label
            {
                Text = "Tên sách:",
                Location = new Point(20, 60),
                AutoSize = true
            };

            txtTenSach = new TextBox
            {
                Location = new Point(130, 56),
                Width = 220
            };

            Label lblTheLoai = new Label
            {
                Text = "Thể loại:",
                Location = new Point(400, 20),
                AutoSize = true
            };

            txtTheLoai = new TextBox
            {
                Location = new Point(500, 16),
                Width = 250
            };

            Label lblSoLuong = new Label
            {
                Text = "Số lượng còn:",
                Location = new Point(400, 60),
                AutoSize = true
            };

            txtSoLuongCon = new TextBox
            {
                Location = new Point(500, 56),
                Width = 250
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
                lblMaSach, txtMaSach,
                lblTenSach, txtTenSach,
                lblTheLoai, txtTheLoai,
                lblSoLuong, txtSoLuongCon,
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
            dgvDauSach = new DataGridView
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

            dgvDauSach.CellClick += DgvDauSach_CellClick;

            Controls.Add(dgvDauSach);
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

        // TAI DANH SACH DAU SACH
        private void LoadDauSach()
        {
            try
            {
                dgvDauSach.DataSource = bus.GetAll();

                if (dgvDauSach.Columns["MaSach"] != null)
                    dgvDauSach.Columns["MaSach"].HeaderText = "Mã sách";

                if (dgvDauSach.Columns["TenSach"] != null)
                    dgvDauSach.Columns["TenSach"].HeaderText = "Tên sách";

                if (dgvDauSach.Columns["TheLoai"] != null)
                    dgvDauSach.Columns["TheLoai"].HeaderText = "Thể loại";

                if (dgvDauSach.Columns["SoLuongCon"] != null)
                    dgvDauSach.Columns["SoLuongCon"].HeaderText =
                        "Số lượng còn";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải danh sách đầu sách!\n" + ex.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // KIEM TRA DU LIEU
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenSach.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sách!");
                txtTenSach.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTheLoai.Text))
            {
                MessageBox.Show("Vui lòng nhập thể loại!");
                txtTheLoai.Focus();
                return false;
            }

            if (!int.TryParse(txtSoLuongCon.Text, out int soLuong)
                || soLuong < 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên không âm!");
                txtSoLuongCon.Focus();
                return false;
            }

            return true;
        }

        // THEM SACH
        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                DauSachDTO sach = new DauSachDTO
                {
                    TenSach = txtTenSach.Text.Trim(),
                    TheLoai = txtTheLoai.Text.Trim(),
                    SoLuongCon = int.Parse(txtSoLuongCon.Text)
                };

                bool result = bus.Insert(sach);

                if (result)
                {
                    MessageBox.Show("Thêm sách thành công!");
                    LoadDauSach();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Không thể thêm sách!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi thêm sách!\n" + ex.Message);
            }
        }

        // SUA SACH
        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            if (!int.TryParse(txtMaSach.Text, out int maSach))
            {
                MessageBox.Show("Vui lòng chọn sách cần sửa!");
                return;
            }

            try
            {
                DauSachDTO sach = new DauSachDTO
                {
                    MaSach = maSach,
                    TenSach = txtTenSach.Text.Trim(),
                    TheLoai = txtTheLoai.Text.Trim(),
                    SoLuongCon = int.Parse(txtSoLuongCon.Text)
                };

                bool result = bus.Update(sach);

                if (result)
                {
                    MessageBox.Show("Cập nhật sách thành công!");
                    LoadDauSach();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật sách!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi cập nhật sách!\n" + ex.Message);
            }
        }

        // XOA SACH
        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaSach.Text, out int maSach))
            {
                MessageBox.Show("Vui lòng chọn sách cần xóa!");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa sách này?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                bool result = bus.Delete(maSach);

                if (result)
                {
                    MessageBox.Show("Xóa sách thành công!");
                    LoadDauSach();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Không thể xóa sách!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi xóa sách!\n" + ex.Message);
            }
        }

        // TIM KIEM
        private void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            try
            {
                dgvDauSach.DataSource =
                    bus.Search(txtTimKiem.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tìm kiếm!\n" + ex.Message);
            }
        }

        // HIEN THI TAT CA
        private void BtnHienThi_Click(object? sender, EventArgs e)
        {
            txtTimKiem.Clear();
            LoadDauSach();
        }

        // LAM MOI
        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            ClearInputs();
            LoadDauSach();
        }

        // CHON DONG TREN BANG
        private void DgvDauSach_CellClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvDauSach.Rows[e.RowIndex];

            txtMaSach.Text =
                Convert.ToString(row.Cells["MaSach"].Value) ?? "";

            txtTenSach.Text =
                Convert.ToString(row.Cells["TenSach"].Value) ?? "";

            txtTheLoai.Text =
                Convert.ToString(row.Cells["TheLoai"].Value) ?? "";

            txtSoLuongCon.Text =
                Convert.ToString(row.Cells["SoLuongCon"].Value) ?? "";
        }

        // XOA TRANG
        private void ClearInputs()
        {
            txtMaSach.Clear();
            txtTenSach.Clear();
            txtTheLoai.Clear();
            txtSoLuongCon.Clear();

            dgvDauSach.ClearSelection();
            txtTenSach.Focus();
        }
    }
}
