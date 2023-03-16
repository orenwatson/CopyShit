Imports System.ComponentModel
Imports System.Data.SqlTypes
Imports System.IO
Imports System.Linq.Expressions
Imports System.Threading
Imports System.Windows.Forms.VisualStyles


Public Class Form1

    Private Class Cancellation
        Inherits Exception

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
    End Class

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
        OpenFileDialog1.InitialDirectory = Path.GetDirectoryName(TextBox1.Text)
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SaveFileDialog1.InitialDirectory = Path.GetDirectoryName(TextBox2.Text)
        SaveFileDialog1.FileName = Path.GetFileName(TextBox1.Text)
        SaveFileDialog1.ShowDialog()
    End Sub

    Private StartTime, LastBlock As DateTime
    Private Copying As Boolean = False
    Private ByteCount, ByteNum As Long

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
            ProgBarFgPanel.Visible = False
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
        kbps = (ByteNum / curtime) ' bytes / ms = kb / s
        KbpsBox.Text = kbps.ToString()
        KbpsBox.Update()
        Dim SinceLastBlock As Double = (Now - LastBlock).TotalMilliseconds
        SinceLastBlockBox.Text = SinceLastBlock
        SinceLastBlockBox.Update()
        If kbps < 1 Then Return
        TimeLeftBox.Text = TimeSpan.FromMilliseconds((ByteCount - ByteNum) / kbps).ToString()
        TimeLeftBox.Update()
        BlockCountBox.Update()
        BlockNumBox.Update()
        TextBox3.Update()
    End Sub

    Private Shared Colours() As Color = {Color.Black, Color.Blue, Color.Turquoise, Color.Green, Color.Yellow, Color.Red}

    Private Sub ReportProg(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        If e.UserState.Item2 IsNot Nothing Then
            ByteCount = e.UserState.Item2
            ByteNum = e.UserState.Item1
            LastBlock = Now
        End If
        TextBox3.Text = e.UserState.Item3
        Copying = True
        BlockCountBox.Text = Int(ByteCount / 2048)
        BlockNumBox.Text = ByteNum / 2048
        ProgBarFgPanel.Width = Convert.ToInt64(Convert.ToDouble(ProgBarBgPanel.Width) * ByteNum / ByteCount)
        ProgBarFgPanel.BackColor = Colours(e.UserState.Item4)
        ProgBarFgPanel.Visible = True
    End Sub

    Private Sub FinishWork(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
        ReenableEverything()
    End Sub


    Private Const blksiz As Long = 2048
    Private Const superblk As Long = 2048 * 2048


    Private Shared Sub SeekOrDont(ByRef fl As FileStream, pos As Long)
        If fl.Position <> pos Then fl.Seek(pos, SeekOrigin.Begin)
    End Sub

    Private Shared Sub FinishOrCancel(ByRef t As Task, ByRef c As CancellationTokenSource, ByRef worker As BackgroundWorker, c_mesg As String, b_mesg As String)
        Dim n As Long = 0
        While Not t.IsCompleted
            If worker.CancellationPending Then
                c.Cancel()
                Throw New Cancellation(c_mesg)
            End If
            t.Wait(TimeSpan.FromMilliseconds(200))
            If 1 = n Then worker.ReportProgress(0, (Nothing, Nothing, b_mesg, 0))
            n = n + 1
        End While
    End Sub

    Private Shared Sub WriteOrCancel(ByRef wrfl As FileStream, siz As Long, ByRef buf() As Byte, ByRef worker As BackgroundWorker)
        Dim c As CancellationTokenSource = New CancellationTokenSource()
        Dim t As Task = wrfl.WriteAsync(buf, 0, siz, c.Token)
        FinishOrCancel(t, c, worker, "Copy Cancelled During Write", "Write Blocking...")
    End Sub

    Private Shared Sub FlushOrCancel(ByRef wrfl As FileStream, ByRef worker As BackgroundWorker)
        Dim c As CancellationTokenSource = New CancellationTokenSource()
        Dim t As Task = wrfl.FlushAsync(c.Token)
        FinishOrCancel(t, c, worker, "Copy Cancelled During Flush", "Flushing Out To Disk...")
    End Sub

    Private Shared Sub CheckAndCorrectBlock(ByRef worker As BackgroundWorker, ByRef rdfl As FileStream, ByRef wrfl As FileStream, pos As Long, siz As Long, ByRef buf() As Byte, ByRef buf2() As Byte)
        Debug.Assert(siz <= blksiz)
        Debug.Assert(siz > 0)
        SeekOrDont(rdfl, pos)
        rdfl.Read(buf, 0, siz)
        SeekOrDont(wrfl, pos)
        wrfl.Read(buf2, 0, siz)
        If Not buf.SequenceEqual(buf2) Then
            wrfl.Seek(pos, SeekOrigin.Begin)
            WriteOrCancel(wrfl, siz, buf, worker)
        End If
    End Sub

    Private Shared Sub CopyBlock(ByRef worker As BackgroundWorker, ByRef rdfl As FileStream, ByRef wrfl As FileStream, pos As Long, siz As Long, ByRef buf() As Byte)
        Debug.Assert(siz <= blksiz)
        Debug.Assert(siz > 0)
        SeekOrDont(rdfl, pos)
        rdfl.Read(buf, 0, siz)
        SeekOrDont(wrfl, pos)
        WriteOrCancel(wrfl, siz, buf, worker)
    End Sub


    Private Shared Sub CheckAndCorrectFile(ByRef rdfl As FileStream, ByRef wrfl As FileStream, ByRef pos As Long, ByRef worker As BackgroundWorker)
        Dim tot, totwr As Long
        tot = rdfl.Length
        totwr = wrfl.Length
        If totwr = 0 Then Return
        If tot < totwr Then
            wrfl.SetLength(tot)
            totwr = tot
        End If
        Dim buffer(blksiz - 1) As Byte
        Dim vldbuf(blksiz - 1) As Byte
        While pos < totwr - blksiz
            worker.ReportProgress(0, (pos, tot, "Checking Existing File, Correcting Wrong Blocks...", 2))
            If worker.CancellationPending Then Throw New Cancellation("Copy Cancelled")
            CheckAndCorrectBlock(worker, rdfl, wrfl, pos, blksiz, buffer, vldbuf)
            pos = pos + blksiz
            If pos Mod superblk = 0 Then FlushOrCancel(wrfl, worker)
        End While
        ' Return true if the entire file is correct. false if more needs to be written.
        If totwr <> tot Then Return
        If pos = totwr Then
            FlushOrCancel(wrfl, worker)
            Return
        End If
        worker.ReportProgress(0, (pos, tot, "Checking Existing File, Correcting Wrong Blocks...", 2))
        CheckAndCorrectBlock(worker, rdfl, wrfl, pos, totwr - pos, buffer, vldbuf)
        pos = totwr
        FlushOrCancel(wrfl, worker)
        Return
    End Sub


    Private Shared Sub WriteOutFile(ByRef rdfl As FileStream, ByRef wrfl As FileStream, ByRef pos As Long, ByRef worker As BackgroundWorker)
        Dim tot, totwr As Long
        Dim buffer(blksiz - 1) As Byte
        tot = rdfl.Length
        totwr = wrfl.Length
        If pos = tot Then Return
        While pos < tot - blksiz
            worker.ReportProgress(0, (pos, tot, "Copying...", 3))
            If worker.CancellationPending Then Throw New Cancellation("Copy Cancelled")
            CopyBlock(worker, rdfl, wrfl, pos, blksiz, buffer)
            pos = pos + blksiz
            If pos Mod superblk = 0 Then FlushOrCancel(wrfl, worker)
        End While
        worker.ReportProgress(0, (pos, tot, "Flushing...", 3))
        If pos <> tot Then
            CopyBlock(worker, rdfl, wrfl, pos, tot - pos, buffer)
        End If
        FlushOrCancel(wrfl, worker)
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
        Dim rdpt, wrpt As String
        rdpt = e.Argument.Item1
        wrpt = e.Argument.Item2
        Try
            rdfl = File.OpenRead(rdpt)
            tot = rdfl.Length
            ' wrfl = New FileStream(wrpt, FileOptions.Asynchronous)
            Dim opts As New FileStreamOptions()
            opts.Options = FileOptions.Asynchronous Or FileOptions.WriteThrough
            opts.Mode = FileMode.OpenOrCreate
            opts.Access = FileAccess.ReadWrite
            wrfl = File.Open(e.Argument.Item2, opts)
            totwr = wrfl.Length
            worker.ReportProgress(0, (0, tot, "Beginning Copy", 0))
            pos = 0
            If ovwr <> CheckState.Checked Then CheckAndCorrectFile(rdfl, wrfl, pos, worker)
            WriteOutFile(rdfl, wrfl, pos, worker)
            If vldt Then
                pos = 0
                CheckAndCorrectFile(rdfl, wrfl, pos, worker)
            End If
            worker.ReportProgress(0, (tot, tot, "Closing Files...", 1))
            rdfl.Dispose()
            wrfl.Dispose()
            worker.ReportProgress(0, (tot, tot, "Done.", 1))
        Catch cx As Cancellation
            worker.ReportProgress(0, (pos, tot, "Copy Cancelled", 4))
            If rdfl IsNot Nothing Then rdfl.Dispose()
            If wrfl IsNot Nothing Then wrfl.Dispose()
        Catch ex As Exception
            worker.ReportProgress(0, (pos, tot, "Error: " & ex.ToString(), 5))
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
