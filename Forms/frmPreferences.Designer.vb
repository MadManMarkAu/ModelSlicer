<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPreferences
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
        Me.components = New System.ComponentModel.Container()
        Me.butOk = New System.Windows.Forms.Button()
        Me.butCancel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbDisplayUnits = New System.Windows.Forms.ComboBox()
        Me.ttTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'butOk
        '
        Me.butOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.butOk.Location = New System.Drawing.Point(166, 39)
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
        Me.butCancel.Location = New System.Drawing.Point(247, 39)
        Me.butCancel.Name = "butCancel"
        Me.butCancel.Size = New System.Drawing.Size(75, 23)
        Me.butCancel.TabIndex = 3
        Me.butCancel.Text = "&Cancel"
        Me.butCancel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
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
        Me.cmbDisplayUnits.Location = New System.Drawing.Point(89, 12)
        Me.cmbDisplayUnits.Name = "cmbDisplayUnits"
        Me.cmbDisplayUnits.Size = New System.Drawing.Size(233, 21)
        Me.cmbDisplayUnits.TabIndex = 1
        Me.ttTip.SetToolTip(Me.cmbDisplayUnits, "Sets the units and format to use when displaying values")
        '
        'frmPreferences
        '
        Me.AcceptButton = Me.butOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.butCancel
        Me.ClientSize = New System.Drawing.Size(334, 74)
        Me.Controls.Add(Me.cmbDisplayUnits)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.butCancel)
        Me.Controls.Add(Me.butOk)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPreferences"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Preferences"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents butOk As Button
    Friend WithEvents butCancel As Button
    Private WithEvents Label1 As Label
    Private WithEvents cmbDisplayUnits As ComboBox
    Private WithEvents ttTip As ToolTip
End Class
