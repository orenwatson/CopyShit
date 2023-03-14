Imports System.ComponentModel
Imports System.IO
Imports System.Linq.Expressions

Public Class Form1
    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
        If TextBox2.Text <> "" Then
            If File.Exists(TextBox2.Text) Then
                TextBox2.Text = Path.GetDirectoryName(TextBox2.Text)
            End If
            If Directory.Exists(TextBox2.Text) Then
                TextBox2.Text = Path.Combine(TextBox2.Text, Path.GetFileName(TextBox1.Text))
            End If
            TextBox2_TextChanged(sender, e)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SaveFileDialog1.FileName = Path.GetFileName(TextBox1.Text)
        SaveFileDialog1.ShowDialog()
    End Sub

    Private StartTime, LastBlock As DateTime
    Private Copying As Boolean = False

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Not BackgroundWorker1.IsBusy Then
            Dim rdfn, wrfn As String
            rdfn = TextBox1.Text
            wrfn = TextBox2.Text
            Button1.Enabled = False
            Button2.Enabled = False
            TextBox1.Enabled = False
            TextBox2.Enabled = False
            Button3.Enabled = False
            OverCheck.Enabled = False
            ValidCheck.Enabled = False
            Button3.Text = "COPYING"
            ProgressBar1.Minimum = 0
            ProgressBar1.Value = 0
            StartTime = Now
            Copying = True
            BackgroundWorker1.RunWorkerAsync((rdfn, wrfn, OverCheck.CheckState, ValidCheck.Checked))
        End If
    End Sub

    Private Sub ReenableEverything()
        Button1.Enabled = True
        Button2.Enabled = True
        TextBox1.Enabled = True
        TextBox2.Enabled = True
        Button3.Enabled = True
        ValidCheck.Enabled = True
        Copying = False
        Button3.Text = "COPY"
    End Sub

    Private Sub UpdateShit()
        If Not Copying Then Return
        Dim kbps, curtime As Double
        curtime = (Now - StartTime).TotalMilliseconds
        If curtime < 1 Then Return
        kbps = (ProgressBar1.Value / curtime)
        KbpsBox.Text = kbps.ToString()
        KbpsBox.Update()
        SinceLastBlockBox.Text = (Now - LastBlock).TotalMilliseconds
        SinceLastBlockBox.Update()
        If kbps < 1 Then Return
        TimeLeftBox.Text = TimeSpan.FromMilliseconds((ProgressBar1.Maximum - ProgressBar1.Value) / kbps).ToString()
        TimeLeftBox.Update()
        ProgressBar1.Update()
        BlockCountBox.Update()
        BlockNumBox.Update()
        TextBox3.Update()
    End Sub

    Private Sub ReportProg(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Maximum = e.UserState.Item2
        ProgressBar1.Minimum = 0
        ProgressBar1.Value = e.UserState.Item1
        BlockCountBox.Text = Int(e.UserState.Item2 / 2048)
        BlockNumBox.Text = e.UserState.Item1 / 2048
        TextBox3.Text = e.UserState.Item3
        LastBlock = Now
    End Sub

    Private Sub FinishWork(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        ReenableEverything()
    End Sub


    Private Const blksiz As Long = 2048

    Private Shared Function CheckBlock(ByRef rdfl As FileStream, ByRef wrfl As FileStream, pos As Long, siz As Long, ByRef buf() As Byte, ByRef buf2() As Byte) As Boolean
        rdfl.Seek(pos, SeekOrigin.Begin)
        rdfl.Read(buf, 0, siz)
        wrfl.Seek(pos, SeekOrigin.Begin)
        wrfl.Read(buf2, 0, siz)
        Return buf.SequenceEqual(buf2)
    End Function

    Private Shared Sub CopyBlock(ByRef rdfl As FileStream, ByRef wrfl As FileStream, pos As Long, siz As Long, ByRef buf() As Byte)
        rdfl.Seek(pos, SeekOrigin.Begin)
        rdfl.Read(buf, 0, siz)
        wrfl.Seek(pos, SeekOrigin.Begin)
        wrfl.Write(buf, 0, siz)
        wrfl.Flush()
    End Sub


    Private Shared Function FileLen(ByRef fl As FileStream) As Long
        fl.Seek(0, SeekOrigin.End)
        Return fl.Position
    End Function

    Private Shared Function CheckAndCorrectFile(ByRef rdfl As FileStream, ByRef wrfl As FileStream, ByRef pos As Long, ByRef worker As BackgroundWorker) As Boolean
        Dim tot, totwr As Long
        tot = FileLen(rdfl)
        totwr = FileLen(wrfl)
        Dim buffer(blksiz - 1) As Byte
        Dim vldbuf(blksiz - 1) As Byte
        worker.ReportProgress(0, (pos, tot, "Checking Existing File, Correcting Wrong Blocks..."))
        While pos < totwr - blksiz
            If Not CheckBlock(rdfl, wrfl, pos, blksiz, buffer, vldbuf) Then
                wrfl.Seek(pos, SeekOrigin.Begin)
                wrfl.Write(buffer, 0, blksiz)
            End If
            pos = pos + blksiz
            worker.ReportProgress(0, (pos, tot, "Checking Existing File, Correcting Wrong Blocks..."))
        End While
        ' Return true if the entire file is correct. false if more needs to be written.
        If totwr <> tot Then Return False
        If pos = totwr Then Return True
        If Not CheckBlock(rdfl, wrfl, pos, totwr - pos, buffer, vldbuf) Then
            wrfl.Seek(pos, SeekOrigin.Begin)
            wrfl.Write(buffer, 0, totwr - pos)
        End If
        Return True
    End Function


    Private Shared Sub WriteOutFile(ByRef rdfl As FileStream, ByRef wrfl As FileStream, ByRef pos As Long, ByRef worker As BackgroundWorker)
        Dim tot As Long
        Dim buffer(blksiz - 1) As Byte
        tot = FileLen(rdfl)
        While pos < tot - blksiz
            CopyBlock(rdfl, wrfl, pos, blksiz, Buffer)
            pos = pos + blksiz
            worker.ReportProgress(0, (pos, tot, "Copying..."))
        End While
        If pos <> tot Then
            worker.ReportProgress(0, (tot, tot, "Flushing..."))
            CopyBlock(rdfl, wrfl, pos, tot - pos, Buffer)
        End If
    End Sub


    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim worker As BackgroundWorker = TryCast(sender, BackgroundWorker)
        Dim rdfl, wrfl As FileStream
        rdfl = Nothing
        wrfl = Nothing
        Dim pos, tot, totwr As Long
        pos = 0
        tot = 2
        Dim ovwr As CheckState = e.Argument.Item3
        Dim vldt As Boolean = e.Argument.Item4
        Try
            rdfl = File.OpenRead(e.Argument.Item1)
            wrfl = File.Open(e.Argument.Item2, FileMode.OpenOrCreate)
            rdfl.Seek(0, SeekOrigin.End)
            worker.ReportProgress(0, (0, tot, "Beginning Copy"))
ValidLabel:
            wrfl.Seek(0, SeekOrigin.End)
            totwr = wrfl.Position
            pos = 0
            Dim buffer(blksiz - 1) As Byte
            Dim vldbuf(blksiz - 1) As Byte
            If ovwr <> CheckState.Checked Then
                worker.ReportProgress(0, (pos, tot, "Checking Existing..."))
                While pos < totwr - blksiz
                    If Not CheckBlock(rdfl, wrfl, pos, blksiz, buffer, vldbuf) Then
                        wrfl.Seek(pos, SeekOrigin.Begin)
                        wrfl.Write(buffer, 0, blksiz)
                    End If
                    pos = pos + blksiz
                    worker.ReportProgress(0, (pos, tot, "Checking Existing..."))
                End While
                If totwr = tot AndAlso (pos = tot OrElse CheckBlock(rdfl, wrfl, pos, tot - pos, buffer, vldbuf)) Then
                    GoTo CloseLabel
                End If
            End If
WriteLabel:
            While pos < tot - blksiz
                CopyBlock(rdfl, wrfl, pos, blksiz, buffer)
                pos = pos + blksiz
                worker.ReportProgress(0, (pos, tot, "Copying..."))
            End While
            If pos <> tot Then
                CopyBlock(rdfl, wrfl, pos, tot - pos, buffer)
            End If
            worker.ReportProgress(0, (tot, tot, "Flushing..."))
CloseLabel:
            If vldt Then
                vldt = False
                ovwr = CheckState.Unchecked
                GoTo ValidLabel
            End If
            rdfl.Dispose()
            wrfl.Dispose()
            worker.ReportProgress(0, (tot, tot, "Done."))
        Catch ex As Exception
            worker.ReportProgress(0, (pos, tot, "Error: " & ex.ToString()))
            If rdfl IsNot Nothing Then rdfl.Dispose()
            If wrfl IsNot Nothing Then wrfl.Dispose()
        End Try
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As CancelEventArgs) Handles SaveFileDialog1.FileOk
        If Not e.Cancel Then
            TextBox2.Text = SaveFileDialog1.FileName
            TextBox2_TextChanged(sender, e)
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        ExistCheck.Checked = File.Exists(TextBox2.Text)
        If Not ExistCheck.Checked Then
            OverCheck.CheckState = CheckState.Indeterminate
            OverCheck.Enabled = False
        Else
            OverCheck.Enabled = True
            OverCheck.Checked = False
        End If
        ExistCheck.Update()
        OverCheck.Update()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        UpdateShit()
    End Sub

End Class
