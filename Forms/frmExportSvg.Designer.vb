<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmExportSvg
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
        Me.chkExportBottom = New System.Windows.Forms.CheckBox()
        Me.chkExportFill = New System.Windows.Forms.CheckBox()
        Me.chkExportTop = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cpColorTop = New ModelSlicer.ColorPicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cpColorFill = New ModelSlicer.ColorPicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cpColorBottom = New ModelSlicer.ColorPicker()
        Me.butExport = New System.Windows.Forms.Button()
        Me.butCancel = New System.Windows.Forms.Button()
        Me.chkDoNotAsk = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkExportBottom
        '
        Me.chkExportBottom.AutoSize = True
        Me.chkExportBottom.Location = New System.Drawing.Point(6, 65)
        Me.chkExportBottom.Name = "chkExportBottom"
        Me.chkExportBottom.Size = New System.Drawing.Size(133, 17)
        Me.chkExportBottom.TabIndex = 2
        Me.chkExportBottom.Text = "Include Bottom Outline"
        Me.chkExportBottom.UseVisualStyleBackColor = True
        '
        'chkExportFill
        '
        Me.chkExportFill.AutoSize = True
        Me.chkExportFill.Location = New System.Drawing.Point(6, 42)
        Me.chkExportFill.Name = "chkExportFill"
        Me.chkExportFill.Size = New System.Drawing.Size(76, 17)
        Me.chkExportFill.TabIndex = 1
        Me.chkExportFill.Text = "Include Fill"
        Me.chkExportFill.UseVisualStyleBackColor = True
        '
        'chkExportTop
        '
        Me.chkExportTop.AutoSize = True
        Me.chkExportTop.Location = New System.Drawing.Point(6, 19)
        Me.chkExportTop.Name = "chkExportTop"
        Me.chkExportTop.Size = New System.Drawing.Size(119, 17)
        Me.chkExportTop.TabIndex = 0
        Me.chkExportTop.Text = "Include Top Outline"
        Me.chkExportTop.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.chkExportBottom)
        Me.GroupBox1.Controls.Add(Me.chkExportTop)
        Me.GroupBox1.Controls.Add(Me.chkExportFill)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(282, 89)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Export Features"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.cpColorBottom)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.cpColorFill)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.cpColorTop)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 107)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(282, 106)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Colors"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(6, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(106, 23)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Top Outline Color:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cpColorTop
        '
        Me.cpColorTop.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cpColorTop.Location = New System.Drawing.Point(118, 19)
        Me.cpColorTop.Name = "cpColorTop"
        Me.cpColorTop.Size = New System.Drawing.Size(158, 23)
        Me.cpColorTop.TabIndex = 1
        Me.cpColorTop.Value = System.Drawing.Color.Empty
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(6, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(106, 23)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Fill Color:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cpColorFill
        '
        Me.cpColorFill.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cpColorFill.Location = New System.Drawing.Point(118, 48)
        Me.cpColorFill.Name = "cpColorFill"
        Me.cpColorFill.Size = New System.Drawing.Size(158, 23)
        Me.cpColorFill.TabIndex = 3
        Me.cpColorFill.Value = System.Drawing.Color.Empty
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(6, 77)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(106, 23)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Bottom Outline Color:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cpColorBottom
        '
        Me.cpColorBottom.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cpColorBottom.Location = New System.Drawing.Point(118, 77)
        Me.cpColorBottom.Name = "cpColorBottom"
        Me.cpColorBottom.Size = New System.Drawing.Size(158, 23)
        Me.cpColorBottom.TabIndex = 5
        Me.cpColorBottom.Value = System.Drawing.Color.Empty
        '
        'butExport
        '
        Me.butExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.butExport.Location = New System.Drawing.Point(138, 219)
        Me.butExport.Name = "butExport"
        Me.butExport.Size = New System.Drawing.Size(75, 23)
        Me.butExport.TabIndex = 3
        Me.butExport.Text = "&Export..."
        Me.butExport.UseVisualStyleBackColor = True
        '
        'butCancel
        '
        Me.butCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.butCancel.Location = New System.Drawing.Point(219, 219)
        Me.butCancel.Name = "butCancel"
        Me.butCancel.Size = New System.Drawing.Size(75, 23)
        Me.butCancel.TabIndex = 4
        Me.butCancel.Text = "&Close"
        Me.butCancel.UseVisualStyleBackColor = True
        '
        'chkDoNotAsk
        '
        Me.chkDoNotAsk.AutoSize = True
        Me.chkDoNotAsk.Location = New System.Drawing.Point(12, 223)
        Me.chkDoNotAsk.Name = "chkDoNotAsk"
        Me.chkDoNotAsk.Size = New System.Drawing.Size(107, 17)
        Me.chkDoNotAsk.TabIndex = 2
        Me.chkDoNotAsk.Text = "&Do not ask again"
        Me.chkDoNotAsk.UseVisualStyleBackColor = True
        '
        'frmExportSvg
        '
        Me.AcceptButton = Me.butExport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.butCancel
        Me.ClientSize = New System.Drawing.Size(306, 254)
        Me.Controls.Add(Me.chkDoNotAsk)
        Me.Controls.Add(Me.butCancel)
        Me.Controls.Add(Me.butExport)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmExportSvg"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Export SVG"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents chkExportBottom As CheckBox
    Friend WithEvents chkExportFill As CheckBox
    Friend WithEvents chkExportTop As CheckBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cpColorTop As ColorPicker
    Friend WithEvents Label3 As Label
    Friend WithEvents cpColorBottom As ColorPicker
    Friend WithEvents Label2 As Label
    Friend WithEvents cpColorFill As ColorPicker
    Friend WithEvents butExport As Button
    Friend WithEvents butCancel As Button
    Friend WithEvents chkDoNotAsk As CheckBox
End Class
