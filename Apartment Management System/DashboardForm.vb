Imports MySql.Data.MySqlClient

Public Class DashboardForm
    Dim connectionString As String = "Server=localhost;Database=oopapartment;Uid=root;Pwd=123456"
    Dim connection As New MySqlConnection(connectionString)

    ' Method to open the database connection
    Private Sub OpenConnection()
        Try
            If connection.State = ConnectionState.Closed Then
                connection.Open()
            End If
        Catch ex As Exception
            MessageBox.Show("Error connecting to database: " & ex.Message)
        End Try
    End Sub

    ' Method to close the database connection
    Private Sub CloseConnection()
        If connection.State = ConnectionState.Open Then
            connection.Close()
        End If
    End Sub

    ' Method to fetch and bind data to labels
    Private Sub LoadDashboardData()
        Try
            OpenConnection()

            ' Query to get the count of available apartments
            Dim availableQuery As String = "SELECT COUNT(*) FROM tbl_rooms WHERE status = 'Vacant'"
            Dim availableCmd As New MySqlCommand(availableQuery, connection)
            Dim availableCount As Integer = Convert.ToInt32(availableCmd.ExecuteScalar())
            avlCount.Text = availableCount.ToString() ' Bind the result to the label

            ' Query to get the count of occupied apartments
            Dim occupiedQuery As String = "SELECT COUNT(*) FROM tbl_rooms WHERE status = 'Occupied'"
            Dim occupiedCmd As New MySqlCommand(occupiedQuery, connection)
            Dim occupiedCount As Integer = Convert.ToInt32(occupiedCmd.ExecuteScalar())
            occCount.Text = occupiedCount.ToString() ' Bind the result to the label

            ' Query to get the count of tenants
            Dim tenantCount As Integer = GetTenantCountFromDatabase()
            tntCount.Text = tenantCount.ToString() ' Bind the result to the label

            CloseConnection()

        Catch ex As Exception
            MessageBox.Show("Error loading dashboard data: " & ex.Message)
        End Try
    End Sub

    ' Method to get tenant count from database
    Private Function GetTenantCountFromDatabase() As Integer
        Dim count As Integer = 0
        Try
            OpenConnection()

            ' SQL query to count tenants
            Dim query As String = "SELECT COUNT(*) FROM tbl_tenants"
            Dim cmd As New MySqlCommand(query, connection)
            count = Convert.ToInt32(cmd.ExecuteScalar())

            CloseConnection()

        Catch ex As Exception
            MessageBox.Show("Error getting tenant count: " & ex.Message)
        End Try

        Return count
    End Function

    ' Event handler for form load
    Private Sub DashboardForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDashboardData() ' Load data when the form is loaded
    End Sub

    ' Public method to refresh dashboard data (including tenant count)
    Public Sub RefreshDashboardData()
        LoadDashboardData()
    End Sub

End Class
