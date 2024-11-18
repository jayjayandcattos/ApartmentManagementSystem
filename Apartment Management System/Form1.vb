
Imports MySql.Data.MySqlClient

Public Class Form1

    Dim connectionString As String = "Server=localhost;Database=oopapartment;Uid=root;"


    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click

        Dim connection As New MySqlConnection(connectionString)
        Dim command As New MySqlCommand("SELECT * FROM users WHERE username = @username AND password = @password", connection)

        command.Parameters.AddWithValue("@username", TextBox1.Text)
        command.Parameters.AddWithValue("@password", TextBox2.Text)

        Try

            connection.Open()

            Dim reader As MySqlDataReader = command.ExecuteReader()

            If reader.HasRows Then

                MessageBox.Show("Welcome, " & TextBox1.Text & ".", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Me.Hide()

                Dim secondForm As New Form2()
                secondForm.StartPosition = FormStartPosition.CenterScreen
                secondForm.Show()
            Else
                MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            reader.Close()

        Catch ex As MySqlException

            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            connection.Close()
        End Try
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub Label4_Click_1(sender As Object, e As EventArgs)

    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Application.Exit()
    End Sub
End Class