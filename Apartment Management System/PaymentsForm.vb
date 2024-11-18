Public Class PaymentsForm
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim popup As New Pop_Up_Take_Fee()

        popup.StartPosition = FormStartPosition.CenterParent

        popup.ShowDialog()
    End Sub

    Private Sub PaymentsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ListView1.Clear()

        ListView1.View = View.Details

        ListView1.Columns.Add("#", 50, HorizontalAlignment.Left)
        ListView1.Columns.Add("Name", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("Date Registered", 150, HorizontalAlignment.Left)
        ListView1.Columns.Add("Monthly Rate", 120, HorizontalAlignment.Left)
        ListView1.Columns.Add("Outstanding Balance", 200, HorizontalAlignment.Left)

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub
End Class