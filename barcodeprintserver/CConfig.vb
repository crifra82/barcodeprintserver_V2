Imports System.Configuration
Imports System.Security.Cryptography
Public Class CConfig
    Public Sub New()
        Try
            Dim config As Configuration
            Dim elm As SettingElement
            Dim grpapp As ConfigurationSectionGroup
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            grpapp = config.GetSectionGroup("applicationSettings")
            Dim secPropSet As ClientSettingsSection
            secPropSet = CType(grpapp.Sections("barcodesrv.My.MySettings"), ClientSettingsSection)
            '
            Globale.percorsoApp = System.Environment.CurrentDirectory
            '
            elm = secPropSet.Settings.Get("Printer")
            Globale.g_printer = elm.Value.ValueXml.InnerText
            '
            elm = secPropSet.Settings.Get("Number")
            Globale.g_number = elm.Value.ValueXml.InnerText
            '
            elm = secPropSet.Settings.Get("FolderToWatch")
            Globale.g_FolderToWatch = elm.Value.ValueXml.InnerText

            elm = secPropSet.Settings.Get("larg")
            Globale.g_larg = elm.Value.ValueXml.InnerText

            elm = secPropSet.Settings.Get("alt")
            Globale.g_lung = elm.Value.ValueXml.InnerText
            '
            elm = secPropSet.Settings.Get("StartMinimized")
            If elm.Value.ValueXml.InnerText.Trim.ToUpper = "YES" Then
                Globale.g_StartMinimized = True
            Else
                Globale.g_StartMinimized = False
            End If


            '
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Configuration reading")
        End Try
    End Sub
    Public Sub LeggeConfig()
    End Sub
    Public Sub ScriviConfig()
        Try

            Dim config As Configuration
            Dim elm As SettingElement
            Dim grpapp As ConfigurationSectionGroup
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            grpapp = config.GetSectionGroup("applicationSettings")
            Dim secPropSet As ClientSettingsSection
            secPropSet = CType(grpapp.Sections("gesmagterm.My.MySettings"), ClientSettingsSection)
            secPropSet.SectionInformation.ForceSave = True
            '
            elm = secPropSet.Settings.Get("ConnectionString")
            elm.Value.ValueXml.InnerText = Globale.ConnectionString
            '
             config.Save()
        Catch ex As ConfigurationErrorsException
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Scrittura configurazione")
        End Try

    End Sub
End Class
