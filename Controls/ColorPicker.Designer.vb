<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ColorPicker
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.butPickColor = New System.Windows.Forms.Button()
        Me.pnlColorDisplay = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        'butPickColor
        '
        Me.butPickColor.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.butPickColor.Location = New System.Drawing.Point(86, 0)
        Me.butPickColor.Margin = New System.Windows.Forms.Padding(0)
        Me.butPickColor.Name = "butPickColor"
        Me.butPickColor.Size = New System.Drawing.Size(30, 23)
        Me.butPickColor.TabIndex = 9
        Me.butPickColor.Text = "..."
        Me.butPickColor.UseVisualStyleBackColor = True
        '
        'pnlColorDisplay
        '
        Me.pnlColorDisplay.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlColorDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlColorDisplay.Location = New System.Drawing.Point(0, 0)
        Me.pnlColorDisplay.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlColorDisplay.Name = "pnlColorDisplay"
        Me.pnlColorDisplay.Size = New System.Drawing.Size(86, 23)
        Me.pnlColorDisplay.TabIndex = 8
        '
        'ColorPicker
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.butPickColor)
        Me.Controls.Add(Me.pnlColorDisplay)
        Me.Name = "ColorPicker"
        Me.Size = New System.Drawing.Size(116, 23)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents pnlColorDisplay As Panel
    Private WithEvents butPickColor As Button
End Class
