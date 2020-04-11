<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.ldDrawer = New ModelSlicer.LineDrawer()
        Me.tbSlice = New System.Windows.Forms.TrackBar()
        CType(Me.tbSlice, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ldDrawer
        '
        Me.ldDrawer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ldDrawer.Location = New System.Drawing.Point(12, 12)
        Me.ldDrawer.Name = "ldDrawer"
        Me.ldDrawer.Size = New System.Drawing.Size(495, 420)
        Me.ldDrawer.TabIndex = 0
        Me.ldDrawer.Text = "LineDrawer1"
        Me.ldDrawer.ViewBounds = CType(resources.GetObject("ldDrawer.ViewBounds"), System.Drawing.RectangleF)
        '
        'tbSlice
        '
        Me.tbSlice.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSlice.Location = New System.Drawing.Point(12, 438)
        Me.tbSlice.Name = "tbSlice"
        Me.tbSlice.Size = New System.Drawing.Size(495, 45)
        Me.tbSlice.TabIndex = 1
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(519, 495)
        Me.Controls.Add(Me.tbSlice)
        Me.Controls.Add(Me.ldDrawer)
        Me.Name = "frmMain"
        Me.Text = "Form1"
        CType(Me.tbSlice, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ldDrawer As LineDrawer
    Friend WithEvents tbSlice As TrackBar
End Class
