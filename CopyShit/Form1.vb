Imports System.ComponentModel
Imports System.Data.SqlTypes
Imports System.IO
Imports System.Linq.Expressions
Imports System.Windows.Forms.VisualStyles

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
            RedCancelButton.Enabled = True
            RedCancelButton.BackColor = Color.Tomato
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
        RedCancelButton.Enabled = False
        RedCancelButton.BackColor = SystemColors.InactiveCaption
    End Sub

    Private Sub UpdateShit()
        If Not Copying Then Return
        Dim kbps, curtime As Double
        curtime = (LastBlock - StartTime).TotalMilliseconds
        If curtime < 1 Then Return
        kbps = (ProgressBar1.Value / curtime)
        KbpsBox.Text = kbps.ToString()
        KbpsBox.Update()
        Dim SinceLastBlock As Double = (Now - LastBlock).TotalMilliseconds
        SinceLastBlockBox.Text = SinceLastBlock
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
        Copying = True
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
    Private Const superblk As Long = 2048 * 2048

    Private Shared Sub CheckAndCorrectBlock(ByRef rdfl As FileStream, ByRef wrfl As FileStream, pos As Long, siz As Long, ByRef buf() As Byte, ByRef buf2() As Byte)
        Debug.Assert(siz <= blksiz)
        Debug.Assert(siz > 0)
        rdfl.Seek(pos, SeekOrigin.Begin)
        rdfl.Read(buf, 0, siz)
        wrfl.Seek(pos, SeekOrigin.Begin)
        wrfl.Read(buf2, 0, siz)
        If Not buf.SequenceEqual(buf2) Then
            wrfl.Seek(pos, SeekOrigin.Begin)
            wrfl.Write(buf, 0, siz)
            wrfl.Flush()
        End If
    End Sub

    Private Shared Sub CopyBlock(ByRef rdfl As FileStream, ByRef wrfl As FileStream, pos As Long, siz As Long, ByRef buf() As Byte)
        Debug.Assert(siz <= blksiz)
        Debug.Assert(siz > 0)
        rdfl.Seek(pos, SeekOrigin.Begin)
        rdfl.Read(buf, 0, siz)
        wrfl.Seek(pos, SeekOrigin.Begin)
        wrfl.Write(buf, 0, siz)
        wrfl.Flush()
    End Sub


    Private Shared Sub CheckAndCorrectFile(ByRef rdfl As FileStream, ByRef wrfl As FileStream, ByRef pos As Long, ByRef worker As BackgroundWorker)
        Dim tot, totwr As Long
        tot = rdfl.Length
        totwr = wrfl.Length
        If totwr = 0 Then Return
        Dim buffer(blksiz - 1) As Byte
        Dim vldbuf(blksiz - 1) As Byte
        While pos < totwr - blksiz
            worker.ReportProgress(0, (pos, tot, "Checking Existing File, Correcting Wrong Blocks..."))
            If worker.CancellationPending Then Throw New Exception("Copy Cancelled")
            CheckAndCorrectBlock(rdfl, wrfl, pos, blksiz, buffer, vldbuf)
            pos = pos + blksiz
            If pos Mod superblk = 0 Then wrfl.Flush(True)
        End While
        ' Return true if the entire file is correct. false if more needs to be written.
        If totwr <> tot Then Return
        If pos = totwr Then
            wrfl.Flush(True)
            Return
        End If
        worker.ReportProgress(0, (pos, tot, "Checking Existing File, Correcting Wrong Blocks..."))
        CheckAndCorrectBlock(rdfl, wrfl, pos, totwr - pos, buffer, vldbuf)
        pos = totwr
        wrfl.Flush(True)
        Return
    End Sub


    Private Shared Sub WriteOutFile(ByRef rdfl As FileStream, ByRef wrfl As FileStream, ByRef pos As Long, ByRef worker As BackgroundWorker)
        Dim tot, totwr As Long
        Dim buffer(blksiz - 1) As Byte
        tot = rdfl.Length
        totwr = wrfl.Length
        If totwr <> tot Then wrfl.SetLength(tot) ' prealloc sectors?
        If pos = tot Then Return
        While pos < tot - blksiz
            worker.ReportProgress(0, (pos, tot, "Copying..."))
            If worker.CancellationPending Then
                wrfl.SetLength(pos)
                Throw New Exception("Copy Cancelled")
            End If
            CopyBlock(rdfl, wrfl, pos, blksiz, buffer)
            pos = pos + blksiz
            If pos Mod superblk = 0 Then wrfl.Flush(True)
        End While
        worker.ReportProgress(0, (pos, tot, "Flushing..."))
        If pos <> tot Then
            CopyBlock(rdfl, wrfl, pos, tot - pos, buffer)
        End If
        wrfl.Flush(True)
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
            tot = rdfl.Length
            wrfl = File.Open(e.Argument.Item2, FileMode.OpenOrCreate)
            totwr = wrfl.Length
            worker.ReportProgress(0, (0, tot, "Beginning Copy"))
            pos = 0
            If ovwr <> CheckState.Checked Then CheckAndCorrectFile(rdfl, wrfl, pos, worker)
            WriteOutFile(rdfl, wrfl, pos, worker)
            If vldt Then
                pos = 0
                CheckAndCorrectFile(rdfl, wrfl, pos, worker)
            End If
            worker.ReportProgress(0, (tot, tot, "Closing Files..."))
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

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Copying Then
            CancelButton_Click(sender, e)
            e.Cancel = True
        End If
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles RedCancelButton.Click
        RedCancelButton.Enabled = False
        RedCancelButton.BackColor = SystemColors.InactiveCaption
        BackgroundWorker1.CancelAsync()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        UpdateShit()
    End Sub

End Class
