<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.tbSlice = New System.Windows.Forms.TrackBar()

        Me.modelSettingsGroupBox = New GroupBox()
        Me.lblUpAxis = New Label()
        Me.yUpRadioButton = New RadioButton()
        Me.zUpRadioButton = New RadioButton()
        Me.lblUnits = New Label()
        Me.unitsComboBox = New ComboBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.nudThickness = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.mdFront = New ModelSlicer.ModelDisplay()
        Me.mdRight = New ModelSlicer.ModelDisplay()
        Me.mdBottom = New ModelSlicer.ModelDisplay()
        Me.mdIso = New ModelSlicer.ModelDisplay()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileReload = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuFileExport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFilePrint = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFilePrintPreview = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuFileExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lbObjects = New System.Windows.Forms.ListBox()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.tsslStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tspbProgress = New System.Windows.Forms.ToolStripProgressBar()
        Me.tsslFile = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblSlices = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblHeight = New System.Windows.Forms.Label()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.lblTotalVolume = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblTotalDepth = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblTotalWidth = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblTotalHeight = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.lblVolume = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.lblDepth = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lblWidth = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        CType(Me.tbSlice, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.modelSettingsGroupBox.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.nudThickness, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbSlice
        '
        Me.tbSlice.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSlice.Location = New System.Drawing.Point(3, 529)
        Me.tbSlice.Name = "tbSlice"
        Me.tbSlice.Size = New System.Drawing.Size(594, 45)
        Me.tbSlice.TabIndex = 1
        Me.tbSlice.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        '
        'modelSettingsGroupBox
        '
        Me.modelSettingsGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.modelSettingsGroupBox.Controls.Add(Me.lblUpAxis)
        Me.modelSettingsGroupBox.Controls.Add(Me.yUpRadioButton)
        Me.modelSettingsGroupBox.Controls.Add(Me.zUpRadioButton)
        Me.modelSettingsGroupBox.Controls.Add(Me.lblUnits)
        Me.modelSettingsGroupBox.Controls.Add(Me.unitsComboBox)
        Me.modelSettingsGroupBox.Location = New System.Drawing.Point(3, 3)
        Me.modelSettingsGroupBox.Name = "modelSettingsGroupBox"
        Me.modelSettingsGroupBox.Size = New System.Drawing.Size(325, 70)
        Me.modelSettingsGroupBox.TabStop = False
        Me.modelSettingsGroupBox.Text = "Model Settings"
        '
        'lblUpAxis
        '
        Me.lblUpAxis.AutoSize = True
        Me.lblUpAxis.Location = New System.Drawing.Point(6, 18)
        Me.lblUpAxis.Name = "lblUpAxis"
        Me.lblUpAxis.Text = "Up Axis:"
        '
        'yUpRadioButton
        '
        Me.yUpRadioButton.AutoSize = True
        Me.yUpRadioButton.Size = New System.Drawing.Size(50, 24)
        Me.yUpRadioButton.Checked = True
        Me.yUpRadioButton.Location = New System.Drawing.Point(Me.lblUpAxis.Left + Me.lblUpAxis.Width + 5, Me.lblUpAxis.Top)
        Me.yUpRadioButton.Name = "yUpRadioButton"
        Me.yUpRadioButton.Text = "Y-Up"
        '
        'zUpRadioButton
        '
        Me.zUpRadioButton.AutoSize = True
        Me.zUpRadioButton.Location = New System.Drawing.Point(Me.yUpRadioButton.Left + Me.yUpRadioButton.Width + 10, Me.yUpRadioButton.Top)
        Me.zUpRadioButton.Name = "zUpRadioButton"
        Me.zUpRadioButton.Text = "Z-Up"
        '
        'lblUnits
        '
        Me.lblUnits.AutoSize = True
        Me.lblUnits.Location = New System.Drawing.Point(6, Me.yUpRadioButton.Top + 25)
        Me.lblUnits.Name = "lblUnits"
        Me.lblUnits.Text = "Units:"
        '
        'unitsComboBox
        '
        Me.unitsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.unitsComboBox.FormattingEnabled = True
        Me.unitsComboBox.Items.AddRange(New Object() {"Millimeters", "Centimeters", "Meters", "Inches", "Feet"})
        Me.unitsComboBox.Location = New System.Drawing.Point(Me.yUpRadioButton.Left, Me.lblUnits.Top - 3)
        Me.unitsComboBox.Name = "unitsComboBox"
        Me.unitsComboBox.SelectedIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.nudThickness)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 73)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(325, 46)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Slicing Parameters"
        '
        'nudThickness
        '
        Me.nudThickness.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudThickness.Location = New System.Drawing.Point(113, 19)
        Me.nudThickness.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudThickness.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudThickness.Name = "nudThickness"
        Me.nudThickness.Size = New System.Drawing.Size(206, 20)
        Me.nudThickness.TabIndex = 1
        Me.nudThickness.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Thickness (mm):"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.99999!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.00001!))
        Me.TableLayoutPanel1.Controls.Add(Me.mdFront, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.mdRight, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.mdBottom, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.mdIso, 1, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(594, 526)
        Me.TableLayoutPanel1.TabIndex = 4
        '
        'mdFront
        '
        Me.mdFront.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mdFront.ForeColor = System.Drawing.Color.Black
        Me.mdFront.Location = New System.Drawing.Point(0, 0)
        Me.mdFront.Margin = New System.Windows.Forms.Padding(0, 0, 3, 3)
        Me.mdFront.Name = "mdFront"
        Me.mdFront.Size = New System.Drawing.Size(293, 260)
        Me.mdFront.TabIndex = 0
        Me.mdFront.Text = "ModelDisplay1"
        '
        'mdRight
        '
        Me.mdRight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mdRight.ForeColor = System.Drawing.Color.Red
        Me.mdRight.Location = New System.Drawing.Point(299, 0)
        Me.mdRight.Margin = New System.Windows.Forms.Padding(3, 0, 0, 3)
        Me.mdRight.Name = "mdRight"
        Me.mdRight.Size = New System.Drawing.Size(295, 260)
        Me.mdRight.TabIndex = 1
        Me.mdRight.Text = "ModelDisplay1"
        '
        'mdBottom
        '
        Me.mdBottom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mdBottom.ForeColor = System.Drawing.Color.Red
        Me.mdBottom.Location = New System.Drawing.Point(0, 266)
        Me.mdBottom.Margin = New System.Windows.Forms.Padding(0, 3, 3, 0)
        Me.mdBottom.Name = "mdBottom"
        Me.mdBottom.Size = New System.Drawing.Size(293, 260)
        Me.mdBottom.TabIndex = 2
        Me.mdBottom.Text = "ModelDisplay1"
        '
        'mdIso
        '
        Me.mdIso.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mdIso.EnableRotation = True
        Me.mdIso.ForeColor = System.Drawing.Color.Red
        Me.mdIso.Location = New System.Drawing.Point(299, 266)
        Me.mdIso.Margin = New System.Windows.Forms.Padding(3, 3, 0, 0)
        Me.mdIso.Name = "mdIso"
        Me.mdIso.Size = New System.Drawing.Size(295, 260)
        Me.mdIso.TabIndex = 3
        Me.mdIso.Text = "ModelDisplay1"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(935, 24)
        Me.MenuStrip1.TabIndex = 5
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileOpen, Me.mnuFileReload, Me.ToolStripMenuItem2, Me.mnuFileExport, Me.mnuFilePrint, Me.mnuFilePrintPreview, Me.ToolStripMenuItem1, Me.mnuFileExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(37, 20)
        Me.mnuFile.Text = "&File"
        '
        'mnuFileOpen
        '
        Me.mnuFileOpen.Name = "mnuFileOpen"
        Me.mnuFileOpen.Size = New System.Drawing.Size(152, 22)
        Me.mnuFileOpen.Text = "&Open..."
        '
        'mnuFileReload
        '
        Me.mnuFileReload.Name = "mnuFileReload"
        Me.mnuFileReload.Size = New System.Drawing.Size(152, 22)
        Me.mnuFileReload.Text = "&Reload..."
        Me.mnuFileReload.Enabled = False
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(149, 6)
        '
        'mnuFileExport
        '
        Me.mnuFileExport.Name = "mnuFileExport"
        Me.mnuFileExport.Size = New System.Drawing.Size(152, 22)
        Me.mnuFileExport.Text = "&Export..."
        Me.mnuFileExport.Enabled = False
        '
        'mnuFilePrint
        '
        Me.mnuFilePrint.Name = "mnuFilePrint"
        Me.mnuFilePrint.Size = New System.Drawing.Size(152, 22)
        Me.mnuFilePrint.Text = "&Print..."
        Me.mnuFilePrint.Enabled = False
        '
        'mnuFilePrintPreview
        '
        Me.mnuFilePrintPreview.Name = "mnuFilePrintPreview"
        Me.mnuFilePrintPreview.Size = New System.Drawing.Size(152, 22)
        Me.mnuFilePrintPreview.Text = "P&rint Preview..."
        Me.mnuFilePrintPreview.Enabled = False
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(149, 6)
        '
        'mnuFileExit
        '
        Me.mnuFileExit.Name = "mnuFileExit"
        Me.mnuFileExit.Size = New System.Drawing.Size(152, 22)
        Me.mnuFileExit.Text = "E&xit"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.lbObjects)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 213)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(325, 268)
        Me.GroupBox2.TabIndex = 6
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Parts"
        '
        'lbObjects
        '
        Me.lbObjects.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbObjects.FormattingEnabled = True
        Me.lbObjects.IntegralHeight = False
        Me.lbObjects.Location = New System.Drawing.Point(6, 19)
        Me.lbObjects.Name = "lbObjects"
        Me.lbObjects.Size = New System.Drawing.Size(313, 243)
        Me.lbObjects.TabIndex = 0
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslStatus, Me.tspbProgress, Me.tsslFile})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 601)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(935, 22)
        Me.StatusStrip1.TabIndex = 7
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'tsslStatus
        '
        Me.tsslStatus.Name = "tsslStatus"
        Me.tsslStatus.Size = New System.Drawing.Size(920, 17)
        Me.tsslStatus.Spring = True
        Me.tsslStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tspbProgress
        '
        Me.tspbProgress.Name = "tspbProgress"
        Me.tspbProgress.Size = New System.Drawing.Size(300, 16)
        Me.tspbProgress.Visible = False
        '
        'tsslFile
        '
        Me.tsslFile.Name = "tsslFile"
        Me.tsslFile.Size = New System.Drawing.Size(920, 17)
        Me.tsslFile.Spring = True
        Me.tsslFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tsslFile.Text = "No File Loaded"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(93, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Number of Layers:"
        '
        'lblSlices
        '
        Me.lblSlices.AutoSize = True
        Me.lblSlices.Location = New System.Drawing.Point(132, 16)
        Me.lblSlices.Name = "lblSlices"
        Me.lblSlices.Size = New System.Drawing.Size(0, 13)
        Me.lblSlices.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 29)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(89, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Bounding Height:"
        '
        'lblHeight
        '
        Me.lblHeight.AutoSize = True
        Me.lblHeight.Location = New System.Drawing.Point(132, 29)
        Me.lblHeight.Name = "lblHeight"
        Me.lblHeight.Size = New System.Drawing.Size(0, 13)
        Me.lblHeight.TabIndex = 8
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 24)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.modelSettingsGroupBox)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TableLayoutPanel1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.tbSlice)
        Me.SplitContainer1.Size = New System.Drawing.Size(935, 577)
        Me.SplitContainer1.SplitterDistance = 331
        Me.SplitContainer1.TabIndex = 8
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.Controls.Add(Me.lblTotalVolume)
        Me.GroupBox4.Controls.Add(Me.Label2)
        Me.GroupBox4.Controls.Add(Me.Label10)
        Me.GroupBox4.Controls.Add(Me.lblTotalDepth)
        Me.GroupBox4.Controls.Add(Me.Label8)
        Me.GroupBox4.Controls.Add(Me.lblTotalWidth)
        Me.GroupBox4.Controls.Add(Me.Label6)
        Me.GroupBox4.Controls.Add(Me.lblTotalHeight)
        Me.GroupBox4.Location = New System.Drawing.Point(3, 126)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(325, 81)
        Me.GroupBox4.TabIndex = 8
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Total Statistics"
        '
        'lblTotalVolume
        '
        Me.lblTotalVolume.AutoSize = True
        Me.lblTotalVolume.Location = New System.Drawing.Point(132, 55)
        Me.lblTotalVolume.Name = "lblTotalVolume"
        Me.lblTotalVolume.Size = New System.Drawing.Size(0, 13)
        Me.lblTotalVolume.TabIndex = 11
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Total Bounding Volume:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 42)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(114, 13)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Total Bounding Depth:"
        '
        'lblTotalDepth
        '
        Me.lblTotalDepth.AutoSize = True
        Me.lblTotalDepth.Location = New System.Drawing.Point(132, 42)
        Me.lblTotalDepth.Name = "lblTotalDepth"
        Me.lblTotalDepth.Size = New System.Drawing.Size(0, 13)
        Me.lblTotalDepth.TabIndex = 9
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 29)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(113, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Total Bounding Width:"
        '
        'lblTotalWidth
        '
        Me.lblTotalWidth.AutoSize = True
        Me.lblTotalWidth.Location = New System.Drawing.Point(132, 29)
        Me.lblTotalWidth.Name = "lblTotalWidth"
        Me.lblTotalWidth.Size = New System.Drawing.Size(0, 13)
        Me.lblTotalWidth.TabIndex = 7
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(116, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Total Bounding Height:"
        '
        'lblTotalHeight
        '
        Me.lblTotalHeight.AutoSize = True
        Me.lblTotalHeight.Location = New System.Drawing.Point(132, 16)
        Me.lblTotalHeight.Name = "lblTotalHeight"
        Me.lblTotalHeight.Size = New System.Drawing.Size(0, 13)
        Me.lblTotalHeight.TabIndex = 5
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.lblVolume)
        Me.GroupBox3.Controls.Add(Me.Label13)
        Me.GroupBox3.Controls.Add(Me.lblDepth)
        Me.GroupBox3.Controls.Add(Me.Label11)
        Me.GroupBox3.Controls.Add(Me.lblWidth)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Controls.Add(Me.lblHeight)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.lblSlices)
        Me.GroupBox3.Location = New System.Drawing.Point(3, 487)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(325, 87)
        Me.GroupBox3.TabIndex = 7
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Part Statistics"
        '
        'lblVolume
        '
        Me.lblVolume.AutoSize = True
        Me.lblVolume.Location = New System.Drawing.Point(132, 68)
        Me.lblVolume.Name = "lblVolume"
        Me.lblVolume.Size = New System.Drawing.Size(0, 13)
        Me.lblVolume.TabIndex = 14
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 68)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(93, 13)
        Me.Label13.TabIndex = 13
        Me.Label13.Text = "Bounding Volume:"
        '
        'lblDepth
        '
        Me.lblDepth.AutoSize = True
        Me.lblDepth.Location = New System.Drawing.Point(132, 55)
        Me.lblDepth.Name = "lblDepth"
        Me.lblDepth.Size = New System.Drawing.Size(0, 13)
        Me.lblDepth.TabIndex = 12
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 55)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(87, 13)
        Me.Label11.TabIndex = 11
        Me.Label11.Text = "Bounding Depth:"
        '
        'lblWidth
        '
        Me.lblWidth.AutoSize = True
        Me.lblWidth.Location = New System.Drawing.Point(132, 42)
        Me.lblWidth.Name = "lblWidth"
        Me.lblWidth.Size = New System.Drawing.Size(0, 13)
        Me.lblWidth.TabIndex = 10
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 42)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(86, 13)
        Me.Label7.TabIndex = 9
        Me.Label7.Text = "Bounding Width:"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(935, 623)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmMain"
        Me.Text = "Slicer"
        CType(Me.tbSlice, System.ComponentModel.ISupportInitialize).EndInit()
        Me.modelSettingsGroupBox.ResumeLayout(False)
        Me.modelSettingsGroupBox.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.nudThickness, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents mdFront As ModelDisplay
    Friend WithEvents tbSlice As TrackBar
    Friend WithEvents mdRight As ModelDisplay
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents nudThickness As NumericUpDown
    Friend WithEvents Label1 As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents mdBottom As ModelDisplay
    Friend WithEvents mdIso As ModelDisplay
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents mnuFile As ToolStripMenuItem
    Friend WithEvents mnuFileOpen As ToolStripMenuItem
    Friend WithEvents mnuFileReload As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents mnuFileExport As ToolStripMenuItem
    Friend WithEvents mnuFilePrint As ToolStripMenuItem
    Friend WithEvents mnuFilePrintPreview As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents mnuFileExit As ToolStripMenuItem
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lbObjects As ListBox
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents tsslStatus As ToolStripStatusLabel
    Friend WithEvents tspbProgress As ToolStripProgressBar
    Friend WithEvents tsslFile As ToolStripStatusLabel
    Friend WithEvents lblSlices As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblHeight As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents Label10 As Label
    Friend WithEvents lblTotalDepth As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents lblTotalWidth As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents lblTotalHeight As Label
    Friend WithEvents lblTotalVolume As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblVolume As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents lblDepth As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents lblWidth As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents modelSettingsGroupBox As GroupBox
    Friend WithEvents lblUpAxis As Label
    Friend WithEvents yUpRadioButton As RadioButton
    Friend WithEvents zUpRadioButton As RadioButton
    Friend WithEvents lblUnits As Label
    Friend WithEvents unitsComboBox As ComboBox
End Class
