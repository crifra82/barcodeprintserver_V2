<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WatchFold
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
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

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WatchFold))
        Me.txt_watchpath = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btn_startwatch = New System.Windows.Forms.Button()
        Me.btn_stop = New System.Windows.Forms.Button()
        Me.txt_folderactivity = New System.Windows.Forms.TextBox()
        Me.IconMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.menuOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.txt_barcode = New System.Windows.Forms.TextBox()
        Me.txt_header = New System.Windows.Forms.TextBox()
        Me.txtDescri1 = New System.Windows.Forms.TextBox()
        Me.txtDescri2 = New System.Windows.Forms.TextBox()
        Me.IconMenu.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txt_watchpath
        '
        Me.txt_watchpath.BackColor = System.Drawing.Color.PowderBlue
        Me.txt_watchpath.Location = New System.Drawing.Point(119, 6)
        Me.txt_watchpath.Name = "txt_watchpath"
        Me.txt_watchpath.Size = New System.Drawing.Size(235, 20)
        Me.txt_watchpath.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Type folder to watch :"
        '
        'btn_startwatch
        '
        Me.btn_startwatch.Location = New System.Drawing.Point(360, 2)
        Me.btn_startwatch.Name = "btn_startwatch"
        Me.btn_startwatch.Size = New System.Drawing.Size(100, 26)
        Me.btn_startwatch.TabIndex = 2
        Me.btn_startwatch.Text = "Start Watching"
        Me.btn_startwatch.UseVisualStyleBackColor = True
        '
        'btn_stop
        '
        Me.btn_stop.Location = New System.Drawing.Point(360, 33)
        Me.btn_stop.Name = "btn_stop"
        Me.btn_stop.Size = New System.Drawing.Size(100, 26)
        Me.btn_stop.TabIndex = 3
        Me.btn_stop.Text = "Stop Watching"
        Me.btn_stop.UseVisualStyleBackColor = True
        '
        'txt_folderactivity
        '
        Me.txt_folderactivity.BackColor = System.Drawing.Color.PowderBlue
        Me.txt_folderactivity.Location = New System.Drawing.Point(12, 65)
        Me.txt_folderactivity.Multiline = True
        Me.txt_folderactivity.Name = "txt_folderactivity"
        Me.txt_folderactivity.Size = New System.Drawing.Size(448, 161)
        Me.txt_folderactivity.TabIndex = 4
        '
        'IconMenu
        '
        Me.IconMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuOpen, Me.menuExit})
        Me.IconMenu.Name = "IconMenu"
        Me.IconMenu.Size = New System.Drawing.Size(112, 48)
        '
        'menuOpen
        '
        Me.menuOpen.Name = "menuOpen"
        Me.menuOpen.Size = New System.Drawing.Size(111, 22)
        Me.menuOpen.Text = "Open"
        '
        'menuExit
        '
        Me.menuExit.Name = "menuExit"
        Me.menuExit.Size = New System.Drawing.Size(111, 22)
        Me.menuExit.Text = "Exit"
        '
        'PrintDocument1
        '
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(41, 33)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(100, 50)
        Me.PictureBox1.TabIndex = 5
        Me.PictureBox1.TabStop = False
        Me.PictureBox1.Visible = False
        '
        'txt_barcode
        '
        Me.txt_barcode.BackColor = System.Drawing.Color.PowderBlue
        Me.txt_barcode.Location = New System.Drawing.Point(147, 37)
        Me.txt_barcode.Name = "txt_barcode"
        Me.txt_barcode.Size = New System.Drawing.Size(144, 20)
        Me.txt_barcode.TabIndex = 6
        Me.txt_barcode.Visible = False
        '
        'txt_header
        '
        Me.txt_header.BackColor = System.Drawing.Color.PowderBlue
        Me.txt_header.Location = New System.Drawing.Point(210, 51)
        Me.txt_header.Name = "txt_header"
        Me.txt_header.Size = New System.Drawing.Size(144, 20)
        Me.txt_header.TabIndex = 7
        Me.txt_header.Visible = False
        '
        'txtDescri1
        '
        Me.txtDescri1.BackColor = System.Drawing.Color.PowderBlue
        Me.txtDescri1.Location = New System.Drawing.Point(147, 63)
        Me.txtDescri1.Name = "txtDescri1"
        Me.txtDescri1.Size = New System.Drawing.Size(144, 20)
        Me.txtDescri1.TabIndex = 8
        Me.txtDescri1.Visible = False
        '
        'txtDescri2
        '
        Me.txtDescri2.BackColor = System.Drawing.Color.PowderBlue
        Me.txtDescri2.Location = New System.Drawing.Point(210, 77)
        Me.txtDescri2.Name = "txtDescri2"
        Me.txtDescri2.Size = New System.Drawing.Size(144, 20)
        Me.txtDescri2.TabIndex = 9
        Me.txtDescri2.Visible = False
        '
        'WatchFold
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.PowderBlue
        Me.ClientSize = New System.Drawing.Size(469, 238)
        Me.Controls.Add(Me.txtDescri2)
        Me.Controls.Add(Me.txtDescri1)
        Me.Controls.Add(Me.txt_header)
        Me.Controls.Add(Me.txt_barcode)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.txt_folderactivity)
        Me.Controls.Add(Me.btn_stop)
        Me.Controls.Add(Me.btn_startwatch)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txt_watchpath)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "WatchFold"
        Me.Text = "Watch folder..."
        Me.IconMenu.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txt_watchpath As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btn_startwatch As System.Windows.Forms.Button
    Friend WithEvents btn_stop As System.Windows.Forms.Button
    Friend WithEvents txt_folderactivity As System.Windows.Forms.TextBox
    Friend WithEvents IconMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents menuOpen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents txt_barcode As System.Windows.Forms.TextBox
    Friend WithEvents txt_header As System.Windows.Forms.TextBox
    Friend WithEvents txtDescri1 As System.Windows.Forms.TextBox
    Friend WithEvents txtDescri2 As System.Windows.Forms.TextBox

End Class
