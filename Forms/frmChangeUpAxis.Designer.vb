<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChangeUpAxis
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
        Me.txtCurrentUpAxis = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbNewUpAxis = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.butCancel = New System.Windows.Forms.Button()
        Me.butOk = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtCurrentUpAxis
        '
        Me.txtCurrentUpAxis.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCurrentUpAxis.Location = New System.Drawing.Point(101, 28)
        Me.txtCurrentUpAxis.Name = "txtCurrentUpAxis"
        Me.txtCurrentUpAxis.ReadOnly = True
        Me.txtCurrentUpAxis.Size = New System.Drawing.Size(200, 20)
        Me.txtCurrentUpAxis.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(83, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Current Up Axis:"
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(289, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "This allows you to change which axis is considered ""Up""."
        '
        'cmbNewUpAxis
        '
        Me.cmbNewUpAxis.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbNewUpAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbNewUpAxis.FormattingEnabled = True
        Me.cmbNewUpAxis.Items.AddRange(New Object() {"X Up", "Y Up", "Z Up"})
        Me.cmbNewUpAxis.Location = New System.Drawing.Point(101, 54)
        Me.cmbNewUpAxis.Name = "cmbNewUpAxis"
        Me.cmbNewUpAxis.Size = New System.Drawing.Size(200, 21)
        Me.cmbNewUpAxis.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 57)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "New Up Axis:"
        '
        'butCancel
        '
        Me.butCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.butCancel.Location = New System.Drawing.Point(226, 81)
        Me.butCancel.Name = "butCancel"
        Me.butCancel.Size = New System.Drawing.Size(75, 23)
        Me.butCancel.TabIndex = 3
        Me.butCancel.Text = "&Cancel"
        Me.butCancel.UseVisualStyleBackColor = True
        '
        'butOk
        '
        Me.butOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.butOk.Location = New System.Drawing.Point(145, 81)
        Me.butOk.Name = "butOk"
        Me.butOk.Size = New System.Drawing.Size(75, 23)
        Me.butOk.TabIndex = 2
        Me.butOk.Text = "&OK"
        Me.butOk.UseVisualStyleBackColor = True
        '
        'frmChangeUpAxis
        '
        Me.AcceptButton = Me.butOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.butCancel
        Me.ClientSize = New System.Drawing.Size(313, 116)
        Me.Controls.Add(Me.butCancel)
        Me.Controls.Add(Me.butOk)
        Me.Controls.Add(Me.cmbNewUpAxis)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtCurrentUpAxis)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmChangeUpAxis"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmChangeUpAxis"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtCurrentUpAxis As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbNewUpAxis As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents butCancel As Button
    Friend WithEvents butOk As Button
End Class
