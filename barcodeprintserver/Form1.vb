Imports System.IO
Imports System.Diagnostics
Imports System.Xml



Public Class WatchFold
    Public Function CleanString(ByVal input As String) As String
        If input Is Nothing Then Return ""
        Return System.Text.RegularExpressions.Regex.Replace(input, "[\x00-\x1F\x7F]", "")
    End Function

    Public watchfolder As FileSystemWatcher
    Public WithEvents TrayIcon As NotifyIcon
    Public VedoFrm As Boolean
    Public bWatching As Boolean = False
    Public icon_ok As New Icon("analysis32.ico")
    Public icon_alert As New Icon("Folder-Warning32.ico")
    Dim _index As Integer = 1 ' global label counter
    Dim _rowPerpage As Integer = 1
    Dim _numRig As Integer = 1
    Dim _posY As Integer
    Dim _startPosY As Integer = 0
    Dim _numTotRig As Integer = 0
    Dim _labelIndex As Integer = 1
    Dim FontHeader As New Font("Arial Black", 16)
    Dim _barcode As String = ""
    Dim _t_copie As Integer = 1
    Dim _copie As Integer = Globale.g_number
    Public _larg As Integer = Globale.g_larg
    Public _lung As Integer = Globale.g_lung

    Dim cf As New CConfig
    Dim hLabel As New Hashtable ' hasttable containing ten sLabel structure 
    Public Structure sLabel
        Dim x As String
        Dim y As String
        Dim font As String
        Dim size As String
        Dim style As String
    End Structure
    Dim labelStructure As sLabel
    Dim PrinterName As String
    '
    Public Structure sLabelrows
        '
        Dim labelfield0 As String
        Dim labelfield1 As String
        Dim labelfield2 As String
        Dim labelfield3 As String
        Dim code128 As String
        Dim ean13 As String
        '
    End Structure
    Dim labelRows As sLabelrows
    'aggiunta riga per file multiplo
    Dim labelRowsList As New List(Of sLabelrows)

    Private Sub btn_startwatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_startwatch.Click
        startWatching()
     End Sub
    Private Sub startWatching()
        Try
            watchfolder = New System.IO.FileSystemWatcher()

            'this is the path we want to monitor
            watchfolder.Path = txt_watchpath.Text
            watchfolder.SynchronizingObject = Me


            'Add a list of Filter we want to specify
            'make sure you use OR for each Filter as we need to
            'all of those 

            watchfolder.NotifyFilter = IO.NotifyFilters.DirectoryName
            watchfolder.NotifyFilter = watchfolder.NotifyFilter Or _
                                       IO.NotifyFilters.FileName
            watchfolder.NotifyFilter = watchfolder.NotifyFilter Or _
                                       IO.NotifyFilters.Attributes

            ' add the handler to each event
            AddHandler watchfolder.Changed, AddressOf logchange
            AddHandler watchfolder.Created, AddressOf logchange
            AddHandler watchfolder.Deleted, AddressOf logchange

            ' add the rename handler as the signature is different
            AddHandler watchfolder.Renamed, AddressOf logrename

            'Set this property to true to start watching
            watchfolder.EnableRaisingEvents = True

            btn_startwatch.Enabled = False
            btn_stop.Enabled = True

            bWatching = True
            'End of code for btn_start_click

        Catch ex As Exception
            bWatching = False
            MsgBox(ex.Message, MsgBoxStyle.Critical, "start watch")
        End Try


    End Sub
    Private Sub logchange(ByVal source As Object, ByVal e As System.IO.FileSystemEventArgs)
        Dim i As Integer

        Try
            If e.ChangeType = IO.WatcherChangeTypes.Changed Then
                txt_folderactivity.Text &= "File " & e.FullPath &
                                        " has been modified" & vbCrLf
                For i = 1 To 1
                    readXmlLabelFile(e.FullPath, i)
                Next


            End If
            If e.ChangeType = IO.WatcherChangeTypes.Created Then
                txt_folderactivity.Text &= "File " & e.FullPath &
                                         " has been created" & vbCrLf
                For index = 1 To 1
                    readXmlLabelFile(e.FullPath, index)
                Next

            End If
            If e.ChangeType = IO.WatcherChangeTypes.Deleted Then

                txt_folderactivity.Text &= "File " & e.FullPath &
                                             " has been deleted" & vbCrLf
            End If
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "logchange")
        End Try
    End Sub
    Private Delegate Sub setNotifyDelegate(ByVal pField As Object)
    Private setNotityDelegateItem As New setNotifyDelegate(AddressOf setNotify)
    Private Sub setNotify(ByVal pField As Object)

    End Sub
    Private Sub btn_stop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_stop.Click
        watchfolder.EnableRaisingEvents = False
        btn_startwatch.Enabled = True
        btn_stop.Enabled = False
        bWatching = False

    End Sub
    Public Sub logrename(ByVal source As Object, ByVal e As  _
                            System.IO.RenamedEventArgs)
        txt_folderactivity.Text &= "File" & e.OldName & _
                      " has been renamed to " & e.Name & vbCrLf
    End Sub

    Private Sub WatchFold_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
    End Sub

    Private Sub WatchFold_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Nuova istanza di TrayIcon (crea in automatico l'icona nella TrayBar)
        Me.TrayIcon = New NotifyIcon
        '
        VedoFrm = True
        'Dato che vedo il Form non vediamo la TrayIcon
        With TrayIcon
            .Visible = False
            .Icon = icon_ok
            'Come TrayIcon diciamo di usare la stessa della Form
            .Text = "Watching folder " & Me.txt_watchpath.Text.Trim & "..." ' Testo visualizzato al passaggio del Mouse sulla TrayIcon
            .ContextMenuStrip = Me.IconMenu
            'Mostro il Menu definito nella Form
        End With
        Me.txt_watchpath.Text = Globale.g_FolderToWatch.Trim
        If Globale.g_StartMinimized Then
            startWatching()
            Me.WindowState = FormWindowState.Minimized
            Me.ShowInTaskbar = False
        End If
        If Me.ShowInTaskbar Then
            VedoFrm = False
            ' Prendo atto del fatto che la Form nn \u232 ? pi\u249 ? visualizzata
            Me.ShowInTaskbar = False
            ' Nascondo il pulsante nella TaskBar
            TrayIcon.Visible = True
            ' Mostro la TrayIcon
        End If

    End Sub
    Private Sub me_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        ' Con questo nascondiamo la TaskBarIcon e mostriamo la TrayBarIcon
        ' N.B. la TrayBarIcon appare dalla PRIMA volta che minimizziamo la form 
        ' e non scompare fino a quando non viene chiusa
        If Not IsNothing(TrayIcon) Then
            With TrayIcon
                If bWatching Then
                    .Icon = icon_ok
                    .Text = "Watching folder " & Me.txt_watchpath.Text.Trim & "..." ' Testo visualizzato al passaggio del Mouse sulla TrayIcon
                Else
                    .Icon = icon_alert
                    .Text = "The Watcher is down..." ' Testo visualizzato al passaggio del Mouse sulla TrayIcon
                End If
            End With
        End If
        If VedoFrm And Me.WindowState = FormWindowState.Minimized Then
            VedoFrm = False
            ' Prendo atto del fatto che la Form nn \u232 ? pi\u249 ? visualizzata
            Me.ShowInTaskbar = False
            ' Nascondo il pulsante nella TaskBar
            TrayIcon.Visible = True
            ' Mostro la TrayIcon
        End If
        ' N.B. Se si vuole generare una TrayIcon duratura per tutta l'esecuzione 
        ' attivarla in fase di Load
    End Sub
    Private Sub menuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuExit.Click
        ' Con questo gestiamo la TrayIcon anche in fase di terminazione
        ' N.B: Se si possiede nella Form un codice di chiusura (tipo il tasto di chiusura) generate
        ' un evento che lo attivi cos\u236 ? da chiudere la TrayIcon prima di uscire
        TrayIcon.Visible = False
        ' "Cancelliamo" l'icona dalla TrayBar
        Me.Close()
        ' Chiudiamo la Form
        ' N.B. quanto scritto sopra
        End
        ' Fine del Programma - Naturalmente non si usa End, ma qui \u232 ? messa solo per 'capire'
    End Sub

    Private Sub menuOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuOpen.Click
        ' Con questo riportiamo la Form in primo piano quando clicchiamo la voce "Open" nel
        ' menu della TrayIcon oppure quando facciamo dippio click su di essa
        Me.WindowState = FormWindowState.Normal
        ' Riportiamo la Form nelle condizioni standard
        Me.ShowInTaskbar = True
        ' Mostriamo il pulsante della Form nella TaskBar
        VedoFrm = True
        TrayIcon.Visible = False
        ' Nascondo la TrayIcon
    End Sub

    Private Sub OldPrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim _rowfont As New System.Drawing.Font("Arial", 8)
        For _ii As Integer = 1 To _rowPerpage
            _barcode = Me.txt_barcode.Text
            genBarcode(_barcode, "code128")
            e.Graphics.DrawString(txt_header.Text, FontHeader, Brushes.Black, 80, _startPosY + _posY)
            e.Graphics.DrawImage(Me.PictureBox1.Image, New Point(80, _startPosY + _posY + 30))
            e.Graphics.DrawString(txtDescri1.Text, _rowfont, Brushes.Black, 80, _startPosY + _posY + 115)
            e.Graphics.DrawString(txtDescri2.Text, _rowfont, Brushes.Black, 80, _startPosY + _posY + 127)
            _posY = _posY + 10
            _numRig = _numRig + 1
            _index = _index + 1
            If _t_copie = _copie Then
                _t_copie = 1
                _labelIndex = _labelIndex + 1
            Else
                _t_copie = _t_copie + 1
            End If
        Next
        _posY = 0
        _numRig = 0
        If _index <= _numTotRig Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If


    End Sub
    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        Try
            Dim larghezza As Integer
            larghezza = Convert.ToInt32(g_larg)
            Dim lunghezza As Integer
            lunghezza = Convert.ToInt32(g_lung)

            For _ii As Integer = 1 To _rowPerpage

                For Each _row As String In hLabel.Keys
                    Dim _tplrow As New sLabel
                    _tplrow = hLabel(_row)
                    With _tplrow
                        Try
                            Dim _style As String = .style
                            Dim lStyle As FontStyle = FontStyle.Regular   ' default

                            Select Case _style.ToLower()
                                Case "bold"
                                    lStyle = FontStyle.Bold
                                Case "normal"
                                    lStyle = FontStyle.Regular
                                Case "strike"
                                    lStyle = FontStyle.Strikeout
                                Case "italic"
                                    lStyle = FontStyle.Italic
                                Case "underline"
                                    lStyle = FontStyle.Underline
                                Case Else
                                    ' Se arriva un valore sconosciuto, resta Regular
                            End Select
                            e.Graphics.PageUnit = GraphicsUnit.Millimeter
                            Dim _rowfont As New System.Drawing.Font(.font, .size, lStyle, GraphicsUnit.Millimeter)
                            Select Case _row.ToString.ToLower
                                Case "labelfield0"
                                    'CODICE 
                                    e.Graphics.DrawString(CleanString(labelRows.labelfield0), _rowfont, Brushes.Black, .x, _startPosY + .y)
                                Case "labelfield1"
                                    'descrizione articolo 
                                    e.Graphics.DrawString(CleanString(labelRows.labelfield1), _rowfont, Brushes.Black, .x, _startPosY + .y)
                                Case "ean13"
                                    'non uso la funzione barcode_x perchè uso la funzione barcode di adhoc
                                    'come font devo usare EanP36Tt sia per ean8 che ean13
                                    If CTran(labelRows.ean13, "") <> "" Then
                                        'Dim stringa As String = barcode_x("ean13", labelRows.ean13
                                        Dim stringa As String = labelRows.ean13
                                        'e.Graphics.DrawString(stringa, New Font("Code EAN13", .size), Brushes.Black, .x, _startPosY + .y
                                        e.Graphics.DrawString(stringa, New Font("EanP36Tt", .size), Brushes.Black, .x, _startPosY + .y)
                                    End If
                                Case "code128"
                                    ' Recupera la stringa del barcode in modo sicuro
                                    Dim codeStr As String = CStr(CTran(labelRows.code128, ""))

                                    ' Controlla che la stringa non sia vuota
                                    If Not String.IsNullOrEmpty(codeStr) Then
                                        ' Genera l'immagine del barcode
                                        Dim barcodeImg As Image = genBarcodeImage(codeStr)

                                        If barcodeImg IsNot Nothing Then
                                            ' Definisci coordinate numeriche sicure
                                            Dim posX As Single = CSng(.x)
                                            Dim posY As Single = CSng(_startPosY + .y)

                                            ' Disegna l'immagine
                                            'e.Graphics.DrawImage(barcodeImg, posX, posY, barcodeImg.Width, barcodeImg.Height
                                            e.Graphics.DrawImage(barcodeImg, posX, posY, larghezza, lunghezza)
                                        End If
                                    End If



                            End Select
                        Catch ex As Exception

                        End Try
                    End With
                Next
                '
                _posY = _posY + 10
                _numRig = _numRig + 1
                _index = _index + 1
                If _t_copie = _copie Then
                    _t_copie = 1
                    _labelIndex = _labelIndex + 1
                Else
                    _t_copie = _t_copie + 1
                End If

            Next
            _posY = 0
            _numRig = 0
            If _index <= _numTotRig Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If

        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "PrintDocument")
        End Try

    End Sub
    Private Function genBarcodeImage(ByVal pString As String) As Image
        Dim B As New Barcodes.Barcode128(Barcodes.Barcode128.BCEncoding.Code128B, False)
        With B
            .BarWidth = 1
            .ShowString = True
            Return .DrawBarCode(pString)
        End With
    End Function
    Private Sub genBarcode(ByVal pString As String, ByVal CodeType As String)
        'non usata
        Select Case CodeType.ToLower
            Case "code128"
                Dim B As Barcodes.Barcode128

                B = New Barcodes.Barcode128(Barcodes.Barcode128.BCEncoding.Code128B, False)
                '
                Dim _dimension As New Size
                _dimension.Height = 30
                _dimension.Width = 150
                With B
                    .BarWidth = 1
                    .ShowString = True
                    PictureBox1.Image = .DrawBarCode(pString)
                    PictureBox1.Size = _dimension
                    PictureBox1.Refresh()
                    'txtCheckSum.Text = .CheckSum
                    'txtCoded.Text = .EncodedString
                    'txtBinary.Text = .BinaryString
                End With
        End Select


    End Sub
    Private Sub printLabel()
        Try
            _numTotRig = _copie
            _numTotRig = _numTotRig
            '_numTotRig = _numTotRig * _copie
            _index = 1
            'If PrinterName = "" Then
            'PrintDocument1.PrinterSettings.PrinterName = Globale.g_printer
            'Else
            'PrintDocument1.PrinterSettings.PrinterName = PrinterName
            'End If

            'prendiamo di default la stampante nel file config e come seconda scelta quella di ahr
            If Globale.g_printer = "" Then
                PrintDocument1.PrinterSettings.PrinterName = PrinterName
            Else
                PrintDocument1.PrinterSettings.PrinterName = Globale.g_printer
            End If


            PrintDocument1.Print()
        Catch ex As Printing.InvalidPrinterException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "PrintLabel")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "PrintLabel")
        End Try
    End Sub
    Private Function __old___readXmlLabelFile(ByVal pFilename As String, i As Integer) As Boolean

        Try
            labelRows = New sLabelrows
            Dim XmlNodo As Xml.XmlNodeList
            Dim _array As New ArrayList
            Dim _File As String = pFilename
            Dim _barcode As String = ""
            Dim _header As String = ""
            Dim _descr1 As String = ""
            Dim _descr2 As String = ""
            'oggetto per il file xml
            Dim Xmlfile As New XmlDocument
            'aggiunta riga per file multiplo
            ' svuoto la lista prima di iniziare
            labelRowsList.Clear()

            '
            'carico il file
            Threading.Thread.CurrentThread.Sleep(g_time)
            Dim XmlLeggi As New XmlTextReader(_File)
            '
            XmlLeggi.WhitespaceHandling = WhitespaceHandling.None
            '
            Xmlfile.Load(XmlLeggi)
            '
            '
            XmlNodo = Xmlfile.GetElementsByTagName("dataroot")
            For Each nodo As XmlNode In XmlNodo
                For Each element As XmlNode In nodo.ChildNodes
                    Dim name As String = element.Name
                    Dim value As String = element.InnerText
                    Select Case name.ToLower
                        Case "templatename"
                            Me.loadTemplate(value)
                        Case "printername"
                            Me.PrinterName = value
                    End Select
                Next
            Next
            '
            XmlNodo = Xmlfile.GetElementsByTagName("row")
            For Each nodo As XmlNode In XmlNodo
                For Each element As XmlNode In nodo.ChildNodes
                    Dim name As String = element.Name
                    Dim value As String = CleanString(CTran(element.InnerText, ""))
                    With labelRows
                        Select Case name.ToLower
                            Case "labelfield0"
                                .labelfield0 = value
                            Case "labelfield1"
                                .labelfield1 = value
                            Case "labelfield2"
                                .labelfield2 = value
                            Case "labelfield3"
                                .labelfield3 = value
                            Case "ean13"
                                .ean13 = value
                            Case "code128"
                                .code128 = value
                        End Select
                    End With
                Next
            Next
            Xmlfile = New XmlDocument
            XmlLeggi.Close()
            XmlLeggi = Nothing
            System.IO.File.Delete(_File)
            printLabel()
            Return True
        Catch ex As IOException
            txt_folderactivity.Text &= "Errore lettura file, numero lettura: " & i & vbCrLf
            Return False
        Catch ex As Exception
            txt_folderactivity.Text &= "errore secondo catch File, numero lettura: " & i & vbCrLf
            MsgBox(ex.Message, MsgBoxStyle.Critical, "readxmllabelfile")
            Return Nothing
        End Try

    End Function

    Private Function readXmlLabelFile(ByVal pFilename As String, i As Integer) As Boolean
        Try
            Threading.Thread.Sleep(g_time)

            Dim xmlDoc As New XmlDocument()
            Dim xmlReader As New XmlTextReader(pFilename) With {
            .WhitespaceHandling = WhitespaceHandling.None
        }

            labelRowsList.Clear()

            xmlDoc.Load(xmlReader)

            ' --- LETTURA TEMPLATE E PRINTER NAME ---
            For Each nodo As XmlNode In xmlDoc.GetElementsByTagName("dataroot")
                For Each element As XmlNode In nodo.ChildNodes
                    Select Case element.Name.ToLower()
                        Case "templatename"
                            loadTemplate(element.InnerText)
                        Case "printername"
                            PrinterName = element.InnerText
                    End Select
                Next
            Next

            ' --- LETTURA DI TUTTE LE RIGHE <row> ---
            For Each nodo As XmlNode In xmlDoc.GetElementsByTagName("row")

                Dim rowItem As New sLabelrows

                For Each element As XmlNode In nodo.ChildNodes
                    Dim tag As String = element.Name.ToLower()
                    Dim value As String = CleanString(CTran(element.InnerText, ""))

                    Select Case tag
                        Case "labelfield0" : rowItem.labelfield0 = value
                        Case "labelfield1" : rowItem.labelfield1 = value
                        Case "labelfield2" : rowItem.labelfield2 = value
                        Case "labelfield3" : rowItem.labelfield3 = value
                        Case "ean13" : rowItem.ean13 = value
                        Case "code128" : rowItem.code128 = value
                    End Select
                Next

                labelRowsList.Add(rowItem)
            Next

            xmlReader.Close()

            ' elimina il file sorgente
            File.Delete(pFilename)

            ' --- STAMPA UNA ETICHETTA PER OGNI RIGA ---
            For Each rowData As sLabelrows In labelRowsList
                labelRows = rowData
                printLabel()
            Next

            Return True

        Catch ex As IOException
            txt_folderactivity.Text &= $"Errore lettura file, numero lettura: {i}" & vbCrLf
            Return False

        Catch ex As Exception
            txt_folderactivity.Text &= $"Errore non gestito, numero lettura: {i}" & vbCrLf
            MsgBox(ex.Message, MsgBoxStyle.Critical, "readXmlLabelFile")
            Return False
        End Try
    End Function

    Private Function loadTemplate(ByVal pFilename As String) As Boolean

        Try
            hLabel.Clear()
            Dim _labelTagArray As New ArrayList
            Dim XmlNodo As Xml.XmlNodeList
            Dim _array As New ArrayList
            Dim _File As String = pFilename
            Dim _barcode As String = ""
            Dim _header As String = ""
            Dim _descr1 As String = ""
            Dim _descr2 As String = ""
            '
            _labelTagArray.Add("labelfield0")
            _labelTagArray.Add("labelfield1")
            _labelTagArray.Add("labelfield2")
            _labelTagArray.Add("labelfield3")
            _labelTagArray.Add("code128")
            _labelTagArray.Add("ean13")
            '
            'oggetto per il file xml
            Dim Xmlfile As New XmlDocument
            '
            'carico il file
            Dim XmlLeggi As New XmlTextReader(_File)
            '
            XmlLeggi.WhitespaceHandling = WhitespaceHandling.None
            '
            Xmlfile.Load(XmlLeggi)
            '
            _labelTagArray.Sort()
            'Loads label rows informations
            For Each _tag As String In _labelTagArray
                '
                XmlNodo = Xmlfile.GetElementsByTagName(_tag)
                For Each nodo As XmlNode In XmlNodo
                    For Each element As XmlNode In nodo.ChildNodes
                        Dim name As String = element.Name
                        Dim value As String = CleanString(CTran(element.InnerText, ""))
                        With labelStructure
                            Select Case name.ToLower
                                Case "x"
                                    .x = value
                                Case "y"
                                    .y = value
                                Case "font"
                                    .font = value
                                Case "size"
                                    .size = value
                                Case "style"
                                    .style = value
                            End Select
                        End With
                    Next
                Next
                hLabel.Add(_tag, labelStructure)
                '
            Next
            '
            Xmlfile = New XmlDocument
            XmlLeggi.Close()
            XmlLeggi = Nothing
            Return True
        Catch ex As IOException
            Return False
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "load template")
            Return Nothing
        End Try

    End Function
    Public Function barcode_x(ByVal CodeTipo As String, ByVal dato As String) As String
        'non usata
        Dim i As Integer = 0
        Dim checksum As Integer = 0
        Dim first As Integer = 0
        Dim CodeBarre As String = ""
        Dim tableA As Boolean
        Dim dummy As Integer = 0
        Dim mini As Integer = 0
        Dim dato1 As String = ""
        '
        Select Case CodeTipo.ToLower
            Case "ean8"
                dato1 = dato
                If dato.Length > 7 Then
                    dato = dato.Substring(0, 7)
                End If
                If dato.Length = 7 Then
                    For i = 1 To 7
                        If Asc(dato.Substring(i - 1, 1)) < 48 Or Asc(dato.Substring(i - 1, 1)) > 57 Then
                            i = 0
                            Exit For
                        End If
                    Next
                    If i = 8 Then
                        'dato = dato & checksum_x(CodeTipo, dato)
                        dato = dato1
                        CodeBarre = ":"
                        For i = 1 To 4
                            CodeBarre = CodeBarre & Chr(65 + Val(dato.Substring(i - 1, 1)))
                        Next
                        CodeBarre = CodeBarre & "*"
                        For i = 5 To 8
                            CodeBarre = CodeBarre & Chr(97 + Val(dato.Substring(i - 1, 1)))
                        Next
                        CodeBarre = CodeBarre & "+"   'Ajout de la marque de fin / Add end mark
                    End If
                End If
            Case "ean13"
                dato1 = dato
                If dato.Length > 12 Then
                    dato = dato.Substring(0, 12)
                End If
                If dato.Length = 12 Then
                    For i = 1 To 12
                        If Asc(dato.Substring(i - 1, 1)) < 48 Or Asc(dato.Substring(i - 1, 1)) > 57 Then
                            i = 0
                            Exit For
                        End If
                    Next
                    If i = 13 Then
                        'dato = dato & checksum_x(CodeTipo, dato)
                        dato = dato1
                        CodeBarre = dato.Substring(0, 1) & Chr(65 + Val(dato.Substring(1, 1)))
                        first = Val(dato.Substring(0, 1))
                        For i = 3 To 7
                            tableA = False
                            Select Case i
                                Case 3
                                    Select Case first
                                        Case 0 To 3
                                            tableA = True
                                    End Select
                                Case 4
                                    Select Case first
                                        Case 0, 4, 7, 8
                                            tableA = True
                                    End Select
                                Case 5
                                    Select Case first
                                        Case 0, 1, 4, 5, 9
                                            tableA = True
                                    End Select
                                Case 6
                                    Select Case first
                                        Case 0, 2, 5, 6, 7
                                            tableA = True
                                    End Select
                                Case 7
                                    Select Case first
                                        Case 0, 3, 6, 8, 9
                                            tableA = True
                                    End Select
                            End Select
                            If tableA Then
                                CodeBarre = CodeBarre & Chr(65 + Val(dato.Substring(i - 1, 1)))
                            Else
                                CodeBarre = CodeBarre & Chr(75 + Val(dato.Substring(i - 1, 1)))
                            End If
                        Next
                        CodeBarre = CodeBarre & "*"
                        For i = 8 To 13
                            CodeBarre = CodeBarre & Chr(97 + Val(dato.Substring(i - 1, 1)))
                        Next
                        CodeBarre = CodeBarre & "+"
                    End If
                End If
            Case "code39"
                If dato.Length > 0 Then
                    For i = 1 To dato.Length
                        Select Case Asc(dato.Substring(i - 1, 1))
                            Case 32, 36, 37, 43, 45 To 57, 65 To 90
                            Case Else
                                i = 0
                                Exit For
                        End Select
                    Next
                    If i > 0 Then
                        CodeBarre = "*" & dato & "*"
                    End If
                End If
            Case "code128"
                If dato.Length > 0 Then
                    For i = 1 To Len(dato)
                        Select Case Asc(dato.Substring(i - 1, 1))
                            Case 32 To 126, 203
                            Case Else
                                i = 0
                                Exit For
                        End Select
                    Next
                    tableA = True
                    If i > 0 Then
                        i = 1
                        Do While i <= dato.Length
                            If tableA Then
                                mini = IIf(i = 1 Or i + 3 = Len(dato), 4, 6)
                                mini = mini - 1
                                If i + mini <= Len(dato) Then
                                    Do While mini >= 0
                                        If Asc(dato.Substring(i + mini - 1, 1)) < 48 Or Asc(dato.Substring(i + mini - 1, 1)) > 57 Then
                                            Exit Do
                                        End If
                                        mini = mini - 1
                                    Loop
                                End If
                                If mini < 0 Then
                                    If i = 1 Then
                                        CodeBarre = Chr(210)
                                    Else
                                        CodeBarre = CodeBarre & Chr(204)
                                    End If
                                    tableA = False
                                Else
                                    If i = 1 Then
                                        CodeBarre = Chr(209)
                                    End If
                                End If
                            End If
                            If Not tableA Then
                                mini = 1
                                If i + mini <= dato.Length Then
                                    Do While mini >= 0
                                        If Asc(dato.Substring(i + mini - 1, 1)) < 48 Or Asc(dato.Substring(i + mini - 1, 1)) > 57 Then
                                            Exit Do
                                        End If
                                        mini = mini - 1
                                    Loop
                                End If
                                If mini < 0 Then
                                    dummy = Val(dato.Substring(i - 1, 2))
                                    dummy = IIf(dummy < 95, dummy + 32, dummy + 105)
                                    CodeBarre = CodeBarre & Chr(dummy)
                                    i = i + 2
                                Else
                                    CodeBarre = CodeBarre & Chr(205)
                                    tableA = True
                                End If
                            End If
                            If tableA Then
                                CodeBarre = CodeBarre & dato.Substring(i - 1, 1)
                                i = i + 1
                            End If
                        Loop
                        For i = 1 To CodeBarre.Length
                            dummy = Asc(CodeBarre.Substring(i - 1, 1))
                            dummy = IIf(dummy < 127, dummy - 32, dummy - 105)
                            If i = 1 Then
                                checksum = dummy
                            End If
                            checksum = (checksum + (i - 1) * dummy) Mod 103
                        Next
                        checksum = IIf(checksum < 95, checksum + 32, checksum + 105)
                        CodeBarre = CodeBarre & Chr(checksum) & Chr(211)
                    End If
                End If
        End Select
        Return CodeBarre
    End Function

    Public Function checksum_x(ByVal CodeTipo As String, ByVal dato As String) As String
        Dim csum As Integer = 0
        Dim k As Integer = 0
        Dim i As Integer = 0
        '
        Select Case CodeTipo
            Case "EAN8"
                k = 7
            Case "EAN13"
                k = 12
        End Select
        For i = k To 1 Step -2
            csum = csum + Val(dato.Substring(i - 1, 1))
        Next
        csum = csum * 3
        For i = k - 1 To 1 Step -2
            csum = csum + Val(dato.Substring(i - 1, 1))
        Next
        csum = (10 - csum Mod 10) Mod 10
        Return CStr(csum)
    End Function

End Class
