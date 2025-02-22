<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPreferences
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
        Me.components = New System.ComponentModel.Container()
        Me.butOk = New System.Windows.Forms.Button()
        Me.butCancel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbDisplayUnits = New System.Windows.Forms.ComboBox()
        Me.ttTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmbImportDefaultUnits = New System.Windows.Forms.ComboBox()
        Me.cmbImportDefaultUpAxis = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkImportUseDefaults = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.cpExportSvgColorBottom = New ModelSlicer.ColorPicker()
        Me.cpExportSvgColorFill = New ModelSlicer.ColorPicker()
        Me.cpExportSvgColorTop = New ModelSlicer.ColorPicker()
        Me.chkExportSvgIncludeBottom = New System.Windows.Forms.CheckBox()
        Me.chkExportSvgIncludeFill = New System.Windows.Forms.CheckBox()
        Me.chkExportSvgIncludeTop = New System.Windows.Forms.CheckBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.chkExportSvgUseDefaults = New System.Windows.Forms.CheckBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'butOk
        '
        Me.butOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.butOk.Location = New System.Drawing.Point(243, 235)
        Me.butOk.Name = "butOk"
        Me.butOk.Size = New System.Drawing.Size(75, 23)
        Me.butOk.TabIndex = 1
        Me.butOk.Text = "&OK"
        Me.butOk.UseVisualStyleBackColor = True
        '
        'butCancel
        '
        Me.butCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.butCancel.Location = New System.Drawing.Point(324, 235)
        Me.butCancel.Name = "butCancel"
        Me.butCancel.Size = New System.Drawing.Size(75, 23)
        Me.butCancel.TabIndex = 2
        Me.butCancel.Text = "&Cancel"
        Me.butCancel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 38)
        Me.Label1.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Display Units:"
        '
        'cmbDisplayUnits
        '
        Me.cmbDisplayUnits.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbDisplayUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDisplayUnits.FormattingEnabled = True
        Me.cmbDisplayUnits.Items.AddRange(New Object() {"Millimeters", "Centimeters", "Meters", "Fractional Inches", "Decimal Inches", "Fractional Feet Inches", "Decimal Feet Inches"})
        Me.cmbDisplayUnits.Location = New System.Drawing.Point(161, 35)
        Me.cmbDisplayUnits.Name = "cmbDisplayUnits"
        Me.cmbDisplayUnits.Size = New System.Drawing.Size(222, 21)
        Me.cmbDisplayUnits.TabIndex = 1
        Me.ttTip.SetToolTip(Me.cmbDisplayUnits, "Sets the units and format to use when displaying values")
        '
        'cmbImportDefaultUnits
        '
        Me.cmbImportDefaultUnits.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbImportDefaultUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbImportDefaultUnits.FormattingEnabled = True
        Me.cmbImportDefaultUnits.Items.AddRange(New Object() {"Millimeters", "Centimeters", "Meters", "Fractional Inches", "Decimal Inches", "Fractional Feet Inches", "Decimal Feet Inches"})
        Me.cmbImportDefaultUnits.Location = New System.Drawing.Point(161, 114)
        Me.cmbImportDefaultUnits.Name = "cmbImportDefaultUnits"
        Me.cmbImportDefaultUnits.Size = New System.Drawing.Size(222, 21)
        Me.cmbImportDefaultUnits.TabIndex = 7
        Me.ttTip.SetToolTip(Me.cmbImportDefaultUnits, "Sets the units and format to use when displaying values")
        '
        'cmbImportDefaultUpAxis
        '
        Me.cmbImportDefaultUpAxis.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbImportDefaultUpAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbImportDefaultUpAxis.FormattingEnabled = True
        Me.cmbImportDefaultUpAxis.Items.AddRange(New Object() {"X up", "Y up", "Z up"})
        Me.cmbImportDefaultUpAxis.Location = New System.Drawing.Point(161, 141)
        Me.cmbImportDefaultUpAxis.Name = "cmbImportDefaultUpAxis"
        Me.cmbImportDefaultUpAxis.Size = New System.Drawing.Size(222, 21)
        Me.cmbImportDefaultUpAxis.TabIndex = 7
        Me.ttTip.SetToolTip(Me.cmbImportDefaultUpAxis, "Sets the units and format to use when displaying values")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.TableLayoutPanel1.SetColumnSpan(Me.Label2, 3)
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(3, 12)
        Me.Label2.Margin = New System.Windows.Forms.Padding(3, 12, 3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 20)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Display"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.TableLayoutPanel1.SetColumnSpan(Me.Label3, 3)
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(3, 71)
        Me.Label3.Margin = New System.Windows.Forms.Padding(3, 12, 3, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 20)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Import"
        '
        'chkImportUseDefaults
        '
        Me.chkImportUseDefaults.AutoSize = True
        Me.chkImportUseDefaults.Location = New System.Drawing.Point(161, 94)
        Me.chkImportUseDefaults.Name = "chkImportUseDefaults"
        Me.chkImportUseDefaults.Size = New System.Drawing.Size(15, 14)
        Me.chkImportUseDefaults.TabIndex = 4
        Me.chkImportUseDefaults.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(23, 94)
        Me.Label4.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(118, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Bypass Import Window:"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.cpExportSvgColorBottom, 2, 13)
        Me.TableLayoutPanel1.Controls.Add(Me.cpExportSvgColorFill, 2, 12)
        Me.TableLayoutPanel1.Controls.Add(Me.cpExportSvgColorTop, 2, 11)
        Me.TableLayoutPanel1.Controls.Add(Me.chkExportSvgIncludeBottom, 2, 10)
        Me.TableLayoutPanel1.Controls.Add(Me.chkExportSvgIncludeFill, 2, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.chkExportSvgIncludeTop, 2, 8)
        Me.TableLayoutPanel1.Controls.Add(Me.Label14, 1, 13)
        Me.TableLayoutPanel1.Controls.Add(Me.Label13, 1, 12)
        Me.TableLayoutPanel1.Controls.Add(Me.Label12, 1, 11)
        Me.TableLayoutPanel1.Controls.Add(Me.Label11, 1, 10)
        Me.TableLayoutPanel1.Controls.Add(Me.Label10, 1, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.Label9, 1, 8)
        Me.TableLayoutPanel1.Controls.Add(Me.Label7, 0, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.cmbImportDefaultUpAxis, 2, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.cmbImportDefaultUnits, 2, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.Label6, 1, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label5, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.chkImportUseDefaults, 2, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Label4, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.cmbDisplayUnits, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label8, 1, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.chkExportSvgUseDefaults, 2, 7)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 15
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(386, 369)
        Me.TableLayoutPanel1.TabIndex = 6
        '
        'cpExportSvgColorBottom
        '
        Me.cpExportSvgColorBottom.Dock = System.Windows.Forms.DockStyle.Top
        Me.cpExportSvgColorBottom.Location = New System.Drawing.Point(161, 338)
        Me.cpExportSvgColorBottom.Name = "cpExportSvgColorBottom"
        Me.cpExportSvgColorBottom.Size = New System.Drawing.Size(222, 23)
        Me.cpExportSvgColorBottom.TabIndex = 8
        Me.cpExportSvgColorBottom.Value = System.Drawing.Color.Empty
        '
        'cpExportSvgColorFill
        '
        Me.cpExportSvgColorFill.Dock = System.Windows.Forms.DockStyle.Top
        Me.cpExportSvgColorFill.Location = New System.Drawing.Point(161, 309)
        Me.cpExportSvgColorFill.Name = "cpExportSvgColorFill"
        Me.cpExportSvgColorFill.Size = New System.Drawing.Size(222, 23)
        Me.cpExportSvgColorFill.TabIndex = 8
        Me.cpExportSvgColorFill.Value = System.Drawing.Color.Empty
        '
        'cpExportSvgColorTop
        '
        Me.cpExportSvgColorTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.cpExportSvgColorTop.Location = New System.Drawing.Point(161, 280)
        Me.cpExportSvgColorTop.Name = "cpExportSvgColorTop"
        Me.cpExportSvgColorTop.Size = New System.Drawing.Size(222, 23)
        Me.cpExportSvgColorTop.TabIndex = 8
        Me.cpExportSvgColorTop.Value = System.Drawing.Color.Empty
        '
        'chkExportSvgIncludeBottom
        '
        Me.chkExportSvgIncludeBottom.AutoSize = True
        Me.chkExportSvgIncludeBottom.Location = New System.Drawing.Point(161, 260)
        Me.chkExportSvgIncludeBottom.Name = "chkExportSvgIncludeBottom"
        Me.chkExportSvgIncludeBottom.Size = New System.Drawing.Size(15, 14)
        Me.chkExportSvgIncludeBottom.TabIndex = 11
        Me.chkExportSvgIncludeBottom.UseVisualStyleBackColor = True
        '
        'chkExportSvgIncludeFill
        '
        Me.chkExportSvgIncludeFill.AutoSize = True
        Me.chkExportSvgIncludeFill.Location = New System.Drawing.Point(161, 240)
        Me.chkExportSvgIncludeFill.Name = "chkExportSvgIncludeFill"
        Me.chkExportSvgIncludeFill.Size = New System.Drawing.Size(15, 14)
        Me.chkExportSvgIncludeFill.TabIndex = 11
        Me.chkExportSvgIncludeFill.UseVisualStyleBackColor = True
        '
        'chkExportSvgIncludeTop
        '
        Me.chkExportSvgIncludeTop.AutoSize = True
        Me.chkExportSvgIncludeTop.Location = New System.Drawing.Point(161, 220)
        Me.chkExportSvgIncludeTop.Name = "chkExportSvgIncludeTop"
        Me.chkExportSvgIncludeTop.Size = New System.Drawing.Size(15, 14)
        Me.chkExportSvgIncludeTop.TabIndex = 11
        Me.chkExportSvgIncludeTop.UseVisualStyleBackColor = True
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(23, 344)
        Me.Label14.Margin = New System.Windows.Forms.Padding(3, 9, 3, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(106, 13)
        Me.Label14.TabIndex = 11
        Me.Label14.Text = "Bottom Outline Color:"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(23, 315)
        Me.Label13.Margin = New System.Windows.Forms.Padding(3, 9, 3, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(49, 13)
        Me.Label13.TabIndex = 10
        Me.Label13.Text = "Fill Color:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(23, 286)
        Me.Label12.Margin = New System.Windows.Forms.Padding(3, 9, 3, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(92, 13)
        Me.Label12.TabIndex = 10
        Me.Label12.Text = "Top Outline Color:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(23, 260)
        Me.Label11.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(117, 13)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "Include Bottom Outline:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(23, 240)
        Me.Label10.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(60, 13)
        Me.Label10.TabIndex = 10
        Me.Label10.Text = "Include Fill:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(23, 220)
        Me.Label9.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(103, 13)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "Include Top Outline:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.TableLayoutPanel1.SetColumnSpan(Me.Label7, 3)
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Blue
        Me.Label7.Location = New System.Drawing.Point(3, 177)
        Me.Label7.Margin = New System.Windows.Forms.Padding(3, 12, 3, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(104, 20)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "SVG Export"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(23, 144)
        Me.Label6.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(115, 13)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Default Import Up Axis:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(23, 117)
        Me.Label5.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(132, 13)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Default Import Mesh Units:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(23, 200)
        Me.Label8.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(118, 13)
        Me.Label8.TabIndex = 9
        Me.Label8.Text = "Bypass Import Window:"
        '
        'chkExportSvgUseDefaults
        '
        Me.chkExportSvgUseDefaults.AutoSize = True
        Me.chkExportSvgUseDefaults.Location = New System.Drawing.Point(161, 200)
        Me.chkExportSvgUseDefaults.Name = "chkExportSvgUseDefaults"
        Me.chkExportSvgUseDefaults.Size = New System.Drawing.Size(15, 14)
        Me.chkExportSvgUseDefaults.TabIndex = 10
        Me.chkExportSvgUseDefaults.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.AutoScroll = True
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.TableLayoutPanel1)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(411, 229)
        Me.Panel1.TabIndex = 7
        '
        'frmPreferences
        '
        Me.AcceptButton = Me.butOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.butCancel
        Me.ClientSize = New System.Drawing.Size(411, 270)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.butCancel)
        Me.Controls.Add(Me.butOk)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPreferences"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Preferences"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents butOk As Button
    Friend WithEvents butCancel As Button
    Private WithEvents Label1 As Label
    Private WithEvents cmbDisplayUnits As ComboBox
    Private WithEvents ttTip As ToolTip
    Friend WithEvents Label2 As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Label4 As Label
    Friend WithEvents chkImportUseDefaults As CheckBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Private WithEvents cmbImportDefaultUpAxis As ComboBox
    Private WithEvents cmbImportDefaultUnits As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents chkExportSvgUseDefaults As CheckBox
    Friend WithEvents Label14 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents chkExportSvgIncludeBottom As CheckBox
    Friend WithEvents chkExportSvgIncludeFill As CheckBox
    Friend WithEvents chkExportSvgIncludeTop As CheckBox
    Friend WithEvents cpExportSvgColorBottom As ColorPicker
    Friend WithEvents cpExportSvgColorFill As ColorPicker
    Friend WithEvents cpExportSvgColorTop As ColorPicker
End Class
