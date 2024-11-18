Public Class Pop_Up_Add_Tenant
    ' Properties to hold tenant details
    Public Property TenantName As String
    Public Property Email As String
    Public Property ContactNo As String
    Public Property UnitNo As String
    Public Property RegistrationDate As String

    Public Property DashboardFormInstance As DashboardForm

    Private Sub SaveTenantButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox4.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox5.Text) Then

            MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        TenantName = TextBox1.Text
        Email = TextBox2.Text
        ContactNo = TextBox3.Text
        UnitNo = TextBox4.Text
        RegistrationDate = TextBox5.Text

        If DashboardFormInstance IsNot Nothing Then
            DashboardFormInstance.RefreshDashboardData() ' Call a method to refresh the dashboard
        End If

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub CancelTenantButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
