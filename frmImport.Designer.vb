<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtFileName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbUnits = New System.Windows.Forms.ComboBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rbUpX = New System.Windows.Forms.RadioButton()
        Me.rbUpY = New System.Windows.Forms.RadioButton()
        Me.rbUpZ = New System.Windows.Forms.RadioButton()
        Me.butImport = New System.Windows.Forms.Button()
        Me.butCancel = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(26, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "File:"
        '
        'txtFileName
        '
        Me.txtFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFileName.Location = New System.Drawing.Point(81, 12)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.ReadOnly = True
        Me.txtFileName.Size = New System.Drawing.Size(560, 20)
        Me.txtFileName.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Mesh Units:"
        '
        'cmbUnits
        '
        Me.cmbUnits.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbUnits.FormattingEnabled = True
        Me.cmbUnits.Items.AddRange(New Object() {"Millimeters", "Centimeters", "Meters", "Inches", "Feet"})
        Me.cmbUnits.Location = New System.Drawing.Point(81, 38)
        Me.cmbUnits.Name = "cmbUnits"
        Me.cmbUnits.Size = New System.Drawing.Size(560, 21)
        Me.cmbUnits.TabIndex = 3
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.rbUpZ)
        Me.GroupBox1.Controls.Add(Me.rbUpY)
        Me.GroupBox1.Controls.Add(Me.rbUpX)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 65)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(629, 46)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Up Axis"
        '
        'rbUpX
        '
        Me.rbUpX.AutoSize = True
        Me.rbUpX.Location = New System.Drawing.Point(6, 19)
        Me.rbUpX.Name = "rbUpX"
        Me.rbUpX.Size = New System.Drawing.Size(49, 17)
        Me.rbUpX.TabIndex = 0
        Me.rbUpX.TabStop = True
        Me.rbUpX.Text = "X-Up"
        Me.rbUpX.UseVisualStyleBackColor = True
        '
        'rbUpY
        '
        Me.rbUpY.AutoSize = True
        Me.rbUpY.Checked = True
        Me.rbUpY.Location = New System.Drawing.Point(61, 19)
        Me.rbUpY.Name = "rbUpY"
        Me.rbUpY.Size = New System.Drawing.Size(49, 17)
        Me.rbUpY.TabIndex = 1
        Me.rbUpY.TabStop = True
        Me.rbUpY.Text = "Y-Up"
        Me.rbUpY.UseVisualStyleBackColor = True
        '
        'rbUpZ
        '
        Me.rbUpZ.AutoSize = True
        Me.rbUpZ.Location = New System.Drawing.Point(116, 19)
        Me.rbUpZ.Name = "rbUpZ"
        Me.rbUpZ.Size = New System.Drawing.Size(49, 17)
        Me.rbUpZ.TabIndex = 2
        Me.rbUpZ.TabStop = True
        Me.rbUpZ.Text = "Z-Up"
        Me.rbUpZ.UseVisualStyleBackColor = True
        '
        'butImport
        '
        Me.butImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.butImport.Location = New System.Drawing.Point(485, 117)
        Me.butImport.Name = "butImport"
        Me.butImport.Size = New System.Drawing.Size(75, 23)
        Me.butImport.TabIndex = 5
        Me.butImport.Text = "&Import"
        Me.butImport.UseVisualStyleBackColor = True
        '
        'butCancel
        '
        Me.butCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.butCancel.Location = New System.Drawing.Point(566, 117)
        Me.butCancel.Name = "butCancel"
        Me.butCancel.Size = New System.Drawing.Size(75, 23)
        Me.butCancel.TabIndex = 6
        Me.butCancel.Text = "&Cancel"
        Me.butCancel.UseVisualStyleBackColor = True
        '
        'frmImport
        '
        Me.AcceptButton = Me.butImport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.butCancel
        Me.ClientSize = New System.Drawing.Size(653, 152)
        Me.Controls.Add(Me.butCancel)
        Me.Controls.Add(Me.butImport)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cmbUnits)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtFileName)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmImport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Import OBJ File"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents txtFileName As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbUnits As ComboBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents rbUpZ As RadioButton
    Friend WithEvents rbUpY As RadioButton
    Friend WithEvents rbUpX As RadioButton
    Friend WithEvents butImport As Button
    Friend WithEvents butCancel As Button
End Class
