
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.BUS;
using LibraryManagement.DTO;

namespace LibraryManagement.GUI
{
    public class FrmPhieuMuon : Form
    {
        private readonly PhieuMuonBUS phieuBUS = new PhieuMuonBUS();
        private readonly DocGiaBUS docGiaBUS = new DocGiaBUS();
        private readonly DauSachBUS dauSachBUS = new DauSachBUS();

        private ComboBox cboDocGia = null!;
        private ComboBox cboSach = null!;
        private ComboBox cboTrangThai = null!;

        private DateTimePicker dtpHanTra = null!;

        private NumericUpDown numSoLuong = null!;

        private DataGridView dgvGioMuon = null!;
        private DataGridView dgvPhieuMuon = null!;
        private DataGridView dgvChiTiet = null!;

        private DataTable gioMuon = null!;

        public FrmPhieuMuon()
        {
            InitializeComponent();
            InitializeCart();
            LoadDocGia();
            LoadSach();
            LoadPhieuMuon();
        }

        private void InitializeComponent()
        {
            Text = "QUẢN LÝ PHIẾU MƯỢN VÀ TRẢ SÁCH";
            Size = new Size(1250, 800);
            MinimumSize = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 9);

            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ PHIẾU MƯỢN VÀ TRẢ SÁCH",
                Dock = DockStyle.Top,
                Height = 55,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 17, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(35, 55, 80)
            };

            // PANEL LAP PHIEU
            Panel pnlLapPhieu = new Panel
            {
                Dock = DockStyle.Top,
                Height = 245,
                BackColor = Color.White,
                Padding = new Padding(12)
            };

            Label lblDocGia = new Label
            {
                Text = "Độc giả:",
                Location = new Point(15, 15),
                AutoSize = true
            };

            cboDocGia = new ComboBox
            {
                Location = new Point(90, 11),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Label lblHanTra = new Label
            {
                Text = "Hạn trả:",
                Location = new Point(370, 15),
                AutoSize = true
            };

            dtpHanTra = new DateTimePicker
            {
                Location = new Point(435, 11),
                Width = 180,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Value = DateTime.Today.AddDays(14),
                MinDate = DateTime.Today
            };

            Label lblSach = new Label
            {
                Text = "Sách:",
                Location = new Point(15, 55),
                AutoSize = true
            };

            cboSach = new ComboBox
            {
                Location = new Point(90, 51),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Label lblSoLuong = new Label
            {
                Text = "Số lượng:",
                Location = new Point(410, 55),
                AutoSize = true
            };

            numSoLuong = new NumericUpDown
            {
                Location = new Point(485, 51),
                Width = 80,
                Minimum = 1,
                Maximum = 100,
                Value = 1
            };

            Button btnThemSach = CreateButton(
                "Thêm sách", Color.DarkCyan, 590, 48, 115);

            Button btnXoaSach = CreateButton(
                "Xóa sách", Color.IndianRed, 715, 48, 110);

            btnThemSach.Click += BtnThemSach_Click;
            btnXoaSach.Click += BtnXoaSach_Click;

            Label lblGioMuon = new Label
            {
                Text = "DANH SÁCH SÁCH ĐƯỢC MƯỢN",
                Location = new Point(15, 90),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            dgvGioMuon = new DataGridView
            {
                Location = new Point(15, 115),
                Size = new Size(800, 75),
                BackgroundColor = Color.White,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false
            };

            Button btnLapPhieu = CreateButton(
                "LẬP PHIẾU MƯỢN", Color.SeaGreen, 15, 195, 160);

            Button btnLamMoi = CreateButton(
                "Làm mới", Color.Gray, 190, 195, 110);

            btnLapPhieu.Click += BtnLapPhieu_Click;
            btnLamMoi.Click += BtnLamMoi_Click;

            pnlLapPhieu.Controls.AddRange(new Control[]
            {
                lblDocGia, cboDocGia,
                lblHanTra, dtpHanTra,
                lblSach, cboSach,
                lblSoLuong, numSoLuong,
                btnThemSach, btnXoaSach,
                lblGioMuon, dgvGioMuon,
                btnLapPhieu, btnLamMoi
            });

            // PANEL DANH SACH PHIEU
            Panel pnlDanhSach = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(12)
            };

            Label lblDanhSach = new Label
            {
                Text = "DANH SÁCH PHIẾU MƯỢN",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            Panel pnlChucNang = new Panel
            {
                Dock = DockStyle.Top,
                Height = 48
            };

            Label lblTrangThai = new Label
            {
                Text = "Trạng thái:",
                Location = new Point(5, 14),
                AutoSize = true
            };

            cboTrangThai = new ComboBox
            {
                Location = new Point(85, 9),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cboTrangThai.Items.AddRange(new object[]
            {
                "Tất cả",
                "Đang mượn",
                "Quá hạn",
                "Đã trả"
            });

            cboTrangThai.SelectedIndex = 0;

            Button btnLoc = CreateButton(
                "Lọc phiếu", Color.DarkCyan, 250, 5, 105);

            Button btnHienThi = CreateButton(
                "Tải lại", Color.SlateBlue, 365, 5, 100);

            Button btnTraSach = CreateButton(
                "Trả sách", Color.SeaGreen, 475, 5, 110);

            Button btnQuaHan = CreateButton(
                "Phiếu quá hạn", Color.DarkOrange, 595, 5, 130);

            btnLoc.Click += BtnLoc_Click;
            btnHienThi.Click += BtnHienThi_Click;
            btnTraSach.Click += BtnTraSach_Click;
            btnQuaHan.Click += BtnQuaHan_Click;

            pnlChucNang.Controls.AddRange(new Control[]
            {
                lblTrangThai, cboTrangThai,
                btnLoc, btnHienThi, btnTraSach, btnQuaHan
            });

            // KHU VUC HIEN THI 2 BANG
            SplitContainer split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 190,
                BackColor = Color.LightGray
            };

            dgvPhieuMuon = CreateGrid();
            dgvChiTiet = CreateGrid();

            Panel pnlPhieu = new Panel
            {
                Dock = DockStyle.Fill
            };

            Label lblPhieu = new Label
            {
                Text = "Thông tin phiếu mượn",
                Dock = DockStyle.Top,
                Height = 25,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            pnlPhieu.Controls.Add(dgvPhieuMuon);
            pnlPhieu.Controls.Add(lblPhieu);

            Panel pnlChiTiet = new Panel
            {
                Dock = DockStyle.Fill
            };

            Label lblChiTiet = new Label
            {
                Text = "Chi tiết sách trong phiếu",
                Dock = DockStyle.Top,
                Height = 25,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            pnlChiTiet.Controls.Add(dgvChiTiet);
            pnlChiTiet.Controls.Add(lblChiTiet);

            split.Panel1.Controls.Add(pnlPhieu);
            split.Panel2.Controls.Add(pnlChiTiet);

            dgvPhieuMuon.CellClick += DgvPhieuMuon_CellClick;

            pnlDanhSach.Controls.Add(split);
            pnlDanhSach.Controls.Add(pnlChucNang);
            pnlDanhSach.Controls.Add(lblDanhSach);

            Controls.Add(pnlDanhSach);
            Controls.Add(pnlLapPhieu);
            Controls.Add(lblTitle);
        }

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

        private Button CreateButton(
            string text,
            Color color,
            int x,
            int y,
            int width)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, 32),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        // KHOI TAO GIO MUON
        private void InitializeCart()
        {
            gioMuon = new DataTable();

            gioMuon.Columns.Add("MaSach", typeof(int));
            gioMuon.Columns.Add("TenSach", typeof(string));
            gioMuon.Columns.Add("SoLuong", typeof(int));

            dgvGioMuon.DataSource = gioMuon;

            dgvGioMuon.Columns["MaSach"]!.HeaderText = "Mã sách";
            dgvGioMuon.Columns["TenSach"]!.HeaderText = "Tên sách";
            dgvGioMuon.Columns["SoLuong"]!.HeaderText = "Số lượng";
        }

        // TAI DOC GIA
        private void LoadDocGia()
        {
            try
            {
                DataTable dt = docGiaBUS.GetAll();

                cboDocGia.DataSource = dt;
                cboDocGia.DisplayMember = "HoTen";
                cboDocGia.ValueMember = "MaDocGia";
                cboDocGia.SelectedIndex = dt.Rows.Count > 0 ? 0 : -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải độc giả!\n" + ex.Message);
            }
        }

        // TAI SACH
        private void LoadSach()
        {
            try
            {
                DataTable dt = dauSachBUS.GetAll();

                cboSach.DataSource = dt;
                cboSach.DisplayMember = "TenSach";
                cboSach.ValueMember = "MaSach";
                cboSach.SelectedIndex = dt.Rows.Count > 0 ? 0 : -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải sách!\n" + ex.Message);
            }
        }

        // TAI PHIEU MUON
        private void LoadPhieuMuon()
        {
            try
            {
                phieuBUS.UpdateOverdueStatus();

                dgvPhieuMuon.DataSource = phieuBUS.GetAll();

                FormatPhieuGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải phiếu mượn!\n" + ex.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void FormatPhieuGrid()
        {
            string[,] columns =
            {
                { "MaPhieu", "Mã phiếu" },
                { "MaDocGia", "Mã độc giả" },
                { "HoTen", "Tên độc giả" },
                { "NgayMuon", "Ngày mượn" },
                { "HanTra", "Hạn trả" },
                { "TrangThai", "Trạng thái" }
            };

            for (int i = 0; i < columns.GetLength(0); i++)
            {
                string name = columns[i, 0];

                if (dgvPhieuMuon.Columns[name] != null)
                {
                    dgvPhieuMuon.Columns[name]!.HeaderText =
                        columns[i, 1];

                    if (name == "NgayMuon" || name == "HanTra")
                    {
                        dgvPhieuMuon.Columns[name]!
                            .DefaultCellStyle.Format = "dd/MM/yyyy";
                    }
                }
            }
        }

        // THEM SACH VAO GIO
        private void BtnThemSach_Click(object? sender, EventArgs e)
        {
            if (cboSach.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn sách!");
                return;
            }

            DataRowView sach =
                (DataRowView)cboSach.SelectedItem;

            int maSach = Convert.ToInt32(sach["MaSach"]);
            string tenSach = Convert.ToString(sach["TenSach"]) ?? "";
            int tonKho = Convert.ToInt32(sach["SoLuongCon"]);
            int soLuong = Convert.ToInt32(numSoLuong.Value);

            if (tonKho <= 0)
            {
                MessageBox.Show("Sách đã hết trong kho!");
                return;
            }

            DataRow[] rows = gioMuon.Select("MaSach = " + maSach);

            int soLuongDaChon = 0;

            if (rows.Length > 0)
                soLuongDaChon = Convert.ToInt32(rows[0]["SoLuong"]);

            if (soLuongDaChon + soLuong > tonKho)
            {
                MessageBox.Show("Số lượng mượn vượt quá số sách còn lại!");
                return;
            }

            if (rows.Length > 0)
            {
                rows[0]["SoLuong"] = soLuongDaChon + soLuong;
            }
            else
            {
                gioMuon.Rows.Add(maSach, tenSach, soLuong);
            }

            numSoLuong.Value = 1;
        }

        // XOA SACH KHOI GIO
        private void BtnXoaSach_Click(object? sender, EventArgs e)
        {
            if (dgvGioMuon.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sách cần xóa!");
                return;
            }

            int index = dgvGioMuon.CurrentRow.Index;

            if (index >= 0 && index < gioMuon.Rows.Count)
                gioMuon.Rows.RemoveAt(index);
        }

        // LAP PHIEU MUON
        private void BtnLapPhieu_Click(object? sender, EventArgs e)
        {
            if (cboDocGia.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả!");
                return;
            }

            if (gioMuon.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một cuốn sách!");
                return;
            }

            try
            {
                PhieuMuonDTO phieu = new PhieuMuonDTO
                {
                    MaDocGia = Convert.ToInt32(cboDocGia.SelectedValue),
                    HanTra = dtpHanTra.Value.Date
                };

                List<ChiTietMuonDTO> chiTiet =
                    new List<ChiTietMuonDTO>();

                foreach (DataRow row in gioMuon.Rows)
                {
                    chiTiet.Add(new ChiTietMuonDTO
                    {
                        MaSach = Convert.ToInt32(row["MaSach"]),
                        SoLuong = Convert.ToInt32(row["SoLuong"])
                    });
                }

                bool result = phieuBUS.Insert(phieu, chiTiet);

                if (result)
                {
                    MessageBox.Show("Lập phiếu mượn thành công!");

                    gioMuon.Clear();
                    LoadSach();
                    LoadPhieuMuon();
                    dgvChiTiet.DataSource = null;
                }
                else
                {
                    MessageBox.Show("Không thể lập phiếu mượn!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi lập phiếu mượn!\n" + ex.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // TRA SACH
        private void BtnTraSach_Click(object? sender, EventArgs e)
        {
            if (dgvPhieuMuon.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu cần trả!");
                return;
            }

            int maPhieu = Convert.ToInt32(
                dgvPhieuMuon.CurrentRow.Cells["MaPhieu"].Value);

            string trangThai = Convert.ToString(
                dgvPhieuMuon.CurrentRow.Cells["TrangThai"].Value) ?? "";

            if (trangThai == "Đã trả")
            {
                MessageBox.Show("Phiếu này đã được trả!");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Xác nhận trả sách cho phiếu số " + maPhieu + "?",
                "Xác nhận trả sách",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                bool result = phieuBUS.ReturnBook(maPhieu);

                if (result)
                {
                    MessageBox.Show("Trả sách thành công!");
                    LoadSach();
                    LoadPhieuMuon();
                    dgvChiTiet.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi trả sách!\n" + ex.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // LOC PHIEU
        private void BtnLoc_Click(object? sender, EventArgs e)
        {
            try
            {
                phieuBUS.UpdateOverdueStatus();

                dgvPhieuMuon.DataSource = phieuBUS.Filter(
                    0,
                    cboTrangThai.SelectedItem?.ToString() ?? "Tất cả");

                FormatPhieuGrid();
                dgvChiTiet.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lọc phiếu!\n" + ex.Message);
            }
        }

        // TAI LAI
        private void BtnHienThi_Click(object? sender, EventArgs e)
        {
            cboTrangThai.SelectedIndex = 0;
            LoadSach();
            LoadPhieuMuon();
            dgvChiTiet.DataSource = null;
        }

        // XEM PHIEU QUA HAN
        private void BtnQuaHan_Click(object? sender, EventArgs e)
        {
            try
            {
                phieuBUS.UpdateOverdueStatus();

                dgvPhieuMuon.DataSource = phieuBUS.GetOverdue();

                FormatPhieuGrid();

                if (dgvPhieuMuon.Columns["SoNgayQuaHan"] != null)
                {
                    dgvPhieuMuon.Columns["SoNgayQuaHan"]!.HeaderText =
                        "Số ngày quá hạn";
                }

                dgvChiTiet.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy phiếu quá hạn!\n" + ex.Message);
            }
        }

        // XEM CHI TIET PHIEU
        private void DgvPhieuMuon_CellClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            try
            {
                int maPhieu = Convert.ToInt32(
                    dgvPhieuMuon.Rows[e.RowIndex]
                        .Cells["MaPhieu"].Value);

                dgvChiTiet.DataSource =
                    phieuBUS.GetChiTiet(maPhieu);

                if (dgvChiTiet.Columns["MaPhieu"] != null)
                    dgvChiTiet.Columns["MaPhieu"]!.HeaderText = "Mã phiếu";

                if (dgvChiTiet.Columns["MaSach"] != null)
                    dgvChiTiet.Columns["MaSach"]!.HeaderText = "Mã sách";

                if (dgvChiTiet.Columns["TenSach"] != null)
                    dgvChiTiet.Columns["TenSach"]!.HeaderText = "Tên sách";

                if (dgvChiTiet.Columns["SoLuong"] != null)
                    dgvChiTiet.Columns["SoLuong"]!.HeaderText = "Số lượng";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi xem chi tiết phiếu!\n" + ex.Message);
            }
        }

        // LAM MOI FORM
        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            gioMuon.Clear();

            if (cboDocGia.Items.Count > 0)
                cboDocGia.SelectedIndex = 0;

            if (cboSach.Items.Count > 0)
                cboSach.SelectedIndex = 0;

            dtpHanTra.Value = DateTime.Today.AddDays(14);
            numSoLuong.Value = 1;

            LoadSach();
            LoadPhieuMuon();

            dgvChiTiet.DataSource = null;
        }
    }
}
