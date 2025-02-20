<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChangeUnits
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtCurrentUnits = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbNewUnits = New System.Windows.Forms.ComboBox()
        Me.butOk = New System.Windows.Forms.Button()
        Me.butCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(283, 31)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "This allows you to change the units used in the model file, if the wrong units we" &
    "re used when the model was loaded."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Current Units:"
        '
        'txtCurrentUnits
        '
        Me.txtCurrentUnits.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCurrentUnits.Location = New System.Drawing.Point(89, 43)
        Me.txtCurrentUnits.Name = "txtCurrentUnits"
        Me.txtCurrentUnits.ReadOnly = True
        Me.txtCurrentUnits.Size = New System.Drawing.Size(206, 20)
        Me.txtCurrentUnits.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "New Units:"
        '
        'cmbNewUnits
        '
        Me.cmbNewUnits.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbNewUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbNewUnits.FormattingEnabled = True
        Me.cmbNewUnits.Items.AddRange(New Object() {"Millimeters", "Centimeters", "Meters", "Inches", "Feet"})
        Me.cmbNewUnits.Location = New System.Drawing.Point(89, 69)
        Me.cmbNewUnits.Name = "cmbNewUnits"
        Me.cmbNewUnits.Size = New System.Drawing.Size(206, 21)
        Me.cmbNewUnits.TabIndex = 1
        '
        'butOk
        '
        Me.butOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.butOk.Location = New System.Drawing.Point(139, 96)
        Me.butOk.Name = "butOk"
        Me.butOk.Size = New System.Drawing.Size(75, 23)
        Me.butOk.TabIndex = 2
        Me.butOk.Text = "&OK"
        Me.butOk.UseVisualStyleBackColor = True
        '
        'butCancel
        '
        Me.butCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.butCancel.Location = New System.Drawing.Point(220, 96)
        Me.butCancel.Name = "butCancel"
        Me.butCancel.Size = New System.Drawing.Size(75, 23)
        Me.butCancel.TabIndex = 3
        Me.butCancel.Text = "&Cancel"
        Me.butCancel.UseVisualStyleBackColor = True
        '
        'frmChangeUnits
        '
        Me.AcceptButton = Me.butOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.butCancel
        Me.ClientSize = New System.Drawing.Size(307, 131)
        Me.Controls.Add(Me.butCancel)
        Me.Controls.Add(Me.butOk)
        Me.Controls.Add(Me.cmbNewUnits)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtCurrentUnits)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmChangeUnits"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Change Model Units"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtCurrentUnits As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents cmbNewUnits As ComboBox
    Friend WithEvents butOk As Button
    Friend WithEvents butCancel As Button
End Class
