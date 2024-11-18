Public Class Pop_Up_Update_Tenant_Info
    Public Property TenantName As String
    Public Property Email As String
    Public Property ContactNo As String
    Public Property UnitNo As String
    Public Property RegistrationDate As String

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles Update.Click
        ' Save the updated data
        TenantName = TextBox1.Text
        Email = TextBox2.Text
        ContactNo = TextBox3.Text
        UnitNo = TextBox4.Text
        RegistrationDate = TextBox5.Text

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Pop_Up_Update_Tenant_Info_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load the tenant's data into the textboxes
        TextBox1.Text = TenantName
        TextBox2.Text = Email
        TextBox3.Text = ContactNo
        TextBox4.Text = UnitNo
        TextBox5.Text = RegistrationDate
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        ' Cancel update and close the form
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class